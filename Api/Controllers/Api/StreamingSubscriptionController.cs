using System.Threading.Tasks;
using API.Abstracts;
using API.Attributes;
using API.Extensions;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Models;

namespace API.Controllers.Api
{
    [AuthorizeMiddleware]
    [Route("api/[controller]")]
    public class StreamingSubscriptionController : BasicController<StreamingSubscription>
    {
        private readonly IStreamingSubscriptionLogic _streamingSubscriptionLogic;

        /// <summary>
        /// Constructor dependency injection
        /// </summary>
        /// <param name="streamingSubscriptionLogic"></param>
        public StreamingSubscriptionController(IStreamingSubscriptionLogic streamingSubscriptionLogic) => _streamingSubscriptionLogic = streamingSubscriptionLogic;

        /// <summary>
        /// Returns instance of logic
        /// </summary>
        /// <returns></returns>
        public override IBasicLogic<StreamingSubscription> BasicLogic() => _streamingSubscriptionLogic;

        /// <summary>
        /// Override GetAll to pass username
        /// </summary>
        /// <returns></returns>
        public override async Task<IActionResult> GetAll()
        {
            return Ok(_streamingSubscriptionLogic.GetAll(HttpContext.Session.GetUseramePassword().username));
        }

        /// <summary>
        /// Override Save to pass username
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public override async Task<IActionResult> Save([FromBody] StreamingSubscription instance)
        {
            return Ok(_streamingSubscriptionLogic.Save(instance, HttpContext.Session.GetUseramePassword().username));
        }
    }
}