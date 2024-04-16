using AutoMapper;
using ITKANSys_api.Utility.ApiResponse;
using ITKANSys_api.Utility.Other;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using System.Reflection;
using System;
using ITKANSys_api.Interfaces;
using System.Net.Http.Headers;
using ITKANSys_api.Models.Entities;

namespace ITKANSys_api.Controllers.Param
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganismeController : ControllerBase
    {
        //Gestionnaire de paramètre
        private IConfiguration configuration;

        //Contexte de la base de données
        private ApplicationDbContext _context;
        private IOrganismeService _organismService;

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public OrganismeController(ApplicationDbContext context,  IOrganismeService organismService)
        {
            _context = context;
            _organismService = organismService;
        }

        /// <summary>
        /// Api pour afficher tous les enregistrements de type Vehicules
        /// </summary>
        /// <remarks>
        ///     Requête :
        ///     POST Vehicules/GetAll
        ///     {
        ///     }
        /// </remarks>
        /// <response code="200">Si la fonction s'est exécutée avec succès, l'api renvoie les informations</response>
        /// <response code="401">Si clients n'a pas le droit d'utiliser l'api, l'api renvoie cette erreur à clients</response>
        /// <response code="500">En cas d'une erreur technique, l'api renvoie cette erreur à clients</response>
        /// <response code="501">En cas d'une erreur due au manque d'un paramètre ou un mauvais format, l'api renvoie cette erreur à clients</response>
        /// <response code="502">En cas d'une erreur dans le format d'un champ ou qu'il soit manquant, l'api renvoie cette erreur à clients</response>
        /// <response code="503">En cas d'une erreur lors de la validation de l'entité, l'api renvoie cette erreur à clients</response>
        [HttpPost, Route("GetAll"), Produces("application/json")]
      // [Authorize]
        public async Task<object> GetAll()
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();
         

            try
            {
                //On appelle le service pour récupérer les données
                dataResult = await _organismService.GetAll();
            }
            catch (Exception ex)
            {
                //On définit le retour avec le détail de l'erreur
                dataResult.codeReponse = CodeReponse.error;
                dataResult.msg = ex.InnerException == null ? ex.Message : ex.InnerException.Message;

            }
            finally
            {
                //On libère les ressources
                _organismService = null;
            }

            //On retourne le résultat
            return JsonConvert.SerializeObject(dataResult);
        }

        /// <summary>
        /// Api pour rechercher les enregistrements de type Vehicules
        /// </summary>
        /// <remarks>
        ///     Requête :
        ///     POST Vehicules/Search
        ///     {
        ///         "take" : 10, 
        ///         "skip" : 0,
        ///         
        ///         "name" : Nom de clients (nvarchar),
        ///         "email" : Email de clients (nvarchar),
        ///     }
        /// </remarks>
        /// <param name="record">Body du post qui contient les données envoyées par clients</param>
        /// <response code="200">Si la fonction s'est exécutée avec succès, l'api renvoie les informations</response>
        /// <response code="401">Si clients n'a pas le droit d'utiliser l'api, l'api renvoie cette erreur à clients</response>
        /// <response code="500">En cas d'une erreur technique, l'api renvoie cette erreur à clients</response>
        /// <response code="501">En cas d'une erreur due au manque d'un paramètre ou un mauvais format, l'api renvoie cette erreur à clients</response>
        /// <response code="502">En cas d'une erreur dans le format d'un champ ou qu'il soit manquant, l'api renvoie cette erreur à clients</response>
        /// <response code="503">En cas d'une erreur lors de la validation de l'entité, l'api renvoie cette erreur à clients</response>
        [HttpPost, Route("Search"), Produces("application/json")]
      // [Authorize]
        public async Task<object> Search(object record)
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();
         

            try
            {
                //On enlève les caractères superflux
                record = record.ToString().Replace("ValueKind = Object : ", "");

                if (record == null || record.ToString().Length == 0)
                {
                    //On définit le retour avec le détail de l'erreur
                    dataResult.codeReponse = CodeReponse.error;
                    dataResult.msg = configuration.GetSection("MessagesAPI:ParamsEmpty").Value;
                }
                else
                {
                    //On appelle le service pour récupérer les données
                    dataResult = await _organismService.Search(record);

                }
            }
            catch (Exception ex)
            {
                //On définit le retour avec le détail de l'erreur
                dataResult.codeReponse = CodeReponse.error;
                dataResult.msg = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
            }
            finally
            {
                //On libère les ressources
                _organismService = null;
              
            }

            //On retourne le résultat
            return JsonConvert.SerializeObject(dataResult);
        }

        /// <summary>
        /// Api pour récupérer le détail d'un objet de type Vehicules
        /// </summary>
        /// <remarks>
        ///     Requête :
        ///     POST Vehicules/Load
        ///     {
        ///         "id" : Id de l'enregistrement de type Vehicules
        ///     }
        /// </remarks>
        /// <param name="record">Body du post qui contient les données envoyées par clients</param>
        /// <response code="200">Si la fonction s'est exécutée avec succès, l'api renvoie les informations</response>
        /// <response code="401">Si clients n'a pas le droit d'utiliser l'api, l'api renvoie cette erreur à clients</response>
        /// <response code="500">En cas d'une erreur technique, l'api renvoie cette erreur à clients</response>
        /// <response code="501">En cas d'une erreur due au manque d'un paramètre ou un mauvais format, l'api renvoie cette erreur à clients</response>
        /// <response code="502">En cas d'une erreur dans le format d'un champ ou qu'il soit manquant, l'api renvoie cette erreur à clients</response>
        /// <response code="503">En cas d'une erreur lors de la validation de l'entité, l'api renvoie cette erreur à clients</response>
        [HttpPost, Route("Load"), Produces("application/json")]
      // [Authorize]
        public async Task<object> Load(object record)
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                //On enlève les caractères superflux
                record = record.ToString().Replace("ValueKind = Object : ", "");

                if (record == null || record.ToString().Length == 0)
                {
                    //On définit le retour avec le détail de l'erreur
                    dataResult.codeReponse = CodeReponse.errorMissingAllParams;
                    dataResult.msg = configuration.GetSection("MessagesAPI:ParamsEmpty").Value;
                }
                else
                {
                    //On appelle le service pour récupérer les données
                    dataResult = await _organismService.Load(record);
                }
            }
            catch (Exception ex)
            {
                //On définit le retour avec le détail de l'erreur
                dataResult.codeReponse = CodeReponse.error;
                dataResult.msg = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
            }
            finally
            {
                //On libère les ressources
                _organismService = null;
            }

            //On retourne le résultat
            return JsonConvert.SerializeObject(dataResult);
        }

        /// <summary>
        /// Api pour récupérer le détail d'un objet de type Vehicules
        /// </summary>
        /// <remarks>
        ///     Requête :
        ///     POST Vehicules/Load
        ///     {
        ///         "id" : Id de l'enregistrement de type Vehicules
        ///     }
        /// </remarks>
        /// <response code="200">Si la fonction s'est exécutée avec succès, l'api renvoie les informations</response>
        /// <response code="401">Si clients n'a pas le droit d'utiliser l'api, l'api renvoie cette erreur à clients</response>
        /// <response code="500">En cas d'une erreur technique, l'api renvoie cette erreur à clients</response>
        /// <response code="501">En cas d'une erreur due au manque d'un paramètre ou un mauvais format, l'api renvoie cette erreur à clients</response>
        /// <response code="502">En cas d'une erreur dans le format d'un champ ou qu'il soit manquant, l'api renvoie cette erreur à clients</response>
        /// <response code="503">En cas d'une erreur lors de la validation de l'entité, l'api renvoie cette erreur à clients</response>
        [HttpGet, Route("CurrentOrganisme"), Produces("application/json")]
      // [Authorize]
        public async Task<object> CurrentOrganisme()
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();
    

            try
            {

                //On appelle le service pour récupérer les données
                dataResult = await _organismService.CurrentOrganisme();
            }
            catch (Exception ex)
            {
                //On définit le retour avec le détail de l'erreur
                dataResult.codeReponse = CodeReponse.error;
                dataResult.msg = ex.InnerException == null ? ex.Message : ex.InnerException.Message;

                
            }
            finally
            {
                //On libère les ressources
                _organismService = null;
            }

            //On retourne le résultat
            return JsonConvert.SerializeObject(dataResult);
        }

        /// <summary>
        /// Api pour ajouter un objet de type Vehicules
        /// </summary>
        /// <remarks>
        ///     Requête :
        ///     POST Vehicules/Save
        ///     {
        ///         
        ///     }
        /// </remarks>
        /// <param name="record">Body du post qui contient les données envoyées par clients</param>
        /// <response code="200">Si la fonction s'est exécutée avec succès, l'api renvoie les informations</response>
        /// <response code="401">Si clients n'a pas le droit d'utiliser l'api, l'api renvoie cette erreur à clients</response>
        /// <response code="500">En cas d'une erreur technique, l'api renvoie cette erreur à clients</response>
        /// <response code="501">En cas d'une erreur due au manque d'un paramètre ou un mauvais format, l'api renvoie cette erreur à clients</response>
        /// <response code="502">En cas d'une erreur dans le format d'un champ ou qu'il soit manquant, l'api renvoie cette erreur à clients</response>
        /// <response code="503">En cas d'une erreur lors de la validation de l'entité, l'api renvoie cette erreur à clients</response>
        [HttpPost, Route("Save"), Produces("application/json")]
      // [Authorize]
        public async Task<object> Save(object record)
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();
         
            try
            {
                //On enlève les caractères superflux
                record = record.ToString().Replace("ValueKind = Object : ", "");

                if (record == null || record.ToString().Length == 0)
                {
                    //On définit le retour avec le détail de l'erreur
                    dataResult.codeReponse = CodeReponse.errorMissingAllParams;
                    dataResult.msg = configuration.GetSection("MessagesAPI:ParamsEmpty").Value;
                }
                else
                {
                    //On appelle le service pour insérer les données
                    dataResult = await _organismService.Save(record);
                }
            }
            catch (Exception ex)
            {
                //On définit le retour avec le détail de l'erreur
                dataResult.codeReponse = CodeReponse.error;
                dataResult.msg = ex.InnerException == null ? ex.Message : ex.InnerException.Message;

            }
            finally
            {
                //On libère les ressources
                _organismService = null;
            }

            //On retourne le résultat
            return JsonConvert.SerializeObject(dataResult);
        }

        /// <summary>
        /// Api pour supprimer un objet de type Vehicules
        /// </summary>
        /// <remarks>
        ///     Requête :
        ///     POST Vehicules/Delete
        ///     {
        ///         "id" : id de l'enregistrement de type Vehicules
        ///     }
        /// </remarks>
        /// <param name="record">L'objet qui contient les données reçue par clients</param>
        /// <response code="200">Si la fonction s'est exécutée avec succès, l'api renvoie les informations</response>
        /// <response code="401">Si clients n'a pas le droit d'utiliser l'api, l'api renvoie cette erreur à clients</response>
        /// <response code="500">En cas d'une erreur technique, l'api renvoie cette erreur à clients</response>
        /// <response code="501">En cas d'une erreur due au manque d'un paramètre ou un mauvais format, l'api renvoie cette erreur à clients</response>
        /// <response code="502">En cas d'une erreur dans le format d'un champ ou qu'il soit manquant, l'api renvoie cette erreur à clients</response>
        /// <response code="503">En cas d'une erreur lors de la validation de l'entité, l'api renvoie cette erreur à clients</response>
        [HttpPost, Route("Delete"), Produces("application/json")]
      // [Authorize]
        public async Task<object> Delete(object record)
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                //On enlève les caractères superflux
                record = record.ToString().Replace("ValueKind = Object : ", "");

                if (record == null || record.ToString().Length == 0)
                {
                    //On définit le retour avec le détail de l'erreur
                    dataResult.codeReponse = CodeReponse.errorMissingAllParams;
                    dataResult.msg = configuration.GetSection("MessagesAPI:ParamsEmpty").Value;
                }
                else
                {
                    //On appelle le service pour supprimer l'enregistrement
                    dataResult = await _organismService.Delete(record);
                }
            }
            catch (Exception ex)
            {
                //On définit le retour avec le détail de l'erreur
                dataResult.codeReponse = CodeReponse.error;
                dataResult.msg = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
            }
            finally
            {
                //On libère les ressources
                _organismService = null;
            }

            //On retourne le résultat
            return JsonConvert.SerializeObject(dataResult);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("SaveImage"), DisableRequestSizeLimit]
        public string SaveImage(IFormFile file)
        {
            try
            {
                var folderName = Path.Combine("wwwroot", "images");
                var dataOrganisme = _context.Organismes.Where(p => p.deleted_at == null).OrderByDescending(p => p.id).Last();

                if (dataOrganisme != null)
                {
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }

                    if (file.Length > 0)
                    {
                        var fileName = DateTime.Now.Ticks + "_" + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var fullPath = Path.Combine(pathToSave, fileName);
                        var dbPath = Path.Combine(folderName, fileName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        // Enregistrez le nom du logo
                        dataOrganisme.logo = fileName;
                        _context.SaveChanges();

                        return dbPath;
                    }
                    else
                    {
                        return null; // Gérez le cas où le fichier est vide
                    }
                }
                else
                {
                    return "Aucun organisme trouvé pour l'enregistrement de l'image.";
                }
            }
            catch (Exception ex)
            {
                return $"Une erreur s'est produite : {ex.Message}";
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetImage")]
        public IActionResult GetImage()
        {
            try
            {
                // Recherche du dernier organisme non supprimé ayant un logo non null
                Organisme organisme = _context.Organismes.Where(p => p.deleted_at == null ).OrderByDescending(p => p.id).Last();

                // Vérification si l'organisme ou la propriété logo est null
                if (organisme == null || string.IsNullOrEmpty(organisme.logo))
                {
                    return NotFound();
                }

                // Chemin complet vers le fichier image
                var folderName = Path.Combine("wwwroot", "images");
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName, organisme.logo);

                // Vérification si le fichier image existe sur le serveur
                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound();
                }

                // Lecture du fichier image dans un flux de mémoire
                var memory = new MemoryStream();
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    stream.CopyTo(memory);
                }
                memory.Position = 0;

                // Retourne le fichier image en tant que réponse HTTP
                return File(memory, GetContentType(filePath), Path.GetFileName(filePath));
            }
            catch (Exception ex)
            {
                // Gestion de l'exception, enregistrement des détails ou retour d'une réponse d'erreur
                return StatusCode(500, $"Une erreur s'est produite : {ex.Message}");
            }
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;

            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }

            return contentType;
        }
    }
}
