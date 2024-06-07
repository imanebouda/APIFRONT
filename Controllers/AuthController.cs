using ITKANSys_api.Config;
using ITKANSys_api.Core.Dtos;
using ITKANSys_api.Core.Interfaces;
using ITKANSys_api.Services;
using ITKANSys_api.Utility.ApiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ITKANSys_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private IConfiguration configuration;
        private ApplicationDbContext _context;

        public AuthController(IAuthService authService, ApplicationDbContext context)
        {
            _authService = authService;
            configuration = AppConfig.GetConfig();
            _context = context;
        }


        [HttpGet("GetAll")]

        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var userDto = await _authService.GetAllAsync();

                if (userDto.IsSucceed)
                {
                    return Ok(userDto.Data);
                }
                else
                {
                    return BadRequest(userDto.Message);
                }
            }
            catch (Exception)
            {
                // Gérez l'exception ici, par exemple, en journalisant l'erreur.
                return StatusCode(500, "Une erreur s'est produite lors de la récupération des utilisateurs.");
            }
        }

        [HttpGet("GetAllPilote")]

        public async Task<IActionResult> GetAllPilote()
        {
            try
            {
                var userDto = await _authService.GetAllPiloteAsync();

                if (userDto.IsSucceed)
                {
                    return Ok(userDto.Data);
                }
                else
                {
                    return BadRequest(userDto.Message);
                }
            }
            catch (Exception)
            {
                // Gérez l'exception ici, par exemple, en journalisant l'erreur.
                return StatusCode(500, "Une erreur s'est produite lors de la récupération des utilisateurs.");
            }
        }
        [HttpGet("GetAllAuditeur")]
        public async Task<IActionResult> GetAllAuditeur()
        {
            try
            {
                var userDto = await _authService.GetAllAuditeur();

                if (userDto.IsSucceed)
                {
                    return Ok(userDto.Data);
                }
                else
                {
                    return BadRequest(userDto.Message);
                }
            }
            catch (Exception)
            {
                // Gérez l'exception ici, par exemple, en journalisant l'erreur.
                return StatusCode(500, "Une erreur s'est produite lors de la récupération des auditeurs.");
            }
        }

        [HttpGet("GetAllCoPilote")]

        public async Task<IActionResult> GetAllCoPilote()
        {
            try
            {
                var userDto = await _authService.GetAllCoPiloteAsync();

                if (userDto.IsSucceed)
                {
                    return Ok(userDto.Data);
                }
                else
                {
                    return BadRequest(userDto.Message);
                }
            }
            catch (Exception)
            {
                // Gérez l'exception ici, par exemple, en journalisant l'erreur.
                return StatusCode(500, "Une erreur s'est produite lors de la récupération des utilisateurs.");
            }
        }



        [HttpGet, Route("GetAllByRole"), Produces("application/json")]
    
        public async Task<IActionResult> GetAllByRoleController()
        {
            try
            {
                var dataResult = await _authService.GetAllByRoleAsync();

                if (dataResult.IsSucceed)
                {
                    return Ok(dataResult.Data);
                }
                else
                {
                    return BadRequest(dataResult.Message);
                }
            }
            catch (Exception)
            {
                // Gérez l'exception ici, par exemple, en journalisant l'erreur.
                return StatusCode(500, "Une erreur s'est produite lors de la récupération des utilisateurs.");
            }
        }



        // Route For Seeding my roles to DB
        [HttpPost, Route("seed-roles")]
        [Authorize]
        public async Task<IActionResult> SeedRoles()
        {
            var seerRoles = await _authService.SeedRolesAsync();

            return Ok(seerRoles);
        }


        // Route -> Register
        [HttpPost , Route("register") ]

        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var registerResult = await _authService.RegisterAsync(registerDto);

            if (registerResult.IsSucceed)
                return Ok(registerResult);

            return BadRequest(registerResult);
        }


        // Route -> Login
        [HttpPost ,Route("login")]
 
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var loginResult = await _authService.Login(loginDto);

            if (loginResult.IsSucceed)
                return Ok(loginResult);

            return Unauthorized(loginResult);
        }



        // Route -> make user -> admin
        [HttpPost, Route("make-admin")]
    
        public async Task<IActionResult> MakeAdmin([FromBody] UpdatePermissionDto updatePermissionDto)
        {
            var operationResult = await _authService.MakeAdminAsync(updatePermissionDto);

            if (operationResult.IsSucceed)
                return Ok(operationResult);

            return BadRequest(operationResult);
        }

        // Route -> make user -> owner
        [HttpPost, Route("make-owner")]
        [Authorize]
        public async Task<IActionResult> MakeOwner([FromBody] UpdatePermissionDto updatePermissionDto)
        {
            var operationResult = await _authService.MakeOwnerAsync(updatePermissionDto);

            if (operationResult.IsSucceed)
                return Ok(operationResult);

            return BadRequest(operationResult);
        }
        /// <summary>
        /// Api pour renvoyer à l'utilisateur un email de récupération du password
        /// </summary>
        /// <remarks>
        ///     Requête :
        ///     POST Users/PasswordForgotten
        ///     {
        ///         "email" : Email Utilisateur
        ///     }
        /// </remarks>
        /// <response code="200">Si la fonction s'est exécutée avec succès, l'api renvoie les informations</response>
        /// <response code="500">En cas d'une erreur technique, l'api renvoie cette erreur à l'utilisateur</response>
        /// <response code="501">En cas d'une erreur due au manque d'un paramètre ou un mauvais format, l'api renvoie cette erreur à l'utilisateur</response>
        /// <response code="502">En cas d'une erreur dans le format d'un champ ou qu'il soit manquant, l'api renvoie cette erreur à l'utilisateur</response>
        /// <response code="503">En cas d'une erreur lors de la validation de l'entité, l'api renvoie cette erreur à l'utilisateur</response>
        [HttpPost, Route("PasswordForgotten"), Produces("application/json")]
        public async Task<object> PasswordForgotten(Object record)
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();
            AuthService _AuthService = new AuthService(_context);

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
                    dataResult = await _AuthService.PasswordForgotten(record);

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
                _AuthService = null;
             
            }

            //On retourne le résultat
            return JsonConvert.SerializeObject(dataResult);
        }

        /// <summary>
        /// Api pour rechercher les enregistrements de type Utilisateurs
        /// </summary>
        /// <remarks>
        ///     Requête :
        ///     POST Users/Search
        ///     {
        ///         "take" : 10, 
        ///         "skip" : 0,
        ///         
        ///         "first_name" : Prénom de l'utilisateur (nvarchar),
        ///         "last_name" : Nom de l'utilisateur (nvarchar),
        ///         "email" : Email de l'utilisateur (nvarchar),
        ///         "id_company" : Companie associée à l'utilisateur (int),
        ///         "id_role" : Rôle associé à l'utilisateur (int),
        ///         "is_active" : Boolean indiquant si l'utilisateur est actif (bit),
        ///     }
        /// </remarks>
        /// <param name="record">Body du post qui contient les données envoyées par l'utilisateur</param>
        /// <response code="200">Si la fonction s'est exécutée avec succès, l'api renvoie les informations</response>
        /// <response code="401">Si l'utilisateur n'a pas le droit d'utiliser l'api, l'api renvoie cette erreur à l'utilisateur</response>
        /// <response code="500">En cas d'une erreur technique, l'api renvoie cette erreur à l'utilisateur</response>
        /// <response code="501">En cas d'une erreur due au manque d'un paramètre ou un mauvais format, l'api renvoie cette erreur à l'utilisateur</response>
        /// <response code="502">En cas d'une erreur dans le format d'un champ ou qu'il soit manquant, l'api renvoie cette erreur à l'utilisateur</response>
        /// <response code="503">En cas d'une erreur lors de la validation de l'entité, l'api renvoie cette erreur à l'utilisateur</response>
        [HttpPost, Route("Search"), Produces("application/json")]
        public async Task<object> Search(Object record)
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();
            AuthService _AuthService = new AuthService(_context);

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
                    dataResult = await _AuthService.Search(record);
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
                _AuthService = null;
            }

            //On retourne le résultat
            return JsonConvert.SerializeObject(dataResult);
        }

        /// <summary>
        /// Api pour ajouter un objet de type Utilisateurs
        /// </summary>
        /// <remarks>
        ///     Requête :
        ///     POST Users/Insert
        ///     {
        ///         
        ///         "first_name" : Prénom de l'utilisateur (nvarchar),
        ///         "last_name" : Nom de l'utilisateur (nvarchar),
        ///         "email" : Email de l'utilisateur (nvarchar),
        ///         "password" : Mot de passe de l'utilisateur (nvarchar),
        ///         "id_company" : Companie associée à l'utilisateur (int),
        ///         "id_role" : Rôle associé à l'utilisateur (int),
        ///         "is_active" : Boolean indiquant si l'utilisateur est actif (bit),
        ///     }
        /// </remarks>
        /// <param name="record">Body du post qui contient les données envoyées par l'utilisateur</param>
        /// <response code="200">Si la fonction s'est exécutée avec succès, l'api renvoie les informations</response>
        /// <response code="401">Si l'utilisateur n'a pas le droit d'utiliser l'api, l'api renvoie cette erreur à l'utilisateur</response>
        /// <response code="500">En cas d'une erreur technique, l'api renvoie cette erreur à l'utilisateur</response>
        /// <response code="501">En cas d'une erreur due au manque d'un paramètre ou un mauvais format, l'api renvoie cette erreur à l'utilisateur</response>
        /// <response code="502">En cas d'une erreur dans le format d'un champ ou qu'il soit manquant, l'api renvoie cette erreur à l'utilisateur</response>
        /// <response code="503">En cas d'une erreur lors de la validation de l'entité, l'api renvoie cette erreur à l'utilisateur</response>
        [HttpPost, Route("Insert"), Produces("application/json")]
       // [Authorize]
        public async Task<object> Insert(Object record)
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();
            AuthService _AuthService = new AuthService(_context);

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
                    dataResult = await _AuthService.Insert(record);
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
                _AuthService = null;
      
            }

            //On retourne le résultat
            return JsonConvert.SerializeObject(dataResult);
        }

        /// <summary>
        /// Api pour modifier un objet de type Utilisateurs
        /// </summary>
        /// <remarks>
        ///     Requête :
        ///     POST Users/Update
        ///     {
        ///         "id" : Id de l'enregistrement de type Utilisateurs,
        ///         
        ///         "first_name" : Prénom de l'utilisateur (nvarchar),
        ///         "last_name" : Nom de l'utilisateur (nvarchar),
        ///         "email" : Email de l'utilisateur (nvarchar),
        ///         "password" : Mot de passe de l'utilisateur (nvarchar),
        ///         "id_company" : Companie associée à l'utilisateur (int),
        ///         "id_role" : Rôle associé à l'utilisateur (int),
        ///         "is_active" : Boolean indiquant si l'utilisateur est actif (bit),
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
        public async Task<object> Update(Object record)
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();
            AuthService _AuthService = new AuthService(_context);

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
                    dataResult = await _AuthService.Update(record);
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
                _AuthService = null;
            }

            //On retourne le résultat
            return JsonConvert.SerializeObject(dataResult);
        }

        /// <summary>
        /// Api pour supprimer un objet de type Utilisateurs
        /// </summary>
        /// <remarks>
        ///     Requête :
        ///     POST Users/Delete
        ///     {
        ///         "id" : id de l'enregistrement de type Utilisateurs
        ///     }
        /// </remarks>
        /// <param name="record">L'objet qui contient les données reçue par l'utilisateur</param>
        /// <response code="200">Si la fonction s'est exécutée avec succès, l'api renvoie les informations</response>
        /// <response code="401">Si l'utilisateur n'a pas le droit d'utiliser l'api, l'api renvoie cette erreur à l'utilisateur</response>
        /// <response code="500">En cas d'une erreur technique, l'api renvoie cette erreur à l'utilisateur</response>
        /// <response code="501">En cas d'une erreur due au manque d'un paramètre ou un mauvais format, l'api renvoie cette erreur à l'utilisateur</response>
        /// <response code="502">En cas d'une erreur dans le format d'un champ ou qu'il soit manquant, l'api renvoie cette erreur à l'utilisateur</response>
        /// <response code="503">En cas d'une erreur lors de la validation de l'entité, l'api renvoie cette erreur à l'utilisateur</response>
        [HttpPost, Route("Delete"), Produces("application/json")]
        public async Task<object> Delete(Object record)
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();
            AuthService _AuthService = new AuthService(_context);
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
                    dataResult = await _AuthService.Delete(record);
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
                _AuthService = null;
            }

            //On retourne le résultat
            return JsonConvert.SerializeObject(dataResult);
        }

        /// <summary>
        /// Api pour modifier le mot de passe d'un utilisateurs
        /// </summary>
        /// <remarks>
        ///     Requête :
        ///     POST Utilisateur/UpdatePassWord
        ///     {
        ///         "id" : Id de l'enregistrement de type Utilisateurs,
        ///         
        ///         "nom" : nom (varchar),
        ///         "prenom" : prenom (varchar),
        ///         "adresse_mail" : adresse_mail (varchar),
        ///         "id_role" : id_role (int),
        ///     }
        /// </remarks>
        /// <param name="record">L'objet qui contient les données reçue par l'utilisateur</param>
        /// <response code="200">Si la fonction s'est exécutée avec succès, l'api renvoie les informations</response>
        /// <response code="401">Si l'utilisateur n'a pas le droit d'utiliser l'api, l'api renvoie cette erreur à l'utilisateur</response>
        /// <response code="500">En cas d'une erreur technique, l'api renvoie cette erreur à l'utilisateur</response>
        /// <response code="501">En cas d'une erreur due au manque d'un paramètre ou un mauvais format, l'api renvoie cette erreur à l'utilisateur</response>
        /// <response code="502">En cas d'une erreur dans le format d'un champ ou qu'il soit manquant, l'api renvoie cette erreur à l'utilisateur</response>
        /// <response code="503">En cas d'une erreur lors de la validation de l'entité, l'api renvoie cette erreur à l'utilisateur</response>
        [HttpPost, Route("UpdatePassWord"), Produces("application/json")]
        public async Task<object> UpdatePassord(Object record)
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();
            AuthService _AuthService = new AuthService(_context);

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
                    dataResult = await _AuthService.UpdatePassWord(record);
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
                _AuthService = null;
            }

            //On retourne le résultat
            return JsonConvert.SerializeObject(dataResult);
        }

    }
}
