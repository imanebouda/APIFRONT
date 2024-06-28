using ITKANSys_api.Interfaces;
using ITKANSys_api.Models.Entities;
using ITKANSys_api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITKANSys_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConstatController : ControllerBase
    {
        private readonly IConstatService _constatService;

        public ConstatController(IConstatService constatService)
        {
            _constatService = constatService;

        }
        [HttpGet]
        public async Task<ActionResult<List<Constat>>> GetAllConstat()
        {
             return await _constatService.GetAllConstat();
           

        }

        [HttpGet("{id}")]

        public async Task<ActionResult<Constat>> GetConstat(int id)
        {
            var result = await _constatService.GetConstat(id);
            if (result is null)
                return NotFound("Constat n ' est existante");


            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Constat>>> AddConstat(Constat constat)
        {
            try
            {
                var result = await _constatService.AddConstat(constat);
                return CreatedAtAction(nameof(GetConstat), new { id = constat.ID }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{id}")]

        public async Task<ActionResult<List<Constat>>> UpdateConstat(int id, Constat request)
        {
            var result = await _constatService.UpdateConstat(id,request);
            if (result is null)
                return NotFound("constat n ' est existante");


            return Ok(result);
        }


        [HttpDelete("{id}")]

        public async Task<ActionResult<List<Constat>>> DeleteConstat(int id)
        {
            var result = await _constatService.DeleteConstat(id);
            if (result is null)
                return NotFound("Constat n ' est existante");


            return Ok(result);
        }
    }
}

