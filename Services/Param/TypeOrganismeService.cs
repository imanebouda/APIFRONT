using ITKANSys_api.Config;
using ITKANSys_api.Models.Entities.Param;
using ITKANSys_api.Utility.ApiResponse;
using ITKANSys_api.Utility.Auth;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System;
using Microsoft.EntityFrameworkCore;
using ITKANSys_api.Interfaces;

namespace ITKANSys_api.Services.Param
{
    public class TypeOrganismeService : ITypeOrganismeService 
    {
        //Gestionnaire de paramètres
        private IConfiguration configuration;

        //Contexte de la base de données
        private ApplicationDbContext _ctx;
        private readonly IUserHelper _userHelper;
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public TypeOrganismeService(ApplicationDbContext context)
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
                    var TypeOds = await _ctx.TypeOrganisme
                        .Where(p => p.deleted_at == null)
                        .AsNoTracking()
                        .OrderBy(x => x.label)
                        .ToListAsync();

                    //On définit le résultat à retourner
                    dataResult.codeReponse = CodeReponse.ok;
                    dataResult.data = TypeOds;
                    dataResult.msg = "success";
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

        public async Task<DataSourceResult> Search(Object record)
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body = "";

            int take;
            int skip;

            string field;
            string order;
            string label;

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

                    //On récupère les paramètres de recherche envoyés dans le body
                    label = (body.label != "" ? (string)body.label : null);

                    //On récupère les paramètres de pagination
                    take = Int32.Parse((String)body.take);
                    skip = Int32.Parse((String)body.skip);

                    //On récupère les paramètres de tri
                    field = (body.field != null ? body.field : "label");
                    order = (body.order != null ? body.order : "DESC");

                    //On se connecte à la base de données
                    using (_ctx)
                    {
                        //On exécute la requête
                        var TypeOds = await _ctx.TypeOrganisme
                            .Where(p => p.deleted_at == null
                             && (label == null || (label != null && p.label.ToLower().Contains(label.ToLower()))))
                            .AsNoTracking()
                            .OrderByDynamic(field, order == "DESC" ? QueryableExtensions.Order.Desc : QueryableExtensions.Order.Asc)
                            .ToListAsync();

                        var resultTake = TypeOds.Skip(skip).Take(take);

                        //On définit le résultat à retourner
                        dataResult.TotalRows = TypeOds.Count();
                        dataResult.NbRows = resultTake.Count();
                        dataResult.codeReponse = CodeReponse.ok;
                        dataResult.data = resultTake;
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
                        var data = new TypeOrganisme
                        {
                            label = body.label,
                        };

                        //On track l'historique de modification
                        data.created_at = DateTime.Now;

                        //On se connecte à la base de données
                        using (_ctx)
                        {
                            //On ajoute l'objet dans le contexte de la base de données
                            await _ctx.TypeOrganisme.AddAsync(data);

                            //On enregistre dans la base de données
                            await _ctx.SaveChangesAsync();

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
            int id = 0;

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
                    if (!IsValidInsert(body))
                    {
                        //On définit le résultat à retourner
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
                                TypeOrganisme TypeOds = _ctx.TypeOrganisme.Where(p => p.deleted_at == null && p.id == id).FirstOrDefault();

                                //Si enregistrement trouvé
                                if (TypeOds != null)
                                {
                                    //On parse les données envoyées dans le body
                                    TypeOds.label = body.label;

                                    //On track l'historique de modification
                                    TypeOds.updated_at = DateTime.Now;

                                    //On enregistre en base de données
                                    await _ctx.SaveChangesAsync();

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
                            using (_ctx)
                            {
                                //On récupère l'enregistrement à partir de l'id
                                TypeOrganisme TypeOds = _ctx.TypeOrganisme
                                    .Where(p => p.deleted_at == null && p.id == id)
                                    .FirstOrDefault();

                                if (TypeOds != null)
                                {
                                    //On track l'historique de modification
                                    TypeOds.deleted_at = DateTime.Now;

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


        /// <summary>
        /// Méthode de validation de de l'existence des paramètres d'insertion de l'entité de type Organisme 
        /// </summary>
        /// <param name="body">Paramètre d'appel de l'api</param>
        /// <returns></returns>
        private bool IsValidInsert(dynamic body)
        {
            //Déclaration des variables
            bool result = true;

            if (body.label == null)
            {
                result = false;
            }

            //On retourne le résultat
            return result;
        }
    }
}
