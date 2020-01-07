using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [AllowAnonymous]
    [Route("")]
    public class HomeController : Controller
    {
        /// <summary>
        /// Home page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }

            return RedirectToAction("About");
        }
        
        /// <summary>
        /// About page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("About")]
        public async Task<IActionResult> About()
        {
            return View();
        }
    }
}