using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using API.Extensions;
using Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Models.Constants;

namespace API.Attributes
{
    public class AuthorizeActionFilter : IAsyncActionFilter
    {
        private readonly IIdentityLogic _identityLogic;

        public AuthorizeActionFilter(IIdentityLogic identityLogic)
        {
            _identityLogic = identityLogic;
        }
        
        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controller = (Controller) context.Controller;
            var method = ((ControllerActionDescriptor) context.ActionDescriptor).MethodInfo;
            
            var controllerLevelAuthorize = controller.GetType().GetCustomAttribute<AuthorizeMiddlewareAttribute>();
            var actionLevelAuthorize = method.GetCustomAttribute<AuthorizeMiddlewareAttribute>();
            
            if (controllerLevelAuthorize == null && actionLevelAuthorize == null) return next();
            
            // Try to get username/password from session
            var (username, password) = context.HttpContext.Session.GetUseramePassword();

            if (_identityLogic.IsAuthenticated(username, password))
            {
                return next();
            }

            // Redirect to not-authenticated
            context.HttpContext.Response.Redirect("Identity/NotAuthenticated");

            return Task.CompletedTask;
        }
    }
}