using System.Collections;
using System.Threading.Tasks;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Abstracts
{
    public abstract class BasicController<T> : Controller where T : IEntity
    {
        [NonAction]
        protected abstract IBasicLogic<T> BasicLogic();

        [HttpGet]
        [Route("")]
        [SwaggerOperation("GetAll")]
        [ProducesResponseType(typeof(IEnumerable), 200)]
        public virtual async Task<IActionResult> GetAll()
        {
            return Ok(BasicLogic().GetAll());
        }

        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation("Get")]
        public virtual async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(BasicLogic().Get(id));
        }

        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation("Update")]
        public virtual async Task<IActionResult> Update([FromRoute] int id, [FromBody] T instance)
        {
            return Ok(BasicLogic().Update(id, instance));
        }

        [HttpDelete]
        [Route("{id}")]
        [SwaggerOperation("Delete")]
        public virtual async Task<IActionResult> Delete([FromRoute] int id)
        {
            return Ok(BasicLogic().Delete(id));
        }
        
        [HttpPost]
        [Route("")]
        [SwaggerOperation("Save")]
        public virtual async Task<IActionResult> Save([FromBody] T instance)
        {
            return Ok(BasicLogic().Save(instance));
        }
    }
}