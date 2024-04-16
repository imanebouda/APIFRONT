using ITKANSys_api.Config;
using ITKANSys_api.Data.Dtos;
using ITKANSys_api.Interfaces;
using ITKANSys_api.Models;

using ITKANSys_api.Models.Dtos;
using ITKANSys_api.Models.Entities;
using ITKANSys_api.Utility.ApiResponse;

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static ITKANSys_api.Utility.ApiResponse.QueryableExtensions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ITKANSys_api.Services
{
    public class ResultatsIndicateurService : IResultatsIndicateurService
    {
        private IConfiguration configuration;

        //Contexte de la base de données
        private ApplicationDbContext _ctx;
        /// <summary>
        /// Constructeur par défaut
        /// < /summary>
        public ResultatsIndicateurService(ApplicationDbContext context)
        {
            configuration = AppConfig.GetConfig();
            _ctx = context;
        }

        public async Task<DataSourceResult> GetAllResultByIdIndicateur(int Annee)
        {
            // Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                if (Annee <= 0)
                {
                    dataResult.IsSucceed = false;
                    dataResult.Message = "Données non trouvées.";
                }
                else
                {
                    // On se connecte à la base de données
                    using (_ctx)
                    {

                        var query = from result in _ctx.ResultatsIndicateurs
                                    join indicateur in _ctx.Indicateurs on result.IndicateurID equals indicateur.ID
                                    join processus in _ctx.Processus on indicateur.ProcessusID equals processus.ID
                                    where result.Annee == Annee
                                    group result by new
                                    {
                                        Annee = result.Annee,
                                        Processus = processus.Libelle,
                                        Pilote =processus.Pilote,
                                        CoPilote=processus.CoPilote,
                                        Indicateur = indicateur.Libelle,
                                        IndicateurID = indicateur.ID,
                                        indicateur.Cible,
                                        indicateur.Frequence,
                                        Periode = result.Periode,
                                        resultID =result.ID
                                    } into groupedResult
                                    select new
                                    {
                                        groupedResult.Key.Annee,
                                        groupedResult.Key.Processus,
                                        groupedResult.Key.Pilote,
                                        groupedResult.Key.CoPilote,
                                        groupedResult.Key.Indicateur,
                                        groupedResult.Key.IndicateurID,
                                        groupedResult.Key.Cible,
                                        groupedResult.Key.Frequence,
                                        Resultats = groupedResult
                                            .GroupBy(gr => gr.Periode)
                                            .Select(gr => new
                                            {
                                                Periode = gr.Key,
                                                Resultat = gr.Max(x => x.Resultat),
                                                ResultatID = gr.First().ID
                                            })
                                            .OrderBy(gr => gr.Periode)
                                            .ToList(),
                                    };

                        var resultats = await query.ToListAsync();
                        var groupedQuery = resultats
                            .GroupBy(item => new
                            {
                                item.Annee,
                                item.Processus,
                                item.Pilote,
                                item.CoPilote,
                                item.Indicateur,
                                item.IndicateurID,
                                item.Cible,
                                item.Frequence
                            })
                            .Select(group => new
                            {
                                group.Key.Annee,
                                group.Key.Processus,
                                group.Key.Pilote,
                                group.Key.CoPilote,
                                group.Key.Indicateur,
                                group.Key.IndicateurID,
                                group.Key.Cible,
                                group.Key.Frequence,
                                Resultats = group.SelectMany(item => item.Resultats).ToList(),
                            });

                        dataResult.codeReponse = CodeReponse.ok;
                        dataResult.data = groupedQuery.ToList();



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
                // Libérez les ressources si nécessaire
            }

            // On retourne le résultat
            return dataResult;
        }


        public async Task<DataSourceResult> InsertResultIndicateur(object insertResultIndicateur)
        {
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body;

            try
            {
                string jsonString = insertResultIndicateur.ToString();
                jsonString = jsonString.Replace("ValueKind = Object : ", "");

                if (string.IsNullOrEmpty(jsonString))
                {
                    dataResult.codeReponse = CodeReponse.errorMissingAllParams;
                    dataResult.msg = configuration.GetSection("MessagesAPI:ParamsEmpty").Value;
                }
                else
                {
                    body = JObject.Parse(jsonString);
                    var IndicateurID = Convert.ToInt32(body.IndicateurID);
                    var CreationDate = Convert.ToDateTime(body.CreationDate);
                    using (_ctx)
                    {
                        if (body.IndicateurID != null)
                        {
                            var ResultatsIndicateurs = new ResultatsIndicateurs
                            {
                                IndicateurID = IndicateurID,
                                Periode = body.Periode,
                                Annee = body.Annee,
                                Resultat = body.Resultat,
                                created_at = DateTime.Now
                            };
                            await _ctx.ResultatsIndicateurs.AddAsync(ResultatsIndicateurs);
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


        public async Task<DataSourceResult> UpdateResultIndicateur(object updateResultIndicateur)

        {
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body;

            try
            {
                string jsonString = updateResultIndicateur.ToString();
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

                        if (body.ID != null && body.IndicateurID != null)
                        {
                            var ID = Convert.ToInt32(body.ID);
                            var IndicateurID = Convert.ToInt32(body.IndicateurID);
                            var Annee = Convert.ToInt32(body.Annee);

                            var existingResultatsIndicateurs = await _ctx.ResultatsIndicateurs.FindAsync(ID);
                            var existingIndicateurs = await _ctx.Indicateurs.FindAsync(IndicateurID);

                            if (existingResultatsIndicateurs != null && existingIndicateurs != null)
                            {

                                existingResultatsIndicateurs.IndicateurID = IndicateurID;
                                existingResultatsIndicateurs.Periode = (string)body.Periode;
                                existingResultatsIndicateurs.Resultat = (decimal)body.Resultat;
                                existingResultatsIndicateurs.Annee = Annee;
                                existingResultatsIndicateurs.updated_at = DateTime.Now;

                                await _ctx.SaveChangesAsync();

                                dataResult.codeReponse = CodeReponse.ok;
                                dataResult.msg = "Le processus a été mis à jour avec succès.";
                            }
                            else
                            {
                                dataResult.codeReponse = CodeReponse.erreur;
                                dataResult.msg = "Le processus n'a pas été trouvé dans la base de données.";
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

        public async Task<DataSourceResult> SupprimerResultIndicateur(object ResultIndicateur)
        {
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body;

            try
            {
                string jsonString = ResultIndicateur.ToString();
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
                            var ID = Convert.ToInt32(body.ID);


                            var existingResultatsIndicateurs = await _ctx.ResultatsIndicateurs.FindAsync(ID);

                            // Marquez le processus comme supprimé (selon votre logique de suppression)
                            existingResultatsIndicateurs.deleted_at = DateTime.UtcNow;

                            // Utilisez la méthode Remove pour marquer l'entité comme supprimée
                            _ctx.ResultatsIndicateurs.Remove(existingResultatsIndicateurs);

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

        public async Task<GetDataResult> DetailResultatsIndicateurs(int ID)
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
                        var ResultatsIndicateurs = await _ctx.ResultatsIndicateurs.FindAsync(ID);

                        if (ResultatsIndicateurs != null)
                        {

                            var existingResultatsIndicateurs = await _ctx.ResultatsIndicateurs.
                                Where(p => p.ID == ID && p.deleted_at == null)
                                .Select(p => new ResultatsIndicateursDto
                                {
                                    ID = p.ID,
                                    Periode = p.Periode,
                                    Annee = p.Annee,
                                    Resultat = p.Resultat,
                                    IndicateurID = p.IndicateurID,
                                    Indicateurs =p.Indicateurs
                                }).FirstOrDefaultAsync(); ;


                            if (existingResultatsIndicateurs != null)
                            {
                                dataResult.IsSucceed = true;
                                dataResult.Message = "Detaill";
                                dataResult.ResultatsIndicateurs = existingResultatsIndicateurs;

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

        public class PRCItem
        {
            public int ID { get; set; }
            public int IndicateurID { get; set; }
            public string Periode { get; set; }
            public int Annee { get; set; }
            public decimal Resultat { get; set; }
            public DateTime CreationDate { get; set; }
            public int Pilote { get; set; }
            public int CoPilote { get; set; }
        }

        public async Task<DataSourceResult> RechercheResultatIndicateurs(string? periode , int annee, DateTime? dateDebut, DateTime? dateFin, int id, string? field, string? order, int take, int skip)
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
                    var result = await _ctx.ResultatsIndicateurs
                        .Where(p => p.deleted_at == null && p.IndicateurID == id
                            && (string.IsNullOrEmpty(periode) || p.Periode.ToLower().Contains(periode.ToLower()))
                            && (annee == 0 || p.Annee == annee)
                            && (!dateDebut.HasValue || p.created_at >= dateDebut)
                            && (!dateFin.HasValue || p.created_at <= dateFin))
                        .Select(p => new PRCItem
                        {
                            ID = p.ID,
                            IndicateurID = p.IndicateurID,
                            Periode = p.Periode,
                            Annee = p.Annee,
                            Resultat = p.Resultat,
                            CreationDate = p.created_at,
                            Pilote =p.Indicateurs.Processus.Pilote,
                            CoPilote = p.Indicateurs.Processus.CoPilote,
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



    }
}
