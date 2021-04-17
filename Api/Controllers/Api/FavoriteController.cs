using System;
using System.IO;
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
    public class FavoriteController : Controller
    {
        private readonly IFavoriteLogic _favoriteLogic;

        private readonly UserManager<User> _userManager;

        public FavoriteController(IFavoriteLogic favoriteLogic, UserManager<User> userManager)
        {
            _favoriteLogic = favoriteLogic;
            _userManager = userManager;
        }
        
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Upload([FromBody]FavoriteViewModel favorite)
        {
            if (favorite.Stream == null)
            {
                return BadRequest("Failed to upload file");
            }
            
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);

            await _favoriteLogic.For(user.Id).UploadFavorite(favorite.Filename, new MemoryStream(
                Convert.FromBase64String(favorite.Stream)
            ));

            return Ok("Successfully uploaded stream");
        }
    }
}