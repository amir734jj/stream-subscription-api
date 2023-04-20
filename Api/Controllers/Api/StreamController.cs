using System.Threading.Tasks;
using Api.Abstracts;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Models;

namespace Api.Controllers.Api;

[Authorize]
[Route("api/[controller]")]
public class StreamController : BasicCrudController<Stream>
{
    private readonly UserManager<User> _userManager;

    private readonly IStreamLogic _streamLogic;

    public StreamController(UserManager<User> userManager,
        IStreamLogic streamLogic)
    {
        _userManager = userManager;
        _streamLogic = streamLogic;
    }

    protected override async Task<IBasicLogic<Stream>> BasicLogic()
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);

        return _streamLogic.For(user);
    }
}