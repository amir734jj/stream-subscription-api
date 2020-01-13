using System.Threading.Tasks;
using Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Models.Enums;
using Models.Models;
using static Models.Enums.UserRoleEnumExtension;

namespace Api.Utilities
{
 /// <summary>
    /// UserInfo Struct
    /// </summary>
    public struct UserInfo
    {
        public string Username { get; set; }
        
        public string Password { get; set; }
        
        public UserRoleEnum UserRoleEnum { get; set; }
    }
    
    public class HttpRequestUtility {
    
        private readonly UserManager<User> _userManager;
        
        private readonly SignInManager<User> _signInManager;

        private readonly HttpContext _ctx;
        
        public HttpRequestUtility(UserManager<User> userManager, SignInManager<User> signInManager, HttpContext ctx)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _ctx = ctx;
        }

        /// <summary>
        /// Extension method to quickly get the username/password
        /// </summary>
        /// <returns></returns>
        public async Task<UserInfo> GetUserInfo()
        {
            var user = await _userManager.GetUserAsync(_ctx.User);

            if (user == null)
            {
                return new UserInfo
                {
                    Username = null,
                    Password = null,
                    UserRoleEnum = UserRoleEnum.Basic
                };
            }

            var role = MostComprehensive(ParseRoles(await _userManager.GetRolesAsync(user)));

            return new UserInfo
            {
                Username = user.UserName,
                Password = user.PasswordHash,
                UserRoleEnum = role
            };
        }

        /// <summary>
        /// Extension method to check whether user is logged in or not
        /// </summary>
        /// <returns></returns>
        public bool IsAuthenticated()
        {
            return _signInManager.IsSignedIn(_ctx.User);
        }
    }
    
    // ReSharper disable once UnusedMember.Global
    public class HttpRequestUtilityBuilder : IHttpRequestUtilityBuilder
    {
        private readonly UserManager<User> _userManager;
        
        private readonly SignInManager<User> _signInManager;

        public HttpRequestUtilityBuilder(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public HttpRequestUtility For(HttpContext ctx)
        {
            return new HttpRequestUtility(_userManager, _signInManager, ctx);
        }
    }
}