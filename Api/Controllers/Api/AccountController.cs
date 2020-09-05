using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.Configs;
using Logic.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models.Models;
using Models.ViewModels.Identities;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using Models.ViewModels.Api;

namespace Api.Controllers.Api
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private const string SetupUserTempDataKey = nameof(SetupUserTempDataKey);

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IUserSetup _userSetup;
        private readonly IUserLogic _userLogic;

        public AccountController(JwtSettings jwtSettings, UserManager<User> userManager,
            SignInManager<User> signManager, IUserSetup userSetup, IUserLogic userLogic)
        {
            _jwtSettings = jwtSettings;
            _userManager = userManager;
            _signManager = signManager;
            _userSetup = userSetup;
            _userLogic = userLogic;
        }

        [HttpGet]
        [Route("")]
        [SwaggerOperation("AccountInfo")]
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);
                
                return Ok(user);
            }

            return Ok(new { });
        }

        [HttpPost]
        [Route("Register")]
        [SwaggerOperation("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel registerViewModel)
        {
            var users = (await _userLogic.GetAll()).ToList();
            
            if (registerViewModel.Password != registerViewModel.PasswordConfirmation)
            {
                return BadRequest(new ErrorViewModel("Password and Password Confirmation do not match"));
            }

            if (users.Any(x => x.UserName.Equals(registerViewModel.Username, StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest(new ErrorViewModel("Username is already taken. Please try another username"));
            }
            
            if (users.Any(x => x.Email.Equals(registerViewModel.Email, StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest(new ErrorViewModel("There is an existing user with this email. Please try another email"));
            }
            
            var user = new User
            {
                Name = registerViewModel.Name,
                Email = registerViewModel.Email,
                UserName = registerViewModel.Username
            };

            // Create user
            var identityResults = new List<IdentityResult>
            {
                await _userManager.CreateAsync(user, registerViewModel.Password)
            };

            if (identityResults.All(x => x.Succeeded))
            {
                return RedirectToAction("Setup", new {userId = user.Id});
            }

            return BadRequest(new ErrorViewModel(new[] {"Failed to register!"}
                .Concat(identityResults.SelectMany(x => x.Errors.Select(y => y.Description))).ToArray()));
        }

        [HttpGet]
        [Route("Setup/{userId}")]
        [SwaggerOperation("Setup")]
        [AllowAnonymous]
        public async Task<IActionResult> Setup([FromRoute] int userId)
        {
            await _userSetup.Setup(userId);

            return Ok("Successfully registered and setup user");
        }

        [HttpPost]
        [Route("Login")]
        [SwaggerOperation("Login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            // Ensure the username and password is valid.
            var user = await _userManager.FindByNameAsync(loginViewModel.Username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginViewModel.Password))
            {
                return BadRequest(new ErrorViewModel("The username or password is invalid."));
            }

            await _signManager.SignInAsync(user, true);

            // Set LastLoginTime
            await _userLogic.Update(user.Id, x => x.LastLoginTime = DateTimeOffset.Now);

            var (token, expires) = ResolveToken(user);

            return Ok(new
            {
                token,
                user.Name,
                user.Email,
                expires
            });
        }

        [Authorize]
        [HttpPost]
        [Route("Logout")]
        [SwaggerOperation("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signManager.SignOutAsync();

            return Ok("Logged-Out");
        }
        
        [Authorize]
        [HttpGet]
        [Route("Refresh")]
        [SwaggerOperation("Refresh")]
        public async Task<IActionResult> Refresh()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
                
            var (token, expires) = ResolveToken(user);

            return Ok(new
            {
                token,
                user.Name,
                user.Email,
                expires
            });
        }

        /// <summary>
        ///     Resolves a token given a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private (string, DateTime) ResolveToken(User user)
        {
            // Generate and issue a JWT token
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Email),    // use email as name
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var expires = DateTime.Now.AddMinutes(_jwtSettings.AccessTokenDurationInMinutes);

            var token = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Issuer,
                claims,
                expires: expires,
                signingCredentials: credentials);

            return (new JwtSecurityTokenHandler().WriteToken(token), expires);
        }
    }
}