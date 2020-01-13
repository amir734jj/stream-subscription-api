using Api.Utilities;
using Microsoft.AspNetCore.Http;

namespace Api.Interfaces
{
    public interface IHttpRequestUtilityBuilder
    {
        HttpRequestUtility For(HttpContext ctx);
    }
}