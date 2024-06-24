using ITKANSys_api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ITKANSys_api.Services
{
    public class Check_listServices:ICheck_listService
    {

        private readonly ApplicationDbContext _context;

        public Check_listServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Check_list>> GetAllCheckListAudit()
        {
            //return await _context.Check_lists.ToListAsync();

            return await _context.Check_lists
                .Include(a => a.typeAudit)
                .Include(a => a.SMQ)
                .Include(a => a.Processus)
                .ToListAsync();
        }

    

        public async Task<Check_list?> GetCheckListAudit(int checkListId)
        {
            return await _context.Check_lists.FindAsync(checkListId);
        }

        public async Task<Check_list> AddCheckListAudit(Check_list checkListAudit)
        {


            // Vérifie l'existence de l'entité TypeAudit
            var existingTypeAudit = await _context.typeAudit.FindAsync(checkListAudit.typeAuditId);
            if (existingTypeAudit == null)
            {
                throw new ArgumentException("Le type d'audit spécifié n'existe pas dans la base de données.");
            }
            checkListAudit.typeAudit = existingTypeAudit;

            // Vérifie l'existence de l'entité Smq
            var existingSmq = await _context.SMQ.FindAsync(checkListAudit.SMQ_ID);
            if (existingSmq == null)
            {
                throw new ArgumentException("Le smq spécifié n'existe pas dans la base de données.");
            }
            checkListAudit.SMQ = existingSmq;

            // Vérifie l'existence de l'entité Processus
            var existingProcessus = await _context.Processus.FindAsync(checkListAudit.ProcessusID);
            if (existingProcessus == null)
            {
                throw new ArgumentException("Le processus spécifié n'existe pas dans la base de données.");
            }
            checkListAudit.Processus = existingProcessus;

            // Ajoute l'objet Check_list au contexte
            _context.Check_lists.Add(checkListAudit);
            await _context.SaveChangesAsync();

            return checkListAudit;
        }

        public async Task<Check_list?> UpdateCheckListAudit(int checkListId, Check_list request)
        {
            var existingCheckListAudit = await _context.Check_lists.FindAsync(checkListId);

            if (existingCheckListAudit == null)
            {
                return null;
            }

            // Update properties
            if (request.typeAuditId != 0) // Vérifie si typeAuditId est fourni et différent de 0
            {
                existingCheckListAudit.typeAuditId = request.typeAuditId;
            }

            if (request.SMQ_ID != 0) // Vérifie si SMQ_ID est fourni et différent de 0
            {
                existingCheckListAudit.SMQ_ID = request.SMQ_ID;
            }

            if (request.ProcessusID != 0) // Vérifie si ProcessusID est fourni et différent de 0
            {
                existingCheckListAudit.ProcessusID = request.ProcessusID;
            }

            // Ajoutez d'autres propriétés à mettre à jour au besoin

            await _context.SaveChangesAsync();

            return existingCheckListAudit;
        }

        public async Task<List<Check_list>?> DeleteCheckListAudit(int checkListId)
        {
            var checkListAudit = await _context.Check_lists.FindAsync(checkListId);

            if (checkListAudit == null)
            {
                return null;
            }

            _context.Check_lists.Remove(checkListAudit);
            await _context.SaveChangesAsync();

            return await _context.Check_lists.ToListAsync();
        }

    }
}
