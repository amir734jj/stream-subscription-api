using API.Abstracts;
using API.Attributes;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Models;

namespace API.Controllers.Api
{
    [AuthorizeMiddleware]
    [Route("api/[controller]")]
    public class UserController : BasicController<User>
    {
        private readonly IUserLogic _userLogic;

        /// <summary>
        /// Constructor dependency injection
        /// </summary>
        /// <param name="userLogic"></param>
        public UserController(IUserLogic userLogic) => _userLogic = userLogic;

        /// <summary>
        /// Returns instance of logic
        /// </summary>
        /// <returns></returns>
        public override IBasicLogic<User> BasicLogic() => _userLogic;
    }
}