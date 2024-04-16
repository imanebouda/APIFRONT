using ITKANSys_api.Core.Interfaces;
using ITKANSys_api.Core.OtherObjects;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ITKANSys_api.Models;
using ITKANSys_api.Data.Dtos;
using Newtonsoft.Json;
using ITKANSys_api.Config;
using ITKANSys_api.Core.Dtos;
using ITKANSys_api.Auth;
using ITKANSys_api.Utility.ApiResponse;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json.Linq;
using System.Reflection;
using static ITKANSys_api.Utility.ApiResponse.QueryableExtensions;

namespace ITKANSys_api.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private IConfiguration configuration;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
             configuration = AppConfig.GetConfig();
        }

        // Get ALL USERS


        public async Task<UserDto> GetAllAsync()
        {
            var dataResult = new UserDto();
            string code = "SP";

            try
            {
                var users = await _context.Users
                    .Where(p => p.deleted_at == null && p.UserRole.Code != code)
                    .AsNoTracking()
                    .Select(p => new UserInfoDto // Utilisez UserInfoDto ici
                    {
                        Id = p.Id,
                        NomCompletUtilisateur = p.NomCompletUtilisateur,
                        Email = p.Email,
                        Username = p.Username,
                        RolesName = p.UserRole.Name,
                        IdRole = p.IdRole
                    })
                    .OrderBy(x => x.NomCompletUtilisateur)
                    .ToListAsync();

                dataResult.IsSucceed = true;
                dataResult.Message = "Opération réussie";
                dataResult.Data = users;
            }
            catch (Exception ex)
            {
                dataResult.IsSucceed = false;
                dataResult.Message = "Erreur lors de la récupération des utilisateurs";
                // Ajoutez des informations de journalisation ici si nécessaire
            }

            return dataResult;
        }

        public async Task<UserDto> GetAllPiloteAsync()
        {
            var dataResult = new UserDto();

            try
            {
                var users = await _context.Users
                    .Where(p => p.deleted_at == null && p.IdRole == StaticUserRoles.Pilote)
                    .AsNoTracking()
                    .Select(p => new UserInfoDto // Utilisez UserInfoDto ici
                    {
                        Id = p.Id,
                        NomCompletUtilisateur = p.NomCompletUtilisateur,
                        Email = p.Email,
                        Username = p.Username,
                        RolesName = p.UserRole.Name,
                        IdRole = p.IdRole
                    })
                    .OrderBy(x => x.NomCompletUtilisateur)
                    .ToListAsync();

                dataResult.IsSucceed = true;
                dataResult.Message = "Opération réussie";
                dataResult.Data = users;
            }
            catch (Exception ex)
            {
                dataResult.IsSucceed = false;
                dataResult.Message = "Erreur lors de la récupération des utilisateurs";
                // Ajoutez des informations de journalisation ici si nécessaire
            }

            return dataResult;
        }

        public async Task<UserDto> GetAllCoPiloteAsync()
        {
            var dataResult = new UserDto();

            try
            {
                var users = await _context.Users
                    .Where(p => p.deleted_at == null && p.IdRole == StaticUserRoles.CoPilote)
                    .AsNoTracking()
                    .Select(p => new UserInfoDto // Utilisez UserInfoDto ici
                    {
                        Id = p.Id,
                        NomCompletUtilisateur = p.NomCompletUtilisateur,
                        Email = p.Email,
                        Username = p.Username,
                        RolesName = p.UserRole.Name,
                        IdRole = p.IdRole
                    })
                    .OrderBy(x => x.NomCompletUtilisateur)
                    .ToListAsync();

                dataResult.IsSucceed = true;
                dataResult.Message = "Opération réussie";
                dataResult.Data = users;
            }
            catch (Exception ex)
            {
                dataResult.IsSucceed = false;
                dataResult.Message = "Erreur lors de la récupération des utilisateurs";
                // Ajoutez des informations de journalisation ici si nécessaire
            }

            return dataResult;
        }

        public async Task<UserDto> GetNameUserById(int ID)
        {
            var dataResult = new UserDto();
            try
            {
                var users = await _context.Users
                    .Where(p => p.deleted_at == null && p.Id ==ID )
                    .AsNoTracking()
                    .Select(p => new UserInfoDto // Utilisez UserInfoDto ici
                    {
                        NomCompletUtilisateur = p.NomCompletUtilisateur,
                    })
                    .ToListAsync();

                dataResult.IsSucceed = true;
                dataResult.Message = "Opération réussie";
                dataResult.Data = users;
            }
            catch (Exception ex)
            {
                dataResult.IsSucceed = false;
                dataResult.Message = "Erreur lors de la récupération des utilisateurs";
                // Ajoutez des informations de journalisation ici si nécessaire
            }

            return dataResult;
        }



        public async Task<UserDto> GetAllByRoleAsync()
        {
            // Déclaration des variables
            UserDto dataResult = new UserDto();

            try
            {
                // On récupère les données de la base de données
                int id_role = int.Parse(configuration.GetSection("Roles:Controller").Value.ToString());
                var users = await _context.Users
                    .Where(p => p.deleted_at == null && p.IdRole == id_role)
                    .AsNoTracking()
                    .Select(p => new UserInfoDto
                    {
                        Id = p.Id,
                        NomCompletUtilisateur = p.NomCompletUtilisateur
                    })
                    .OrderBy(x => x.NomCompletUtilisateur)
                    .ToListAsync();

                // On définit le résultat à retourner
            
              
                dataResult.IsSucceed = true;
                dataResult.Data = users;

            }
            catch (Exception ex)
            {
                // On définit le retour en cas d'erreur
                dataResult.IsSucceed = false;
                dataResult.Message = "Erreur lors de la récupération des utilisateurs";
                
            }
            finally
            {
                // Vous pouvez ajouter des actions à effectuer après la récupération des données
            }

            // On retourne le résultat
            return dataResult;
        }

        public async Task<AuthServiceResponseDto> Login(LoginDto record)
        {
            //Déclaration des variables
            AuthServiceResponseDto dataResult = new AuthServiceResponseDto();

            dynamic body = "";
            var dictionary = new Dictionary<string, string>();

            string? username;
            string? password;

            try
            {
                //On enlève les caractères superflux
                if (record.Password == null || record.UserName == null)
                {
                    //On définit le retour avec le détail de l'erreur
                    dataResult.IsSucceed = false;
                    dataResult.Message = configuration.GetSection("MessagesAPI:ParamsEmpty").Value;
                }
                else
                {

                    //On récupère les paramètres de recherche envoyés dans le body
                    username = record.UserName != "" ? (string)record.UserName : null;
                    password = record.Password != "" ? (string)record.Password : null;

                    //On se connecte à la base de données
                    using (_context)
                    {
                        string en = configuration.GetSection("Divers:EncryptionKey").Value;
                        //On crypte le password
                        var pwd = CryptageHelper.ByteArrayToBase64Hex(CryptageHelper.Encryption(password, en));

                        //On récupère les données de la base de données
                        var _Users = await _context.Users
                                        .Where(p => p.deleted_at == null && p.Username == username && p.Password == pwd)
                                        .AsNoTracking()
                                        .Select(p => new
                                        {
                                            p.Id,
                                            p.Token,
                                            p.NomCompletUtilisateur,
                                            p.Email,
                                            Roles_name = p.UserRole.Name,
                                            p.IdRole,
                                            permissions = _context.PermissionRoles.Where(per => per.deleted_at == null && per.RoleId == p.IdRole)
                                                        .Select(p => new
                                                        {
                                                            p.Id,
                                                            name = p.Permissions.Controller + "-" + p.Permissions.Methode
                                                        })
                                                    .ToList()

                                        })
                                        .FirstOrDefaultAsync();

                        if (_Users != null)
                        {
                            var id = CryptageHelper.ByteArrayToBase64Hex(CryptageHelper.Encryption(_Users.Id.ToString(), configuration.GetSection("Divers:EncryptionKey").Value));

                            //on génère le token et sa durée 
                            var XKestrelDescriptor = new SecurityTokenDescriptor
                            {
                                Subject = new ClaimsIdentity(new Claim[] {
                                                             new Claim(ClaimTypes.Actor, _Users.Id.ToString()),
                                                             new Claim(ClaimTypes.PrimarySid, id),
                                                             new Claim(ClaimTypes.Role, _Users.IdRole.ToString()),
                                                             new Claim(ClaimTypes.NameIdentifier, _Users.NomCompletUtilisateur)
                                                                        }),
                                Expires = DateTime.Now.AddDays(1000),
                                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JWT:Secret").Value)),
                                                                                                      SecurityAlgorithms.HmacSha256Signature)
                            };

                            var XKestrelHandler = new JwtSecurityTokenHandler();
                            var securityXKestrel = XKestrelHandler.CreateToken(XKestrelDescriptor);
                            var XKestrel = XKestrelHandler.WriteToken(securityXKestrel);

                            //on ajoute les infos de l'utilisateur dans un dictionnaire
                            dictionary.Add("XKestrel", XKestrel);
                            dictionary.Add("user_id", _Users.Id.ToString());
                            dictionary.Add("nom_complet_utilisateur", _Users.NomCompletUtilisateur);
                            dictionary.Add("user_role", _Users.Id.ToString());
                            dictionary.Add("role_libelle", _Users.Roles_name.ToString());
                            dictionary.Add("id_role", _Users.IdRole.ToString());
                            dictionary.Add("permissions", JsonConvert.SerializeObject(_Users.permissions));

                            dataResult.Message = "Authentifié";
                            dataResult.IsSucceed = true;
                        }
                        else
                        {
                            dataResult.Message = "Non authentifié";
                            dataResult.IsSucceed = false;
                        }

                        dataResult.Data = dictionary;
                    }
                }
            }
            catch (Exception ex)
            {
                //On définit le retour avec le détail de l'erreur
                dataResult.IsSucceed = false;
                dataResult.Message = (ex.InnerException == null) ? ex.Message : ex.InnerException.Message;

                //On log l'erreur
            }
            finally
            {

            }

            //On retourne le résultat
            return dataResult;
        }

        public async Task<DataSourceResult> PasswordForgotten(Object record)
        {
            // Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body = "";
            var dictionary = new Dictionary<string, string>();

            string email;
            long id;
            long time;

            try
            {
                // On enlève les caractères superflus
                record = record.ToString().Replace("ValueKind = Object : ", "");

                if (record == null || record.ToString().Length == 0)
                {
                    // On définit le retour avec le détail de l'erreur
                    dataResult.codeReponse = CodeReponse.error;
                    dataResult.msg = configuration.GetSection("MessagesAPI:ParamsEmpty").Value;
                }
                else
                {
                    body = JObject.Parse(record.ToString());

                    // On valide l'existence de l'ensemble des champs attendus dans le body
                    if (body.email == null)
                    {
                        // On définit le retour avec le détail de l'erreur
                        dataResult.codeReponse = CodeReponse.errorInvalidMissingParams;
                        dataResult.msg = configuration.GetSection("MessagesAPI:ParamsInvalid").Value;
                    }
                    else
                    {
                        email = (string)body.email;

                        // Si email non trouvé
                        if (string.IsNullOrEmpty(email))
                        {
                            // On définit le retour avec le détail de l'erreur
                            dataResult.codeReponse = CodeReponse.error;
                            dataResult.msg = configuration.GetSection("MessagesAPI:ParamsInvalid").Value + " email ne peut pas être vide.";
                        }
                        else
                        {
                            // On se connecte à la base de données
                            using (_context)
                            {
                                // On récupère l'objet de type Utilisateurs
                                var _Users = await _context.Users
                                                    .Where(x => x.Email == email && x.deleted_at == null)
                                                    .AsNoTracking()
                                                    .Select(p => new
                                                    {
                                                        p.Id,
                                                        p.NomCompletUtilisateur,
                                                        p.Email,
                                                        p.Password,
                                                        p.IdRole
                                                    })
                                                    .FirstOrDefaultAsync();

                                if (_Users == null)
                                {
                                    // On définit le retour avec le détail de l'erreur
                                    dataResult.codeReponse = CodeReponse.error;
                                    dataResult.msg = configuration.GetSection("MessagesAPI:UnknownRecord").Value;
                                }
                                else
                                {
                                    id = _Users.Id; // Récupération de l'ID de l'utilisateur

                                    time = DateTime.Now.Ticks + 77;

                                    string nouveauMotDePasse = GenererMotDePasseAleatoire(12); // Choisir la longueur du mot de passe

                                    var passwordHas = await _context.Users
                                        .Where(u => u.deleted_at == null && u.Id == id)
                                        .SingleOrDefaultAsync();

                                    if (id != 0 && passwordHas != null) // On modifie
                                    {
                                        passwordHas.Password = CryptageHelper.ByteArrayToBase64Hex(CryptageHelper.Encryption(nouveauMotDePasse, configuration.GetSection("Divers:EncryptionKey").Value));
                                        passwordHas.updated_at = DateTime.Now;

                                        _context.Users.Update(passwordHas);

                                        await _context.SaveChangesAsync();

                                        // On définit le résultat à retourner
                                        dataResult.data = nouveauMotDePasse;
                                        dataResult.codeReponse = CodeReponse.ok;
                                        dataResult.IsSucceed = true;
                                        dataResult.msg = "Mot de passe modifié avec succès";
                                    }

                                    // On définit le résultat à retourner
                                    dataResult.data = nouveauMotDePasse;
                                    dataResult.TotalRows = (_Users != null ? 1 : 0);
                                    dataResult.codeReponse = CodeReponse.ok;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // On définit le retour avec le détail de l'erreur
                dataResult.codeReponse = CodeReponse.error;
                dataResult.msg = (ex.InnerException == null) ? ex.Message : ex.InnerException.Message;
            }
            finally
            {
                // Code à exécuter en fin de traitement si nécessaire
            }

            // On retourne le résultat
            return dataResult;
        }


        private string GenererMotDePasseAleatoire(int longueur)
        {
            const string caracteresPossibles = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_+";

            Random random = new Random();
            char[] motDePasse = new char[longueur];

            for (int i = 0; i < longueur; i++)
            {
                motDePasse[i] = caracteresPossibles[random.Next(caracteresPossibles.Length)];
            }

            return new string(motDePasse);
        }


        public async Task<DataSourceResult> Search(Object record)
        {
            // Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body = "";

            int take;
            int skip;
            int? id_role;

            string field;
            string order;
            string nom_complet_utilisateur;
            string email;
            string code = "SP";

            try
            {
                // On enlève les caractères superflus
                record = record.ToString().Replace("ValueKind = Object : ", "");

                if (record == null || record.ToString().Length == 0)
                {
                    // On définit le retour avec le détail de l'erreur
                    dataResult.codeReponse = CodeReponse.error;
                    dataResult.msg = configuration.GetSection("MessagesAPI:ParamsEmpty").Value;
                }
                else
                {
                    body = JObject.Parse(record.ToString());

                    // On récupère les paramètres de recherche envoyés dans le body
                    nom_complet_utilisateur = (string)body.nom_complet_utilisateur;
                    email = (string)body.email;
                    id_role = (int?)body.id_role;

                    // On récupère les paramètres de pagination
                    take = Int32.Parse((String)body.take);
                    skip = Int32.Parse((String)body.skip);

                    // On récupère les paramètres de tri
                    field = (string)body.colone ?? "NomCompletUtilisateur";
                    order = (string)body.order ?? "DESC";

                    // On se connecte à la base de données
                    using (_context)
                    {
                        // On exécute la requête
                        var usersQuery = _context.Users
                            .Where(p => p.deleted_at == null && p.UserRole.Code != code 
                                && (string.IsNullOrEmpty(nom_complet_utilisateur) || p.NomCompletUtilisateur.ToLower().Contains(nom_complet_utilisateur.ToLower()))
                                && (string.IsNullOrEmpty(email) || p.Email.ToLower().Contains(email.ToLower()))
                                && (!id_role.HasValue || p.IdRole == id_role))
                            .AsNoTracking()
                            .Select(p => new
                            {
                                p.Id,
                                p.NomCompletUtilisateur,
                                p.Username,
                                p.Email,
                                Roles_name = p.UserRole.Name,
                                p.IdRole
                            });

                        // On ordonne dynamiquement
                        var orderedQuery = usersQuery.OrderByDynamic(field, order == "DESC" ? Order.Desc : Order.Asc);

                        // On récupère le nombre total d'enregistrements avant la pagination
                        var totalRows = await usersQuery.CountAsync();

                        // On applique la pagination
                        var resultTake = await orderedQuery.Skip(skip).Take(take).ToListAsync();

                        // On définit le résultat à retourner
                        dataResult.TotalRows = totalRows;
                        dataResult.NbRows = resultTake.Count;
                        dataResult.codeReponse = CodeReponse.ok;
                        dataResult.data = resultTake;
                        dataResult.msg = "success";
                    }
                }

            }
            catch (Exception ex)
            {
                // On définit le retour avec le détail de l'erreur
                dataResult.codeReponse = CodeReponse.error;
                dataResult.msg = (ex.InnerException == null) ? ex.Message : ex.InnerException.Message;

            }
            finally
            {
                // Nettoyage si nécessaire
            }

            // On retourne le résultat
            return dataResult;
        }


        public async Task<DataSourceResult> Insert(Object record)
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body;

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
                    //On récupère les données envoyées dans le body
                    body = JObject.Parse(record.ToString());

                    //On valide l'existence de l'ensemble des champs attendus dans le body
                    if (!IsValidInsert(body))
                    {
                        //On définit le résultat à retourner
                        dataResult.codeReponse = CodeReponse.errorInvalidMissingParams;
                        dataResult.msg = configuration.GetSection("MessagesAPI:ParamsInvalid").Value;
                    }
                    else
                    {
                        //On parse les informations envoyées dans le body
                        var data = new User
                        {
                            NomCompletUtilisateur = body.nom_complet_utilisateur,
                            Email = body.email,
                            Password = body.password.ToString() != "" ? CryptageHelper.ByteArrayToBase64Hex(CryptageHelper.Encryption(body.password.ToString(), configuration.GetSection("Divers:EncryptionKey").Value)) : "",
                            IdRole = body.id_role,
                            Username = body.username
                        };

                        //On track l'historique de modification
                        data.created_at = DateTime.Now;

                        //On se connecte à la base de données
                        using (_context)
                        {
                            //On ajoute l'objet dans le contexte de la base de données
                            await _context.Users.AddAsync(data);

                            //On enregistre dans la base de données
                            await _context.SaveChangesAsync();

                            //On définit le résultat à retourner
                            dataResult.codeReponse = CodeReponse.ok;
                            dataResult.msg = "Ajouté";
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                //On définit le retour avec le détail de l'erreur
                dataResult.codeReponse = CodeReponse.error;
                dataResult.msg = (ex.InnerException == null) ? ex.Message : ex.InnerException.Message;

                //On log l'erreur
            }
            finally
            {

            }

            //On retourne le résultat
            return dataResult;
        }


        public async Task<DataSourceResult> Update(Object record)
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body;
            int Id = 0;

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
                    //On récupère les données envoyées dans le body
                    body = JObject.Parse(record.ToString());

                    //On valide l'objet
                    if (!IsValidUpdate(body))
                    {
                        //On définit le résultat à retourner
                        dataResult.codeReponse = CodeReponse.errorInvalidMissingParams;
                        dataResult.msg = configuration.GetSection("MessagesAPI:ParamsInvalid").Value;
                    }
                    else
                    {
                        int.TryParse((string)body.Id, out Id);

                        //Si id non trouvé
                        if (Id == 0)
                        {
                            //On définit le retour avec le détail de l'erreur
                            dataResult.codeReponse = CodeReponse.error;
                            dataResult.msg = configuration.GetSection("MessagesAPI:ParamsInvalid").Value + " id doit être un entier.";
                        }
                        else
                        {
                            //On se connecte à la base de données
                            using (_context)
                            {
                                //On récupère l'enregistrement à partir de l'id
                                User Users = _context.Users.Where(p => p.deleted_at == null && p.Id == Id).FirstOrDefault();

                                //Si enregistrement trouvé
                                if (Users != null)
                                {
                                    //On parse les données envoyées dans le body
                                    Users.NomCompletUtilisateur = body.NomCompletUtilisateur;
                                    Users.Email = body.Email;
                                    Users.IdRole = body.IdRole;
                                    Users.Username = body.Username;


                                    //On track l'historique de modification
                                    Users.updated_at = DateTime.Now;

                                    //On enregistre en base de données
                                    await _context.SaveChangesAsync();

                                    //On définit le résultat à retourner
                                    dataResult.codeReponse = CodeReponse.ok;
                                    dataResult.msg = "Modifié";
                                }

                                else
                                {
                                    //On définit le résultat à retourner
                                    dataResult.codeReponse = CodeReponse.error;
                                    dataResult.msg = configuration.GetSection("MessagesAPI:UnknownRecord").Value; ;
                                }
                            }
                        }
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

            }

            //On retourne le résultat
            return dataResult;
        }


        public async Task<DataSourceResult> Delete(Object record)
        {
            //Déclaration des variables
            dynamic body;
            DataSourceResult dataResult = new DataSourceResult();
            int id;

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
                    //On récupère les données envoyées dans le body
                    body = JObject.Parse(record.ToString());

                    //On valide l'existence de l'ensemble des champs attendus dans le body
                    if (body.id == null)
                    {
                        //On définit le retour avec le détail de l'erreur
                        dataResult.codeReponse = CodeReponse.errorInvalidMissingParams;
                        dataResult.msg = configuration.GetSection("MessagesAPI:ParamsInvalid").Value;
                    }
                    else
                    {
                        int.TryParse((string)body.id, out id);

                        //Si id non trouvé
                        if (id == 0)
                        {
                            //On définit le retour avec le détail de l'erreur
                            dataResult.codeReponse = CodeReponse.error;
                            dataResult.msg = configuration.GetSection("MessagesAPI:ParamsInvalid").Value + " id doit être un entier.";
                        }
                        else
                        {
                            //On se connecte à la base de données
                            using (_context)
                            {
                                //On récupère l'enregistrement à partir de l'id
                                User Users = _context.Users
                                    .Where(p => p.deleted_at == null && p.Id == id)
                                    .FirstOrDefault();

                                if (Users != null)
                                {
                                    //On track l'historique de modification
                                    Users.deleted_at = DateTime.Now;

                                    //On enregistre en base de données
                                    await _context.SaveChangesAsync();

                                    //On définit le résultat à retourner
                                    dataResult.codeReponse = CodeReponse.ok;
                                    dataResult.msg = "Supprimé";
                                }
                                else
                                {
                                    //On définit le résultat à retourner
                                    dataResult.codeReponse = CodeReponse.error;
                                    dataResult.msg = configuration.GetSection("MessagesAPI:UnknownRecord").Value;
                                }
                            }
                        }
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

            }

            //On retourne le résultat
            return dataResult;
        }

        public async Task<DataSourceResult> UpdatePassWord(Object record)
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body = "";
            var dictionary = new Dictionary<string, string>();
            JArray Attachements = new JArray();

            string email;

            int id;

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
                    body = JObject.Parse(record.ToString());

                    //On valide l'existence de l'ensemble des champs attendus dans le body
                    if (body.newPassword == null)
                    {
                        //On définit le retour avec le détail de l'erreur
                        dataResult.codeReponse = CodeReponse.errorInvalidMissingParams;
                        dataResult.msg = configuration.GetSection("MessagesAPI:ParamsInvalid").Value;
                    }
                    else
                    {
                        email = (string)body.email;

                        //Si email non trouvé
                        if (email == "")
                        {
                            //On définit le retour avec le détail de l'erreur
                            dataResult.codeReponse = CodeReponse.error;
                            dataResult.msg = configuration.GetSection("MessagesAPI:ParamsInvalid").Value + " id doit être un entier.";
                        }
                        else
                        {
                            //les paramètres pour la pagiantion 
                            body = JObject.Parse(record.ToString());
                            id = int.Parse((string)body.id);

                            //On se connecte à la base de données
                            using (_context)
                            {
                                var passwordHas = await _context.Users
                                .Where(u => u.deleted_at == null && u.Id == id)
                                .SingleOrDefaultAsync();

                                if (id != 0 && passwordHas != null) //on  modifie
                                {
                                    passwordHas.Password = CryptageHelper.ByteArrayToBase64Hex(CryptageHelper.Encryption((string)body.newPassword, configuration.GetSection("Divers:EncryptionKey").Value));
                                    passwordHas.updated_at = DateTime.Now;

                                    _context.Users.Update(passwordHas);

                                    await _context.SaveChangesAsync();

                                    //On définit le résultat à retourner
                                    dataResult.codeReponse = CodeReponse.ok;
                                    dataResult.msg = "Mot de passe modifie avec succes";
                                }
                            }
                        }
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
            }

            //On retourne le résultat
            return dataResult;
        }



        public async Task<AuthServiceResponseDto> MakeAdminAsync(UpdatePermissionDto updatePermissionDto)
        {
            // Recherchez l'utilisateur par son nom d'utilisateur
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == updatePermissionDto.UserName);

            if (user == null)
            {
                return new AuthServiceResponseDto
                {
                    IsSucceed = false,
                    Message = "L'utilisateur n'a pas été trouvé."
                };
            }


            // Vérifiez si l'utilisateur a déjà le rôle d'administrateur
            if (user.IdRole == StaticUserRoles.ADMIN)
            {
                return new AuthServiceResponseDto
                {
                    IsSucceed = false,
                    Message = "L'utilisateur est déjà administrateur."
                };
            }

            // Mettez à jour le rôle de l'utilisateur pour le rendre administrateur
            user.IdRole = StaticUserRoles.ADMIN;
            user. updated_at = DateTime.Now;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return new AuthServiceResponseDto
            {
                IsSucceed = true,
                Message = "L'utilisateur a été promu administrateur avec succès."
            };
        }


        public async Task<AuthServiceResponseDto> MakeOwnerAsync(UpdatePermissionDto updatePermissionDto)
        {
            // Recherchez l'utilisateur par son nom d'utilisateur
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == updatePermissionDto.UserName);

            if (user == null)
            {
                return new AuthServiceResponseDto
                {
                    IsSucceed = false,
                    Message = "L'utilisateur n'a pas été trouvé."
                };
            }

            // Vérifiez si l'utilisateur a déjà le rôle de propriétaire
            if (user.IdRole == StaticUserRoles.OWNER)
            {
                return new AuthServiceResponseDto
                {
                    IsSucceed = false,
                    Message = "L'utilisateur est déjà propriétaire."
                };
            }

            // Mettez à jour le rôle de l'utilisateur pour le rendre propriétaire
            user.IdRole = StaticUserRoles.OWNER;
            user. updated_at = DateTime.Now;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return new AuthServiceResponseDto
            {
                IsSucceed = true,
                Message = "L'utilisateur a été promu propriétaire avec succès."
            };
        }


        public async Task<AuthServiceResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            // Vérifiez si un utilisateur avec le même nom d'utilisateur existe déjà
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == registerDto.UserName);

            if (existingUser != null)
            {
                return new AuthServiceResponseDto
                {
                    IsSucceed = false,
                    Message = "Ce nom d'utilisateur est déjà utilisé. Veuillez en choisir un autre."
                };
            }

            // Créez un nouvel utilisateur
            var newUser = new User
            {
                Username = registerDto.UserName,
                NomCompletUtilisateur = registerDto.LastName + registerDto.FirstName,
                Email = registerDto.Email,
                Password = registerDto.Password.ToString() != "" ? CryptageHelper.ByteArrayToBase64Hex(CryptageHelper.Encryption(registerDto.Password.ToString(), configuration.GetSection("Divers:EncryptionKey").Value)) : "",  // Assurez-vous de sécuriser le mot de passe correctement
                Token = "",  // Générez un jeton d'authentification si nécessaire
                IdRole = StaticUserRoles.USER,  // Affectez le rôle d'utilisateur par défaut
                created_at = DateTime.Now,  // Enregistrez la date et l'heure de création
                updated_at = null,  // Initialisez la date de mise à jour comme null
                deleted_at = null  // Initialisez la date de suppression comme null

             };

            // Ajoutez le nouvel utilisateur à la base de données
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return new AuthServiceResponseDto
            {
                IsSucceed = true,
                Message = "L'utilisateur a été enregistré avec succès."
            };
        }


        public async Task<AuthServiceResponseDto> SeedRolesAsync()
        {
            // Vérifiez d'abord si les rôles existent déjà dans la base de données
            var rolesExist = await _context.Roles.AnyAsync();

            if (rolesExist)
            {
                return new AuthServiceResponseDto
                {
                    IsSucceed = true,
                    Message = "Les rôles existent déjà dans la base de données."
                };
            }

            // Si les rôles n'existent pas, vous pouvez les créer ici
            // Vous pouvez créer les rôles en utilisant les objets Role que vous avez définis dans votre modèle

            var ownerRole = new Role
            {
                Name = "OWNER",
                Code = "OWNER",
                Created_at = DateTime.Now
            };

            var adminRole = new Role
            {
                Name = "ADMIN",
                Code = "ADMIN",
                Created_at = DateTime.Now
            };

            var userRole = new Role
            {
                Name = "USER",
                Code = "USER",
                Created_at = DateTime.Now
            };

            // Ajoutez les rôles à la base de données
            _context.Roles.Add(ownerRole);
            _context.Roles.Add(adminRole);
            _context.Roles.Add(userRole);

            // Enregistrez les modifications dans la base de données
            await _context.SaveChangesAsync();

            return new AuthServiceResponseDto
            {
                IsSucceed = true,
                Message = "Les rôles ont été semés avec succès dans la base de données."
            };
        }



        /// <summary>
        /// Méthode de validation de de l'existence des paramètres d'insertion de l'entité de type Utilisateurs
        /// </summary>
        /// <param name="body">Paramètre d'aappel de l'api</param>
        /// <returns></returns>
        private bool IsValidInsert(dynamic body)
        {

            bool result = true;

            if (body.nom_complet_utilisateur == null)
            {
                result = false;
            }


            if (body.email == null)
            {
                result = false;
            }

            if (body.password == null)
            {
                result = false;
            }

            if (body.id_role == null)
            {
                result = false;
            }

            if (body.username == null)
            {
                result = false;
            }


            //On retourne le résultat
            return result;
        }

        /// <summary>
        /// Méthode de validation de l'existence des paramètres d'update de l'entité de type Utilisateurs
        /// </summary>
        /// <param name="body">Paramètre d'aappel de l'api</param>
        /// <returns></returns>
        private bool IsValidUpdate(dynamic body)
        {
            //Déclaration des variables
            bool result = true;

            if (body.NomCompletUtilisateur == null)
            {
                result = false;
            }

            if (body.Email == null)
            {
                result = false;
            }

            if (body.IdRole == null)
            {
                result = false;
            }

            if (body.Username == null)
            {
                result = false;
            }

            //On retourne le résultat
            return result;
        }



    }
}
