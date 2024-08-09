using Microsoft.EntityFrameworkCore;

namespace ITKANSys_api.Interfaces
{
    public interface IHistoryReclamationRepository
    {

        Task<IEnumerable<HistoryReclamation>> GetAllAsync();
        Task<HistoryReclamation> GetByIdAsync(int id);
        Task<HistoryReclamation> AddAsync(HistoryReclamation historyReclamation);
        Task UpdateAsync(HistoryReclamation historyReclamation);
        Task DeleteAsync(HistoryReclamation historyReclamation);
    }

    public class HistoryReclamationRepository : IHistoryReclamationRepository
    {
        private readonly ApplicationDbContext _context;

        public HistoryReclamationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HistoryReclamation>> GetAllAsync()
        {
            return await _context.HistoryReclamation.ToListAsync();
        }

        public async Task<HistoryReclamation> GetByIdAsync(int id)
        {
            return await _context.HistoryReclamation.FindAsync(id);
        }

        public async Task<HistoryReclamation> AddAsync(HistoryReclamation historyReclamation)
        {
            _context.HistoryReclamation.Add(historyReclamation);
            await _context.SaveChangesAsync();
            return historyReclamation;
        }

        public async Task UpdateAsync(HistoryReclamation historyReclamation)
        {
            _context.HistoryReclamation.Update(historyReclamation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(HistoryReclamation historyReclamation)
        {
            _context.HistoryReclamation.Remove(historyReclamation);
            await _context.SaveChangesAsync();
        }
    }
}
