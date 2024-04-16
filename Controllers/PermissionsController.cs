using ITKANSys_api.Services.Gestions;
using ITKANSys_api.Utility.ApiResponse;
using ITKANSys_api.Utility.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using System.Reflection;
using System;
using ITKANSys_api.Config;
using ITKANSys_api.Utility.Other;

namespace ITKANSys_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        //Gestionnaire de paramètre
        private IConfiguration configuration;

        //Contexte de la base de données
        private ApplicationDbContext _context;
        private readonly IUserHelper _userHelper;
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public PermissionsController(ApplicationDbContext context)
        {
            configuration = AppConfig.GetConfig();
            _context = context;
           
        }
        

        /// <summary>
        /// Api pour afficher tous les enregistrements de type Permissions
        /// </summary>
        /// <remarks>
        ///     Requête :
        ///     POST Permissions/GetAll
        ///     {
        ///     }
        /// </remarks>
        /// <response code="200">Si la fonction s'est exécutée avec succès, l'api renvoie les informations</response>
        /// <response code="401">Si clients n'a pas le droit d'utiliser l'api, l'api renvoie cette erreur à clients</response>
        /// <response code="500">En cas d'une erreur technique, l'api renvoie cette erreur à clients</response>
        /// <response code="501">En cas d'une erreur due au manque d'un paramètre ou un mauvais format, l'api renvoie cette erreur à clients</response>
        /// <response code="502">En cas d'une erreur dans le format d'un champ ou qu'il soit manquant, l'api renvoie cette erreur à clients</response>
        /// <response code="503">En cas d'une erreur lors de la validation de l'entité, l'api renvoie cette erreur à clients</response>
        [HttpGet, Route("GetAll"), Produces("application/json")]
        //[Authorize]
        public async Task<object> GetAll()
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();
            PermissionsService _permissionsService = new PermissionsService(_context);

            try
            {
                //On appelle le service pour récupérer les données
                dataResult = await _permissionsService.GetAll();
            }
            catch (Exception ex)
            {
                //On définit le retour avec le détail de l'erreur
                dataResult.codeReponse = CodeReponse.error;
                dataResult.msg = (ex.InnerException == null) ? ex.Message : ex.InnerException.Message;

            }
            finally
            {
                //On libère les ressources
                _permissionsService = null;
            }

            //On retourne le résultat
            return JsonConvert.SerializeObject(dataResult);
        }

        /// <summary>
        /// Api pour afficher tous les enregistrements de type Permissions
        /// </summary>
        /// <remarks>
        ///     Requête :
        ///     POST Permissions/GetAllByUser
        ///     {
        ///     }
        /// </remarks>
        /// <response code="200">Si la fonction s'est exécutée avec succès, l'api renvoie les informations</response>
        /// <response code="401">Si clients n'a pas le droit d'utiliser l'api, l'api renvoie cette erreur à clients</response>
        /// <response code="500">En cas d'une erreur technique, l'api renvoie cette erreur à clients</response>
        /// <response code="501">En cas d'une erreur due au manque d'un paramètre ou un mauvais format, l'api renvoie cette erreur à clients</response>
        /// <response code="502">En cas d'une erreur dans le format d'un champ ou qu'il soit manquant, l'api renvoie cette erreur à clients</response>
        /// <response code="503">En cas d'une erreur lors de la validation de l'entité, l'api renvoie cette erreur à clients</response>
        [HttpPost, Route("GetAllByRole"), Produces("application/json")]
        //[Authorize]
        public async Task<object> GetAllByRole(Object record)
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();
            PermissionsService _permissionsService = new PermissionsService(_context);

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
                    dataResult = await _permissionsService.GetAllByRole(record);

                }
            }
            catch (Exception ex)
            {
                //On définit le retour avec le détail de l'erreur
                dataResult.codeReponse = CodeReponse.error;
                dataResult.msg = (ex.InnerException == null) ? ex.Message : ex.InnerException.Message;

           
            }
            finally
            {
                //On libère les ressources
                _permissionsService = null;
            }

            //On retourne le résultat
            return JsonConvert.SerializeObject(dataResult);
        }

        /// <summary>
        /// Api pour ajouter un objet de type Permissions
        /// </summary>
        /// <remarks>
        ///     Requête :
        ///     POST Permissions/Save
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
        //[Authorize]
        public async Task<object> Save(Object record)
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();
            PermissionsService _permissionsService = new PermissionsService(_context);

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
                    dataResult = await _permissionsService.Save(record);

                }
            }
            catch (Exception ex)
            {
                //On définit le retour avec le détail de l'erreur
                dataResult.codeReponse = CodeReponse.error;
                dataResult.msg = (ex.InnerException == null) ? ex.Message : ex.InnerException.Message;
            }
            finally
            {
                //On libère les ressources
                _permissionsService = null;
            }

            //On retourne le résultat
            return JsonConvert.SerializeObject(dataResult);
        }

        /// <summary>
        /// Api pour supprimer un objet de type Permissions
        /// </summary>
        /// <remarks>
        ///     Requête :
        ///     POST Permissions/Delete
        ///     {
        ///         "id" : id de l'enregistrement de type Permissions
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
     
        public async Task<object> Delete(Object record)
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();
            PermissionsService _permissionsService = new PermissionsService(_context);
            CustomHelper customHelper = new CustomHelper(_context);

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
                    //On vérifie l'existence de la clé api
                    dataResult = customHelper.CheckHeaderRequest(Request);

                    //Si pas d'erreur on appelle le service
                    if (dataResult.codeReponse != CodeReponse.unauthorized
                        && dataResult.codeReponse != CodeReponse.unauthorizedIP
                        && dataResult.codeReponse != CodeReponse.error
                        && dataResult.codeReponse != CodeReponse.errorInvalidParams
                        && dataResult.codeReponse != CodeReponse.errorMissingAllParams
                        && dataResult.codeReponse != CodeReponse.errorInvalidMissingParams)
                    {
                        //On appelle le service pour supprimer l'enregistrement
                        dataResult = await _permissionsService.Delete(record);
                    }
                }
            }
            catch (Exception ex)
            {
                //On définit le retour avec le détail de l'erreur
                dataResult.codeReponse = CodeReponse.error;
                dataResult.msg = (ex.InnerException == null) ? ex.Message : ex.InnerException.Message;
            }
            finally
            {
                //On libère les ressources
                _permissionsService = null;
                customHelper = null;
            }

            //On retourne le résultat
            return JsonConvert.SerializeObject(dataResult);
        }

    }
}
