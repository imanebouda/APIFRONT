using ITKANSys_api.Interfaces;
using ITKANSys_api.Models;
using ITKANSys_api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITKANSys_api.Services
{
    public interface IComiteeReclamationService
    {
        Task<IEnumerable<ComiteeReclamation>> GetAllComiteeReclamationsAsync();
        Task<ComiteeReclamation> GetComiteeReclamationByIdAsync(int id);
        Task<ComiteeReclamation> CreateComiteeReclamationAsync(ComiteeReclamation comiteeReclamation);
        Task<bool> UpdateComiteeReclamationAsync(int id, ComiteeReclamation comiteeReclamation);
        Task DeleteComiteeReclamationAsync(int id);
        Task<IEnumerable<User>> GetConcernedUsersAsync(int roleId);
        Task<IEnumerable<User>> GetConcernedUserAsync(int id);
        Task RemoveUserFromComiteeAsync(int reclamationId, int userId);


    }

    public class ComiteeReclamationService : IComiteeReclamationService
    {
        private readonly IComiteeReclamationRepository _comiteeReclamationRepository;
        private readonly ApplicationDbContext _context;

        public ComiteeReclamationService(IComiteeReclamationRepository comiteeReclamationRepository, ApplicationDbContext context)
        {
            _comiteeReclamationRepository = comiteeReclamationRepository;
            _context = context;
        }

        public async Task<IEnumerable<ComiteeReclamation>> GetAllComiteeReclamationsAsync()
        {
            return await _comiteeReclamationRepository.GetAllAsync();
        }

        public async Task<ComiteeReclamation> GetComiteeReclamationByIdAsync(int id)
        {
            return await _comiteeReclamationRepository.GetByIdAsync(id);
        }

        public async Task<ComiteeReclamation> CreateComiteeReclamationAsync(ComiteeReclamation comiteeReclamation)
        {
            return await _comiteeReclamationRepository.AddAsync(comiteeReclamation);
        }

        public async Task<bool> UpdateComiteeReclamationAsync(int id, ComiteeReclamation comiteeReclamation)
        {
            var existingComiteeReclamation = await _comiteeReclamationRepository.GetByIdAsync(id);
            if (existingComiteeReclamation == null)
            {
                return false;
            }

            existingComiteeReclamation.ReclamationID = comiteeReclamation.ReclamationID;
            existingComiteeReclamation.ConcernedID = comiteeReclamation.ConcernedID;
            existingComiteeReclamation.CreationDate = comiteeReclamation.CreationDate;

            await _comiteeReclamationRepository.UpdateAsync(existingComiteeReclamation);
            return true;
        }

        public async Task DeleteComiteeReclamationAsync(int id)
        {
            var comiteeReclamation = await _comiteeReclamationRepository.GetByIdAsync(id);
            if (comiteeReclamation != null)
            {
                await _comiteeReclamationRepository.DeleteAsync(comiteeReclamation);
            }
        }

        public async Task<IEnumerable<User>> GetConcernedUsersAsync(int roleId)
        {
            return await _context.Users
                                 .Where(u => u.IdRole == roleId)
                                 .ToListAsync();
        }
        public async Task<IEnumerable<User>> GetConcernedUserAsync(int id)
        {
            return await _context.ComiteeReclamation
                .Where(cr => cr.ReclamationID == id)
                .Select(cr => cr.ConcernedUser) // Use navigation property to select User
                .ToListAsync();
        }
        public async Task RemoveUserFromComiteeAsync(int reclamationId, int userId)
        {
            var comiteeReclamation = await _context.ComiteeReclamation
                .FirstOrDefaultAsync(c => c.ReclamationID == reclamationId && c.ConcernedID == userId);

            if (comiteeReclamation != null)
            {
                _context.ComiteeReclamation.Remove(comiteeReclamation);
                await _context.SaveChangesAsync();
            }
        }


    }
}
