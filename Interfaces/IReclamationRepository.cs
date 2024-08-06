using ITKANSys_api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITKANSys_api.Interfaces
{
    public interface IReclamationRepository
    {
        Task<IEnumerable<Reclamation>> GetAllAsync();
        Task<Reclamation> GetByIdAsync(int id);
        Task<Reclamation> AddAsync(Reclamation reclamation);
        Task UpdateAsync(Reclamation reclamation);
        Task DeleteAsync(Reclamation reclamation);
        Task<List<Reclamation>?> SearchReclamationByStatus(string status);
        Task<IEnumerable<Reclamation>> GetWithReclamantAsync();

    }

    public class ReclamationRepository : IReclamationRepository
    {
        private readonly ApplicationDbContext _context;

        public ReclamationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

       
        public async Task<IEnumerable<Reclamation>> GetAllAsync()
        {
            return await _context.Reclamations.ToListAsync();
        }

        public async Task<Reclamation> GetByIdAsync(int id)
        {
            return await _context.Reclamations.FindAsync(id);
        }

        public async Task<Reclamation> AddAsync(Reclamation reclamation)
        {
            _context.Reclamations.Add(reclamation);
            await _context.SaveChangesAsync();
            return reclamation;
        }

        public async Task UpdateAsync(Reclamation reclamation)
        {
            _context.Entry(reclamation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Gérer les exceptions liées à la concurrence
                throw new Exception("La mise à jour a échoué en raison d'une exception de concurrence.");
            }
        }

        public async Task DeleteAsync(Reclamation reclamation)
        {
            _context.Reclamations.Remove(reclamation);
            await _context.SaveChangesAsync();
        }


        public async Task<List<Reclamation>?> SearchReclamationByStatus(string Status)
        {
            return await _context.Reclamations
                .Where(r => r.Status == Status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reclamation>> GetWithReclamantAsync()
        {
            return await _context.Reclamations.Include(r => r.Reclamant).ToListAsync();
        }
    }
}
