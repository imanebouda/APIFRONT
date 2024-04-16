using ITKANSys_api.Config;
using ITKANSys_api.Models.Entities;
using ITKANSys_api.Utility.ApiResponse;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Reflection;
using System;
using Microsoft.EntityFrameworkCore;

using ITKANSys_api.Interfaces;

namespace ITKANSys_api.Services
{
    public class OrganismeService : IOrganismeService
    {
        //Gestionnaire de paramètres
        private IConfiguration configuration;

        //Contexte de la base de données
        private ApplicationDbContext _ctx;
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public OrganismeService(ApplicationDbContext context )
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
                    var Organismes = await _ctx.Organismes
                        .Where(p => p.deleted_at == null)
                        .AsNoTracking()
                        .Select(p => new
                        {
                            p.id,
                            p.code,
                            p.address,
                            p.approval,
                            p.phone,
                            p.gsm,
                            p.city,
                            p.email,
                            p.logo,
                            p.social_reason,
                            open_date = p.open_date,
                            p.brand,
                            p.TypeOrganisme.label,
                        })
                        .OrderBy(x => x.code)
                        .ToListAsync();

                    //On définit le résultat à retourner
                    dataResult.codeReponse = CodeReponse.ok;
                    dataResult.data = Organismes;
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
            string code;
            string email;

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
                    code = (body.code != "" ? (string)body.code : null);
                    email = (body.email != "" ? (string)body.email : null);

                    //On récupère les paramètres de pagination
                    take = Int32.Parse((String)body.take);
                    skip = Int32.Parse((String)body.skip);

                    //On récupère les paramètres de tri
                    field = (body.field != null ? body.field : "code");
                    order = (body.order != null ? body.order : "DESC");

