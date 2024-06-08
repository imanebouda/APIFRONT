﻿using AutoMapper;
using ITKANSys_api.Config;
using ITKANSys_api.Interfaces;
using ITKANSys_api.Models;
using ITKANSys_api.Models.Dtos;
using ITKANSys_api.Models.Entities;
using ITKANSys_api.Utility.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace ITKANSys_api.Services
{

    public class AuditService : IAuditService
    {

        private static List<Audit> audits = new List<Audit>
    {// new Audit
       /* {
            ID = 1,
            NomAudit = "audit Informatique",
            status = "en cours",
            typeAudit = "informatique"
        },
        new Audit
        {
            ID = 2,
            NomAudit = "audit Informatique",
            status = "en cours",
            typeAudit = "informatique"
        }*/

         
    };
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly formatObject _formatObject;
        private IConfiguration configuration;

        public AuditService(ApplicationDbContext context, IMapper mapper , formatObject formatObject)
        {
            _context = context;
            _mapper = mapper;
            _formatObject = formatObject;
            configuration = AppConfig.GetConfig();

        }
       
        public async Task<Audit> AddAudit(Audit audit)
        {
            DataSourceResult dataResult = new DataSourceResult();

            var existingTypeAudit = await _context.typeAudit.FindAsync(audit.typeAuditId);
            if (existingTypeAudit == null)
            {
                throw new ArgumentException("Le type de audit spécifié n'existe pas dans la base de données.");
            }

            // Assurez-vous que la référence au type de checklist est correctement définie dans l'objet CheckListAudit
            audit.typeAudit = existingTypeAudit;

            // Ajoutez l'objet CheckListAudit au contexte sans ajouter de nouvelle ligne à la table TypeCheckListAudit
            _context.Audit.Add(audit);
            await _context.SaveChangesAsync();

            return audit;

        }


        public async Task<List<Audit>?> DeleteAudit(int id)
        {
<<<<<<< HEAD
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body;

            try
            {
                body = JObject.Parse(jsonString);

                if (body.id != null)
                {
                    int ID = Convert.ToInt32(body.id);

                    var existingAudit = await _context.Audit.FirstOrDefaultAsync(a => a.ID == ID);


                    if (existingAudit != null)
                    {
                        _context.Audit.Remove(existingAudit);
                        await _context.SaveChangesAsync();

                        dataResult.codeReponse = CodeReponse.ok;
                        dataResult.msg = "Supprimé";
                    }
                    else
                    {
                        dataResult.codeReponse = CodeReponse.erreur;
                        dataResult.msg = "Audit non trouvé";
                    }
                }
                else
                {
                    dataResult.codeReponse = CodeReponse.erreur;
                    dataResult.msg = "ID de l'audit non spécifié";
                }
            }
            catch (Exception ex)
            {
                dataResult.codeReponse = CodeReponse.error;
                dataResult.msg = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
            }

            return dataResult;
        }

        /*2
        public async Task<DataSourceResult> DeleteAudit(object audit)
        {
            DataSourceResult dataResult = new DataSourceResult();
            dynamic body;

            try
            {
                string jsonString = audit.ToString();
                jsonString = jsonString.Replace("ValueKind = Object : ", "");

                if (string.IsNullOrEmpty(jsonString))
                {
                    dataResult.codeReponse = CodeReponse.errorMissingAllParams;
                    dataResult.msg = configuration.GetSection("MessagesAPI:ParamsEmpty").Value;
                }
                else
                {
                    body = JObject.Parse(jsonString);
                    using (_context)
                    {

                        if (body.id != null)
                        {
                            int ID = Convert.ToInt32(body.id);

                            var existingProcedure = await _context.Audit.FindAsync(ID);



                            // Marquez le processus comme supprimé (selon votre logique de suppression)


                            // Utilisez la méthode Remove pour marquer l'entité comme supprimée
                            _context.Audit.Remove(existingProcedure);

                            // Enregistrez les modifications dans la base de données
                            await _context.SaveChangesAsync();
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

        */
        /*
        public async Task<DataSourceResult> DeleteAudit(int id)
        {
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                var auditToDelete = await _context.Audit.FindAsync(id);
                if (auditToDelete == null)
                {
                    dataResult.codeReponse = CodeReponse.erreur;
                    dataResult.msg = "L'audit spécifié n'a pas été trouvé.";
                }
                else
                {
                    _context.Audit.Remove(auditToDelete);
                    await _context.SaveChangesAsync();
                    dataResult.codeReponse = CodeReponse.ok;
                    dataResult.msg = "Audit supprimé avec succès.";
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
        */
        /*
        public  async Task<DataSourceResult> DeleteAudit(object audit)
        {


            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                string jsonString = audit.ToString();
                jsonString = jsonString.Replace("ValueKind = Object : ", "");

                if (string.IsNullOrEmpty(jsonString))
                {
                    dataResult.codeReponse = CodeReponse.errorMissingAllParams;
                    dataResult.msg = configuration.GetSection("MessagesAPI:ParamsEmpty").Value;
                }
                else
                {
                    dynamic deleteAudit = JObject.Parse(jsonString);
                    using (_context)
                    {
                        if (deleteAudit.id != null)
                        {
                            int id = Convert.ToInt32(deleteAudit.id);

                            var auditToDelete = await _context.Audit.FindAsync(id);
                            if (auditToDelete == null)
                            {
                                dataResult.codeReponse = CodeReponse.erreur;
                                dataResult.msg = "L'audit spécifié n'a pas été trouvé.";
                            }
                            else
                            {
                                _context.Audit.Remove(auditToDelete);
                                await _context.SaveChangesAsync();
                                dataResult.codeReponse = CodeReponse.ok;
                                dataResult.msg = "Audit supprimé avec succès.";
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
                var errorResponse = new DataSourceResult
                {
                    IsSucceed = false,
                    Message = (ex.InnerException == null) ? ex.Message : ex.InnerException.Message,
                };

                // Log l'erreur si nécessaire.

                return errorResponse;
            }
            return dataResult;

        }*/

        public async Task<List<Audit>> GetAllAudit()
        
        {



            try
            {
                var audits = await _context.Audit.ToListAsync();
                return audits;
            }
            catch (Exception ex)
            {
                // Gérer l'exception ici
                // Vous pouvez enregistrer les détails de l'exception dans les journaux ou effectuer d'autres actions nécessaires.
                // Ne pas oublier de logger l'exception si nécessaire.
                Console.WriteLine($"Une exception s'est produite : {ex.Message}");
                throw; // Remonte l'exception pour la gestion centralisée des erreurs, si elle est configurée.
            }
        
    }
  

           

        

        public async Task<Audit?> GetAudit(int id)
        {
            try
            {
                var audit = await _context.Audit.FindAsync(id);
                if (audit is null)
                    return null;

                return audit;
            }
            catch (Exception ex)
            {
                // Gérer l'exception ici
                // Vous pouvez enregistrer les détails de l'exception dans les journaux ou effectuer d'autres actions nécessaires.
                // Ne pas oublier de logger l'exception si nécessaire.
                Console.WriteLine($"Une exception s'est produite : {ex.Message}");
                throw; // Remonte l'exception pour la gestion centralisée des erreurs, si elle est configurée.
            }
           
        }
        public async Task<DataSourceResult> UpdateAudit(int id, object audit)
        {
            DataSourceResult dataResult = new DataSourceResult();

            try
            {
                // Convertir l'objet en chaîne JSON
                string jsonString = audit.ToString();
                jsonString = jsonString.Replace("ValueKind = Object : ", "");
                dynamic updatedAudit = JObject.Parse(jsonString);

                // Rechercher l'audit à mettre à jour
                var existingAudit = await _context.Audit.FindAsync(id);
                if (existingAudit == null)
                {
                    dataResult.codeReponse = CodeReponse.erreur;
                    dataResult.msg = "L'audit spécifié n'a pas été trouvé.";
                    return dataResult;
                }

                // Mettre à jour les propriétés de l'audit
                if (updatedAudit.nomAudit != null)
                    existingAudit.NomAudit = updatedAudit.nomAudit;
                if (updatedAudit.dateAudit != null)
                    existingAudit.DateAudit = Convert.ToDateTime(updatedAudit.dateAudit).AddDays(1);
                if (updatedAudit.status != null)
                    existingAudit.status = updatedAudit.status;
                if (updatedAudit.description != null)
                    existingAudit.description = updatedAudit.description;
                if (updatedAudit.typeAudit != null)
                    existingAudit.typeAudit = updatedAudit.typeAudit;

                // Enregistrer les modifications dans la base de données
                await _context.SaveChangesAsync();

                dataResult.codeReponse = CodeReponse.ok;
                dataResult.msg = "Audit mis à jour avec succès.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une exception s'est produite : {ex.Message}");
                dataResult.codeReponse = CodeReponse.erreur;
                dataResult.msg = "Une exception s'est produite lors de la mise à jour de l'audit.";
            }
            return dataResult;
=======
            var audit = await _context.Audit.FindAsync(id);
            if (audit == null)
                return null;
>>>>>>> 7d731497bc5de5f582f9c84fecac832e8e0f1223

            _context.Audit.Remove(audit);
            await _context.SaveChangesAsync();

            return await GetAllAudit();
        }
<<<<<<< HEAD

       public async Task<List<Audit>> GetAuditsByDate(DateTime date)
        {
            try
            {
                var audits = await _context.Audit
                    .Where(a => a.DateAudit.Date == date.Date)
                    .ToListAsync();
                return audits;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une exception s'est produite : {ex.Message}");
                throw;
            }
        }
        public async Task<List<Audit>> GetAuditsByType(string type)
        {
            try
            {
                var audits = await _context.Audit
                    .Where(a => a.typeAudit == type)
                    .ToListAsync();
                return audits;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une exception s'est produite : {ex.Message}");
                throw;
            }
        }
=======
       
        public async Task<List<Audit>> GetAllAudit()
        {
        return await _context.Audit
                .Include(c => c.typeAudit)
                .ToListAsync();
        }
        public async Task<Audit?> GetAudit(int id)
        {
            return await _context.Audit
                .Include(c => c.typeAudit)
                .FirstOrDefaultAsync(c => c.ID == id);
        }

        public async Task<List<Audit>?> UpdateAudit(int id, Audit request)
        {
            var audit = await _context.Audit.FindAsync(id);
            if (audit == null)
                return null;
            // Check if the provided typechecklist_id exists in the TypeCheckList table
            var typeCheckListExists = await _context.TypeCheckList.AnyAsync(t => t.id == request.typeAuditId);
            if (!typeCheckListExists)
            {
                throw new Exception($"TypeCheckList with id {request.typeAuditId} does not exist.");
            }

            // Mettre à jour les propriétés de l'audit

                    audit.NomAudit = request.NomAudit;
                    audit.DateAudit = request.DateAudit;
                    audit.status = request.status;
                    audit.description = request.description;
                    audit.typeAuditId = request.typeAuditId;

            // Enregistrer les modifications dans la base de données
            await _context.SaveChangesAsync();

            return await GetAllAudit();
        }
      
        public async Task<List<Audit>> searchAuditByType(int typeAuditId)
        {
            return await _context.Audit
                .Where(c => c.typeAudit.id == typeAuditId)
                .ToListAsync();
        }
        
>>>>>>> 7d731497bc5de5f582f9c84fecac832e8e0f1223
    }




}
