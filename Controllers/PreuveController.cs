using ITKANSys_api.Interfaces;
using ITKANSys_api.Models.Entities;
using ITKANSys_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITKANSys_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreuveController : ControllerBase
    {
        private readonly IPreuve _preuveService;

        public PreuveController(IPreuve preuveService)
        {
            _preuveService = preuveService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Preuve>>> GetAllPreuves()
        {
            var preuves = await _preuveService.GetAllPreuves();
            return Ok(preuves);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Preuve>> GetPreuve(int id)
        {
            var result = await _preuveService.GetPreuve(id);
            if (result == null)
                return NotFound("Preuve n'existe pas");

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Preuve>> AddPreuve(Preuve preuve)
        {
            var result = await _preuveService.AddPreuve(preuve);
            return CreatedAtAction(nameof(GetPreuve), new { id = result.ID }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Preuve>> UpdatePreuve(int id, Preuve preuve)
        {
            var result = await _preuveService.UpdatePreuve(id, preuve);
            if (result == null)
                return NotFound("Preuve n'existe pas");

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePreuve(int id)
        {
            var result = await _preuveService.DeletePreuve(id);
            if (result == null)
                return NotFound("Preuve n'existe pas");

            return NoContent();
        }
    }
}
