using ITKANSys_api.Config;
using ITKANSys_api.Interfaces;
using ITKANSys_api.Models;
using ITKANSys_api.Models.Dtos;
using ITKANSys_api.Models.Entities;
using ITKANSys_api.Utility.ApiResponse;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using static ITKANSys_api.Utility.ApiResponse.QueryableExtensions;

namespace ITKANSys_api.Services
{
    public class IndicateurService : IIndicateurService 
    {
        private IConfiguration configuration;

        //Contexte de la base de données
        private ApplicationDbContext _ctx;
        /// <summary>
        /// Constructeur par défaut
        /// < /summary>
        public IndicateurService(ApplicationDbContext context)
        {
            configuration = AppConfig.GetConfig();
            _ctx = context;
        }

        public async Task<DataSourceResult> GetAllIndicateurs(int ID)
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                //On se connecte à la base de données
                using (_ctx)
                {
                    //On récupère les données de la base de données
                    var Ddp = await _ctx.Indicateurs
                        .Where(p => p.deleted_at == null && p.ProcessusID == ID)
                        .OrderBy(p => p.created_at) // Triez par date de création
                        .Select(p => new
                        {
                            p.ID,
                            p.Libelle,
                            p.Cible,
                            p.Formule,
                            p.Frequence,
                            p.ProcessusID,
                            p.Tolerance,
                            p.created_at
                        })
                        .ToListAsync();
                    dataResult.codeReponse = CodeReponse.ok;
                    dataResult.data = Ddp;
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

        public async Task<DataSourceResult> InsertIndicateurs(object insertIndicateurs)
        {
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body;

            try
            {
                string jsonString = insertIndicateurs.ToString();
                jsonString = jsonString.Replace("ValueKind = Object : ", "");

                if (string.IsNullOrEmpty(jsonString))
                {
                    dataResult.codeReponse = CodeReponse.errorMissingAllParams;
                    dataResult.msg = configuration.GetSection("MessagesAPI:ParamsEmpty").Value;
                }
                else
                {
                    body = JObject.Parse(jsonString);
                    var ProcessusID = Convert.ToInt32(body.ProcessusID);
                    using (_ctx)
                    {
                        if (body.ProcessusID != null)
                        {
                            var Indicateurs = new Indicateurs
                            {
                                ProcessusID = ProcessusID,
                                Libelle = body.Libelle,
                                Cible = body.Cible,
                                Formule =body.Formule,
                                Frequence = body.Frequence,
                                Tolerance = body.Tolerance,
                                created_at = DateTime.Now
                            };
                            await _ctx.Indicateurs.AddAsync(Indicateurs);
                            await _ctx.SaveChangesAsync();
                            dataResult.codeReponse = CodeReponse.ok;
                            dataResult.msg = "Ajouté";
                        }
                        else
                        {
                            dataResult.codeReponse = CodeReponse.erreur;
                            dataResult.msg = configuration.GetSection("MessagesAPI:ParamsEmpty").Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dataResult.IsSucceed = true;
                dataResult.Message = (ex.InnerException == null) ? ex.Message : ex.InnerException.Message;
                // Log l'erreur si nécessaire.
            }
            return dataResult;
        }



        public class PRCItem
        {
            public int ID { get; set; }
            public string Libelle { get; set; }
            public int ProcessusID { get; set; }
            public double Cible { get; set; }
            public string Formule { get; set; }
            public string Frequence { get; set; }
            public double Tolerance { get; set; }
            public Processus Processus { get; set; }
            public DateTime Date { get; set; }
        }

        public async Task<DataSourceResult> RechercheIndicateurs(int id, string? titre,string ? frequence, DateTime? dateDebut, DateTime? dateFin, string? field, string? order, int take, int skip)
        {
            try
            {
                if (id != 0)
                {
                    // Assurez-vous que les valeurs par défaut sont correctes
                    take = take != 0 ? take : 10;
                    skip = skip != 0 ? skip : 0;
                    // Assurez-vous que les valeurs "field" et "order" sont définies
                    field = !string.IsNullOrEmpty(field) ? field : "ID";
                    order = !string.IsNullOrEmpty(order) ? order : "DESC";
                    var dateDebutString = dateDebut?.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
                    var dateFinString = dateFin?.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
                    // Assurez-vous que "order" est "ASC" ou "DESC"
                    Order direction = (order.ToUpper() == "DESC") ? Order.Desc : Order.Asc;

                    // Exécutez la requête
                    var result = await _ctx.Indicateurs
                        .Where(p => p.deleted_at == null && p.ProcessusID == id
                            && (string.IsNullOrEmpty(titre) || p.Libelle.ToLower().Contains(titre.ToLower()))
                              && (string.IsNullOrEmpty(frequence) || p.Frequence.ToLower().Contains(frequence.ToLower()))
                            && (!dateDebut.HasValue || p.created_at >= dateDebut)
                            && (!dateFin.HasValue || p.created_at <= dateFin))

                        .Select(p => new PRCItem
                        {
                            ID = p.ID,
                            ProcessusID = p.ProcessusID,
                            Libelle = p.Libelle,
                            Cible = (float)p.Cible,
                            Formule = p.Formule,
                            Frequence = p.Frequence,
                            Tolerance = (float)p.Tolerance,
                            Date = p.created_at,
                            Processus = p.Processus,
                        })
                        .OrderByDynamic(field, direction)
                        .ToListAsync();

                    var totalRows = result.Count();
                    var resultTake = result.Skip(skip).Take(take).ToList();

                    // Créez la réponse
                    var response = new DataSourceResult
                    {
                        TotalRows = totalRows,
                        NbRows = resultTake.Count(),
                        IsSucceed = true,
                        Message = "success",
                        data = resultTake
                    };

                    return response;

                }
                else
                {
                    // En cas d'erreur, créez une réponse d'erreur
                    var Response = new DataSourceResult
                    {
                        IsSucceed = false,
                        Message = "id processus introuvable ",
                    };

                    // Log l'erreur

                    // Retournez la réponse d'erreur
                    return Response;
                }

            }
            catch (Exception ex)
            {
                // En cas d'erreur, créez une réponse d'erreur
                var errorResponse = new DataSourceResult
                {
                    IsSucceed = false,
                    Message = (ex.InnerException == null) ? ex.Message : ex.InnerException.Message,
                };

                // Log l'erreur

                // Retournez la réponse d'erreur
                return errorResponse;
            }
        }

        public async Task<DataSourceResult> UpdateIndicateurs(object Indicateurs)
        {
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body;

            try
            {
                string jsonString = Indicateurs.ToString();
                jsonString = jsonString.Replace("ValueKind = Object : ", "");

                if (string.IsNullOrEmpty(jsonString))
                {
                    dataResult.codeReponse = CodeReponse.errorMissingAllParams;
                    dataResult.msg = configuration.GetSection("MessagesAPI:ParamsEmpty").Value;
                }
                else
                {
                    body = JObject.Parse(jsonString);
                    using (_ctx)
                    {
                        if (body.ID != null && body.ProcessusID != null)
                        {

                            var IndicateursID = Convert.ToInt32(body.ID);
                            var ProcessusID = Convert.ToInt32(body.ProcessusID);


                            var existingProcessus = await _ctx.Processus.FindAsync(ProcessusID);
                            var existingIndicateurs = await _ctx.Indicateurs.FindAsync(IndicateursID);


                            if (existingProcessus != null && existingIndicateurs != null)
                            {
                                existingIndicateurs.Libelle = (string)body.Libelle;
                                existingIndicateurs.Frequence = (string)body.Frequence;
                                existingIndicateurs.ProcessusID = ProcessusID;
                                existingIndicateurs.Cible = (float)body.Cible;
                                existingIndicateurs.Formule = (string)body.Formule;
                                existingIndicateurs.Tolerance = (float)body.Tolerance;
                                existingIndicateurs.updated_at = DateTime.Now;

                                await _ctx.SaveChangesAsync();

                                dataResult.codeReponse = CodeReponse.ok;
                                dataResult.msg = "Le processus Objectifs a été mis à jour avec succès.";
                            }
                            else
                            {
                                dataResult.codeReponse = CodeReponse.erreur;
                                dataResult.msg = "Le processus Objectifs n'a pas été trouvé dans la base de données.";
                            }
                        }
                        else
                        {
                            dataResult.codeReponse = CodeReponse.erreur;
                            dataResult.msg = configuration.GetSection("MessagesAPI:ParamsEmpty").Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dataResult.IsSucceed = false;
                dataResult.Message = (ex.InnerException == null) ? ex.Message : ex.InnerException.Message;
                // Log l'erreur si nécessaire.
            }

            return dataResult;
        }


        public async Task<DataSourceResult> SupprimerIndicateurs(object Indicateurs)
        {
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body;

            try
            {
                string jsonString = Indicateurs.ToString();
                jsonString = jsonString.Replace("ValueKind = Object : ", "");

                if (string.IsNullOrEmpty(jsonString))
                {
                    dataResult.codeReponse = CodeReponse.errorMissingAllParams;
                    dataResult.msg = configuration.GetSection("MessagesAPI:ParamsEmpty").Value;
                }
                else
                {
                    body = JObject.Parse(jsonString);
                    using (_ctx)
                    {

                        if (body.ID != null)
                        {
                            var IndicateursID = Convert.ToInt32(body.ID);


                            var existingIndicateurs = await _ctx.Indicateurs.FindAsync(IndicateursID);

                            // Marquez le processus comme supprimé (selon votre logique de suppression)
                            existingIndicateurs.deleted_at = DateTime.Now;

                            // Utilisez la méthode Remove pour marquer l'entité comme supprimée
                            _ctx.Indicateurs.Remove(existingIndicateurs);

                            // Enregistrez les modifications dans la base de données
                            await _ctx.SaveChangesAsync();
                            dataResult.codeReponse = CodeReponse.ok;
                            dataResult.msg = "Supprimé";

                        }
                        else
                        {
                            dataResult.codeReponse = CodeReponse.erreur;
                            dataResult.msg = configuration.GetSection("MessagesAPI:ParamsEmpty").Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var errorResponse = new DataSourceResult
                {
                    IsSucceed = false,
                    Message = (ex.InnerException == null) ? ex.Message : ex.InnerException.Message,
                };

                // Log l'erreur si nécessaire.

                return errorResponse;
            }
            return dataResult;
        }


        public async Task<GetDataResult> DetailIndicateur(int ID)
        {
            GetDataResult dataResult = new GetDataResult();

            try
            {

                if (ID <= 0)
                {
                    dataResult.IsSucceed = false;
                    dataResult.Message = "Données non trouvées.";
                }
                else
                {
                    using (_ctx)
                    {
                        var Indicateurs = await _ctx.Indicateurs.FindAsync(ID);

                        if (Indicateurs != null)
                        {

                            var existingProcessus = await _ctx.Indicateurs.
                                Where(p => p.ID == ID && p.deleted_at == null)
                                .Select(p => new IndicateursDto
                                {
                                    ID = p.ID,
                                    Cible = p.Cible,
                                    Libelle = p.Libelle,
                                    Tolerance = p.Tolerance,
                                    Formule = p.Formule,
                                    Frequence = p.Frequence,
                                    created_at = p.created_at,
                                    Processus = p.Processus
                                }).FirstOrDefaultAsync(); ;


                            if (existingProcessus != null)
                            {
                                dataResult.IsSucceed = true;
                                dataResult.Message = "Detaill";
                                dataResult.DataIndicateur = existingProcessus;

                            }
                            else
                            {
                                dataResult.IsSucceed = false;
                                dataResult.Message = "L'un des éléments spécifiés n'a pas été trouvé.";
                            }
                        }
                        else
                        {
                            dataResult.IsSucceed = false;
                            dataResult.Message = configuration.GetSection("MessagesAPI:ParamsEmpty").Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var errorResponse = new GetDataResult
                {
                    IsSucceed = false,
                    Message = (ex.InnerException == null) ? ex.Message : ex.InnerException.Message,
                };

                // Enregistrer l'erreur si nécessaire.

                return errorResponse;
            }
            return dataResult;
        }

    }
}
