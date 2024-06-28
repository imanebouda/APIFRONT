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
            // Vérifie l'existence de l'entité TypeCheckListAudit
            var existingTypeCheckListAudit = await _context.TypeCheckList.FindAsync(checkListAudit.typechecklist_id);
            if (existingTypeCheckListAudit == null)
            {
                throw new ArgumentException("Le type de checklist spécifié n'existe pas dans la base de données.");
            }
            checkListAudit.TypeCheckListAudit = existingTypeCheckListAudit;

            // Vérifie l'existence de l'entité Check_list
            var existingCheckList = await _context.Check_lists.FindAsync(checkListAudit.CheckListAuditId);
            if (existingCheckList == null)
            {
                throw new ArgumentException("La checklist spécifiée n'existe pas dans la base de données.");
            }
            checkListAudit.CheckList = existingCheckList;

            // Ajoute l'objet CheckListAudit au contexte
            _context.CheckListAudits.Add(checkListAudit);
            await _context.SaveChangesAsync();

            return checkListAudit;
        }


        public async Task<CheckListAudit> UpdateCheckListAudit(int id, CheckListAudit updatedCheckListAudit)
        {
            // Recherche l'entité CheckListAudit par ID
            var existingCheckListAudit = await _context.CheckListAudits.FindAsync(id);
            if (existingCheckListAudit == null)
            {
                throw new ArgumentException("La checklist spécifiée n'existe pas dans la base de données.");
            }

            // Vérifie l'existence de l'entité TypeCheckListAudit
            var existingTypeCheckListAudit = await _context.TypeCheckList.FindAsync(updatedCheckListAudit.typechecklist_id);
            if (existingTypeCheckListAudit == null)
            {
                throw new ArgumentException("Le type de checklist spécifié n'existe pas dans la base de données.");
            }
            existingCheckListAudit.TypeCheckListAudit = existingTypeCheckListAudit;

            // Vérifie l'existence de l'entité Check_list
            var existingCheckList = await _context.Check_lists.FindAsync(updatedCheckListAudit.CheckListAuditId);
            if (existingCheckList == null)
            {
                throw new ArgumentException("La checklist spécifiée n'existe pas dans la base de données.");
            }
            existingCheckListAudit.CheckList = existingCheckList;

            // Met à jour les autres propriétés de CheckListAudit
            existingCheckListAudit.name = updatedCheckListAudit.name;
            existingCheckListAudit.niveau = updatedCheckListAudit.niveau;
            existingCheckListAudit.code = updatedCheckListAudit.code;
            existingCheckListAudit.description = updatedCheckListAudit.description;

            // Sauvegarde les modifications dans le contexte
            await _context.SaveChangesAsync();

            return existingCheckListAudit;
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