                    //On se connecte à la base de données
                    using (_ctx)
                    {
                        //On exécute la requête
                        var Organismes = await _ctx.Organismes
                            .Where(p => p.deleted_at == null
                             && (code == null || (code != null && p.code.ToLower().Contains(code.ToLower())))
                             && (email == null || (email != null && p.email.ToLower().Contains(email.ToLower()))))
                            .AsNoTracking()
                            .Select(p => new
                            {
                                p.id,
                                p.code,
                                p.brand,
                                p.address,
                                p.approval,
                                p.phone,
                                p.gsm,
                                p.city,
                                p.email,
                                p.logo,
                                p.social_reason,
                                p.zip_code,
                                p.TypeOrganisme.label,
                                p.id_type_organisme,
                                open_date = p.open_date,
                            })
                            .OrderByDynamic(field, order == "DESC" ? QueryableExtensions.Order.Desc : QueryableExtensions.Order.Asc)
                            .ToListAsync();

                        var resultTake = Organismes.Skip(skip).Take(take);

                        //On définit le résultat à retourner
                        dataResult.TotalRows = Organismes.Count();
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

        public async Task<DataSourceResult> Load(Object record)
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body = "";
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
                                //On récupère l'objet de type Organisme
                                var _Organisme = await _ctx.Organismes
                                                    .Where(x => x.id == id && x.deleted_at == null)
                                                    .AsNoTracking()
                                                    .Select(p => new
                                                    {
                                                        p.id,
                                                        p.code,
                                                        p.address,
                                                        p.approval,
                                                        p.phone,
                                                        p.gsm,
                                                        p.city,
                                                        p.email,
                                                        p.logo,
                                                        p.social_reason,
                                                        open_date = p.open_date,
                                                        p.brand,
                                                        p.TypeOrganisme.label
                                                    })
                                                    .FirstOrDefaultAsync();

                                if (_Organisme == null)
                                {
                                    //On définit le retour avec le détail de l'erreur
                                    dataResult.codeReponse = CodeReponse.error;
                                    dataResult.msg = configuration.GetSection("MessagesAPI:UnknownRecord").Value;
                                }
                                else
                                {
                                    //On définit le résultat à retourner
                                    dataResult.TotalRows = (_Organisme != null ? 1 : 0);
                                    dataResult.codeReponse = CodeReponse.ok;
                                    dataResult.data = JsonConvert.SerializeObject(_Organisme);
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

        public async Task<DataSourceResult> CurrentOrganisme()
        {
            // Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                // On se connecte à la base de données
                using (_ctx)
                {
                    // On récupère l'objet de type Organisme
                    var _Organisme = await _ctx.Organismes
                        .Where(x => x.deleted_at == null)
                        .AsNoTracking()
                        .OrderByDescending(p => p.id)
                        .Select(p => new
                        {
                            p.id,
                            p.code,
                            p.brand,
                            p.address,
                            p.zip_code,
                            p.approval,
                            p.phone,
                            p.gsm,
                            p.city,
                            p.email,
                            p.social_reason,
                            TypeOrganismeLabel = p.TypeOrganisme.label, // Utilisation d'un alias pour éviter les erreurs liées aux valeurs null
                            p.id_type_organisme,
                            open_date = p.open_date
                        })
                        .LastOrDefaultAsync();

                    if (_Organisme == null)
                    {
                        // On définit le retour avec le détail de l'erreur
                        dataResult.codeReponse = CodeReponse.error;
                        dataResult.msg = configuration.GetSection("MessagesAPI:UnknownRecord").Value;
                    }
                    else
                    {
                        // On définit le résultat à retourner
                        dataResult.TotalRows = 1;
                        dataResult.codeReponse = CodeReponse.ok;
                        dataResult.data = JsonConvert.SerializeObject(_Organisme);
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
                // Éventuellement, d'autres opérations de nettoyage
            }

            // On retourne le résultat
            return dataResult;
        }

        public async Task<DataSourceResult> Save(Object record)
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body;
            int id = 0;
            DateTime created_at = DateTime.Now;

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
                        int.TryParse((string)body.id, out id);
                        Organisme organisme = _ctx.Organismes.Where(p => p.deleted_at == null && p.id == id).FirstOrDefault();

                        if (organisme == null)
                        {
                            organisme = new Organisme();
                        }
                        else
                        {
                            created_at = organisme.created_at;
                        };

                        if (body.id_type_organisme != null)
                            body.id_type_organisme = body.id_type_organisme;

                            organisme.id_type_organisme = (int)body.id_type_organisme;
                            organisme.code = body.code;
                            organisme.brand = body.brand;
                            organisme.social_reason = body.social_reason;
                            organisme.approval = body.approval;
                            organisme.address = body.address;
                            organisme.zip_code = body.zip_code;
                            organisme.city = body.city;
                            organisme.phone = body.phone;
                            organisme.gsm = body.gsm;
                            organisme.email = body.email;
                            organisme.logo = "img";
                            organisme.open_date = body.open_date;
                            organisme.created_at = (organisme.id == 0) ? DateTime.Now : created_at;
                            organisme.updated_at = DateTime.Now;

                        if (organisme.id == 0)
                        {
                            organisme.created_at = DateTime.Now;
                            _ctx.Organismes.Add(organisme);
                            dataResult.msg = "ajouter";
                        }
                        else
                        {

                            organisme.created_at = created_at;
                            organisme.updated_at = DateTime.Now;
                            _ctx.Organismes.Update(organisme);
                            dataResult.msg = "modifier";
                        }
                        //On track l'historique de modification

                        await _ctx.SaveChangesAsync();
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
                                Organisme Organisme = _ctx.Organismes
                                    .Where(p => p.deleted_at == null && p.id == id)
                                    .FirstOrDefault();

                                if (Organisme != null)
                                {
                                    //On track l'historique de modification
                                    Organisme.deleted_at = DateTime.Now;

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
        /// <param name="body">Paramètre d'aappel de l'api</param>
        /// <returns></returns>
        private bool IsValidInsert(dynamic body)
        {
            //Déclaration des variables
            bool result = true;

            if (body.code == null)
            {
                result = false;
            }

            if (body.address == null)
            {
                result = false;
            }

            if (body.approval == null)
            {
                result = false;
            }

            if (body.phone == null)
            {
                result = false;
            }

            if (body.gsm == null)
            {
                result = false;
            }

            if (body.city == null)
            {
                result = false;
            }

            if (body.email == null)
            {
                result = false;
            }

            if (body.social_reason == null)
            {
                result = false;
            }

            if (body.open_date == null)
            {
                result = false;
            }

            //On retourne le résultat
            return result;
        }
    }
}
