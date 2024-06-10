using ITKANSys_api.Interfaces;
using ITKANSys_api.Models.Entities;
using ITKANSys_api.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ITKANSys_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        private readonly IAuditService _auditService;

        public AuditController(IAuditService auditService)
        {
            _auditService = auditService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Audit>?>> GetAllAudit()
        {
            var audits = await _auditService.GetAllAudit();
            return Ok(audits);
        }
       

        [HttpGet("{id}")]
        public async Task<ActionResult<Audit>> GetAudit(int id)
        {
            var result = await _auditService.GetAudit(id);
            if (result is null)
                return NotFound("Audit n'existe pas");

            return Ok(result);
        }

        [HttpPost, Produces("application/json")]
        public async Task<ActionResult<Audit>> AddAudit(Audit audit)
        {
            var result = await _auditService.AddAudit(audit);
            return CreatedAtAction(nameof(GetAudit), new { id = result.ID }, result);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<Audit>> UpdateAudit(int id, Audit audit)
        {
            var result = await _auditService.UpdateAudit(id, audit);
            if (result == null)
                return NotFound("Audit n'existe pas");

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAudit(int id)
        {
            var result = await _auditService.DeleteAudit(id);
            if (result == null)
                return NotFound("Audit n'existe pas");

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<Audit>>> SearchAuditByType(int typeAuditId)
        {
            var result = await _auditService.SearchAuditByType(typeAuditId);
            if (result == null || result.Count == 0)
                return NotFound("Aucun audit trouvé pour le type spécifié.");

            return Ok(result);
        }
    }
}
