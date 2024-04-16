using ITKANSys_api.Data.OtherObjects;
using ITKANSys_api.Data.Services;
using ITKANSys_api.Interfaces;
using ITKANSys_api.Models;
using ITKANSys_api.Services;
using ITKANSys_api.Utility.ApiResponse;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using FileInfo = ITKANSys_api.Interfaces.FileInfo;

namespace ITKANSys_api.Controllers
{
    [Route("api/ProcesDocuments")]
    public class ProcesDocumentsController : ControllerBase
    {
        private readonly ApplicationDbContext _ctx;
        private readonly IProcesDocumentsService _procesDocumentsService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProcesDocumentsController(ApplicationDbContext context ,IProcesDocumentsService procesDocumentsService, IWebHostEnvironment webHostEnvironment)
        {
            _ctx = context;
            _procesDocumentsService = procesDocumentsService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        [Route("ajouter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<object> UploadFile([FromForm] IFormFile file, [FromForm] string Libelle, [FromForm] int ProcessusID, CancellationToken cancellationtoken)
        {
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                if (file == null )
                {
                    dataResult.codeReponse = CodeReponse.errorMissingFile;
                    dataResult.msg = "Le fichier est manquant.";
                }
                else
                {
                    dataResult = await _procesDocumentsService.InsertProcesDocuments(file , Libelle , ProcessusID , cancellationtoken);
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

        [HttpGet]
        [Route("download")]
        public async Task<IActionResult> DownloadProcesDocument(int ProcesDocument)
        {
            var document = await _ctx.ProcesDocuments
                            .Where(p => p.ProcessusID == ProcesDocument && p.deleted_at == null && p.Perime == StaticDocuments.NonValide)
                            .OrderByDescending(p => p.CreationDate)
                            .FirstOrDefaultAsync();

            if (document == null)
            {
                return NotFound(); // Retourne une réponse 404 si le document n'est pas trouvé.
            }

            var filePath = Path.Combine(document.FilePath, document.FileName);
            var extension = document.FileName.Split('.')[document.FileName.Split('.').Length - 1];

            if (System.IO.File.Exists(filePath))
            {
                var memory = new MemoryStream();
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;

                // Déterminez le type de contenu en fonction de l'extension du fichier.
                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(document.FileName, out var contentType))
                {
                    contentType = "application/octet-stream";
                }

                // Retourne le fichier en tant que réponse de type fichier.
                return File(memory, contentType, $"{document.FileName}.{extension}");
            }
            else
            {
                return NotFound(); // Retourne une réponse 404 si le fichier n'existe pas sur le serveur.
            }
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<object> UpdateProcesDocuments( [FromForm] string Libelle, [FromForm] int ID, CancellationToken cancellationtoken)
        {
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                if (ID == null)
                {
                    dataResult.codeReponse = CodeReponse.errorMissingFile;
                    dataResult.msg = "Le nouveau fichier est manquant.";
                }
                else
                {
                    dataResult = await _procesDocumentsService.UpdateProcesDocuments(Libelle ,ID);
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
        public async Task<object> DetailProcesDocuments(int ID)
        {
            GetDataResult dataResult = await _procesDocumentsService.DetailProcesDocuments(ID);

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
                    dataResult = await _procesDocumentsService.SupprimerProcesDocuments(jsonString);
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

        [HttpPost, Route("modifier-perime"), Produces("application/json")]
        public async Task<object> ModifierPerimeDocument([FromBody] object processusDocuments)
        {
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                dataResult = await _procesDocumentsService.UpdatePerimeState(processusDocuments);
            }
            catch (Exception ex)
            {
                dataResult.codeReponse = CodeReponse.error;
                dataResult.msg = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
            }

            return JsonConvert.SerializeObject(dataResult);


        }
        [HttpPost("CheckValidDocument")]
        public async Task<IActionResult> CheckValidDocument( int documentId, int id , string documentType)
        {
            try
            {
                var hasValidDocument = await _procesDocumentsService.DocumentContainsValidDocument( documentId, id, documentType);

                return Ok(hasValidDocument);
            }
            catch (Exception ex)
            {
                // Log l'exception si nécessaire
                return StatusCode(500, "Une erreur interne est survenue lors de la vérification du document.");
            }
        }



        [HttpGet("recherche-documents")]
        public async Task<object> RechercheDocumentsPerime(int procedureID, int processusID, bool MQ, bool PQ, string? code, string? version, string? libelle, DateTime? dateDebut, DateTime? dateFin, string field, string order, int take, int skip)
        {
            DataSourceResult dataResult = new DataSourceResult();
            try
            {
                dataResult = await _procesDocumentsService.RechercheDocumentsPerime(procedureID, processusID,MQ ,PQ, code, version, libelle, dateDebut, dateFin, field, order, take, skip);

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
