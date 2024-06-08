using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ITKANSys_api.Models.Entities;
using ITKANSys_api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ITKANSys_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeAuditController : ControllerBase
    {
        private readonly ITypeAuditService _typeAuditService;

        public TypeAuditController(ITypeAuditService typeAuditService)
        {
            _typeAuditService = typeAuditService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TypeAudit>>> GetAllTypeAudits()
        {
            var typeAudits = await _typeAuditService.GetAllTypeAudits();
            return Ok(typeAudits);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TypeAudit>> GetTypeAudit(int id)
        {
            var typeAudit = await _typeAuditService.GetTypeAudit(id);
            if (typeAudit == null)
            {
                return NotFound();
            }
            return Ok(typeAudit);
        }

        [HttpPost]
        public async Task<ActionResult<TypeAudit>> AddTypeAudit(TypeAudit typeAudit)
        {
            var addedTypeAudit = await _typeAuditService.AddTypeAudit(typeAudit);
            return Ok(addedTypeAudit);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTypeAudit(int id, TypeAudit typeAudit)
        {
            if (id != typeAudit.id)
            {
                return BadRequest();
            }
            var updatedTypeAudit = await _typeAuditService.UpdateTypeAudit(id, typeAudit);
            if (updatedTypeAudit == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTypeAudit(int id)
        {
            var deletedTypeAudit = await _typeAuditService.DeleteTypeAudit(id);
            if (deletedTypeAudit == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
