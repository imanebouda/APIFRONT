using Microsoft.EntityFrameworkCore;

namespace ITKANSys_api.Interfaces
{
    public class ReclamantRepository
    {

        private readonly ApplicationDbContext _context;

        public ReclamantRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Reclamant>> GetAllAsync()
        {
            return await _context.Reclamants.ToListAsync();
        }

        public async Task<Reclamant> GetByIdAsync(int id)
        {
            return await _context.Reclamants.FindAsync(id);
        }

        public async Task<Reclamant> CreateAsync(Reclamant reclamant)
        {
            _context.Reclamants.Add(reclamant);
            await _context.SaveChangesAsync();
            return reclamant;
        }

        public async Task UpdateAsync(Reclamant reclamant)
        {
            _context.Entry(reclamant).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var reclamant = await _context.Reclamants.FindAsync(id);
            if (reclamant != null)
            {
                _context.Reclamants.Remove(reclamant);
                await _context.SaveChangesAsync();
            }
        }
    }
}
