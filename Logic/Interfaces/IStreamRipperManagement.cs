using System.Collections.Generic;
using Models.Enums;

namespace Logic.Interfaces
{
    public interface IStreamRipperManagement
    {
        Dictionary<int, StreamStatus> Status();
        
        bool Start(int id);

        bool Stop(int id);
    }
}