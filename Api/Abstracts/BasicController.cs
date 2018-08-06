using System.Collections;
using System.Threading.Tasks;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Abstracts
{
    public abstract class BasicController<T> : Controller
    {
        [NonAction]
        public abstract IBasicLogic<T> BasicLogic();

        [HttpGet]
        [Route("")]
        [SwaggerOperation("GetAll")]
        [ProducesResponseType(typeof(IEnumerable), 200)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(BasicLogic().GetAll());
        }

        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation("Get")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(BasicLogic().Get(id));
        }

        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation("Update")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] T instance)
        {
            return Ok(BasicLogic().Update(id, instance));
        }

        [HttpDelete]
        [Route("{id}")]
        [SwaggerOperation("Delete")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return Ok(BasicLogic().Delete(id));
        }
        
        [HttpPost]
        [Route("")]
        [SwaggerOperation("Save")]
        public async Task<IActionResult> Save([FromBody] T instance)
        {
            return Ok(BasicLogic().Save(instance));
        }
    }
}