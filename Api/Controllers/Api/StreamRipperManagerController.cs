using System.Threading.Tasks;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Models;

namespace Api.Controllers.Api
{
    [Authorize]
    [Route("api/[controller]")]
    public class StreamRipperManagerController : Controller
    {
        private readonly IStreamRipperManager _streamRipper;
        
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// Constructor dependency injection
        /// </summary>
        /// <param name="streamRipper"></param>
        /// <param name="userManager"></param>
        public StreamRipperManagerController(IStreamRipperManager streamRipper, UserManager<User> userManager)
        {
            _streamRipper = streamRipper;
            _userManager = userManager;
        }
        
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Status()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);

            var status = await _streamRipper.For(user).Status();
            
            return Ok(status);
        }
        
        [HttpPost]
        [Route("{id}/start")]
        public async Task<IActionResult> Start([FromRoute] int id)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);

            await _streamRipper.For(user).Start(id);
            
            return Ok(await _streamRipper.For(user).Status());
        }

        [HttpPost]
        [Route("{id}/stop")]
        public async Task<IActionResult> Stop([FromRoute] int id)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);

            await _streamRipper.For(user).Stop(id);

            return Ok(await _streamRipper.For(user).Status());
        }
    }
}