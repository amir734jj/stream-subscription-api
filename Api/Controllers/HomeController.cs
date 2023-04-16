using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

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
    public IActionResult Index()
    {
        return View();
    }
}