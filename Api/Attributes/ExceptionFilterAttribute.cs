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
            var result = new BadRequestObjectResult(new ErrorViewModel(context.Exception.Message));

            context.Result = result;
        }
    }
}