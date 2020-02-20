using System.Threading.Tasks;
using Api.Abstracts;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Models;

namespace Api.Controllers.Api
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : BasicCrudController<User>
    {
        private readonly IUserLogic _userLogic;

        /// <summary>
        /// Constructor dependency injection
        /// </summary>
        /// <param name="userLogic"></param>
        public UserController(IUserLogic userLogic)
        {
            _userLogic = userLogic;
        }

        /// <summary>
        /// Returns instance of logic
        /// </summary>
        /// <returns></returns>
        protected override async Task<IBasicLogic<User>> BasicLogic()
        {
            return _userLogic;
        }
    }
}