using System.Threading.Tasks;
using Api.Controllers.Abstracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Models.ViewModels.Identities;
using reCAPTCHA.AspNetCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]")]
    public class IdentityController : AbstractAccountController
    {
        private readonly UserManager<User> _userManager;

        private readonly SignInManager<User> _signInManager;

        private readonly RoleManager<IdentityRole<int>> _roleManager;

        private readonly IRecaptchaService _recaptcha;

        public IdentityController(UserManager<User> userManager, SignInManager<User> signInManager,
            RoleManager<IdentityRole<int>> roleManager, IRecaptchaService recaptcha)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _recaptcha = recaptcha;
        }

        public override UserManager<User> ResolveUserManager()
        {
            return _userManager;
        }

        public override SignInManager<User> ResolveSignInManager()
        {
            return _signInManager;
        }

        public override RoleManager<IdentityRole<int>> ResolveRoleManager()
        {
            return _roleManager;
        }

        /// <summary>
        ///     View page to login
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Login")]
        [SwaggerOperation("Login")]
        public async Task<IActionResult> Login()
        {
            if (TempData.ContainsKey("Error"))
            {
                var prevError = (string) TempData["Error"];

                ModelState.AddModelError("", prevError);

                TempData.Clear();
            }

            return View();
        }

        /// <summary>
        ///     Handles login the user
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("LoginHandler")]
        [SwaggerOperation("LoginHandler")]
        public async Task<IActionResult> LoginHandler(LoginViewModel loginViewModel)
        {
            var recaptcha = await _recaptcha.Validate(Request);

            if (!recaptcha.success)
            {
                TempData["Error"] = "There was an error validating recatpcha. Please try again!";

                return RedirectToAction("Login");
            }

            var result = await base.Login(loginViewModel);

            if (result)
            {
                return Redirect(Url.Content("~/"));
            }

            return RedirectToAction("NotAuthenticated");
        }

        /// <summary>
        ///     View page to register
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Register")]
        [SwaggerOperation("Register")]
        public async Task<IActionResult> Register()
        {
            if (TempData.ContainsKey("Error"))
            {
                var prevError = (string) TempData["Error"];

                ModelState.AddModelError("", prevError);

                TempData.Clear();
            }

            return View();
        }

        /// <summary>
        ///     Register the user
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("RegisterHandler")]
        [SwaggerOperation("RegisterHandler")]
        public async Task<IActionResult> RegisterHandler(RegisterViewModel registerViewModel)
        {
            var recaptcha = await _recaptcha.Validate(Request);

            if (!recaptcha.success)
            {
                TempData["Error"] = "There was an error validating recatpcha. Please try again!";

                return RedirectToAction("Register");
            }

            // Save the user
            var result = await Register(registerViewModel);

            if (result)
            {
                return RedirectToAction("Login");
            }

            return RedirectToAction("Register");
        }

        /// <summary>
        ///     Not authenticated view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("NotAuthenticated")]
        [SwaggerOperation("NotAuthenticated")]
        public async Task<IActionResult> NotAuthenticated()
        {
            return View();
        }

        /// <summary>
        ///     Not authorized view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Logout")]
        [SwaggerOperation("Logout")]
        public async Task<IActionResult> Logout()
        {
            await base.Logout();

            return RedirectToAction("Login");
        }
    }
}