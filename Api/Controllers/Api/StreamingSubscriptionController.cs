using System.Threading.Tasks;
using API.Abstracts;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Models;

namespace API.Controllers.Api
{
    [Authorize]
    [Route("api/[controller]")]
    public class StreamingSubscriptionController : BasicController<StreamingSubscription>
    {
        private readonly IStreamingSubscriptionLogic _streamingSubscriptionLogic;
        
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// Constructor dependency injection
        /// </summary>
        /// <param name="streamingSubscriptionLogic"></param>
        /// <param name="userManager"></param>
        public StreamingSubscriptionController(IStreamingSubscriptionLogic streamingSubscriptionLogic,  UserManager<User> userManager)
        {
            _streamingSubscriptionLogic = streamingSubscriptionLogic;
            _userManager = userManager;
        }

        /// <summary>
        /// Returns instance of logic
        /// </summary>
        /// <returns></returns>
        protected override IBasicLogic<StreamingSubscription> BasicLogic()
        {
            return _streamingSubscriptionLogic;
        }

        /// <summary>
        /// Override GetAll to pass username
        /// </summary>
        /// <returns></returns>
        public override async Task<IActionResult> GetAll()
        {
            var user = await _userManager.GetUserAsync(User);
            
            return Ok(_streamingSubscriptionLogic.GetAll(user.UserName));
        }

        /// <summary>
        /// Override Save to pass username
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public override async Task<IActionResult> Save([FromBody] StreamingSubscription instance)
        {
            var user = await _userManager.GetUserAsync(User);

            return Ok(_streamingSubscriptionLogic.Save(instance, user.UserName));
        }
    }
}