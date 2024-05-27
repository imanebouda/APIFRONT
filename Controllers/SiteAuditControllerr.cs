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

namespace ITKANSys_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteAuditController : ControllerBase
    {
        private readonly ISiteAuditService _SiteAuditService;

        public SiteAuditController(ISiteAuditService SiteAudiService)
        {
            _SiteAuditService = SiteAudiService;
        }

        [HttpGet]
        public async Task<ActionResult<List<SiteAudit>>> getAllSiteAudits()
        {
            return await _SiteAuditService.getAllSiteAudits();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SiteAudit>> getSingleSiteAudit(int id)
        {
            var result = await _SiteAuditService.getSiteAudit(id);
            if (result is null)
                return NotFound("SiteAudit not found");

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<List<SiteAudit>>> addSiteAudit(SiteAudit siteAudit)

        {
            var result = await _SiteAuditService.addSiteAudit(siteAudit);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<SiteAudit>>> updateSiteAudit(int id, SiteAudit request)
        {
            var result = await _SiteAuditService.updateSiteAudit(id, request);
            if (result is null)
                return NotFound("SiteAudit not found");

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<SiteAudit>>> deleteSiteAudit(int id)
        {
            var result = await _SiteAuditService.deleteSiteAudit(id);
            if (result is null)
                return NotFound("SiteAudit not found");

            return Ok(result);
        }
    }
}
