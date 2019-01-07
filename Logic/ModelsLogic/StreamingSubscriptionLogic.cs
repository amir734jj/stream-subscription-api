using System.Collections.Generic;
using System.Linq;
using Dal.Interfaces;
using Logic.Abstracts;
using Logic.Interfaces;
using Models.Models;

namespace Logic.ModelsLogic
{
    public class StreamingSubscriptionLogic : BasicLogicAbstract<StreamingSubscription>, IStreamingSubscriptionLogic
    {
        private readonly IStreamingSubscriptionDal _streamingSubscriptionDal;
        private readonly IUserDal _userDal;

        /// <summary>
        /// Constructor dependency injection
        /// </summary>
        /// <param name="streamingSubscriptionDal"></param>
        /// <param name="userDal"></param>
        public StreamingSubscriptionLogic(IStreamingSubscriptionDal streamingSubscriptionDal, IUserDal userDal)
        {
            _streamingSubscriptionDal = streamingSubscriptionDal;
            _userDal = userDal;
        }

        /// <summary>
        /// Returns DAL
        /// </summary>
        /// <returns></returns>
        public override IBasicDal<StreamingSubscription> GetBasicCrudDal() => _streamingSubscriptionDal;

        /// <summary>
        /// Pass username to GetAll
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StreamingSubscription> GetAll(string username)
        {
            // Call base with extra filter
            return base.GetAll()
                .Where(x => x.User != null && x.User.Username == username);
        }

        /// <summary>
        /// Pass username to Save
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public StreamingSubscription Save(StreamingSubscription instance, string username)
        {
            instance.User = _userDal.GetAll().FirstOrDefault(x => x.Username == username);

            // Call base
            return base.Save(instance);
        }
    }
}