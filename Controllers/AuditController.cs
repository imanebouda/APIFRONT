
using Azure.Core;
using ITKANSys_api.Interfaces;
using ITKANSys_api.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<List<Audit>>> GetAllAudit()
        {

           return _auditService.GetAllAudit();
           

        }

        [HttpGet("{id}")]
        
        public async Task<ActionResult<Audit>> GetAudit(int id)
        {
            var result = _auditService.GetAudit(id);
            if (result is null)
                return NotFound("Audit n ' est existante");


            return Ok(result);
        }

        [HttpPost]

        public async Task<ActionResult<List<Audit>>> AddAudit(Audit audit)
        {

            var result = _auditService.AddAudit(audit);
            if (result is null)
                return NotFound("Audit n ' est existante");


            return Ok(result);
        }


        [HttpPut("{id}")]

        public async Task<ActionResult<List<Audit>>> UpdateAudit(int id ,Audit request)
        {
            var result = _auditService.UpdateAudit(id,request);
            if (result is null)
                    return NotFound("Audit n ' est existante");


            return Ok(result);
        }


        [HttpDelete("{id}")]

        public async Task<ActionResult<List<Audit>>> DeleteAudit(int id)
        {
            var result = _auditService.DeleteAudit(id);
            if(result is null)
                    return NotFound("Audit n ' est existante");

          
            return Ok(result);
        }
    }
}
