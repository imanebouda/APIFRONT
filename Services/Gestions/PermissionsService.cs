using ITKANSys_api.Config;
using ITKANSys_api.Models;
using ITKANSys_api.Utility.ApiResponse;
using ITKANSys_api.Utility.Auth;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System;
using Microsoft.EntityFrameworkCore;
using ITKANSys_api.Interfaces;

namespace ITKANSys_api.Services.Gestions
{
    /// <summary>
    /// Service permettant de gérer les objets de type Permissions
    /// </summary>
    public class PermissionsService  : IPermissionsService
    {
        //Gestionnaire de paramètres
        private IConfiguration configuration;

        //Contexte de la base de données
        private ApplicationDbContext _ctx;
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public PermissionsService(ApplicationDbContext context)
        {
            configuration = AppConfig.GetConfig();
            _ctx = context;
        }

        public async Task<DataSourceResult> GetAll()
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                //On se connecte à la base de données
                using (_ctx)
                {
                    //On récupère les données de la base de données
                    var Permissionss = await _ctx.Permissions
                        .Where(p => p.deleted_at == null)
                        .AsNoTracking()
                        .Select(p => new
                        {
                            p.Id,
                            p.Controller,
                            p.Methode
                        })
                        .OrderBy(x => x.Controller)
                        .ToListAsync();
                    //On définit le résultat à retourner
                    dataResult.codeReponse = CodeReponse.ok;
                    dataResult.data = Permissionss;
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

        public async Task<DataSourceResult> GetAllByRole(Object record)
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body = "";

            int id_role = 0;
            string controller;
            try
            {
                //On récupère les données envoyées dans le body
                body = JObject.Parse(record.ToString());

                //On récupère les paramètres de recherche envoyés dans le body
                controller = (body.controller != "" ? (string)body.controller : null);

                int.TryParse((string)body.id_role, out id_role);

                //On se connecte à la base de données
                using (_ctx)
                {
                    //On valide l'existence de l'ensemble des champs attendus dans le body
                    if (id_role == 0)
                    {
                        //On définit le retour avec le détail de l'erreur
                        dataResult.codeReponse = CodeReponse.errorInvalidMissingParams;
                        dataResult.msg = configuration.GetSection("MessagesAPI:ParamsInvalid").Value;
                    }
                    else
                    {
                        //On récupère les données de la base de données
                        var permission_users = await _ctx.Permissions
                            .Where(p => p.deleted_at == null
                            && (controller == null || (controller != null && p.Controller.ToLower().Contains(controller.ToLower()))))
                            .AsNoTracking()
                            .Select(p => new
                            {
                                p.Id,
                                name = p.Controller + "-" + p.Methode,
                                haspermiss = _ctx.PermissionRoles.Where(rp => rp.RoleId == id_role && rp.PermissionId == p.Id && rp.deleted_at == null).Any(),
                            })
                            .OrderBy(x => x.name)
                            .ToListAsync();

                        //On définit le résultat à retourner
                        dataResult.codeReponse = CodeReponse.ok;
                        dataResult.data = permission_users;
                        dataResult.msg = "success";
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

        public async Task<DataSourceResult> Save(Object record)
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
                    int id_role = body.id_role;

                    int id_role_controller = int.Parse(configuration.GetSection("Roles:Controller:SuperAdmin").Value.ToString());
                    List<PermissionRole> permissions = new List<PermissionRole>();
                    if (id_role == id_role_controller)
                    {
                        int id_permession_consulter = int.Parse(configuration.GetSection("Permessions:inspection").Value.ToString());
                        permissions = _ctx.PermissionRoles.Where(p => p.RoleId == id_role && p.PermissionId != id_permession_consulter).ToList();
                    }
                    else
                    {
                        permissions = _ctx.PermissionRoles.Where(p => p.RoleId == id_role).ToList();
                    }

                    _ctx.PermissionRoles.RemoveRange(permissions);

                    if (body.access.Count > 0)
                    {
                        foreach (var item in body.access)
                        {
                            int id_permission = item.Id;

                            //On récupère les données de la base de données
                            PermissionRole permissionAdd = new PermissionRole
                            {
                                RoleId = id_role,
                                PermissionId = id_permission,
                                created_at = DateTime.Now
                            };

                            await _ctx.PermissionRoles.AddAsync(permissionAdd);


                        }
                    }

                    await _ctx.SaveChangesAsync();
                    dataResult.msg = "Ajouté";
                    dataResult.codeReponse = CodeReponse.ok;


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
                            using (_ctx)
                            {
                                //On récupère l'enregistrement à partir de l'id
                                PermissionRole permission_users = _ctx.PermissionRoles
                                    .Where(p => p.deleted_at == null && p.Id == id)
                                    .FirstOrDefault();

                                if (permission_users != null)
                                {
                                    //On track l'historique de modification
                                    permission_users.deleted_at = DateTime.Now;

                                    //On enregistre en base de données
                                    await _ctx.SaveChangesAsync();

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
    }
}
