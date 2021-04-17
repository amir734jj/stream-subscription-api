using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models.ViewModels.Api;

namespace Api.Attributes
{
    public class ExceptionFilterAttribute : IAsyncExceptionFilter
    {
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            var result =
                new BadRequestObjectResult(new ErrorViewModel(ResolveExceptionMessages(context.Exception).ToArray()));

            context.Result = result;
        }
        
        private static IEnumerable<string> ResolveExceptionMessages(Exception exception)
        {
            return exception == null
                ? Enumerable.Empty<string>()
                : new[] {exception.Message}.Concat(ResolveExceptionMessages(exception.InnerException));
        }
    }
}