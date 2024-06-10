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
            return await _context.Constat.ToListAsync();


        }







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
        }
    }
}
