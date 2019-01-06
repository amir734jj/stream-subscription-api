using System.Threading.Tasks;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Controllers.Api
{
    [Authorize]
    [Route("api/[controller]")]
    public class StreamRipperManagementController : Controller
    {
        private readonly IStreamRipperManagement _streamRipperManagement;
        
        /// <summary>
        /// Constructor dependency injection
        /// </summary>
        /// <param name="streamRipperManagement"></param>
        public StreamRipperManagementController(IStreamRipperManagement streamRipperManagement) => _streamRipperManagement = streamRipperManagement;
        
        [HttpGet]
        [Route("status")]
        [SwaggerOperation("Status")]
        public async Task<IActionResult> Status()
        {
            return Ok(_streamRipperManagement.Status());
        }
        
        [HttpGet]
        [Route("start/{id}")]
        [SwaggerOperation("Start")]
        public async Task<IActionResult> Start([FromRoute] int id)
        {
            return Ok(_streamRipperManagement.Start(id));
        }
        
        [HttpGet]
        [Route("stop/{id}")]
        [SwaggerOperation("Stop")]
        public async Task<IActionResult> Stop([FromRoute] int id)
        {
            return Ok(_streamRipperManagement.Stop(id));
        }
    }
}