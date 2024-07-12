using ITKANSys_api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ITKANSys_api.Services
{
    public class PreuveService : IPreuve
    {
        private readonly ApplicationDbContext _context;

        public PreuveService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Models.Entities.Preuve>> GetAllPreuves()
        {
            return await _context.Preuves
                .Include(p => p.Action)
                .ToListAsync();
        }

        public async Task<Models.Entities.Preuve?> GetPreuve(int preuveId)
        {
            return await _context.Preuves
                .Include(p => p.Action)
                .FirstOrDefaultAsync(p => p.ID == preuveId);
        }

        public async Task<Models.Entities.Preuve> AddPreuve(Models.Entities.Preuve preuve)
        {
            // Vérifie l'existence de l'entité Action
            var existingAction = await _context.Actions.FindAsync(preuve.ActionId);
            if (existingAction == null)
            {
                throw new ArgumentException("L'action spécifiée n'existe pas dans la base de données.");
            }
            preuve.Action = existingAction;

            // Ajoute l'objet Preuve au contexte
            _context.Preuves.Add(preuve);
            await _context.SaveChangesAsync();

            return preuve;
        }

        public async Task<Models.Entities.Preuve?> UpdatePreuve(int preuveId, Models.Entities.Preuve request)
        {
            var existingPreuve = await _context.Preuves.FindAsync(preuveId);

            if (existingPreuve == null)
            {
                return null;
            }

            // Mise à jour des propriétés
            existingPreuve.filename = request.filename;
            existingPreuve.filepath = request.filepath;
            existingPreuve.CreationDate = request.CreationDate;

            if (request.ActionId != 0) // Vérifie si ActionId est fourni et différent de 0
            {
                var existingAction = await _context.Actions.FindAsync(request.ActionId);
                if (existingAction == null)
                {
                    throw new ArgumentException("L'action spécifiée n'existe pas dans la base de données.");
                }
                existingPreuve.ActionId = request.ActionId;
                existingPreuve.Action = existingAction;
            }

            await _context.SaveChangesAsync();

            return existingPreuve;
        }

        public async Task<List<Models.Entities.Preuve>?> DeletePreuve(int preuveId)
        {
            var preuve = await _context.Preuves.FindAsync(preuveId);

            if (preuve == null)
            {
                return null;
            }

            _context.Preuves.Remove(preuve);
            await _context.SaveChangesAsync();

            return await _context.Preuves.ToListAsync();
        }
    }
}
