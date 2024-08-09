using ITKANSys_api.Models.Dtos;

using ITKANSys_api.Models.Entities;
using ITKANSys_api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITKANSys_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryReclamationsController : ControllerBase
    {
        private readonly IHistoryReclamationService _historyReclamationService;

        public HistoryReclamationsController(IHistoryReclamationService historyReclamationService)
        {
            _historyReclamationService = historyReclamationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HistoryReclamation>>> GetHistoryReclamations()
        {
            var historyReclamations = await _historyReclamationService.GetAllHistoryReclamationsAsync();
            return Ok(historyReclamations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HistoryReclamation>> GetHistoryReclamation(int id)
        {
            var historyReclamation = await _historyReclamationService.GetHistoryReclamationByIdAsync(id);
            if (historyReclamation == null)
            {
                return NotFound();
            }
            return Ok(historyReclamation);
        }

        [HttpPost]
        public async Task<ActionResult<HistoryReclamation>> PostHistoryReclamation(HistoryReclamationDto historyReclamationDto)
        {
            var historyReclamation = new HistoryReclamation
            {
                ReclamationID = historyReclamationDto.ReclamationID,
                Commentaire = historyReclamationDto.Commentaire,
                CreationDate = historyReclamationDto.CreationDate
            };

            await _historyReclamationService.CreateHistoryReclamationAsync(historyReclamation);
            return CreatedAtAction(nameof(GetHistoryReclamation), new { id = historyReclamation.ID }, historyReclamation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutHistoryReclamation(int id, HistoryReclamationDto historyReclamationDto)
        {
            var historyReclamation = new HistoryReclamation
            {
                ID = id,
                ReclamationID = historyReclamationDto.ReclamationID,
                Commentaire = historyReclamationDto.Commentaire,
                CreationDate = historyReclamationDto.CreationDate
            };

            var result = await _historyReclamationService.UpdateHistoryReclamationAsync(id, historyReclamation);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHistoryReclamation(int id)
        {
            await _historyReclamationService.DeleteHistoryReclamationAsync(id);
            return NoContent();
        }
    }
}

