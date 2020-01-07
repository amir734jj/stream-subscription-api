using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Enums;
using Models.Models;
using Models.ViewModels.Identities;

namespace Api.Controllers.Abstracts
{
    public abstract class AbstractAccountController : Controller
    {
        public abstract UserManager<User> ResolveUserManager();

        public abstract SignInManager<User> ResolveSignInManager();

        public abstract RoleManager<IdentityRole<int>> ResolveRoleManager();

        public async Task<bool> Register(RegisterViewModel registerViewModel)
        {
            var role = ResolveUserManager().Users.Any() ? UserRoleEnum.Basic : UserRoleEnum.Admin;

            var user = new User
            {
                Fullname = registerViewModel.Fullname,
                UserName = registerViewModel.Username,
                Email = registerViewModel.Email,
                PhoneNumber = registerViewModel.PhoneNumber,
                SecurityStamp = Guid.NewGuid().ToString(),
                Role = role
            };

            var result1 = (await ResolveUserManager().CreateAsync(user, registerViewModel.Password)).Succeeded;

            if (!result1)
            {
                return false;
            }

            var result2 = true;
            
            foreach (var subRole in role.SubRoles())
            {
                if (!await ResolveRoleManager().RoleExistsAsync(subRole.ToString()))
                {
                    await ResolveRoleManager().CreateAsync(new IdentityRole<int>(subRole.ToString()));
                    
                    result2 &= (await ResolveUserManager().AddToRoleAsync(user, subRole.ToString())).Succeeded;
                }
            }

            return result2;
        }

        public async Task<bool> Login(LoginViewModel loginViewModel)
        {
            // Ensure the username and password is valid.
            var result = await ResolveUserManager().FindByNameAsync(loginViewModel.Username);

            if (result == null || !await ResolveUserManager().CheckPasswordAsync(result, loginViewModel.Password))
            {
                return false;
            }

            await ResolveSignInManager().SignInAsync(result, true);

            // Generate and issue a JWT token
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, result.Email)
            };

            var identity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme,
                ClaimTypes.Name, ClaimTypes.Role);

            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.Now.AddDays(1),
                IsPersistent = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(principal), authProperties);

            return true;
        }

        public async Task Logout()
        {
            await ResolveSignInManager().SignOutAsync();

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}