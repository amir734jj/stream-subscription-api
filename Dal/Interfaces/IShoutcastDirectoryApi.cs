using System.Collections.Generic;
using System.Threading.Tasks;
using Models.ViewModels.Shoutcast;

namespace Dal.Interfaces
{
    public interface IShoutcastDirectoryApi
    {
        public List<ShoutCastStream> Result { get; }
    }
}