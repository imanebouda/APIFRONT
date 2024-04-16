using ITKANSys_api.Interfaces;
using ITKANSys_api.Services;
using ITKANSys_api.Utility.ApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ITKANSys_api.Controllers
{
    [Route("api/Indicateur")]
    [ApiController]
    public class IndicateurController : ControllerBase
    {
        private readonly IIndicateurService _indicateurService;

        public IndicateurController(IIndicateurService indicateurService)
        {
            _indicateurService = indicateurService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllIndicateurs(int ID)
        {
            try
            {
                var result = await _indicateurService.GetAllIndicateurs(ID);
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


        [HttpGet("recherche")]
        public async Task<object> RechercheIndicateurs(int id, string? titre, string? frequence, DateTime? dateDebut, DateTime? dateFin, string? field, string? order, int take, int skip)
        {
            DataSourceResult dataResult = new DataSourceResult();
            try
            {
                dataResult = await _indicateurService.RechercheIndicateurs(id, titre,frequence, dateDebut, dateFin, field, order, take, skip);

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
        public async Task<object> AjouterinsertIndicateurs([FromBody] object insertIndicateurs)
        {
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                string jsonString = insertIndicateurs.ToString();
                jsonString = jsonString.Replace("ValueKind = Object : ", "");

                if (string.IsNullOrEmpty(jsonString))
                {
                    dataResult.codeReponse = CodeReponse.errorMissingAllParams;
                }
                else
                {
                    dataResult = await _indicateurService.InsertIndicateurs(jsonString);
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
        public async Task<object> UpdateIndicateurs([FromBody] object indecateurData)
        {
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                string jsonString = indecateurData.ToString();
                jsonString = jsonString.Replace("ValueKind = Object : ", "");

                if (string.IsNullOrEmpty(jsonString))
                {
                    dataResult.codeReponse = CodeReponse.errorMissingAllParams;
                }
                else
                {
                    dataResult = await _indicateurService.UpdateIndicateurs(jsonString);
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

        public async Task<object> SupprimerIndicateurs([FromBody] object indecateurData)
        {
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                string jsonString = indecateurData.ToString();
                jsonString = jsonString.Replace("ValueKind = Object : ", "");

                if (string.IsNullOrEmpty(jsonString))
                {
                    dataResult.codeReponse = CodeReponse.errorMissingAllParams;
                }
                else
                {
                    dataResult = await _indicateurService.SupprimerIndicateurs(jsonString);
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

        public async Task<object> DetailIndicateur(int ID)
        {
            GetDataResult dataResult = await _indicateurService.DetailIndicateur(ID);

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
