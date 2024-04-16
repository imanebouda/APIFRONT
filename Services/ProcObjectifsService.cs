using ITKANSys_api.Config;
using ITKANSys_api.Interfaces;
using ITKANSys_api.Models.Entities;
using ITKANSys_api.Models;
using ITKANSys_api.Utility.ApiResponse;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using static ITKANSys_api.Utility.ApiResponse.QueryableExtensions;

namespace ITKANSys_api.Services
{
    public class ProcObjectifsService : IProcObjectifsService
    {
        private IConfiguration configuration;

        //Contexte de la base de données
        private ApplicationDbContext _ctx;
        /// <summary>
        /// Constructeur par défaut
        /// < /summary>
        public ProcObjectifsService(ApplicationDbContext context)
        {
            configuration = AppConfig.GetConfig();
            _ctx = context;
        }

        public async Task<DataSourceResult> GetAllProcObjectifs(int ID)
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                //On se connecte à la base de données
                using (_ctx)
                {
                    //On récupère les données de la base de données
                    var Ddp = await _ctx.ProcObjectifs
                        .Where(p => p.deleted_at == null && p.ProcID == ID)
                        .OrderBy(p => p.created_at) // Triez par date de création
                        .Select(p => new
                        {
                            p.ID,
                            p.Title,
                            p.Description,
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


        public async Task<DataSourceResult> InsertProcObjectifs(object insertProcessus)
        {
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body;

            try
            {
                string jsonString = insertProcessus.ToString();
                jsonString = jsonString.Replace("ValueKind = Object : ", "");

                if (string.IsNullOrEmpty(jsonString))
                {
                    dataResult.codeReponse = CodeReponse.errorMissingAllParams;
                    dataResult.msg = configuration.GetSection("MessagesAPI:ParamsEmpty").Value;
                }
                else
                {
                    body = JObject.Parse(jsonString);
                    var ProcID = Convert.ToInt32(body.ProcID);
                    using (_ctx)
                    {
                        if (body.ProcID != null)
                        {
                            var ProcesObjectif = new ProcObjectifs
                            {
                                ProcID = ProcID,
                                Title = body.Title,
                                Description = body.Description,
                                created_at = DateTime.UtcNow
                            };
                            await _ctx.ProcObjectifs.AddAsync(ProcesObjectif);
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
            public int ProcessusID { get; set; }
            public Procedures Procedures { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime Date { get; set; }
        }

        public async Task<DataSourceResult> RechercheProcObjectifs(int id, string titre, DateTime? dateDebut, DateTime? dateFin, string field, string order, int take, int skip)
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
                    var result = await _ctx.ProcObjectifs
                        .Where(p => p.deleted_at == null && p.ProcID == id
                            && (string.IsNullOrEmpty(titre) || p.Title.ToLower().Contains(titre.ToLower()))
                            && (!dateDebut.HasValue || p.created_at >= dateDebut)
                            && (!dateFin.HasValue || p.created_at <= dateFin))

                        .Select(p => new PRCItem
                        {
                            ID = p.ID,
                            Title = p.Title,
                            Description = p.Description,
                            Procedures = p.Procedures,
                            Date = p.created_at
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


        public async Task<DataSourceResult> UpdateProcObjectifs(object processus)
        {
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body;

            try
            {
                string jsonString = processus.ToString();
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
                        if (body.ID != null && body.ProcID != null)
                        {
                            var ProcObjectifsID = Convert.ToInt32(body.ID);
                            var ProcID = Convert.ToInt32(body.ProcID);

                            var existingProcedure = await _ctx.Procedures.FindAsync(ProcID);
                            var existingProcObjectifs = await _ctx.ProcObjectifs.FindAsync(ProcObjectifsID);

                            if (existingProcedure != null && existingProcObjectifs != null)
                            {
                                existingProcObjectifs.Description = (string)body.Description;
                                existingProcObjectifs.Title = (string)body.Title;
                                existingProcObjectifs.ProcID = ProcID;
                                existingProcObjectifs.updated_at = DateTime.Now;

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

        public async Task<DataSourceResult> SupprimerProcObjectifs(object processus)
        {
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body;

            try
            {
                string jsonString = processus.ToString();
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
                            var ProcObjectifsID = Convert.ToInt32(body.ID);


                            var existingProcessus = await _ctx.ProcObjectifs.FindAsync(ProcObjectifsID);

                            // Marquez le processus comme supprimé (selon votre logique de suppression)
                            existingProcessus.deleted_at = DateTime.Now;

                            // Utilisez la méthode Remove pour marquer l'entité comme supprimée
                            _ctx.ProcObjectifs.Remove(existingProcessus);

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

    }
}
