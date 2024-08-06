using ITKANSys_api.Interfaces;
using ITKANSys_api.Models;
using Microsoft.EntityFrameworkCore;

namespace ITKANSys_api.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetUsersByRoleAsync(int roleId);
        Task<IEnumerable<User>> GetUserByRolesAsync(List<int> roleIds);
        Task<bool> DeleteUserAsync(int id);
    }


        public class UserService : IUserService
        {
            private readonly ApplicationDbContext _context;
            private readonly IUserRepository _userRepository;

        public UserService(ApplicationDbContext context, IUserRepository userRepository)
            {
                _context = context;
            _userRepository = userRepository;
        }

            public async Task<IEnumerable<User>> GetAllUsersAsync()
            {
                return await _context.Users.ToListAsync();
            }


        public async Task<IEnumerable<User>> GetUsersByRoleAsync(int roleId)
        {
            return await _userRepository.GetUsersByRoleAsync(roleId);
        }
        public async Task<IEnumerable<User>> GetUserByRolesAsync(List<int> roleIds)
        {
            return await _context.Users
                .Where(u => roleIds.Contains(u.IdRole))
                .ToListAsync();
        }
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            // Delete dependent ComiteeReclamation records
            var comiteeReclamations = await _context.ComiteeReclamation
                .Where(cr => cr.ConcernedID == id)
                .ToListAsync();

            _context.ComiteeReclamation.RemoveRange(comiteeReclamations);

            // Now delete the user
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }



    }
}


