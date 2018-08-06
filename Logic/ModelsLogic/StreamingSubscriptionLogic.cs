using Dal.Interfaces;
using Logic.Abstracts;
using Logic.Interfaces;
using Models;
using Models.Models;

namespace Logic.ModelsLogic
{
    public class StreamingSubscriptionLogic : BasicLogicAbstract<StreamingSubscription>, IStreamingSubscriptionLogic
    {
        private readonly IStreamingSubscriptionDal _streamingSubscriptionDal;

        /// <summary>
        /// Constructor dependency injection
        /// </summary>
        /// <param name="streamingSubscriptionDal"></param>
        public StreamingSubscriptionLogic(IStreamingSubscriptionDal streamingSubscriptionDal) => _streamingSubscriptionDal = streamingSubscriptionDal;

        /// <summary>
        /// Returns DAL
        /// </summary>
        /// <returns></returns>
        public override IBasicDal<StreamingSubscription> GetBasicCrudDal() => _streamingSubscriptionDal;
    }
}