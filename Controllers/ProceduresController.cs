using ITKANSys_api.Data.Dtos;
using ITKANSys_api.Data.Services;
using ITKANSys_api.Interfaces;
using ITKANSys_api.Models;
using ITKANSys_api.Models.Dtos;
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
    [Route("api/procedures")]
    public class ProceduresController 
    {
        private readonly IProceduresService _proceduresService;

        public ProceduresController(IProceduresService proceduresService)
        {
            _proceduresService = proceduresService;
        }

        [HttpGet]
        public async Task<object> GetProcessus(int ProcessusID)
        {
            DataSourceResult dataResult = new DataSourceResult();
            try
            {
                dataResult = await _proceduresService.GetAllByIdProcessus(ProcessusID);
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
            catch (Exception ex)
            {
                dataResult.IsSucceed = false;
                dataResult.Message = "data notfound";
                // Loggez l'exception ici si nécessaire
                return JsonConvert.SerializeObject(dataResult);
            }
        }



        [HttpGet("recherche")]
        public async Task<object> RechercheProcedures(int id, string code, string libelle, DateTime? dateDebut, DateTime? dateFin,  string field, string order, int take, int skip)
        {
            DataSourceResult dataResult = new DataSourceResult();
            try
            {
                dataResult = await _proceduresService.RechercheProcedures(code,libelle,dateDebut,dateFin, id, field, order,take, skip);

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
            catch (Exception ex)
            {
                dataResult.IsSucceed = false;
                dataResult.Message = "data notfound";
                // Loggez l'exception ici si nécessaire
                return JsonConvert.SerializeObject(dataResult);
            }
        }

        [HttpPost, Route("ajouter"), Produces("application/json")]
        public async Task<object> AjouterProcedure([FromBody] object insertProcedure)
        {
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                string jsonString = insertProcedure.ToString();
                jsonString = jsonString.Replace("ValueKind = Object : ", "");

                if (string.IsNullOrEmpty(jsonString))
                {
                    dataResult.codeReponse = CodeReponse.errorMissingAllParams;
                }
                else
                {
                    dataResult = await _proceduresService.InsertProcedure(jsonString);
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
        public async Task<object> UpdateProcedure([FromBody] object procedureData)
        {
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                string jsonString = procedureData.ToString();
                jsonString = jsonString.Replace("ValueKind = Object : ", "");

                if (string.IsNullOrEmpty(jsonString))
                {
                    dataResult.codeReponse = CodeReponse.errorMissingAllParams;
                }
                else
                {
                    dataResult = await _proceduresService.UpdateProcedure(jsonString);
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

        public async Task<object> SupprimerProcedure([FromBody] object processusData)
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
                    dataResult = await _proceduresService.SupprimerProcedure(jsonString);
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
            GetDataResult dataResult = await _proceduresService.DetailProcedure(ID);

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


    }
}
