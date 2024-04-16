﻿using ITKANSys_api.Interfaces;
using ITKANSys_api.Services.Param;
using ITKANSys_api.Utility.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace ITKANSys_api.Controllers.Param
{
    [Route("api/[controller]")]
    [ApiController]
    public class SitesController : ControllerBase
    {
        private readonly ISiteService _sitesService;

        public SitesController(ISiteService sitesService)
        {
            _sitesService = sitesService;
        }

        [HttpPost, Route("Search"), Produces("application/json")]
        public async Task<object> Search(Object record)
        {
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                record = record.ToString().Replace("ValueKind = Object : ", "");

                if (record == null || record.ToString().Length == 0)
                {
                    dataResult.codeReponse = CodeReponse.error;
                    dataResult.msg = "Paramètres manquants";
                }
                else
                {
                    dataResult = await _sitesService.Search(record);
                }
            }
            catch (Exception ex)
            {
                dataResult.codeReponse = CodeReponse.error;
                dataResult.msg = (ex.InnerException == null) ? ex.Message : ex.InnerException.Message;
            }

            return JsonConvert.SerializeObject(dataResult);
        }

        [HttpPost, Route("Insert"), Produces("application/json")]
        public async Task<object> Insert(Object record)
        {
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                record = record.ToString().Replace("ValueKind = Object : ", "");

                if (record == null || record.ToString().Length == 0)
                {
                    dataResult.codeReponse = CodeReponse.error;
                    dataResult.msg = "Paramètres manquants";
                }
                else
                {
                    dataResult = await _sitesService.Insert(record);
                }
            }
            catch (Exception ex)
            {
                dataResult.codeReponse = CodeReponse.error;
                dataResult.msg = (ex.InnerException == null) ? ex.Message : ex.InnerException.Message;
            }

            return JsonConvert.SerializeObject(dataResult);
        }

        [HttpPost, Route("Update"), Produces("application/json")]
        public async Task<object> Update(Object record)
        {
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                record = record.ToString().Replace("ValueKind = Object : ", "");

                if (record == null || record.ToString().Length == 0)
                {
                    dataResult.codeReponse = CodeReponse.error;
                    dataResult.msg = "Paramètres manquants";
                }
                else
                {
                    dataResult = await _sitesService.Update(record);
                }
            }
            catch (Exception ex)
            {
                dataResult.codeReponse = CodeReponse.error;
                dataResult.msg = (ex.InnerException == null) ? ex.Message : ex.InnerException.Message;
            }

            return JsonConvert.SerializeObject(dataResult);
        }

        [HttpPost, Route("Delete"), Produces("application/json")]
        public async Task<object> Delete(Object record)
        {
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                record = record.ToString().Replace("ValueKind = Object : ", "");

                if (record == null || record.ToString().Length == 0)
                {
                    dataResult.codeReponse = CodeReponse.error;
                    dataResult.msg = "Paramètres manquants";
                }
                else
                {
                    dataResult = await _sitesService.Delete(record);
                }
            }
            catch (Exception ex)
            {
                dataResult.codeReponse = CodeReponse.error;
                dataResult.msg = (ex.InnerException == null) ? ex.Message : ex.InnerException.Message;
            }

            return JsonConvert.SerializeObject(dataResult);
        }
    }
}
