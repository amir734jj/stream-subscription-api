using System.Threading.Tasks;
using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
        /// <summary>
        /// Home page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.IsAuthenticated())
            {
                return View();
            }

            return RedirectToAction("About");
        }
        
        /// <summary>
        /// About page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> About()
        {
            return View();
        }
    }
}