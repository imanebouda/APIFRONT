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
    public class ProcesDocumentsService : IProcesDocumentsService
    {
        private IConfiguration configuration;

        //Contexte de la base de données
        private ApplicationDbContext _ctx;
        /// <summary>
        /// Constructeur par défaut
        /// < /summary>
        public ProcesDocumentsService(ApplicationDbContext context)
        {
            configuration = AppConfig.GetConfig();
            _ctx = context;
        }
        public async Task<DataSourceResult> InsertProcesDocuments( IFormFile file,string Libelle, int ProcessusID, CancellationToken cancellationtoken)
        {
            DataSourceResult dataResult = new DataSourceResult();
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                var filename = DateTime.Now.Ticks.ToString() + extension;

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files\\Processus");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var exactpath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files\\Processus", filename);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Maintenant, enregistrez les informations du fichier dans la base de données
                var processus = new ProcesDocuments
                {
                    Libelle = Libelle,
                    ProcessusID = ProcessusID,
                    FilePath =  filepath,
                    FileName = filename,
                    Perime = StaticDocuments.NonValide,
                    CreationDate = DateTime.Now,
                    created_at = DateTime.Now,
                };

                await _ctx.ProcesDocuments.AddAsync(processus);
                await _ctx.SaveChangesAsync();

                // Mettre à jour tous les documents précédents avec "Perime" à "true"
                var documentsPrecedents = await _ctx.ProcesDocuments
                    .Where(doc => doc.ProcessusID == ProcessusID && doc.ID != processus.ID)
                    .ToListAsync();

                foreach (var document in documentsPrecedents)
                {
                    document.Perime = StaticDocuments.Valide;
                }

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



        public async Task<DataSourceResult> UpdateProcesDocuments( string Libelle  ,int ID)
        {
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body;

            try
            {               
                    using (_ctx)
                    {
                        if (ID != 0)
                        {
                            var ProcesDocumentID = Convert.ToInt32(ID);

                            var existingProcesDocument = await _ctx.ProcesDocuments.FindAsync(ProcesDocumentID);

                            if (existingProcesDocument != null)
                            {
                                // Mettez à jour uniquement les propriétés qui sont présentes dans le corps de la demande.
                                if (Libelle != null)
                                {
                                    existingProcesDocument.Libelle = Libelle;
                                }
                                existingProcesDocument.updated_at = DateTime.Now;

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




        public async Task<GetDataResult> DetailProcesDocuments(int processusID)
        {
            GetDataResult dataResult = new GetDataResult();

            try
            {
                if (processusID <= 0)
                {
                    dataResult.IsSucceed = false;
                    dataResult.Message = "Données non trouvées.";
                }
                else
                {
                    using (_ctx)
                    {
                        var existingProcessusDocument = await _ctx.ProcesDocuments
                            .Where(p => p.ProcessusID == processusID && p.deleted_at == null && p.Perime == StaticDocuments.NonValide)
                            .OrderByDescending(p => p.CreationDate)
                            .FirstOrDefaultAsync();

                        // Récupérer des informations sur le processus
                        var existingProcessus = await _ctx.Processus
                            .Where(p => p.ID == processusID)
                            .FirstOrDefaultAsync();
                        if (existingProcessusDocument != null)
                        {
                            // Vous pouvez mapper les propriétés de existingProcessus vers ProcesDocumentsDto si nécessaire
                            var resultProcesDocumentsDto = new ProcesDocumentsDto
                            {
                                ID = existingProcessusDocument.ID,
                                Libelle = existingProcessusDocument.Libelle,
                                Perime = existingProcessusDocument.Perime,
                                CreationDate = existingProcessusDocument.CreationDate,
                                FileName = existingProcessusDocument.FileName,
                                FilePath = existingProcessusDocument.FilePath,
                                ExtensionFile = "." + existingProcessusDocument.FileName.Split('.')[existingProcessusDocument.FileName.Split('.').Length - 1],
                                ProcessusID = existingProcessusDocument.ProcessusID,
                                Pilote = existingProcessus.Pilote,
                                CoPilote = existingProcessus.CoPilote
                            };

                            dataResult.IsSucceed = true;
                            dataResult.Message = "Détails";
                            dataResult.Data = resultProcesDocumentsDto;
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



        public async Task<DataSourceResult> SupprimerProcesDocuments(object processus)
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
                            var ProcesDocumentsID = Convert.ToInt32(body.ID);


                            var existingProcessus = await _ctx.ProcesDocuments.FindAsync(ProcesDocumentsID);

                            existingProcessus.deleted_at = DateTime.Now;

                            _ctx.ProcesDocuments.Remove(existingProcessus);

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

        public async Task<DataSourceResult> UpdatePerimeState(object processusDocuments)
        {
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body;

            try
            {
                string jsonString = processusDocuments.ToString();
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
                        int processusId = Convert.ToInt32(body.processusId);
                        ProcesDocuments existingDocument = await _ctx.ProcesDocuments.FindAsync(documentId);

                        if (existingDocument != null)
                        {
                            if (newState == StaticDocuments.NonValide)
                            {
                                existingDocument.Perime = StaticDocuments.Valide;
                            }
                            else
                            {
                                existingDocument.Perime = StaticDocuments.NonValide;
                            }
                            // Ensuite, mettez à jour tous les autres documents
                            var otherDocuments = await _ctx.ProcesDocuments
                                .Where(pd => pd.ID != existingDocument.ID &&  pd.ProcessusID == existingDocument.ProcessusID)
                                .ToListAsync();

                            foreach (var doc in otherDocuments)
                            {
                                doc.Perime = StaticDocuments.Valide; // Mettez à jour le statut comme nécessaire
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


        public async Task<bool> DocumentContainsValidDocument(int documentId, int id , string documentType )
        {
            try
            {
                using (_ctx)
                {
                    if (documentType == "ProcesDocuments")
                    {
                        // Vérifie s'il existe un document valide lié au document de processus spécifié
                        var hasValidDocument = await _ctx.ProcesDocuments
                            .AnyAsync(pd => pd.ID != documentId && pd.Perime == StaticDocuments.NonValide && pd.ProcessusID == id);

                        return hasValidDocument;
                    }
                    else if (documentType == "ProcDocuments")
                    {
                        // Vérifie s'il existe un document valide lié au document de procédure spécifié
                        var hasValidDocument = await _ctx.ProcDocuments
                            .AnyAsync(pd => pd.ID != documentId && pd.Perimé == StaticDocuments.NonValide && pd.ProcID == id);

                        return hasValidDocument;
                    }

                    // Gérer d'autres types de documents si nécessaire

                    return false;
                }
            }
            catch (Exception ex)
            {
                // Log l'exception si nécessaire
                return false;
            }
        }

        public class PRCItem
        {
            public int ID { get; set; }
            public string Libelle { get; set; }
            public int Perime { get; set; }
            public DateTime CreationDate { get; set; }
            public Processus Processus { get; set; }
            public Procedures Procedure { get; set; }
            public string FileName { get; set; }
            public string FilePath { get; set; }
            public string TypeDocument { get; set; }
            public string MQ { get; set; }
            public string PQ { get; set; }
        }



        public async Task<DataSourceResult> RechercheDocumentsPerime(int procedureID, int processusID,bool MQ, bool PQ , string? code, string? version, string? libelle, DateTime? dateDebut, DateTime? dateFin, string field, string order, int take, int skip)
        {
            try
            {
                List<PRCItem> resultProcesDocuments = null;
                List<PRCItem> resultProcDocuments = null;
                List<PRCItem> resultMQDocuments = null;
                List<PRCItem> resultPQDocuments = null;
                // Assurez-vous que les valeurs par défaut sont correctes
                processusID = (procedureID > 0) ? 0 : processusID;
                
                take = take != 0 ? take : 10;
                skip = skip != 0 ? skip : 0;
                // Assurez-vous que les valeurs "field" et "order" sont définies
                field = !string.IsNullOrEmpty(field) ? field : "ID";
                order = !string.IsNullOrEmpty(order) ? order : "DESC";
                var dateDebutString = dateDebut?.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
                var dateFinString = dateFin?.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
                // Assurez-vous que "order" est "ASC" ou "DESC"
                Order direction = (order.ToUpper() == "DESC") ? Order.Desc : Order.Asc;

                // Liste résultat pour ProcesDocuments

                if (procedureID == 0 && MQ != true && PQ != true)
                {
                    // Exécutez la requête pour ProcesDocuments
                    resultProcesDocuments = await _ctx.ProcesDocuments
                        .Where(p => p.deleted_at == null && p.Perime == StaticDocuments.Valide
                            && (string.IsNullOrEmpty(libelle) || p.Libelle.ToLower().Contains(libelle.ToLower()))
                            && (string.IsNullOrEmpty(code) || p.Processus.Code.ToLower().Contains(code.ToLower()))
                            && (string.IsNullOrEmpty(version) || p.Processus.Version.ToLower().Contains(version.ToLower()))
                            && (processusID == 0 || p.ProcessusID == processusID)
                            && (!dateDebut.HasValue || p.CreationDate >= dateDebut)
                            && (!dateFin.HasValue || p.CreationDate <= dateFin))
                        .Select(p => new PRCItem
                        {
                            ID = p.ID,
                            Libelle = p.Libelle,
                            Perime = p.Perime,
                            Processus = p.Processus,
                            CreationDate = p.CreationDate,
                            FileName = p.FileName,
                            FilePath = p.FilePath,
                            TypeDocument = "ProcesDocuments"
                        })
                        .OrderByDynamic(field, direction)
                        .ToListAsync();
                }

                if (MQ != true && PQ != true)
                {
                    // Liste résultat pour ProcDocuments
                    resultProcDocuments = await _ctx.ProcDocuments
                      .Where(p => p.deleted_at == null && p.Perimé == StaticDocuments.Valide
                          && (string.IsNullOrEmpty(libelle) || p.Libelle.ToLower().Contains(libelle.ToLower()))
                          && (string.IsNullOrEmpty(code) || p.Procedure.Code.ToLower().Contains(code.ToLower()))
                          && (string.IsNullOrEmpty(version) || p.Procedure.Version.ToLower().Contains(version.ToLower()))
                          && (procedureID == 0 || p.ProcID == procedureID)
                          && (!dateDebut.HasValue || p.CreationDate >= dateDebut)
                          && (!dateFin.HasValue || p.CreationDate <= dateFin))
                      .Select(p => new PRCItem
                      {
                          ID = p.ID,
                          Libelle = p.Libelle,
                          Perime = p.Perimé,
                          Procedure = p.Procedure,
                          CreationDate = p.CreationDate,
                          FileName = p.FileName,
                          FilePath = p.FilePath,
                          TypeDocument = "ProcDocuments"
                      })
                      .OrderByDynamic(field, direction)
                      .ToListAsync();
                }

                  if( MQ == true)
                  {

                    // Liste résultat pour ProcDocuments
                     resultMQDocuments = await _ctx.MQ
                      .Where(p => p.deleted_at == null && p.Perime == StaticDocuments.Valide
                          && (string.IsNullOrEmpty(libelle) || p.Libelle.ToLower().Contains(libelle.ToLower()))
                          && (string.IsNullOrEmpty(version) || p.Version.ToLower().Contains(version.ToLower()))
                          && (!dateDebut.HasValue || p.CreationDate >= dateDebut)
                          && (!dateFin.HasValue || p.CreationDate <= dateFin))
                      .Select(p => new PRCItem
                      {
                          ID = p.ID,
                          Libelle = p.Libelle,
                          MQ = p.Libelle,
                          Perime = p.Perime,
                          CreationDate = p.CreationDate,
                          FileName = p.FileName,
                          FilePath = p.FilePath,
                          TypeDocument = "MQDocuments"
                      })
                      .OrderByDynamic(field, direction)
                      .ToListAsync();
                  }

                  if (PQ == true)
                  {
                    // Liste résultat pour ProcDocuments
                     resultPQDocuments = await _ctx.PQ
                      .Where(p => p.deleted_at == null && p.Perime == StaticDocuments.Valide
                          && (string.IsNullOrEmpty(libelle) || p.Libelle.ToLower().Contains(libelle.ToLower()))
                          && (string.IsNullOrEmpty(version) || p.Version.ToLower().Contains(version.ToLower()))
                          && (!dateDebut.HasValue || p.CreationDate >= dateDebut)
                          && (!dateFin.HasValue || p.CreationDate <= dateFin))
                      .Select(p => new PRCItem
                      {
                          ID = p.ID,
                          Libelle = p.Libelle,
                          PQ = p.Libelle,
                          Perime = p.Perime,
                          CreationDate = p.CreationDate,
                          FileName = p.FileName,
                          FilePath = p.FilePath,
                          TypeDocument = "PQDocuments"
                      })
                      .OrderByDynamic(field, direction)
                      .ToListAsync();
                  }

                var combinedResult = (resultProcesDocuments ?? Enumerable.Empty<PRCItem>())
                    .Concat(resultProcDocuments ?? Enumerable.Empty<PRCItem>())
                    .Concat(resultMQDocuments ?? Enumerable.Empty<PRCItem>())
                    .Concat(resultPQDocuments ?? Enumerable.Empty<PRCItem>());

                // Paginer et retourner les résultats
                var totalRows = combinedResult.Count();
                var resultTake = combinedResult.Skip(skip).Take(take).ToList();

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
