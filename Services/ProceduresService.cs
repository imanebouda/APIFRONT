using ITKANSys_api.Config;
using ITKANSys_api.Data.Dtos;
using ITKANSys_api.Interfaces;
using ITKANSys_api.Models;
using ITKANSys_api.Models.Dtos;
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
    public class ProceduresService : IProceduresService
    {

        private IConfiguration configuration;

        //Contexte de la base de données
        private ApplicationDbContext _ctx;
        /// <summary>
        /// Constructeur par défaut
        /// < /summary>
        public ProceduresService(ApplicationDbContext context)
        {
            configuration = AppConfig.GetConfig();
            _ctx = context;


        }

        public async Task<DataSourceResult> GetAllByIdProcessus(int ProcessusID)
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                if (ProcessusID <= 0)
                {
                    dataResult.IsSucceed = false;
                    dataResult.Message = "Données non trouvées.";
                }
                else
                {
                    //On se connecte à la base de données
                    using (_ctx)
                    {
                        //On récupère les données de la base de données
                        var Ddp = await _ctx.Procedures
                            .Where(p => p.deleted_at == null && p.ProcessusID == ProcessusID)
                            .OrderBy(p => p.created_at) // Triez par date de création
                            .Select(p => new
                            {
                                p.ID,
                                p.ProcessusID,
                                p.Description,
                                p.Libelle,
                                p.Code,
                                p.Version,
                                p.created_at
                            })
                            .ToListAsync();
                        dataResult.codeReponse = CodeReponse.ok;
                        dataResult.data = Ddp;
                        dataResult.IsSucceed = true;
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

        public class PRCItem
        {
            public int ID { get; set;}
            public string Code { get; set;}
            public string Libelle { get; set;}
            public string Description { get; set;}
            public string Version { get; set; }
            public string Processus { get; set; }
            public int Pilote { get; set; }
            public int CoPilote { get; set; }
            public int ProcessusID { get; set; }
            public DateTime CreationDate { get; set; }
        }

        public async Task<DataSourceResult> RechercheProcedures(string code, string libelle, DateTime? dateDebut, DateTime? dateFin, int id, string field, string order, int take, int skip)
        {
            try
            {
                    // Assurez-vous que les valeurs par défaut sont correctes
                take =take != 0 ? take : 10;
                skip = skip != 0 ? skip : 0;
                // Assurez-vous que les valeurs "field" et "order" sont définies
                field = !string.IsNullOrEmpty(field) ? field : "ID";
                order = !string.IsNullOrEmpty(order) ? order : "DESC";
                var dateDebutString = dateDebut?.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
                var dateFinString = dateFin?.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
                // Assurez-vous que "order" est "ASC" ou "DESC"
                Order direction = (order.ToUpper() == "DESC") ? Order.Desc : Order.Asc;

                // Exécutez la requête
                var result = await _ctx.Procedures
                    .Where(p => p.deleted_at == null && p.ProcessusID == id
                        && (string.IsNullOrEmpty(code) || p.Code.ToLower().Contains(code.ToLower()))
                        && (string.IsNullOrEmpty(libelle) || p.Libelle.ToLower().Contains(libelle.ToLower()))
                        && (!dateDebut.HasValue || p.created_at >= dateDebut)
                        && (!dateFin.HasValue || p.created_at <= dateFin))
                    .Select(p => new PRCItem
                    {
                        ID = p.ID,
                        Code = p.Code,
                        Libelle = p.Libelle,
                        Version = p.Version,
                        Pilote =p.Processus.Pilote,
                        CoPilote = p.Processus.CoPilote,
                        Processus = p.Processus.Libelle,
                        ProcessusID = p.ProcessusID,
                        Description = p.Description,
                        CreationDate = p.CreationDate
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

        public async Task<DataSourceResult> InsertProcedure(object insertProcedure)
        {
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body;

            try
            {
                string jsonString = insertProcedure.ToString();
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
                    var CreationDate = Convert.ToDateTime(body.CreationDate).AddDays(1);
                    using (_ctx)
                    {
                        if (body.ProcessusID != null )
                        {
                            var procedures = new Procedures
                            {
                                ProcessusID = ProcessusID,
                                Code = body.Code,
                                Version = body.Version,
                                Libelle = body.Libelle,
                                Description = body.Description,
                                CreationDate = CreationDate,
                                created_at = DateTime.Now
                            };
                            await _ctx.Procedures.AddAsync(procedures);
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

        public async Task<DataSourceResult> UpdateProcedure(object updateProcedure)
        
        {
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body;

            try
            {
                string jsonString = updateProcedure.ToString();
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
                            var ID = Convert.ToInt32(body.ID);
                            var processusID = Convert.ToInt32(body.ProcessusID);
                            var CreationDate = Convert.ToDateTime(body.CreationDate).AddDays(1);

                            var existingProcedure = await _ctx.Procedures.FindAsync(ID);
                            var existingProcessus = await _ctx.Processus.FindAsync(processusID);
                         
                            if ( existingProcedure != null && existingProcessus != null)
                            {
                         
                                existingProcedure.Libelle = (string)body.Libelle;
                                existingProcedure.ProcessusID = processusID;
                                existingProcedure.Description = (string)body.Description;
                                existingProcedure.Code = (string)body.Code;
                                existingProcedure.Version = (string)body.Version;
                                existingProcedure.CreationDate = CreationDate;
                                existingProcedure.updated_at = DateTime.Now;

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


        public async Task<DataSourceResult> SupprimerProcedure(object procedure)
        {
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body;

            try
            {
                string jsonString = procedure.ToString();
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
                            int ID = Convert.ToInt32(body.ID);

                            var existingProcedure = await _ctx.Procedures.FindAsync(ID);
                      
                            var existingProcDocuments = _ctx.ProcDocuments.Where(p => p.ProcID == ID).ToList();
                            var existingProcObjectifs = _ctx.ProcObjectifs.Where(p => p.ProcID == ID).ToList();

                            _ctx.ProcDocuments.RemoveRange(existingProcDocuments);
                            _ctx.ProcObjectifs.RemoveRange(existingProcObjectifs);

                            // Marquez le processus comme supprimé (selon votre logique de suppression)
                            existingProcedure.deleted_at = DateTime.UtcNow;

                            // Utilisez la méthode Remove pour marquer l'entité comme supprimée
                            _ctx.Procedures.Remove(existingProcedure);

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


        public async Task<GetDataResult> DetailProcedure(int ID)
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
                        var Processus = await _ctx.Procedures.FindAsync(ID);

                        if (Processus != null)
                        {

                            var existingProcessus = await _ctx.Procedures.
                                Where(p => p.ID == ID && p.deleted_at == null)
                                .Select(p => new ProcedureDto
                                {
                                    ID = p.ID,
                                    Code = p.Code,
                                    Libelle = p.Libelle,
                                    Version = p.Version,
                                    Description = p.Description,
                                    created_at = p.created_at,
                                    Processus = p.Processus
                                }).FirstOrDefaultAsync(); ;


                            if (existingProcessus != null)
                            {
                                dataResult.IsSucceed = true;
                                dataResult.Message = "Detaill";
                                dataResult.DataProcedure = existingProcessus;

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
