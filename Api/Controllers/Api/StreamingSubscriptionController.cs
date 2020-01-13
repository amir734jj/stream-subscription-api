using System.Threading.Tasks;
using Api.Abstracts;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Models.ViewModels.Streams;

namespace Api.Controllers.Api
{
    [Authorize]
    [Route("api/[controller]")]
    public class StreamingSubscriptionController : BasicController<Stream>
    {
        private readonly IStreamingLogic _streamingLogic;
        
        private readonly UserManager<User> _userManager;
        
        private readonly IStreamRipperManagement _streamRipperManagement;

        /// <summary>
        /// Constructor dependency injection
        /// </summary>
        /// <param name="streamingLogic"></param>
        /// <param name="streamRipperManagement"></param>
        /// <param name="userManager"></param>
        public StreamingSubscriptionController(IStreamingLogic streamingLogic, IStreamRipperManagement streamRipperManagement , UserManager<User> userManager)
        {
            _streamingLogic = streamingLogic;
            _streamRipperManagement = streamRipperManagement;
            _userManager = userManager;
        }

        /// <summary>
        /// Returns instance of logic
        /// </summary>
        /// <returns></returns>
        protected override IBasicLogic<Stream> BasicLogic()
        {
            return _streamingLogic;
        }

        /// <summary>
        /// Override GetAll to pass username
        /// </summary>
        /// <returns></returns>
        public override async Task<IActionResult> GetAll()
        {
            var user = await _userManager.GetUserAsync(User);
            
            return Ok(_streamRipperManagement.Get(user));
        }

        /// <summary>
        /// Override Save to pass username
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public override async Task<IActionResult> Save([FromBody] Stream instance)
        {
            var user = await _userManager.GetUserAsync(User);

            return Ok(_streamRipperManagement.Save(instance, user));
        }
    }
}