using ITKANSys_api.Services.Param;
using ITKANSys_api.Utility.ApiResponse;
using ITKANSys_api.Utility.Auth;
using ITKANSys_api.Utility.Other;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using System.Reflection;
using System;
using ITKANSys_api.Interfaces;

namespace ITKANSys_api.Controllers.Param
{
    [Route("api/[controller]")]
    [ApiController]
    /// <summary>
    /// Controlleur permettant de gérer les objets de type Vehicules
    /// </summary>
    public class ParametragesController : ControllerBase
    {
        //Gestionnaire de paramètre
        private IConfiguration configuration;
        private readonly IParametrageService _parametrageService;

        //Contexte de la base de données
        private ApplicationDbContext _context;
        private readonly IUserHelper _userHelper;

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public ParametragesController(ApplicationDbContext context , IParametrageService parametrageService)
        {
            _context = context;
            _parametrageService = parametrageService;

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
        [HttpGet, Route("GetAll"), Produces("application/json")]
       // [Authorize]
        public async Task<object> GetAll()
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();
            ParametrageService _parametrageService = new ParametrageService(_context);
            CustomHelper customHelper = new CustomHelper(_context);

            try
            {
              
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
                _parametrageService = null;
                customHelper = null;
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
        public async Task<object> Search(Object record)
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();
            ParametrageService _parametrageService = new ParametrageService(_context);
            CustomHelper customHelper = new CustomHelper(_context);

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
                    dataResult = await _parametrageService.Search(record);

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
                _parametrageService = null;
                customHelper = null;
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
        [HttpPost, Route("Insert"), Produces("application/json")]
      //  [Authorize]
        public async Task<object> Insert(Object record)
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();
            ParametrageService _parametrageService = new ParametrageService(_context);
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
                    //On appelle le service pour insérer les données
                    dataResult = await _parametrageService.Insert(record);
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
                _parametrageService = null;
                customHelper = null;
            }

            //On retourne le résultat
            return JsonConvert.SerializeObject(dataResult);
        }

        /// <summary>
        /// Api pour modifier un objet de type Parametrage
        /// </summary>
        /// <remarks>
        ///     Requête :
        ///     POST Roles/Update
        ///     {
        ///         "id" : Id de l'enregistrement de type Parametrage,
        ///         "label" : Nom du Parametrage (nvarchar),
        ///         "value" : Nom du  Parametrage (nvarchar),
        ///     }
        /// </remarks>
        /// <param name="record">L'objet qui contient les données reçue par l'utilisateur</param>
        /// <response code="200">Si la fonction s'est exécutée avec succès, l'api renvoie les informations</response>
        /// <response code="401">Si l'utilisateur n'a pas le droit d'utiliser l'api, l'api renvoie cette erreur à l'utilisateur</response>
        /// <response code="500">En cas d'une erreur technique, l'api renvoie cette erreur à l'utilisateur</response>
        /// <response code="501">En cas d'une erreur due au manque d'un paramètre ou un mauvais format, l'api renvoie cette erreur à l'utilisateur</response>
        /// <response code="502">En cas d'une erreur dans le format d'un champ ou qu'il soit manquant, l'api renvoie cette erreur à l'utilisateur</response>
        /// <response code="503">En cas d'une erreur lors de la validation de l'entité, l'api renvoie cette erreur à l'utilisateur</response>
        [HttpPost, Route("Update"), Produces("application/json")]
       // [Authorize]
        public async Task<object> Update(Object record)
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();
            ParametrageService _parametrageService = new ParametrageService(_context);
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
                    //On appelle le service pour updater les données
                    dataResult = await _parametrageService.Update(record);
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
                _parametrageService = null;
                customHelper = null;
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
        public async Task<object> Delete(Object record)
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();
            ParametrageService _parametrageService = new ParametrageService(_context);
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
                    //On appelle le service pour supprimer l'enregistrement
                    dataResult = await _parametrageService.Delete(record);
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
                _parametrageService = null;
                customHelper = null;
            }

            //On retourne le résultat
            return JsonConvert.SerializeObject(dataResult);
        }
    }
}
