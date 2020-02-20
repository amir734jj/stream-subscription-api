using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Enums;
using Models.Models;

namespace Logic.Interfaces
{
    public interface IStreamRipperManager
    {
        public IStreamRipperManagerImpl For(User user);
    }

    public interface IStreamRipperManagerImpl
    {
        Task<bool> Start(int id);

        Task<bool> Stop(int id);

        Task<Dictionary<Stream, StreamStatusEnum>> Status();
    }
}