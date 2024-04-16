using ITKANSys_api.Interfaces;
using ITKANSys_api.Utility.ApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ITKANSys_api.Controllers
{
    [Route("api/ProcObjectifs")]
    [ApiController]
    public class ProcObjectifsController : ControllerBase
    {
        private readonly IProcObjectifsService _procedureObjectifsService;

        public ProcObjectifsController(IProcObjectifsService procedureObjectifsService)
        {
            _procedureObjectifsService = procedureObjectifsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProcesObjectifs(int ID)
        {
            try
            {
                var result = await _procedureObjectifsService.GetAllProcObjectifs(ID);
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
        public async Task<object> RechercheProcessus(int id, string? titre, DateTime? dateDebut, DateTime? dateFin, string? field, string? order, int take, int skip)
        {
            DataSourceResult dataResult = new DataSourceResult();
            try
            {
                dataResult = await _procedureObjectifsService.RechercheProcObjectifs(id, titre, dateDebut, dateFin, field, order, take, skip);

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
                    dataResult = await _procedureObjectifsService.InsertProcObjectifs(jsonString);
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
        public async Task<object> UpdateProcesObjectifs([FromBody] object processusData)
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
                    dataResult = await _procedureObjectifsService.UpdateProcObjectifs(jsonString);
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

        public async Task<object> SupprimerProcesObjectifs([FromBody] object processusData)
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
                    dataResult = await _procedureObjectifsService.SupprimerProcObjectifs(jsonString);
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
    }
}
