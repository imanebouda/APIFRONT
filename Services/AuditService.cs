using AutoMapper;
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

        public AuditService(ApplicationDbContext context, IMapper mapper, formatObject formatObject)
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
            var audit = await _context.Audit.FindAsync(id);
            if (audit == null)
                return null;

            _context.Audit.Remove(audit);
            await _context.SaveChangesAsync();

            return await GetAllAudit();
        }

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

      
    }
}