using ITKANSys_api.Interfaces;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Threading.Tasks;


namespace ITKANSys_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActionController : Controller
    {
        private readonly IActionService _actionService;

        public ActionController(IActionService actionService)
        {
            _actionService = actionService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Models.Entities.Action>>> GetAllActions()
        {
            var actions = await _actionService.GetAllActions();
            return Ok(actions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Entities.Action>> GetAction(int id)
        {
            var result = await _actionService.GetAction(id);
            if (result is null)
                return NotFound("Action n'existe pas");

            return Ok(result);
        }

        [HttpPost, Produces("application/json")]
        public async Task<ActionResult<Models.Entities.Action>> AddAction(Models.Entities.Action action)
        {
            var result = await _actionService.AddAction(action);
            return CreatedAtAction(nameof(GetAction), new { id = result.ID }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Models.Entities.Action>> UpdateAction(int id,
           Models.Entities.Action action)
        {
            var result = await _actionService.UpdateAction(id, action);
            if (result == null)
                return NotFound("Action n'existe pas");

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAction(int id)
        {
            var result = await _actionService.DeleteAction(id);
            if (result == null)
                return NotFound("Action n'existe pas");

            return NoContent();
        }

      
    }
}
