using System;
using System.Threading.Tasks;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Models.Enums;
using Models.Models;
using Models.ViewModels.Streams;

namespace Api.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [AllowAnonymous]
    [Route("[controller]")]
    public class StreamController : Controller
    {
        private readonly IStreamRipperManagement _streamRipperManagement;
        
        private readonly UserManager<User> _userManager;
        
        private readonly IStreamingLogic _streamingLogic;

        public StreamController(IStreamRipperManagement streamRipperManagement, UserManager<User> userManager, IStreamingLogic streamingLogic)
        {
            _streamRipperManagement = streamRipperManagement;
            _userManager = userManager;
            _streamingLogic = streamingLogic;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var streams = await _streamRipperManagement.Status(user);
            
            return View(streams);
        }
        
        [HttpGet]
        [Route("Add")]
        public async Task<IActionResult> AddStream()
        {
            return View("Add");
        }
        
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddStreamHandler(AddStreamViewModel addStreamViewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            
            var stream = await _streamRipperManagement.Save(addStreamViewModel, user);

            TempData[nameof(Stream)] = stream.Id;
            
            return RedirectToAction("AddCredentials", new
            {
                serviceType = addStreamViewModel.ServiceType
            });
        }

        [HttpGet]
        [Route("Add/{serviceType}")]
        public async Task<IActionResult> AddCredentials(ServiceTypeEnum serviceType)
        {
            switch (serviceType)
            {
                case ServiceTypeEnum.FTP when TempData.ContainsKey(nameof(Stream)):
                    var streamId = TempData[nameof(Stream)];
                    var stream = await _streamingLogic.Get((int) streamId);
                    TempData.Clear();    // clean-up
                    return View("Services/Ftp");
                default:
                    throw new ArgumentOutOfRangeException(nameof(serviceType), serviceType, null);
            }
        }
    }
}