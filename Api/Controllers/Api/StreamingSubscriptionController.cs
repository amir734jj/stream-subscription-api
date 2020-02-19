using System.Threading.Tasks;
using Api.Abstracts;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Models;

namespace Api.Controllers.Api
{
    [Authorize]
    [Route("api/[controller]")]
    public class StreamingSubscriptionController : BasicController<Stream>
    {
        private readonly IStreamLogic _streamLogic;
        
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// Constructor dependency injection
        /// </summary>
        /// <param name="streamLogic"></param>
        /// <param name="userManager"></param>
        public StreamingSubscriptionController(IStreamLogic streamLogic, UserManager<User> userManager)
        {
            _streamLogic = streamLogic;
            _userManager = userManager;
        }

        /// <summary>
        /// Returns instance of logic
        /// </summary>
        /// <returns></returns>
        protected override IBasicLogic<Stream> BasicLogic()
        {
            return _streamLogic;
        }

        /// <summary>
        /// Override GetAll to pass username
        /// </summary>
        /// <returns></returns>
        public override async Task<IActionResult> GetAll()
        {
            var user = await _userManager.GetUserAsync(User);
            
            return Ok(_streamLogic.GetAll(user));
        }

        /// <summary>
        /// Override Save to pass username
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public override async Task<IActionResult> Save([FromBody] Stream instance)
        {
            var user = await _userManager.GetUserAsync(User);

            return Ok(_streamLogic.Save(user, instance));
        }
    }
}