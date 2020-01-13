using System.Threading.Tasks;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Models.ViewModels.Streams;

namespace Api.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [AllowAnonymous]
    [Route("[controller]")]
    public class StreamController : Controller
    {
        private readonly IStreamRipperManagement _streamingLogic;
        
        private readonly UserManager<User> _userManager;

        public StreamController(IStreamRipperManagement streamingLogic, UserManager<User> userManager)
        {
            _streamingLogic = streamingLogic;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var streams = await _streamingLogic.Status(user);
            
            return View(streams);
        }
        
        [HttpGet]
        [Route("Add")]
        public async Task<IActionResult> Add()
        {
            return View("Add");
        }
        
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddHandler(AddStreamViewModel addStreamViewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            
            await _streamingLogic.Save(addStreamViewModel, user);
            
            return RedirectToAction("Index");
        }
    }
}