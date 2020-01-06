using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Enums;

namespace Logic.Interfaces
{
    public interface IStreamRipperManagement
    {
        Task<Dictionary<int, StreamStatus>> Status();
        
        Task<bool> Start(int id);

        Task<bool> Stop(int id);
    }
}