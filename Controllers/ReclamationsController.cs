using ITKANSys_api.Services;
using ITKANSys_api.Models.Entities;
using ITKANSys_api.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ITKANSys_api.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ITKANSys_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReclamationsController : ControllerBase
    {
        private readonly IReclamationService _reclamationService;
        private readonly ApplicationDbContext _context;

        public ReclamationsController(IReclamationService reclamationService, ApplicationDbContext context)
        {
            _reclamationService = reclamationService;
            _context = context;
        }

        // GET: api/reclamations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reclamation>>> GetReclamations()
        {
            var reclamations = await _reclamationService.GetAllReclamationsAsync();
            return Ok(reclamations);
        }

        // GET: api/reclamations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reclamation>> GetReclamation(int id)
        {
            var reclamation = await _reclamationService.GetReclamationByIdAsync(id);
            if (reclamation == null)
            {
                return NotFound();
            }
            return Ok(reclamation);
        }

        // POST: api/reclamations
        [HttpPost]
        public async Task<ActionResult<Reclamation>> CreateReclamation(ReclamationDto reclamationDto)
        {
            reclamationDto.CreationDate = DateTime.ParseExact(reclamationDto.CreationDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
            var reclamation = new Reclamation
            {
                Objet = reclamationDto.Objet,
                Détail = reclamationDto.Détail,
                Analyse = reclamationDto.Analyse,
                Status = reclamationDto.Status,
                ReclamantID = reclamationDto.ReclamantID,
                ResponsableID = reclamationDto.ResponsableID,
                CreationDate = reclamationDto.CreationDate
            };

            try
            {
                var createdReclamation = await _reclamationService.CreateReclamationAsync(reclamation);
                return CreatedAtAction(nameof(GetReclamation), new { id = createdReclamation.ID }, createdReclamation);
            }
            catch (DbUpdateException ex)
            {
                // Gérer les erreurs de base de données, telles que les contraintes de clé étrangère
                return BadRequest(new { message = ex.InnerException?.Message ?? ex.Message });
            }
        }

       

        // DELETE: api/reclamations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReclamation(int id)
        {
            await _reclamationService.DeleteReclamationAsync(id);
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<Reclamation>>> SearchReclamationsAsync(string status, DateTime? creationdate)
        {
            var result = await _reclamationService.SearchReclamationsAsync(status, creationdate);
            if (result == null || result.Count == 0)
                return NotFound("Aucune réclamation trouvée pour le statut spécifié.");

            return Ok(result);
        }

        [HttpPost("{id}/defineComite")]
        public async Task<IActionResult> DefineComite(int id, [FromBody] ComiteeReclamationDto comiteeReclamationDto)
        {
            var reclamation = await _reclamationService.GetReclamationByIdAsync(id);
            if (reclamation == null)
            {
                return NotFound();
            }

            foreach (var roleId in comiteeReclamationDto.UserIds)
            {
                var comiteReclamation = new ComiteeReclamation
                {
                    ReclamationID = id,
                    ConcernedID = roleId,
                    CreationDate = DateTime.UtcNow
                };
                await _reclamationService.AddComiteeReclamationAsync(comiteReclamation);
            }

            return NoContent();
        }

        [HttpGet("getReclamationsWithReclamant")]
        public async Task<ActionResult<List<Reclamation>>> GetReclamationsWithReclamant()
        {
            var result = await _reclamationService.GetReclamationsWithReclamant();
            if (result == null || result.Count == 0)
                return NotFound("Aucune réclamation trouvée.");

            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReclamation(int id, ReclamationDto reclamationDto)
        {
            Console.WriteLine("Received Reclamation DTO: ", JsonConvert.SerializeObject(reclamationDto));

            if (id != reclamationDto.Id)
            {
                return BadRequest("Reclamation ID mismatch");
            }

            var reclamation = await _reclamationService.GetReclamationByIdAsync(id);
            if (reclamation == null)
            {
                return NotFound();
            }

            reclamation.Objet = reclamationDto.Objet;
            reclamation.Détail = reclamationDto.Détail;
            reclamation.Analyse = reclamationDto.Analyse;
            reclamation.Status = reclamationDto.Status;
            reclamation.ReclamantID = reclamationDto.ReclamantID;
            reclamation.ResponsableID = reclamationDto.ResponsableID;
            reclamation.CreationDate = reclamationDto.CreationDate;

            try
            {
                await _reclamationService.UpdateReclamationAsync(reclamation);
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new { message = ex.InnerException?.Message ?? ex.Message });
            }
        }
        [HttpGet("details/{id}")]
        public async Task<ActionResult> GetReclamationDetails(int id)
        {
            var reclamation = await _context.Reclamations
                .Include(r => r.Reclamant)
                .Include(r => r.userId)
                .Where(r => r.ID == id)
                .Select(r => new
                {
                    r.ID,
                    r.Objet,
                    r.Détail,
                    r.Analyse,
                    r.Status,
                    ReclamantNom = r.Reclamant.Nom,
                    ReclamantPrenom = r.Reclamant.Prénom,
                    ResponsableNomComplet = r.userId.NomCompletUtilisateur,
                    r.CreationDate
                })
                .FirstOrDefaultAsync();

            if (reclamation == null)
            {
                return NotFound();
            }

            return Ok(reclamation);
        }


    }
}
