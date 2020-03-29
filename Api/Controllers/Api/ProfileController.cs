using System.Threading.Tasks;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Models.ViewModels.Api;

namespace Api.Controllers.Api
{
    [Authorize]
    [ApiController]
    [Route("Api/[controller]")]
    public class ProfileController : Controller
    {
        private readonly UserManager<User> _userManager;
        
        private readonly IProfileLogic _profileLogic;
        
        private readonly IUserLogic _userLogic;

        public ProfileController(UserManager<User> userManager, IProfileLogic profileLogic, IUserLogic userLogic)
        {
            _userManager = userManager;
            _profileLogic = profileLogic;
            _userLogic = userLogic;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);

            var userWithNonNullReferenceProperties = await _userLogic.Get(user.Id);
            
            return Ok(new ProfileViewModel(userWithNonNullReferenceProperties));
        }
        
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Update([FromBody] ProfileViewModel profileViewModel)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);

            await _profileLogic.Update(user, profileViewModel);

            return Ok(profileViewModel);
        }
    }
}