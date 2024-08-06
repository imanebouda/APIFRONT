using ITKANSys_api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITKANSys_api.Interfaces
{
    public interface IComiteeReclamationRepository
    {
        Task<IEnumerable<ComiteeReclamation>> GetAllAsync();
        Task<ComiteeReclamation> GetByIdAsync(int id);
        Task<ComiteeReclamation> AddAsync(ComiteeReclamation comiteeReclamation);
        Task UpdateAsync(ComiteeReclamation comiteeReclamation);
        Task DeleteAsync(ComiteeReclamation comiteeReclamation);
    }

    public class ComiteeReclamationRepository : IComiteeReclamationRepository
    {
        private readonly ApplicationDbContext _context;

        public ComiteeReclamationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ComiteeReclamation>> GetAllAsync()
        {
            return await _context.ComiteeReclamation.ToListAsync();
        }

        public async Task<ComiteeReclamation> GetByIdAsync(int id)
        {
            return await _context.ComiteeReclamation.FindAsync(id);
        }

        public async Task<ComiteeReclamation> AddAsync(ComiteeReclamation comiteeReclamation)
        {
            _context.ComiteeReclamation.Add(comiteeReclamation);
            await _context.SaveChangesAsync();
            return comiteeReclamation;
        }

        public async Task UpdateAsync(ComiteeReclamation comiteeReclamation)
        {
            _context.ComiteeReclamation.Update(comiteeReclamation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ComiteeReclamation comiteeReclamation)
        {
            _context.ComiteeReclamation.Remove(comiteeReclamation);
            await _context.SaveChangesAsync();
        }
    }
}
