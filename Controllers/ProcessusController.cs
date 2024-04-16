using ITKANSys_api.Data.Dtos;
using ITKANSys_api.Interfaces;
using ITKANSys_api.Models;
using ITKANSys_api.Services;
using ITKANSys_api.Utility.ApiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using System.Reflection;
using static ITKANSys_api.Utility.ApiResponse.QueryableExtensions;


namespace ITKANSys_api.Controllers
{
    [Route("api/processus")]
    public class ProcessusController : ControllerBase
    {
        private readonly IProcessusServices _processusService;

        public ProcessusController(IProcessusServices processusService)
        {
            _processusService = processusService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProcessus()
        {
            try
            {
                var result = await _processusService.GetAll();
                if (result.IsSucceed == true)
                {
                    return Ok(result.data);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [HttpGet("GetProcessusData")]
        public async Task<object> GetProcessusData()
        {
  
            DataSourceResult dataResult = new DataSourceResult();
            dataResult =  await _processusService.GetProcessusData();
            if (dataResult.IsSucceed == true)
            {
                return JsonConvert.SerializeObject(dataResult);
            }
            else
            {
                // Gérez les autres codes de réponse comme vous le souhaitez
                if (dataResult.IsSucceed == false)
                {
                    // Loggez l'erreur ici si nécessaire
                    return JsonConvert.SerializeObject(dataResult);
                }
                else
                {
                    return JsonConvert.SerializeObject(dataResult);
                }
            }
        }
        
        [HttpGet("GetProcessusByPilote")]
         public async Task<object> GetProcessusByPiloteOrCoPilote(int pilote , int coPilote)
        {

            DataSourceResult dataResult = new DataSourceResult();
            dataResult = await _processusService.GetProcessusByPiloteOrCoPilote(pilote ,coPilote);
            if (dataResult.IsSucceed == true)
            {
                return JsonConvert.SerializeObject(dataResult);
            }
            else
            {
                // Gérez les autres codes de réponse comme vous le souhaitez
                if (dataResult.IsSucceed == false)
                {
                    // Loggez l'erreur ici si nécessaire
                    return JsonConvert.SerializeObject(dataResult);
                }
                else
                {
                    return JsonConvert.SerializeObject(dataResult);
                }
            }
        }


        [HttpGet("recherche")]
        public async Task<object> RechercheProcessus(string code, string libelle, DateTime? dateDebut, DateTime? dateFin, int categorie, string field, string order, int take, int skip)
        {
            DataSourceResult dataResult = new DataSourceResult();
            try
            {
                 dataResult = await _processusService.RechercheProcessus(code, libelle, dateDebut, dateFin, categorie, field, order, take,  skip);

                if (dataResult.IsSucceed == true)
                {
                    return JsonConvert.SerializeObject(dataResult);
                }
                else
                {
                    // Gérez les autres codes de réponse comme vous le souhaitez
                    if (dataResult.IsSucceed ==  false)
                    {
                        // Loggez l'erreur ici si nécessaire
                        return JsonConvert.SerializeObject(dataResult);
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(dataResult);
                    }
                }
            }
            catch (Exception ex)
            {
                dataResult.IsSucceed = false;
                dataResult.Message = "data notfound";
                // Loggez l'exception ici si nécessaire
                return JsonConvert.SerializeObject(dataResult);
            }
        }

        [HttpPost, Route("ajouter"), Produces("application/json")]
        public async Task<object> AjouterProcessus([FromBody] object insertProcessus)
        {
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                string jsonString = insertProcessus.ToString();
                jsonString = jsonString.Replace("ValueKind = Object : ", "");

                if (string.IsNullOrEmpty(jsonString))
                {
                    dataResult.codeReponse = CodeReponse.errorMissingAllParams;
                }
                else
                {
                    dataResult = await _processusService.InsertProcessus(jsonString);
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


        [HttpPut, Route("update"), Produces("application/json")]
        public async Task<object> UpdateProcessus([FromBody] object processusData)
        {
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                string jsonString = processusData.ToString();
                jsonString = jsonString.Replace("ValueKind = Object : ", "");

                if (string.IsNullOrEmpty(jsonString))
                {
                    dataResult.codeReponse = CodeReponse.errorMissingAllParams;
                }
                else
                {
                    dataResult = await _processusService.UpdateProcessus(jsonString);
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





       [HttpPost, Route("Delete"), Produces("application/json")]

        public async Task<object> SupprimerProcessus([FromBody] object processusData)
        {
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                string jsonString = processusData.ToString();
                jsonString = jsonString.Replace("ValueKind = Object : ", "");

                if (string.IsNullOrEmpty(jsonString))
                {
                    dataResult.codeReponse = CodeReponse.errorMissingAllParams;
                }
                else
                {
                    dataResult = await _processusService.SupprimerProcessus(jsonString);
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



        [HttpGet, Route("Detail")]

        public async Task<object> DetailProcessus(int ID)
        {
            GetDataResult dataResult = await _processusService.DetailProcessus(ID);

            if (!dataResult.IsSucceed)
            {
                // Gérer l'erreur de manière appropriée, par exemple, en utilisant une classe d'erreur spécifique.
                var errorResponse = new GetDataResult
                {
                    IsSucceed = false,
                    Message = dataResult.Message,
                };
                return JsonConvert.SerializeObject(errorResponse);
            }

            return JsonConvert.SerializeObject(dataResult);
        }


        [HttpGet("GetCategories")]

        public async Task<object> GetCategories()
        {
            DataSourceResult result = new DataSourceResult();
            try
            {
                result = await _processusService.GetAllCategories();
                if (result.IsSucceed == true)
                {
                    return JsonConvert.SerializeObject(result);
                }
                return JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Message = "data notfound";
                // Loggez l'exception ici si nécessaire
                return JsonConvert.SerializeObject(result);
            }
        }

        [HttpGet("GetSMQ")]
     
        public async Task<object> GetSMQ()
        {
            DataSourceResult result = new DataSourceResult();
            try
            {
                result = await _processusService.GetAllSMQ();
                if (result.IsSucceed == true)
                {
                    return JsonConvert.SerializeObject(result);
                }
                return JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Message = "data notfound";
                // Loggez l'exception ici si nécessaire
                return JsonConvert.SerializeObject(result);
            }
        }

    }
}
