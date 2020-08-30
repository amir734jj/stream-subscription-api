using System.Threading.Tasks;
using Models.ViewModels.Config;
using Refit;

namespace Dal.Interfaces
{
    public interface ISimpleConfigServer
    {
        [Get("/{key}")]
        Task<GlobalConfigViewModel> Load(string key);

        [Put("/{key}")]
        Task<GlobalConfigViewModel> Update(string key, GlobalConfigViewModel value);
    }
}