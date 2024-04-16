using ITKANSys_api.Config;
using ITKANSys_api.Data.OtherObjects;
using ITKANSys_api.Models.Dtos;
using ITKANSys_api.Models;
using ITKANSys_api.Utility.ApiResponse;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using static ITKANSys_api.Utility.ApiResponse.QueryableExtensions;
using System;
using ITKANSys_api.Interfaces;

namespace ITKANSys_api.Services
{
    public class PQService : IPQService 
    {
        private IConfiguration configuration;

        //Contexte de la base de données
        private ApplicationDbContext _ctx;
        /// <summary>
        /// Constructeur par défaut
        /// < /summary>
        public PQService(ApplicationDbContext context)
        {
            configuration = AppConfig.GetConfig();
            _ctx = context;
        }
        public async Task<DataSourceResult> InsertPQ(IFormFile file, string libelle, int smqID,string version , string description, DateTime dateApplication, CancellationToken cancellationtoken)
        {
            DataSourceResult dataResult = new DataSourceResult();
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                var filename = DateTime.Now.Ticks.ToString() + extension;

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files\\PQ");

                DateTime Date = dateApplication.AddDays(1);
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var exactpath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files\\PQ", filename);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Maintenant, enregistrez les informations du fichier dans la base de données
                var pq = new PQ
                {
                    Libelle = libelle,
                    SMQID = smqID,
                    FilePath = filepath,
                    FileName = filename,
                    Version = version,
                    Description = description,
                    DateApplication = Date,
                    Perime = StaticDocuments.NonValide,
                    CreationDate = DateTime.Now,
                    created_at = DateTime.Now,
                };

                await _ctx.PQ.AddAsync(pq);
                await _ctx.SaveChangesAsync();

                // Mettre à jour tous les documents précédents avec "Perime" à "true"
                var documentsPrecedents = await _ctx.PQ
                    .Where(doc => doc.SMQID == smqID && doc.ID != pq.ID)
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


        public async Task<byte[]> GetPdf(int pqId)
        {
            try
            {
                var pq = await _ctx.PQ.FindAsync(pqId);

                if (pq != null)
                {
                    var filePath = Path.Combine(pq.FilePath, pq.FileName);

                    if (File.Exists(filePath))
                    {
                        return await File.ReadAllBytesAsync(filePath);
                    }
                }

                // Gérer le cas où le fichier PDF n'est pas trouvé.
                throw new FileNotFoundException("Fichier PDF non trouvé.");
            }
            catch (Exception ex)
            {
                // Gérer l'erreur si nécessaire.
                throw new ApplicationException("Erreur lors de la récupération du fichier PDF.", ex);
            }
        }



        public async Task<DataSourceResult> UpdatePQ(string Libelle, int ID, int SMQID, string Version, string Description, DateTime DateApplication)
        {
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body;

            try
            {
                using (_ctx)
                {
                    if (ID != 0)
                    {
                        var PQ_ID = Convert.ToInt32(ID);
                        DateTime Date = DateApplication.AddDays(1);
                        var existingPQ = await _ctx.PQ.FindAsync(PQ_ID);

                        if (existingPQ != null)
                        {
                            existingPQ.Libelle = Libelle;
                            existingPQ.SMQID = SMQID;
                            existingPQ.Version = Version;
                            existingPQ.Description = Description;
                            existingPQ.DateApplication = Date;
                            existingPQ.updated_at = DateTime.Now;

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




        public async Task<GetDataResult> DetailPQ()
        {
            GetDataResult dataResult = new GetDataResult();

            try
            {
                    using (_ctx)
                    {
                        var existingPQ = await _ctx.PQ
                            .Where(p => p.deleted_at == null && p.Perime == StaticDocuments.NonValide)
                            .OrderByDescending(p => p.CreationDate)
                            .FirstOrDefaultAsync();
                   
                        if (existingPQ != null)
                        {
                        int SMQID = existingPQ.SMQID;
                        var SMQ = await _ctx.SMQ.FirstOrDefaultAsync(s => s.ID == SMQID);
                        // Vous pouvez mapper les propriétés de existingProcessus vers ProcesDocumentsDto si nécessaire
                        var resultPQDto = new PQDto
                            {
                                ID = existingPQ.ID,
                                Libelle = existingPQ.Libelle,
                                Perime = existingPQ.Perime,
                                CreationDate = existingPQ.CreationDate,
                                DateApplication = existingPQ.DateApplication,
                                Version = existingPQ.Version,
                                Description = existingPQ.Description,
                                FileName = existingPQ.FileName,
                                FilePath = existingPQ.FilePath,
                                ExtensionFile = "." + existingPQ.FileName.Split('.')[existingPQ.FileName.Split('.').Length - 1],
                                SMQ =SMQ,
                                TypeDocument = "PQ",
                         };

                            dataResult.IsSucceed = true;
                            dataResult.Message = "Détails";
                            dataResult.DataPQ = resultPQDto;
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



        public async Task<DataSourceResult> SupprimerPQ(object pq)
        {
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body;

            try
            {
                string jsonString = pq.ToString();
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
                            var pqID = Convert.ToInt32(body.ID);


                            var existingPQ = await _ctx.PQ.FindAsync(pqID);

                            existingPQ.deleted_at = DateTime.Now;

                            _ctx.PQ.Remove(existingPQ);

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

        public async Task<DataSourceResult> UpdatePerimeState(object PQdocument)
        {
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body;

            try
            {
                string jsonString = PQdocument.ToString();
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
                        int documentId = Convert.ToInt32(body.documentId);
                        var newState = Convert.ToInt32(body.newState);
                        PQ existingDocument = await _ctx.PQ.FindAsync(documentId);

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
                            var otherDocuments = await _ctx.PQ
                                .Where(pd => pd.ID != existingDocument.ID)
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


        public async Task<bool> DocumentContainsValidDocument(int documentId,  string documentType)
        {
            try
            {
                using (_ctx)
                {
                    if (documentType == "ProcesDocuments")
                    {
                        // Vérifie s'il existe un document valide lié au document de processus spécifié
                        var hasValidDocument = await _ctx.PQ
                            .AnyAsync(pd => pd.ID != documentId && pd.Perime == StaticDocuments.NonValide );

                        return hasValidDocument;
                    }
                    else if (documentType == "ProcDocuments")
                    {
                        // Vérifie s'il existe un document valide lié au document de procédure spécifié
                        var hasValidDocument = await _ctx.PQ
                            .AnyAsync(pd => pd.ID != documentId && pd.Perime == StaticDocuments.NonValide );

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
        }



        public async Task<DataSourceResult> RechercheDocumentsPerime(int procedureID, int SmqID, string? code, string? version, string? libelle, DateTime? dateDebut, DateTime? dateFin, string field, string order, int take, int skip)
        {
            try
            {
                // Assurez-vous que les valeurs par défaut sont correctes
                SmqID = (procedureID > 0) ? 0 : SmqID;
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
                List<PRCItem> resultProcesDocuments = new List<PRCItem>();

                if (procedureID == 0)
                {
                    // Exécutez la requête pour ProcesDocuments
                    resultProcesDocuments = await _ctx.ProcesDocuments
                        .Where(p => p.deleted_at == null && p.Perime == StaticDocuments.Valide
                            && (string.IsNullOrEmpty(libelle) || p.Libelle.ToLower().Contains(libelle.ToLower()))
                            && (string.IsNullOrEmpty(code) || p.Processus.Code.ToLower().Contains(code.ToLower()))
                            && (string.IsNullOrEmpty(version) || p.Processus.Version.ToLower().Contains(version.ToLower()))
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

                // Liste résultat pour ProcDocuments
                var resultProcDocuments = await _ctx.ProcDocuments
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

                // Combinez les résultats
                var combinedResult = resultProcesDocuments.Concat(resultProcDocuments);

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
