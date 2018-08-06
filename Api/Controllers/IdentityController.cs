using System.Threading.Tasks;
using API.Extensions;
using Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Constants;
using Models.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Controllers
{
    [Route("[controller]")]
    public class IdentityController : Controller
    {
        private readonly IIdentityLogic _identityLogic;
        
        private readonly IUserLogic _userLogic;

        public IdentityController(IIdentityLogic identityLogic, IUserLogic userLogic)
        {
            _identityLogic = identityLogic;
            _userLogic = userLogic;
        }

        /// <summary>
        /// View page to login
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Login")]
        [SwaggerOperation("Login")]
        public async Task<IActionResult> Login()
        {
            return View(new User());
        }
        
        /// <summary>
        /// View page to register
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Register")]
        [SwaggerOperation("Register")]
        public async Task<IActionResult> Register()
        {
            return View(new User());
        }

        /// <summary>
        /// Login the user
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("LoginAction")]
        [SwaggerOperation("LoginAction")]
        public async Task<IActionResult> LoginAction(User user)
        {
            _identityLogic.TryLogin(user.Username, user.Password, out var result);

            // Set session values
            if (result)
            {
                HttpContext.Session.SetString(ApiConstants.Username, user.Username);
                HttpContext.Session.SetString(ApiConstants.Password, user.Password);
                HttpContext.Session.SetString(ApiConstants.Authenticated.Key, ApiConstants.Authenticated.Value);
            }
            
            return result ? (IActionResult) Redirect(Url.Content("~/")) : RedirectToAction("NotAuthenticated");
        }
        
        /// <summary>
        /// Register the user
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("RegisterAction")]
        [SwaggerOperation("RegisterAction")]
        public async Task<IActionResult> RegisterAction(User user)
        {
            // Save the user
            var result = _userLogic.Save(user);

            return result != null ? RedirectToAction("Login") : RedirectToAction("RegisterAction");
        }

        /// <summary>
        /// Login the user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("LogoutAction")]
        [SwaggerOperation("LogoutAction")]
        public async Task<IActionResult> LogoutAction()
        {
            var (username, password) = HttpContext.Session.GetUseramePassword();
            
            _identityLogic.TryLogout(username, password, out var result);
            
            // Remove session values
            if (result)
            {
                HttpContext.Session.Clear();
            }

            return result ? (IActionResult) Redirect(Url.Content("~/")) : RedirectToAction("NotAuthenticated");
        }
        
        /// <summary>
        /// Not authenticated view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("NotAuthenticated")]
        [SwaggerOperation("NotAuthenticated")]
        public async Task<IActionResult> NotAuthenticated()
        {
            return View();
        }
    }
}