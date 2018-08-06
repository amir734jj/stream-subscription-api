using System.Threading.Tasks;
using API.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [AuthorizeMiddleware]
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}