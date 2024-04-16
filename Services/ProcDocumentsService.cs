using ITKANSys_api.Config;
using ITKANSys_api.Models;
using ITKANSys_api.Utility.ApiResponse;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ITKANSys_api.Data.OtherObjects;
using ITKANSys_api.Interfaces;
using ITKANSys_api.Data.Dtos;
using ITKANSys_api.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Diagnostics;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using FileInfo = ITKANSys_api.Interfaces.FileInfo;
using Azure;
using System.Net.Mime;
using Microsoft.AspNetCore.StaticFiles;
using static ITKANSys_api.Utility.ApiResponse.QueryableExtensions;

namespace ITKANSys_api.Services
{
    public class ProcDocumentsService : IProcDocumentsService 
    {
        private IConfiguration configuration;

        //Contexte de la base de données
        private ApplicationDbContext _ctx;
        /// <summary>
        /// Constructeur par défaut
        /// < /summary>
        public ProcDocumentsService(ApplicationDbContext context)
        {
            configuration = AppConfig.GetConfig();
            _ctx = context;
        }

        public async Task<DataSourceResult> GetAll(int procID)
        {
            //Déclaration des variables
            DataSourceResult dataResult = new DataSourceResult();
            try
            {
                if (procID <= 0)
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
                        var Ddp = await _ctx.ProcDocuments
                            .Where(p => p.deleted_at == null && p.ProcID == procID && p.Perimé == StaticDocuments.NonValide && p.Type == "AS")
                            .OrderBy(p => p.created_at) // Triez par date de création
                            .Select(p => new ProcDocumentsDto
                            {
                                ID = p.ID,
                                Libelle = p.Libelle,
                                Perimé = p.Perimé,
                                CreationDate = p.CreationDate,
                                FileName = p.FileName,
                                FilePath = p.FilePath,
                                ProcID = p.ProcID,
                                ExtensionFile = Path.GetExtension(p.FileName),
                                Procedure = p.Procedure
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

        public async Task<DataSourceResult> InsertProcDocuments(IFormFile file, string Libelle,string type, int ProcID, CancellationToken cancellationtoken)
        {
            DataSourceResult dataResult = new DataSourceResult();
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                var filename = DateTime.Now.Ticks.ToString() + extension;

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files\\Procedures");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var exactpath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files\\Procedures", filename);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                // Maintenant, enregistrez les informations du fichier dans la base de données
                var procedure = new ProcDocuments
                {
                    Libelle = Libelle,
                    ProcID = ProcID,
                    FilePath = filepath,
                    FileName = filename,
                    Type = type,
                    Perimé = StaticDocuments.NonValide,
                    CreationDate = DateTime.Now,
                    created_at = DateTime.Now,
                };

                await _ctx.ProcDocuments.AddAsync(procedure);
                await _ctx.SaveChangesAsync();

                // Mettre à jour tous les documents précédents avec "Perimé" à "true"
               // var documentsPrecedents = await _ctx.ProcDocuments
                  //  .Where(doc => doc.ProcID == ProcID && doc.ID != procedure.ID)
                  //  .ToListAsync();

              //  foreach (var document in documentsPrecedents)
             //   {
                //    document.Perimé = StaticDocuments.Valide;
              //  }

                await _ctx.SaveChangesAsync();

                dataResult.codeReponse = CodeReponse.ok;
                dataResult.msg = "Ajouté";
            }
            catch (Exception ex)
            {
                dataResult.IsSucceed = false;
                dataResult.Message = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                // Gérer l'erreur si nécessaire.
            }

            return dataResult;
        }



        public async Task<DataSourceResult> UpdateProcDocuments( string Libelle ,  int ID)
        {
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body;

            try
            {
                

                    using (_ctx)
                    {
                        if (ID != 0)
                        {
                            var ProcDocumentID = Convert.ToInt32(ID);

                            var existingProcDocument = await _ctx.ProcDocuments.FindAsync(ProcDocumentID);

                            if (existingProcDocument != null)
                            {
                                // Mettez à jour uniquement les propriétés qui sont présentes dans le corps de la demande.
                                if (Libelle != null)
                                {
                                    existingProcDocument.Libelle = Libelle;
                                }

                                existingProcDocument.updated_at = DateTime.Now;

                                await _ctx.SaveChangesAsync();

                                dataResult.codeReponse = CodeReponse.ok;
                                dataResult.msg = "Le document du processus a été mis à jour avec succès.";
                            }
                            else
                            {
                                dataResult.codeReponse = CodeReponse.erreur;
                                dataResult.msg = "Le document du processus n'a pas été trouvé dans la base de données.";
                            }
                        }
                        else
                        {
                            dataResult.codeReponse = CodeReponse.erreur;
                            dataResult.msg = configuration.GetSection("MessagesAPI:ParamsEmpty").Value;
                        }
                    }
                
            }
            catch (Exception ex)
            {
                dataResult.IsSucceed = false;
                dataResult.Message = (ex.InnerException == null) ? ex.Message : ex.InnerException.Message;
                // Gérez l'erreur si nécessaire.
            }

            return dataResult;
        }




        public async Task<GetDataResult> DetailProcDocuments(int procID)
        {
            GetDataResult dataResult = new GetDataResult();

            try
            {
                if (procID <= 0)
                {
                    dataResult.IsSucceed = false;
                    dataResult.Message = "Données non trouvées.";
                }
                else
                {
                    using (_ctx)
                    {
                        var existingProcedure = await _ctx.ProcDocuments
                            .Where(p => p.ProcID == procID && p.deleted_at == null && p.Perimé == StaticDocuments.NonValide && p.Type == "T" )
                            .OrderByDescending(p => p.CreationDate)
                            .FirstOrDefaultAsync();

                        if (existingProcedure != null)
                        {
                            // Vous pouvez mapper les propriétés de existingProcessus vers ProcesDocumentsDto si nécessaire
                            var resultProcesDocumentsDto = new ProcDocumentsDto
                            {
                                ID = existingProcedure.ID,
                                Libelle = existingProcedure.Libelle,
                                Perimé = existingProcedure.Perimé,
                                CreationDate = existingProcedure.CreationDate,
                                FileName = existingProcedure.FileName,
                                FilePath = existingProcedure.FilePath,
                                ProcID = existingProcedure.ProcID,
                                ExtensionFile = "." + existingProcedure.FileName.Split('.')[existingProcedure.FileName.Split('.').Length - 1],
                                Procedure = existingProcedure.Procedure
                            };

                            dataResult.IsSucceed = true;
                            dataResult.Message = "Détails";
                            dataResult.ProcDocument = resultProcesDocumentsDto;
                        }
                        else
                        {
                            dataResult.IsSucceed = false;
                            dataResult.Message = "Aucune version non périmée trouvée pour ce document.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dataResult.IsSucceed = false;
                dataResult.Message = (ex.InnerException == null) ? ex.Message : ex.InnerException.Message;
                // Enregistrez l'erreur si nécessaire.
            }
            return dataResult;
        }



        public async Task<DataSourceResult> SupprimerProcDocuments(object procedure)
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
                            var ProcDocumentsID = Convert.ToInt32(body.ID);


                            var existingProcedure = await _ctx.ProcDocuments.FindAsync(ProcDocumentsID);

                            existingProcedure.deleted_at = DateTime.Now;

                            _ctx.ProcDocuments.Remove(existingProcedure);

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

        public async Task<DataSourceResult> UpdatePeriméState(object procedureDocuments)
        {
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body;

            try
            {
                string jsonString = procedureDocuments.ToString();
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
                        var documentId = Convert.ToInt32(body.documentId);
                        var newState = Convert.ToInt32(body.newState);
                        var existingDocument = await _ctx.ProcDocuments.FindAsync(documentId);

                        if (existingDocument != null)
                        {
                            if (newState == StaticDocuments.NonValide)
                            {
                                existingDocument.Perimé = StaticDocuments.Valide;
                            }
                            else
                            {
                                existingDocument.Perimé = StaticDocuments.NonValide;
                            }
                            await _ctx.SaveChangesAsync();
                            dataResult.codeReponse = CodeReponse.ok;
                            dataResult.msg = "Modifié";
                        }
                        else
                        {
                            dataResult.codeReponse = CodeReponse.erreur;
                            dataResult.msg = "Le document n'a pas été trouvé dans la base de données.";
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


        public class PRCItem
        {
            public int ID { get; set; }
            public string Libelle { get; set; }
            public int Perimé { get; set; }
            public DateTime CreationDate { get; set; }
            public string FileName { get; set; }
            public string FilePath { get; set; }
        }


        public async Task<DataSourceResult> RechercheProcDocumentsPerime(string libelle, DateTime? dateDebut, DateTime? dateFin, string field, string order, int take, int skip)
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
                var result = await _ctx.ProcDocuments
                    .Where(p => p.deleted_at == null && p.Perimé == StaticDocuments.Valide 
                        && (string.IsNullOrEmpty(libelle) || p.Libelle.ToLower().Contains(libelle.ToLower()))
                        && (!dateDebut.HasValue || p.CreationDate >= dateDebut)
                        && (!dateFin.HasValue || p.CreationDate <= dateFin))
                    .Select(p => new PRCItem
                    {
                        ID = p.ID,
                        Libelle = p.Libelle,
                        Perimé = p.Perimé,
                        CreationDate = p.CreationDate,
                        FileName = p.FileName,
                        FilePath = p.FilePath,
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

    }
}
