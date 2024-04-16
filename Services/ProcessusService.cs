using ITKANSys_api.Config;
using ITKANSys_api.Data.Dtos;
using ITKANSys_api.Data.OtherObjects;
using ITKANSys_api.Interfaces;
using ITKANSys_api.Models;
using ITKANSys_api.Models.Dtos;
using ITKANSys_api.Models.Entities;
using ITKANSys_api.Models.Entities.Param;
using ITKANSys_api.Utility.ApiResponse;

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static ITKANSys_api.Utility.ApiResponse.QueryableExtensions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ITKANSys_api.Data.Services
{
    public class ProcessusService : IProcessusServices
    {
        private IConfiguration configuration;

        //Contexte de la base de données
        private ApplicationDbContext _ctx;
        /// <summary>
        /// Constructeur par défaut
        /// < /summary>
        public ProcessusService(ApplicationDbContext context)
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
                    var Ddp = await _ctx.Processus
                        .Where(p => p.deleted_at == null)
                        .OrderBy(p => p.created_at) // Triez par date de création
                        .Select(p => new
                        {
                            p.ID,
                            p.SMQ_ID,
                            p.USER_ID,
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
            public int ID { get; set; }
            public string Code { get; set; }
            public string Libelle { get; set; }
            public string Description { get; set; }
            public string Version { get; set; }
            public int Pilote { get; set; }
            public int CoPilote { get; set; }
            public string NamePilote { get; set; }
            public string NameCoPilote { get; set; }
            public DateTime CreationDate { get; set; }
            public SMQ SMQ { get; set; }
            public Categories Categories { get; set; }
            public User Users { get; set; }
        }

        public async Task<DataSourceResult> RechercheProcessus(string code, string libelle, DateTime? dateDebut, DateTime? dateFin, int categorie, string field, string order, int take, int skip)
        {
            try
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
                var result = await _ctx.Processus
                    .Where(p => p.deleted_at == null
                        && (string.IsNullOrEmpty(code) || p.Code.ToLower().Contains(code.ToLower()))
                        && (string.IsNullOrEmpty(libelle) || p.Libelle.ToLower().Contains(libelle.ToLower()))
                        && (!dateDebut.HasValue || p.created_at >= dateDebut)
                        && (!dateFin.HasValue || p.created_at <= dateFin)
                        && (categorie == 0 || p.Categories.ID == categorie))
                    .Select(p => new PRCItem
                    {
                        ID = p.ID,
                        Code = p.Code,
                        Libelle = p.Libelle,
                        Version = p.Version,
                        Description = p.Description,
                        SMQ = p.SMQ,
                        Categories = p.Categories,
                        NamePilote = p.PiloteUser.NomCompletUtilisateur,
                        NameCoPilote = p.CoPiloteUser.NomCompletUtilisateur,
                        Pilote = p.Pilote,
                        CoPilote = p.CoPilote,
                        Users = p.Users,
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

        public class ProcessusItem
        {
            public int id { get; set; }
            public string libelle { get; set; }
        }


        public async Task<DataSourceResult> GetProcessusByPiloteOrCoPilote(int pilote, int coPilote)
        {
            try
            {
                // Exécutez la requête
                var result = await _ctx.Processus
                    .Where(p => p.deleted_at == null
                        && (pilote == 0 || p.Pilote == pilote)
                        && (coPilote == 0 || p.CoPilote == coPilote))
                    .Select(p => new ProcessusItem
                    {
                        id = p.ID,
                        libelle = p.Libelle,
                    })
                    .ToListAsync();

                var resultTake = result.ToList();

                // Créez la réponse
                var response = new DataSourceResult
                {
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


        public async Task<DataSourceResult> InsertProcessus(object insertProcessus)
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
                    var USER_ID = Convert.ToInt32(body.USER_ID);
                    var CreationDate = Convert.ToDateTime(body.CreationDate).AddDays(1);
                    using (_ctx)
                    {
                        if (body.SMQ_ID != null && body.Categorie_ID != null && USER_ID != null)
                        {
                            var processus = new Processus
                            {
                                SMQ_ID = body.SMQ_ID,
                                Categorie_ID = body.Categorie_ID,
                                USER_ID = USER_ID,
                                Code = body.Code,
                                Version = body.Version,
                                Libelle = body.Libelle,
                                Description = body.Description,
                                Pilote = body.Pilote,
                                CoPilote = body.CoPilote,
                                CreationDate = body.CreationDate,
                                created_at = DateTime.Now
                            };
                            await _ctx.Processus.AddAsync(processus);
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


        public async Task<DataSourceResult> UpdateProcessus(object processus)
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

                        if (body.ID != null && body.SMQ_ID != null && body.Categorie_ID != null && body.USER_ID != null && body.Pilote != null && body.CoPilote != null)
                        {
                            var SmQID = Convert.ToInt32(body.SMQ_ID);
                            var processusID = Convert.ToInt32(body.ID);
                            var categorieID = Convert.ToInt32(body.Categorie_ID);
                            var userID = Convert.ToInt32(body.USER_ID);
                            var PiloteID = Convert.ToInt32(body.Pilote);
                            var CoPiloteID = Convert.ToInt32(body.CoPilote);
                            var CreationDate = Convert.ToDateTime(body.CreationDate).AddDays(1);

                            var existingProcessus = await _ctx.Processus.FindAsync(processusID);
                            var existingSMQ = await _ctx.SMQ.FindAsync(SmQID);
                            var existingCategorie = await _ctx.Categories.FindAsync(categorieID);
                            var existingUser = await _ctx.Users.FindAsync(userID);

                            if (existingProcessus != null && existingSMQ != null && existingCategorie != null && existingUser != null)
                            {
                                existingProcessus.SMQ_ID = SmQID;
                                existingProcessus.USER_ID = userID;
                                existingProcessus.Libelle = (string)body.Libelle;
                                existingProcessus.Categorie_ID = categorieID;
                                existingProcessus.Description = (string)body.Description;
                                existingProcessus.Code = (string)body.Code;
                                existingProcessus.Version = (string)body.Version;
                                existingProcessus.Pilote = PiloteID;
                                existingProcessus.CoPilote = CoPiloteID;
                                existingProcessus.CreationDate = CreationDate;
                                existingProcessus.updated_at = DateTime.Now;

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


        public async Task<DataSourceResult> SupprimerProcessus(object processus)
        {
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body;
            int processusID;

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
                            processusID = Convert.ToInt32(body.ID);


                            var existingProcessus = await _ctx.Processus.FindAsync(processusID);
                            var existingProcedures = _ctx.Procedures.Where(p => p.ProcessusID == processusID).ToList();

                            var existingProcesDocuments = _ctx.ProcesDocuments.Where(p => p.ProcessusID == processusID).ToList();
                            var existingProcesObjectifs = _ctx.ProcesObjectifs.Where(p => p.ProcessusID == processusID).ToList();


                            foreach (var procedure in existingProcedures)
                            {
                                var existingProcDocuments = _ctx.ProcDocuments.Where(p => p.ProcID == procedure.ID).ToList();
                                var existingProcObjectifs = _ctx.ProcObjectifs.Where(p => p.ProcID == procedure.ID).ToList();

                                _ctx.ProcDocuments.RemoveRange(existingProcDocuments);
                                _ctx.ProcObjectifs.RemoveRange(existingProcObjectifs);
                            }
                            _ctx.Procedures.RemoveRange(existingProcedures);
                            _ctx.ProcesDocuments.RemoveRange(existingProcesDocuments);
                            _ctx.ProcesObjectifs.RemoveRange(existingProcesObjectifs);

                            // Marquez le processus comme supprimé (selon votre logique de suppression)
                            existingProcessus.deleted_at = DateTime.UtcNow;

                            // Utilisez la méthode Remove pour marquer l'entité comme supprimée
                            _ctx.Processus.Remove(existingProcessus);

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


        public async Task<DataSourceResult> GetAllCategories()
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                //On se connecte à la base de données
                using (_ctx)
                {
                    //On récupère les données de la base de données
                    var Ddp = await _ctx.Categories
                        .Where(p => p.deleted_at == null)
                        .OrderBy(p => p.created_at) // Triez par date de création
                        .Select(p => new
                        {
                            p.ID,
                            p.Libelle,
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

        public async Task<DataSourceResult> GetAllSMQ()
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                //On se connecte à la base de données
                using (_ctx)
                {
                    //On récupère les données de la base de données
                    var Ddp = await _ctx.SMQ
                        .Where(p => p.deleted_at == null)
                        .OrderBy(p => p.created_at) // Triez par date de création
                        .Select(p => new
                        {
                            p.ID,
                            p.Libelle,
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



        public async Task<GetDataResult> DetailProcessus(int ID)
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
                        var Processus = await _ctx.Processus.FindAsync(ID);

                        if (Processus != null)
                        {

                            var existingProcessus = await _ctx.Processus.
                                Where(p => p.ID == ID && p.deleted_at == null)
                                .Select(p => new ProcessusDto
                                {
                                    ID = p.ID,
                                    Code = p.Code,
                                    Libelle = p.Libelle,
                                    Version = p.Version,
                                    Description = p.Description,
                                    Pilote = p.Pilote,
                                    CoPilote = p.CoPilote,
                                    NamePilote = p.PiloteUser.NomCompletUtilisateur,
                                    NameCoPilote = p.CoPiloteUser.NomCompletUtilisateur,
                                    SMQ = p.SMQ,
                                    Categories = p.Categories,
                                    Users = p.Users,
                                    created_at = p.created_at
                                }).FirstOrDefaultAsync(); ;


                            if (existingProcessus != null)
                            {
                                dataResult.IsSucceed = true;
                                dataResult.Message = "Detaill";
                                dataResult.data = existingProcessus;

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
       

        public async Task<DataSourceResult> GetProcessusData()
        {
            // Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                // On se connecte à la base de données
                using (_ctx)
                {
                    var query = from procedure in _ctx.Procedures
                                join processus in _ctx.Processus on procedure.ProcessusID equals processus.ID
                                select new
                                {
                                    ProcedureID = procedure.ID,
                                    Procedure = procedure.Libelle,
                                    ProcessusID = processus.ID,
                                    Processus = processus.Libelle,
                                    Pilote = processus.Pilote,
                                    CoPilote = processus.CoPilote
                                };

                    var result = await query.ToListAsync();
                    var groupedQuery = result
                            .GroupBy(item => new
                            {
                                item.ProcessusID,
                                item.Processus,
                                item.CoPilote,
                                item.Pilote
                            })
                            .Select(group => new
                            {
                                ProcessusID = group.Key.ProcessusID,
                                Processus = group.Key.Processus,
                                Pilote = group.Key.Pilote,
                                CoPilote = group.Key.CoPilote,
                                Procedures = group.Select(item => new
                                {
                                    ProcedureID = item.ProcedureID,
                                    Procedure = item.Procedure,
                                    Documents = _ctx.ProcDocuments
                                                    .Where(doc => doc.ProcID == item.ProcedureID && doc.Perimé == StaticDocuments.NonValide )
                                                    .Select(doc => new
                                                    {
                                                        DocumentID = doc.ID,
                                                        Document = doc.Libelle,
                                                        Pilote = doc.Procedure.Processus.Pilote,
                                                        CoPilote = doc.Procedure.Processus.CoPilote,
                                                        DocumentPath = doc.FilePath,
                                                        DocumentFileName = doc.FileName,
                                                        state =doc.Perimé
                                                    }).ToList(),
                                    Objectifs = _ctx.ProcObjectifs
                                                    .Where(obj => obj.ProcID == item.ProcedureID)
                                                    .Select(obj => new
                                                    {
                                                        ObjectifID = obj.ID,
                                                        Objectif = obj.Title,
                                                        Description = obj.Description,
                                                        Date =obj.created_at      
                                                    }).ToList()
                                }).ToList(),
                                ProcessDocuments = _ctx.ProcesDocuments
                                                    .Where(doc => doc.ProcessusID == group.Key.ProcessusID && doc.Perime == StaticDocuments.NonValide)
                                                    .Select(doc => new
                                                    {
                                                        ProcessDocID = doc.ID,
                                                        ProcessDoc = doc.Libelle,
                                                        Pilote =doc.Processus.Pilote,
                                                        CoPilote=doc.Processus.CoPilote,
                                                        ProcessDocPath = doc.FilePath,
                                                        ProcessDocFileName = doc.FileName,
                                                        state = doc.Perime
                                                    }).ToList(),
                                ProcessObjectifs = _ctx.ProcesObjectifs
                                                    .Where(obj => obj.ProcessusID == group.Key.ProcessusID)
                                                    .Select(obj => new
                                                    {
                                                        ProcessObjID = obj.ID,
                                                        ProcessObj = obj.Title,
                                                        ProcessObjDesc = obj.Description,
                                                        ProcessObjDate = obj.created_at
                                                        
                                                    }).ToList(),
                            });

                    dataResult.codeReponse = CodeReponse.ok;
                    dataResult.data = groupedQuery.ToList();

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



    }
}
