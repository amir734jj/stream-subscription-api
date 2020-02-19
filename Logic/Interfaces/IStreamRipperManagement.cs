using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Enums;
using Models.Models;

namespace Logic.Interfaces
{
    public interface IStreamRipperManagement
    {
        Task<bool> Start(User user, int id);

        Task<bool> Stop(int id);
        
        /// <summary>
        /// Returns all StreamingSubscription which matching username
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<Dictionary<Stream, StreamStatusEnum>> Status(User user);
    }
}