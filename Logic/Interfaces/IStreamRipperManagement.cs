using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Enums;
using Models.Models;
using Models.ViewModels.Streams;

namespace Logic.Interfaces
{
    public interface IStreamRipperManagement
    {
        Task<bool> Start(int id);

        Task<bool> Stop(int id);
        
        /// <summary>
        /// Returns all StreamingSubscription which matching username
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<StreamsStatusViewModel> Status(User user);

        /// <summary>
        /// Saves the StreamingSubscription with filled-in user field
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<Stream> Save(AddStreamViewModel instance, User user);
        
        /// <summary>
        /// Returns all StreamingSubscription which matching username
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IEnumerable<Stream>> Get(User user);

        /// <summary>
        /// Save a stream + it's user data
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<Stream> Save(Stream instance, User user);
    }
}