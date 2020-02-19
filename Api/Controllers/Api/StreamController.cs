using System.Threading.Tasks;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Models.ViewModels.Streams;

namespace Api.Controllers.Api
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class StreamController : Controller
    {
        private readonly IStreamRipperManagement _streamRipperManagement;
        
        private readonly UserManager<User> _userManager;
        
        private readonly IStreamLogic _streamLogic;

        public StreamController(IStreamRipperManagement streamRipperManagement, UserManager<User> userManager, IStreamLogic streamLogic)
        {
            _streamRipperManagement = streamRipperManagement;
            _userManager = userManager;
            _streamLogic = streamLogic;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var streams = await _streamRipperManagement.Status(user);
            
            return Ok(streams);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddStreamHandler(AddStreamViewModel addStreamViewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            
            var stream = await _streamRipperManagement.Save(addStreamViewModel, user);

            return Ok(stream);
        }
    }
}