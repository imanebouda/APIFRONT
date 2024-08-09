using ITKANSys_api.Models;
using ITKANSys_api.Models.Dtos;

using ITKANSys_api.Models.Entities;
using ITKANSys_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITKANSys_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComiteeReclamationsController : ControllerBase
    {
        private readonly IComiteeReclamationService _comiteeReclamationService;
        private readonly ApplicationDbContext _context;
        private readonly IReclamationService _reclamationService;

        public ComiteeReclamationsController(IComiteeReclamationService comiteeReclamationService, ApplicationDbContext context, IReclamationService reclamationService)
        {
            _comiteeReclamationService = comiteeReclamationService;
            _context = context;
            _reclamationService = reclamationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComiteeReclamation>>> GetComiteeReclamations()
        {
            var comiteeReclamations = await _comiteeReclamationService.GetAllComiteeReclamationsAsync();
            return Ok(comiteeReclamations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ComiteeReclamation>> GetComiteeReclamation(int id)
        {
            var comiteeReclamation = await _comiteeReclamationService.GetComiteeReclamationByIdAsync(id);
            if (comiteeReclamation == null)
            {
                return NotFound();
            }
            return Ok(comiteeReclamation);
        }

        [HttpPost]
        public async Task<ActionResult<ComiteeReclamation>> PostComiteeReclamation(ComiteeReclamationDto comiteeReclamationDto)
        {
            var comiteeReclamation = new ComiteeReclamation
            {
                ReclamationID = comiteeReclamationDto.ReclamationID,
                ConcernedID = comiteeReclamationDto.ConcernedID,
                CreationDate = comiteeReclamationDto.CreationDate
            };

            await _comiteeReclamationService.CreateComiteeReclamationAsync(comiteeReclamation);
            return CreatedAtAction(nameof(GetComiteeReclamation), new { id = comiteeReclamation.ID }, comiteeReclamation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutComiteeReclamation(int id, ComiteeReclamationDto comiteeReclamationDto)
        {
            var comiteeReclamation = new ComiteeReclamation
            {
                ID = id,
                ReclamationID = comiteeReclamationDto.ReclamationID,
                ConcernedID = comiteeReclamationDto.ConcernedID,
                CreationDate = comiteeReclamationDto.CreationDate
            };

            var result = await _comiteeReclamationService.UpdateComiteeReclamationAsync(id, comiteeReclamation);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComiteeReclamation(int id)
        {
            await _comiteeReclamationService.DeleteComiteeReclamationAsync(id);
            return NoContent();
        }

        [HttpGet("GetConcernedUsers/{roleId}")]
        public async Task<IEnumerable<User>> GetConcernedUsers(int roleId)
        {
            return await _comiteeReclamationService.GetConcernedUsersAsync(roleId);
        }
        [HttpPost("{id}/defineComite")]
        public async Task<IActionResult> DefineComite(int id, [FromBody] ComiteeReclamationDto comiteeReclamationDto)
        {
            var reclamation = await _reclamationService.GetReclamationByIdAsync(id);
            if (reclamation == null)
            {
                return NotFound();
            }

            var existingComitee = await _context.ComiteeReclamation
                .Where(c => c.ReclamationID == id)
                .ToListAsync();
            _context.ComiteeReclamation.RemoveRange(existingComitee);

            foreach (var userId in comiteeReclamationDto.UserIds)
            {
                var comiteeReclamation = new ComiteeReclamation
                {
                    ReclamationID = id,
                    ConcernedID = userId,
                    CreationDate = DateTime.Now
                };
                _context.ComiteeReclamation.Add(comiteeReclamation);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpGet("{id}/concernedUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetConcernedUser(int id)
        {
            var users = await _comiteeReclamationService.GetConcernedUserAsync(id);
            if (users == null || !users.Any())
            {
                return NotFound();
            }
            return Ok(users);
        }

        [HttpDelete("{reclamationId}/removeUser/{userId}")]
        public async Task<IActionResult> RemoveUserFromComitee(int reclamationId, int userId)
        {
            var comiteeReclamation = await _context.ComiteeReclamation
                .FirstOrDefaultAsync(c => c.ReclamationID == reclamationId && c.ConcernedID == userId);

            if (comiteeReclamation == null)
            {
                return NotFound();
            }

            _context.ComiteeReclamation.Remove(comiteeReclamation);
            await _context.SaveChangesAsync();

            return NoContent();
        }



    }



}
