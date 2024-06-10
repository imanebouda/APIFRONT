using ITKANSys_api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ITKANSys_api.Services
{
    public class TypeConstatService : ITypeConstatServicecs
    {


        private readonly ApplicationDbContext _context;

        public TypeConstatService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TypeContat>> GetAllTypeConstat()
        {
           return await _context.TypeContat.ToListAsync();



        }
     



    }
}
