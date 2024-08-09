using ITKANSys_api.Models.Entities;
using ITKANSys_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITKANSys_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReclamantsController : ControllerBase
    {
        private readonly ReclamantService _service;
        private readonly ApplicationDbContext _context;

        public ReclamantsController(ReclamantService service, ApplicationDbContext context)
        {
            _service = service;
            _context = context;
        }

        // GET: api/Reclamants
        [HttpGet]
        public async Task<ActionResult<List<Reclamant>>> GetReclamants()
        {
            var reclamants = await _service.GetAllReclamantsAsync();
            return Ok(reclamants);
        }

        // GET: api/Reclamants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reclamant>> GetReclamant(int id)
        {
            var reclamant = await _service.GetReclamantByIdAsync(id);
            if (reclamant == null)
            {
                return NotFound();
            }
            return reclamant;
        }

        // POST: api/Reclamants
        [HttpPost]
        public async Task<ActionResult<Reclamant>> PostReclamant(Reclamant reclamant)
        {
            await _service.CreateReclamantAsync(reclamant);
            return CreatedAtAction(nameof(GetReclamant), new { id = reclamant.Id }, reclamant);
        }

        // PUT: api/Reclamants/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReclamant(int id, [FromBody] Reclamant updatedReclamant)
        {
            if (id != updatedReclamant.Id)
            {
                return BadRequest("Reclamant ID mismatch.");
            }

            var existingReclamant = await _context.Reclamants.FindAsync(id);
            if (existingReclamant == null)
            {
                return NotFound();
            }

            // Update properties
            existingReclamant.Nom = updatedReclamant.Nom;
            existingReclamant.Prénom = updatedReclamant.Prénom;
            existingReclamant.Email = updatedReclamant.Email;
            existingReclamant.Mobile = updatedReclamant.Mobile;
            existingReclamant.Adresse = updatedReclamant.Adresse;
            existingReclamant.Ville = updatedReclamant.Ville;
            existingReclamant.CreationDate = updatedReclamant.CreationDate;

            _context.Entry(existingReclamant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReclamantExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        private bool ReclamantExists(int id)
        {
            return _context.Reclamants.Any(e => e.Id == id);
        }




        // DELETE: api/Reclamants/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReclamant(int id)
        {
            await _service.DeleteReclamantAsync(id);
            return NoContent();
        }
    }
}
