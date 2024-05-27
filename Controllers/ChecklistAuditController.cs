using Microsoft.AspNetCore.Mvc;
using ITKANSys_api.Models.Entities;
using ITKANSys_api.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITKANSys_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChecklistAuditController : ControllerBase
    {
        private readonly IChecklistAuditService _checklistAuditService;

        public ChecklistAuditController(IChecklistAuditService checklistAuditService)
        {
            _checklistAuditService = checklistAuditService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CheckListAudit>>> GetAllChecklistAudits()
        {
            var checkListAudits = await _checklistAuditService.GetAllCheckListAudit();
            return Ok(checkListAudits);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CheckListAudit>> GetSingleChecklistAudit(int id)
        {
            var result = await _checklistAuditService.GetCheckListAudit(id);
            if (result == null)
                return NotFound("ChecklistAudit not found");

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<CheckListAudit>> AddChecklistAudit(CheckListAudit checklistAudit)
        {
            var result = await _checklistAuditService.AddCheckListAudit(checklistAudit);
            return CreatedAtAction(nameof(GetSingleChecklistAudit), new { id = checklistAudit.id }, checklistAudit);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CheckListAudit>> UpdateChecklistAudit(int id, CheckListAudit request)
        {
            var result = await _checklistAuditService.UpdateCheckListAudit(id, request);
            if (result == null)
                return NotFound("ChecklistAudit not found");

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteChecklistAudit(int id)
        {
            var result = await _checklistAuditService.DeleteCheckListAudit(id);
            if (result == null)
                return NotFound("ChecklistAudit not found");

            return NoContent();
        }
        [HttpGet("search")]
        public async Task<ActionResult<List<CheckListAudit>>> SearchChecklistByType(int typeChecklistId)
        {
            var result = await _checklistAuditService.SearchChecklistByType(typeChecklistId);
            if (result == null || result.Count == 0)
                return NotFound("No checklists found for the specified type.");

            return Ok(result);
        }
    }
}
