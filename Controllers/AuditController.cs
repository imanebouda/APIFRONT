﻿
using Azure.Core;
using ITKANSys_api.Data.Services;
using ITKANSys_api.Interfaces;
using ITKANSys_api.Models.Dtos;
using ITKANSys_api.Models.Entities;
using ITKANSys_api.Services;
using ITKANSys_api.Utility.ApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ITKANSys_api.Controllers
{//1
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

           return  await _auditService.GetAllAudit();
           

        }

        [HttpGet("{id}")]
        
        public async Task<ActionResult<Audit>> GetAudit(int id)
        {
            var result = await _auditService.GetAudit(id);
            if (result is null)
                return NotFound("Audit n ' est existante");


            return Ok(result);
        }

        [HttpPost , Produces("application/json")]

        public async Task<ActionResult<CheckListAudit>> AddAudit(Audit audit)
        {
            var result = await _auditService.AddAudit(audit);
            return CreatedAtAction(nameof(GetAudit), new { id = audit.ID }, audit);

        }

       
        [HttpPut("{id}")]
        //Task<DataSourceResult> UpdateAudit(int id, object audit)
        public async Task<object> UpdateAudit(int id, [FromBody] object audit)
        {
            DataSourceResult dataResult = new DataSourceResult();

            //var result = _auditService.UpdateAudit(id,request);
            try
            {
                string jsonString = audit.ToString();
                jsonString = jsonString.Replace("ValueKind = Object : ", "");

                if (string.IsNullOrEmpty(jsonString))
                {
                    dataResult.codeReponse = CodeReponse.errorMissingAllParams;
                }
                else
                {
                  //  dataResult = await _auditService.UpdateAudit(id, jsonString);
                }
            }
            catch (Exception ex)
            {
                dataResult.codeReponse = CodeReponse.error;
                dataResult.msg = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
            }
            finally
            {
                // Libérez les ressources ici si nécessaire.
            }

            return JsonConvert.SerializeObject(dataResult);


        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAudit(int id)
        {
            var result = await _auditService.DeleteAudit(id);
            if (result == null)
                return NotFound("Audit not found");

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<Audit>>> SearchAuditByType(int typeAuditId)
        {
            var result = await _auditService.searchAuditByType(typeAuditId);
            if (result == null || result.Count == 0)
                return NotFound("No audit found for the specified type.");

            return Ok(result);
        }


    }
}
