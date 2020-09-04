using System;
using System.Linq;
using System.Threading.Tasks;
using Dal.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Api
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class ShoutcastDirectoryController : Controller
    {
        private readonly IShoutcastDirectoryApi _shoutcastDirectoryApi;

        public ShoutcastDirectoryController(IShoutcastDirectoryApi shoutcastDirectoryApi)
        {
            _shoutcastDirectoryApi = shoutcastDirectoryApi;
        }

        [HttpGet]
        [Route("url/{id}")]
        public async Task<IActionResult> Collect([FromRoute] int id)
        {
            var url = await _shoutcastDirectoryApi.Url(id);

            return Ok(url);
        }
        
        [HttpGet]
        [Route("")]
        public IActionResult Collect()
        {
            var result = _shoutcastDirectoryApi.Result;

            return Ok(result);
        }
    }
}