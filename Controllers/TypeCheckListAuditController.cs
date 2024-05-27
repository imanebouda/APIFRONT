using ITKANSys_api.Utility.ApiResponse;
using ITKANSys_api.Utility.Other;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using System.Reflection;
using System;
using ITKANSys_api.Config;
using ITKANSys_api.Services.Gestions;
using ITKANSys_api.Models;

using ITKANSys_api.Models.Entities;
using Azure.Core;
using ITKANSys_api.Interfaces;
using ITKANSys_api.Services;


namespace ITKANSys_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeCheckListAuditController : ControllerBase
    {
        private readonly ITypeCheckListAuditService _TypeChecklistAuditService;

        public TypeCheckListAuditController(ITypeCheckListAuditService typeCheckListAudits)
        {
            _TypeChecklistAuditService = typeCheckListAudits;
        }

        [HttpGet]
        public async Task<ActionResult<List<TypeCheckListAudit>>> getAllTypeChecklistAudits()
        {
            // return _ChecklistAuditService.GetAllCheckListAudit();
            return await _TypeChecklistAuditService.getAllTypeCheckListAudit();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<TypeCheckListAudit>> getSingleTypeChecklistAudit(int id)
        {
            var result = await _TypeChecklistAuditService.getTypeCheckListAudit(id);
            if (result is null)
                return NotFound("TypeChecklistAudit not found");

            return Ok(result);
        }


        [HttpPost]
        public async Task<ActionResult<List<TypeCheckListAudit>>> addTypeChecklistAudit(TypeCheckListAudit typeCheckListAudit)
        {
            var result = _TypeChecklistAuditService.addTypeCheckListAudit(typeCheckListAudit);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<TypeCheckListAudit>>> updateTypeChecklistAudit(int id, TypeCheckListAudit request)
        {
            var result = await _TypeChecklistAuditService.updateTypeCheckListAudit(id, request);
            if (result is null)
                return NotFound("TypeChecklistAudit not found");

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<TypeCheckListAudit>>> deleteTypeChecklistAudit(int id)
        {
            var result = await _TypeChecklistAuditService.deleteTypeCheckListAudit(id);
            if (result is null)
                return NotFound("TypeChecklistAudit not found");

            return Ok(result);
        }
    }
}

