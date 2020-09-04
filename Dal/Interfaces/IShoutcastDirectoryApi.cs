using System.Collections.Generic;
using System.Threading.Tasks;
using Models.ViewModels.Shoutcast;

namespace Dal.Interfaces
{
    public interface IShoutcastDirectoryApi
    {
        Task Setup();
        
        public List<ShoutCastStream> Result { get; }
    }
}