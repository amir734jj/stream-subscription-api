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
        [Route("genres")]
        public IActionResult Genres()
        {
            var genres = _shoutcastDirectoryApi.Result.Select(x => x.Genre).Distinct().ToList();

            return Ok(genres);
        }
        
        [HttpGet]
        [Route("")]
        public IActionResult Collect([FromQuery] string name = "", [FromQuery] string genre = "")
        {
            var result = _shoutcastDirectoryApi.Result;

            if (!string.IsNullOrEmpty(name))
            {
                result = result.Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            
            if (!string.IsNullOrEmpty(genre))
            {
                result = result.Where(x => x.Genre.Contains(genre, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            result = result.ToList();

            return Ok(result);
        }
    }
}