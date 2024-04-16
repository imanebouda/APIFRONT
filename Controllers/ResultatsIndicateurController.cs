using ITKANSys_api.Data.Dtos;
using ITKANSys_api.Data.Services;
using ITKANSys_api.Interfaces;
using ITKANSys_api.Models;
using ITKANSys_api.Models.Dtos;
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
    [Route("api/ResultatsIndicateurs")]
    [ApiController]
    public class ResultatsIndicateurController : ControllerBase
    {
        private readonly IResultatsIndicateurService _resultatsIndicateurService;

        public ResultatsIndicateurController(IResultatsIndicateurService resultatsIndicateurService)
        {
            _resultatsIndicateurService = resultatsIndicateurService;
        }

        [HttpGet]
        public async Task<object> GetResultIndicateur(int Annee)
        {
            DataSourceResult dataResult = new DataSourceResult();
            try
            {
                dataResult = await _resultatsIndicateurService.GetAllResultByIdIndicateur(Annee);
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
        public async Task<object> AjouterResultIndicateur([FromBody] object insertResultIndicateur)
        {
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                string jsonString = insertResultIndicateur.ToString();
                jsonString = jsonString.Replace("ValueKind = Object : ", "");

                if (string.IsNullOrEmpty(jsonString))
                {
                    dataResult.codeReponse = CodeReponse.errorMissingAllParams;
                }
                else
                {
                    dataResult = await _resultatsIndicateurService.InsertResultIndicateur(jsonString);
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
        public async Task<object> UpdateResultIndicateur([FromBody] object ResultIndicateurData)
        {
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                string jsonString = ResultIndicateurData.ToString();
                jsonString = jsonString.Replace("ValueKind = Object : ", "");

                if (string.IsNullOrEmpty(jsonString))
                {
                    dataResult.codeReponse = CodeReponse.errorMissingAllParams;
                }
                else
                {
                    dataResult = await _resultatsIndicateurService.UpdateResultIndicateur(jsonString);
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

        public async Task<object> SupprimerResultIndicateur([FromBody] object ResultIndicateurData)
        {
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                string jsonString = ResultIndicateurData.ToString();
                jsonString = jsonString.Replace("ValueKind = Object : ", "");

                if (string.IsNullOrEmpty(jsonString))
                {
                    dataResult.codeReponse = CodeReponse.errorMissingAllParams;
                }
                else
                {
                    dataResult = await _resultatsIndicateurService.SupprimerResultIndicateur(jsonString);
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

        [HttpGet("recherche")]
        public async Task<object> RechercheResultatIndicateurs(string? periode, int annee, DateTime? dateDebut, DateTime? dateFin, int id, string? field, string? order, int take, int skip)
        {
            DataSourceResult dataResult = new DataSourceResult();
            try
            {
                dataResult = await _resultatsIndicateurService.RechercheResultatIndicateurs(periode , annee, dateDebut, dateFin, id, field, order, take, skip);

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

    }
}

