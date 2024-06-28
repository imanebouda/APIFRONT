using ITKANSys_api.Interfaces;
using ITKANSys_api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITKANSys_api.Services
{
    public class ConstatService : IConstatService
    {



        private readonly ApplicationDbContext _context;

        public ConstatService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Constat>> GetAllConstat()
        {
            //return await _context.Constat.ToListAsync();
            return await _context.Constat
                .Include(a => a.typeConstat)
                .Include(a => a.Checklist)
               .ToListAsync();


        }
        public async Task<List<Constat>> AddConstat(Constat constat)
        {
            // Vérifie l'existence de l'entité TypeConstat
            var existingTypeConstat = await _context.Set<TypeContat>().FindAsync(constat.typeConstatId);
            if (existingTypeConstat == null)
            {
                throw new ArgumentException("Le type de constat spécifié n'existe pas dans la base de données.");
            }
            constat.typeConstat = existingTypeConstat;

            // Vérifie l'existence de l'entité CheckListAudit
            var existingChecklist = await _context.Set<CheckListAudit>().FindAsync(constat.ChecklistId);
            if (existingChecklist == null)
            {
                throw new ArgumentException("La checklist spécifiée n'existe pas dans la base de données.");
            }
            constat.Checklist = existingChecklist;

            // Ajoute l'objet Constat au contexte
            _context.Set<Constat>().Add(constat);
            await _context.SaveChangesAsync();

            // Retourne la liste mise à jour des Constats (ou toute autre opération nécessaire)
            return await _context.Set<Constat>().ToListAsync();
        }



        public async Task<List<Constat>?> DeleteConstat(int id)
        {
            var constat = await _context.Set<Constat>().FindAsync(id);

            if (constat != null)
            {
                _context.Set<Constat>().Remove(constat);
                await _context.SaveChangesAsync();
                return await _context.Set<Constat>().ToListAsync();
            }

            return null;
        }

        public async Task<Constat?> GetConstat(int id)
        {
            return await _context.Set<Constat>().FindAsync(id);
        }

        public async Task<List<Constat>?> UpdateConstat(int id, Constat request)
        {
            var constat = await _context.Set<Constat>().FindAsync(id);

            if (constat != null)
            {
                // Fetch related entities
                var typeConstat = await _context.Set<TypeContat>().FindAsync(request.typeConstatId);
                var checklist = await _context.Set<CheckListAudit>().FindAsync(request.ChecklistId);

                if (typeConstat != null && checklist != null)
                {
                    constat.constat = request.constat;
                    constat.typeConstatId = request.typeConstatId;
                    constat.ChecklistId = request.ChecklistId;
                    constat.typeConstat = typeConstat;
                    constat.Checklist = checklist;

                    _context.Set<Constat>().Update(constat);
                    await _context.SaveChangesAsync();
                    return await _context.Set<Constat>().ToListAsync();
                }
            }

            return null;
        }



        /*


        Task<List<Constat>> IConstatService.AddConstat(Constat constat)
        {
            throw new NotImplementedException();
        }

        Task<List<Constat>?> IConstatService.DeleteConstat(int id)
        {
            throw new NotImplementedException();
        }

       

        Task<Constat?> IConstatService.GetConstat(int id)
        {
            throw new NotImplementedException();
        }

        Task<List<Constat>?> IConstatService.UpdateConstat(int id, Constat request)
        {
            throw new NotImplementedException();
        }*/
    }
}
