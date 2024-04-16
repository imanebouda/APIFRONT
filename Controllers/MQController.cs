using ITKANSys_api.Data.OtherObjects;
using ITKANSys_api.Interfaces;
using ITKANSys_api.Utility.ApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ITKANSys_api.Controllers
{
    [Route("api/ManuelQualite")]
    public class MQController : ControllerBase
    {
        private readonly ApplicationDbContext _ctx;
        private readonly IMQService _MQService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MQController(ApplicationDbContext context, IMQService MQService, IWebHostEnvironment webHostEnvironment)
        {
            _ctx = context;
            _MQService = MQService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        [Route("ajouter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<object> UploadFile([FromForm] IFormFile file, [FromForm] string Libelle, [FromForm] int SmqID, [FromForm] string version, [FromForm] string Description, [FromForm] DateTime dateApplication, CancellationToken cancellationtoken)
        {
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                if (file == null)
                {
                    dataResult.codeReponse = CodeReponse.errorMissingFile;
                    dataResult.msg = "Le fichier est manquant.";
                }
                else
                {
                    dataResult = await _MQService.InsertMQ(file, Libelle, SmqID, version, Description, dateApplication, cancellationtoken);
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
        public async Task<IActionResult> DownloadMQDocument(int MQDocumentID)
        {
            var document = await _ctx.MQ
                            .Where(p => p.ID == MQDocumentID && p.deleted_at == null && p.Perime == StaticDocuments.NonValide)
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
        public async Task<object> UpdateProcesDocuments([FromForm] string Libelle, [FromForm] int ID, [FromForm] int SmqID, [FromForm] string version, [FromForm] string Description, [FromForm] DateTime dateApplication)
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
                    dataResult = await _MQService.UpdateMQ(Libelle, ID, SmqID, version, Description, dateApplication);
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

        [HttpGet("get-pdf")]
        public async Task<IActionResult> GetPdf(int MQId)
        {
            try
            {
                var MQ = await _ctx.MQ.FindAsync(MQId);

                if (MQ != null)
                {
                    var filePath = Path.Combine(MQ.FilePath, MQ.FileName);

                    if (System.IO.File.Exists(filePath))
                    {
                        var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

                        // Vous pouvez ajuster le type de fichier selon vos besoins.
                        return File(fileBytes, "application/pdf", MQ.FileName);
                    }
                }

                // Gérer le cas où le fichier PDF n'est pas trouvé.
                return NotFound("Fichier PDF non trouvé.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erreur interne du serveur lors de la récupération du fichier PDF.");
            }
        }


        [HttpGet, Route("Detail")]
        public async Task<object> DetailProcesDocuments()
        {
            GetDataResult dataResult = await _MQService.DetailMQ();

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

        public async Task<object> SupprimerMQ([FromBody] object MQData)
        {
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                string jsonString = MQData.ToString();
                jsonString = jsonString.Replace("ValueKind = Object : ", "");

                if (string.IsNullOrEmpty(jsonString))
                {
                    dataResult.codeReponse = CodeReponse.errorMissingAllParams;
                }
                else
                {
                    dataResult = await _MQService.SupprimerMQ(jsonString);
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
                dataResult = await _MQService.UpdatePerimeState(processusDocuments);
            }
            catch (Exception ex)
            {
                dataResult.codeReponse = CodeReponse.error;
                dataResult.msg = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
            }

            return JsonConvert.SerializeObject(dataResult);


        }
        [HttpPost("CheckValidDocument")]
        public async Task<IActionResult> CheckValidDocument(int documentId, int id, string documentType)
        {
            try
            {
                var hasValidDocument = await _MQService.DocumentContainsValidDocument(documentId, documentType);

                return Ok(hasValidDocument);
            }
            catch (Exception ex)
            {
                // Log l'exception si nécessaire
                return StatusCode(500, "Une erreur interne est survenue lors de la vérification du document.");
            }
        }

    }
}
