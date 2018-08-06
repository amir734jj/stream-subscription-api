using API.Abstracts;
using API.Attributes;
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
    }
}