using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.Configs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.Models;
using Models.ViewModels.Identities;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers.Api
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signManager;
        private readonly IOptions<JwtSettings> _jwtSettings;

        public AccountController(IOptions<JwtSettings> jwtSettings, UserManager<User> userManager,
            SignInManager<User> signManager)
        {
            _jwtSettings = jwtSettings;
            _userManager = userManager;
            _signManager = signManager;
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

            if (identityResults.Aggregate(true, (b, result) => b && result.Succeeded))
            {
                return Ok("Successfully registered!");
            }

            return BadRequest("Failed to register!");
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
                return BadRequest(new
                {
                    error = "", // OpenIdConnectConstants.Errors.InvalidGrant,
                    error_description = "The username or password is invalid."
                });
            }

            await _signManager.SignInAsync(user, true);

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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var expires = DateTime.Now.AddMinutes(_jwtSettings.Value.AccessTokenDurationInMinutes);

            var token = new JwtSecurityToken(
                _jwtSettings.Value.Issuer,
                _jwtSettings.Value.Issuer,
                claims,
                expires: expires,
                signingCredentials: credentials);

            return (new JwtSecurityTokenHandler().WriteToken(token), expires);
        }
    }
}