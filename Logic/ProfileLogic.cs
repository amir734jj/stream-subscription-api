using System.Threading.Tasks;
using Logic.Interfaces;
using Models.Models;
using Models.ViewModels;

namespace Logic
{
    public class ProfileLogic : IProfileLogic
    {
        private readonly IUserLogic _userLogic;

        public ProfileLogic(IUserLogic userLogic)
        {
            _userLogic = userLogic;
        }

        public async Task Update(User user, ProfileViewModel profileViewModel)
        {
            await _userLogic.Update(user.Id, entity =>
            {
                entity.Name = profileViewModel.Name;
            });
        }
    }
}