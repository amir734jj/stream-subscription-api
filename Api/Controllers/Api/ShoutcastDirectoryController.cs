using System.Linq;
using Dal.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Api
{
    [Authorize]
    [Route("api/[controller]")]
    public class ShoutcastDirectoryController : Controller
    {
        private readonly IShoutcastDirectoryApi _shoutcastDirectoryApi;

        public ShoutcastDirectoryController(IShoutcastDirectoryApi shoutcastDirectoryApi)
        {
            _shoutcastDirectoryApi = shoutcastDirectoryApi;
        }

        
        [HttpGet]
        [Route("")]
        public IActionResult Collect([FromQuery] string name = "", [FromQuery] string genre = "")
        {
            var result = _shoutcastDirectoryApi.Result;

            if (!string.IsNullOrEmpty(name))
            {
                result = result.Where(x => x.Name.Contains(name)).ToList();
            }
            
            if (!string.IsNullOrEmpty(genre))
            {
                result = result.Where(x => x.Genre.Contains(genre)).ToList();
            }

            return Ok(result);
        }
    }
}