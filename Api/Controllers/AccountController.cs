using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    /// <summary>
    ///     Dummy controller to be compliant 
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    [AllowAnonymous]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            return RedirectToAction("Login", "Identity");
        }
    }
}