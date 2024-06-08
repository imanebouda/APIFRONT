using ITKANSys_api.Interfaces;
using ITKANSys_api.Models.Entities;
using ITKANSys_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITKANSys_api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProgrammeAuditController : Controller
    {
        private readonly IProgrammeAudit _programmeauditService;

        public ProgrammeAuditController(IProgrammeAudit programmeauditService)
        {
            _programmeauditService = programmeauditService;

        }


        [HttpGet("programme-audits/{auditId}")]
        public async Task<ActionResult<List<ProgrammeAudit>>> GetProgrammeAuditsForAudit(int auditId)
        {
            var programmeAudits = await _programmeauditService.GetProgrammeAuditsForAudit(auditId);
            if (programmeAudits == null)
            {
                return NotFound();
            }
            return programmeAudits;
        }



        [HttpGet("programme-audits")]
        public async Task<ActionResult<List<ProgrammeAudit>>> GetProgrammeAudits()
        {
            var programmeAudits = await _programmeauditService.GetProgrammeAudits();
            return Ok(programmeAudits);
        }
    }
}
