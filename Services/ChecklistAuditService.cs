using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITKANSys_api.Interfaces;
using ITKANSys_api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITKANSys_api.Services
{
    public class CheckListAuditService : IChecklistAuditService
    {
        private readonly ApplicationDbContext _context;

        public CheckListAuditService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CheckListAudit>> GetAllCheckListAudit()
        {
            return await _context.CheckListAudits
                .Include(c => c.TypeCheckListAudit)
                .ToListAsync();
        }

        public async Task<CheckListAudit?> GetCheckListAudit(int id)
        {
            return await _context.CheckListAudits
                .Include(c => c.TypeCheckListAudit)
                 .Include(c => c.CheckList)
                .FirstOrDefaultAsync(c => c.id == id);
        }


        public async Task<List<CheckListAudit>> GetQuestionsForCheckListAudit(int checkListAuditId)

        {
            return await _context.CheckListAudits
                .Where(c => c.CheckListAuditId == checkListAuditId)
                .Include(c => c.TypeCheckListAudit) // Inclure les données de TypeCheckListAudit si nécessaire
                .ToListAsync();
        }

        public async Task<CheckListAudit> AddCheckListAudit(CheckListAudit checkListAudit)
        {
            // Vérifiez si le type de checklist existe dans la table TypeCheckListAudit
            var existingTypeCheckList = await _context.TypeCheckList.FindAsync(checkListAudit.typechecklist_id);

            if (existingTypeCheckList == null)
            {
                throw new ArgumentException("Le type de checklist spécifié n'existe pas dans la base de données.");
            }

            // Assurez-vous que la référence au type de checklist est correctement définie dans l'objet CheckListAudit
            checkListAudit.TypeCheckListAudit = existingTypeCheckList;

            // Ajoutez l'objet CheckListAudit au contexte sans ajouter de nouvelle ligne à la table TypeCheckListAudit
            _context.CheckListAudits.Add(checkListAudit);
            await _context.SaveChangesAsync();

            return checkListAudit;
        }

        public async Task<List<CheckListAudit>?> UpdateCheckListAudit(int id, CheckListAudit request)
        {
            var checkListAudit = await _context.CheckListAudits.FindAsync(id);
            if (checkListAudit == null)
                return null;

            // Check if the provided typechecklist_id exists in the TypeCheckList table
            var typeCheckListExists = await _context.TypeCheckList.AnyAsync(t => t.id == request.typechecklist_id);
            if (!typeCheckListExists)
            {
                throw new Exception($"TypeCheckList with id {request.typechecklist_id} does not exist.");
            }

            checkListAudit.name = request.name;
            checkListAudit.niveau = request.niveau;
            checkListAudit.code = request.code;
            checkListAudit.description = request.description;
            checkListAudit.typechecklist_id = request.typechecklist_id;

            await _context.SaveChangesAsync();

            return await GetAllCheckListAudit();
        }


        public async Task<List<CheckListAudit>?> DeleteCheckListAudit(int id)
        {
            var checkListAudit = await _context.CheckListAudits.FindAsync(id);
            if (checkListAudit == null)
                return null;

            _context.CheckListAudits.Remove(checkListAudit);
            await _context.SaveChangesAsync();

            return await GetAllCheckListAudit();
        }

        public async Task<List<CheckListAudit>> SearchChecklistByType(int typeChecklistId)
        {
            return await _context.CheckListAudits
                .Where(c => c.TypeCheckListAudit.id == typeChecklistId)
                .ToListAsync();
        }
    }
}
