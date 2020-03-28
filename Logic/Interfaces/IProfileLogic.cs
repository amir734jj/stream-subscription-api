using System.Threading.Tasks;
using Models.Models;
using Models.ViewModels;
using Models.ViewModels.Api;

namespace Logic.Interfaces
{
    public interface IProfileLogic
    {
        Task Update(User user, ProfileViewModel profileViewModel);
    }
}