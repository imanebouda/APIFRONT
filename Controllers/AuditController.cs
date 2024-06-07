
using Azure.Core;
using ITKANSys_api.Data.Services;
using ITKANSys_api.Interfaces;
using ITKANSys_api.Models.Dtos;
using ITKANSys_api.Models.Entities;
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

            return await _auditService.GetAllAudit();


        }

        [HttpGet("{id}")]

        public async Task<ActionResult<Audit>> GetAudit(int id)
        {
            var result = await _auditService.GetAudit(id);
            if (result is null)
                return NotFound("Audit n ' est existante");


            return Ok(result);
        }

        [HttpPost, Produces("application/json")]

        public async Task<object> AddAudit([FromBody] object audit)
        {
            DataSourceResult dataResult = new DataSourceResult();

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
                    dataResult = await _auditService.AddAudit(jsonString);
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

        /*[HttpPut("{id}")]

        public async Task<DataSourceResult> UpdateAudit(int id, object audit)
        {
            var result = _auditService.UpdateAudit(id,request);
            if (result is null)
                    return NotFound("Audit n ' est existante");


            return Ok(result);
        }
        */
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
                    dataResult = await _auditService.UpdateAudit(id, jsonString);
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
        //public async Task<object> DeleteAudit(int id)

        //public async Task<object> DeleteAudit([FromBody] object audit)


        /*
        [HttpPost, Route("Delete"), Produces("application/json")]
        public async Task<object> DeleteAudit([FromBody] object auditData)
        {
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                string jsonString = auditData.ToString();
                jsonString = jsonString.Replace("ValueKind = Object : ", "");

                if (string.IsNullOrEmpty(jsonString))
                {
                    dataResult.codeReponse = CodeReponse.errorMissingAllParams;
                }
                else
                {
                    dataResult = await _auditService.DeleteAudit(jsonString);
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
        }*/
        [HttpPost]
        [Route("DeleteAudit")]
        [Produces("application/json")]
        public async Task<object> DeleteAudit([FromBody] object auditData)
        {
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                string jsonString = auditData.ToString();
                jsonString = jsonString.Replace("ValueKind = Object : ", "");

                if (string.IsNullOrEmpty(jsonString))
                {
                    dataResult.codeReponse = CodeReponse.errorMissingAllParams;
                }
                else
                {
                    dataResult = await _auditService.DeleteAudit(jsonString);
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
        [HttpGet("byDate")]
        public async Task<ActionResult<List<Audit>>> GetAuditsByDate([FromQuery] DateTime date)
        {
            try
            {
                var audits = await _auditService.GetAuditsByDate(date);
                return Ok(audits);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une exception s'est produite : {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("byType")]
        public async Task<ActionResult<List<Audit>>> GetAuditsByType([FromQuery] string type)
        {
            try
            {
                var audits = await _auditService.GetAuditsByType(type);
                return Ok(audits);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une exception s'est produite : {ex.Message}");
                return StatusCode(500, "Internal server error");
            }


            /*2
            [HttpPost, Route("DeleteAudit"), Produces("application/json")]
            public async Task<object> DeleteAudit([FromBody] object auditData)
            {
                DataSourceResult dataResult = new DataSourceResult();

                try
                {
                    string jsonString = auditData.ToString();
                    jsonString = jsonString.Replace("ValueKind = Object : ", "");

                    if (string.IsNullOrEmpty(jsonString))
                    {
                        dataResult.codeReponse = CodeReponse.errorMissingAllParams;
                    }
                    else
                    {
                        dataResult = await _auditService.DeleteAudit(jsonString);
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
            }*/


            /*
            [HttpDelete, Route("Audit/Delete/{id}"), Produces("application/json")]
            public async Task<object> DeleteAudit(int id)
            {
                DataSourceResult dataResult = new DataSourceResult();

                try
                {
                    var auditToDelete = await _auditService.DeleteAudit(id);
                    return auditToDelete; // ou tout autre traitement nécessaire ici
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
            }*/
            /*8
            [HttpPost, Route("Audit/Delete"), Produces("application/json")]
            public async Task<object> DeleteAudit([FromBody] dynamic auditData)
            {
                DataSourceResult dataResult = new DataSourceResult();

                try
                {
                    int id = auditData.id;
                    if (id > 0)
                    {
                        dataResult = await _auditService.DeleteAudit(id);
                    }
                    else
                    {
                        dataResult.codeReponse = CodeReponse.error;
                        dataResult.msg = "ID de l'audit invalide.";
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
            }*/


        }
    }
}
