using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Models.Models;

namespace Logic.Providers
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        private readonly UserManager<User> _userManager;

        public CustomUserIdProvider(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        
        public string GetUserId(HubConnectionContext connection)
        {
            return _userManager.FindByEmailAsync(connection.User.Identity.Name).Result.Id.ToString();
        }
    }
}