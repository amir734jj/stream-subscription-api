using System.Collections.Generic;
using Models;
using Models.Models;

namespace Logic.Interfaces
{
    public interface IStreamingSubscriptionLogic : IBasicLogic<StreamingSubscription>
    {
        /// <summary>
        /// Returns all StreamingSubscription which matching username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        IEnumerable<StreamingSubscription> GetAll(string username);

        /// <summary>
        /// Saves the StreamingSubscription with filled-in user field
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        StreamingSubscription Save(StreamingSubscription instance, string username);
    }
}