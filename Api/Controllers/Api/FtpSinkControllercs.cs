using System.Threading.Tasks;
using Api.Abstracts;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Models.Models.Sinks;

namespace Api.Controllers.Api;

[Authorize]
[Route("api/[controller]")]
public class FtpSinkController : BasicCrudController<FtpSink>
{
    private readonly UserManager<User> _userManager;

    private readonly IFtpSinkLogic _ftpSinkLogic;

    public FtpSinkController(UserManager<User> userManager,
        IFtpSinkLogic ftpSinkLogic)
    {
        _userManager = userManager;
        _ftpSinkLogic = ftpSinkLogic;
    }

    protected override async Task<IBasicLogic<FtpSink>> BasicLogic()
    {
        var user = await _userManager.FindByEmailAsync(User.Identity.Name);

        return _ftpSinkLogic.For(user);
    }
}