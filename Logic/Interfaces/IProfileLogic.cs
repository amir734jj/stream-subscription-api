using System.Threading.Tasks;
using Models.Models;
using Models.ViewModels;

namespace Logic.Interfaces
{
    public interface IProfileLogic
    {
        Task Update(User user, ProfileViewModel profileViewModel);
    }
}