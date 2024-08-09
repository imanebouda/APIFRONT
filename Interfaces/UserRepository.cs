using ITKANSys_api.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace ITKANSys_api.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersByRoleAsync(int roleId);
        

    }

    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(int roleId)
        {
            return await _context.Users.Where(u => u.IdRole == roleId).ToListAsync();
        }

      
    }

}
