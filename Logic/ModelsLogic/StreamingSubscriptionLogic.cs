using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        protected override IBasicDal<StreamingSubscription> GetBasicCrudDal()
        {
            return _streamingSubscriptionDal;
        }

        /// <summary>
        /// Pass username to GetAll
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<StreamingSubscription>> GetAll(User user)
        {
            return await _streamingSubscriptionDal.Get(x => x.User.UserName == user.UserName);
        }

        /// <summary>
        /// Pass username to Save
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<StreamingSubscription> Save(StreamingSubscription instance, User user)
        {
            instance.User = (await _userDal.Get(x => x.UserName == user.UserName)).FirstOrDefault() ?? throw new Exception("Invalid username");

            // Call base
            return await base.Save(instance);
        }
    }
}