using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers.Api;

[Authorize]
[Route("api/[controller]")]
public class UserController : Controller
{
    private readonly IUserLogic _userLogic;

    private readonly UserManager<User> _userManager;

    /// <summary>
    /// Constructor dependency injection
    /// </summary>
    /// <param name="userLogic"></param>
    /// <param name="userManager"></param>
    public UserController(IUserLogic userLogic, UserManager<User> userManager)
    {
        _userLogic = userLogic;
        _userManager = userManager;
    }

    [HttpGet]
    [Route("")]
    [SwaggerOperation("GetAll")]
    [ProducesResponseType(typeof(IEnumerable<object>), 200)]
    public async Task<IActionResult> GetAll()
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);

        var users = (await _userLogic.GetAll()).Select(x => x.Id == user.Id ? x.ToAnonymousObject() : x.Obfuscate());

        return Ok(users);
    }
}