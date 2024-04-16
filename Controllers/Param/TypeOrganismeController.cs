using ITKANSys_api.Services.Param;
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
using ITKANSys_api.Interfaces;

namespace ITKANSys_api.Controllers.Param
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeOrganismeController : ControllerBase
    {
        //Gestionnaire de paramètre
        private IConfiguration configuration;


        //Contexte de la base de données
        private ApplicationDbContext _context;
        private readonly IUserHelper _userHelper;
        private readonly ITypeOrganismeService _TypeOrganisme;

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public TypeOrganismeController(ApplicationDbContext context, ITypeOrganismeService TypeOrganisme)
        {
            _context = context;
            _TypeOrganisme = TypeOrganisme;
        }

        /// <summary>
        /// Api pour afficher tous les enregistrements de type Organisme
        /// </summary>
        /// <remarks>
        ///     Requête :
        ///     POST TypeOrdreService/GetAll
        ///     {
        ///         "label" : type Organisme (nvarchar),
        ///     }
        /// </remarks>
        /// <response code="200">Si la fonction s'est exécutée avec succès, l'api renvoie les informations</response>
        /// <response code="401">Si clients n'a pas le droit d'utiliser l'api, l'api renvoie cette erreur à clients</response>
        /// <response code="500">En cas d'une erreur technique, l'api renvoie cette erreur à clients</response>
        /// <response code="501">En cas d'une erreur due au manque d'un paramètre ou un mauvais format, l'api renvoie cette erreur à clients</response>
        /// <response code="502">En cas d'une erreur dans le format d'un champ ou qu'il soit manquant, l'api renvoie cette erreur à clients</response>
        /// <response code="503">En cas d'une erreur lors de la validation de l'entité, l'api renvoie cette erreur à clients</response>
        [HttpPost, Route("GetAll"), Produces("application/json")]
        //[Authorize]
        public async Task<object> GetAll()
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                //On appelle le service pour récupérer les données
                dataResult = await _TypeOrganisme.GetAll();
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
            }

            //On retourne le résultat
            return JsonConvert.SerializeObject(dataResult);
        }

    }
}
