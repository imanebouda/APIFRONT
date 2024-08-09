using ITKANSys_api.Interfaces;
using ITKANSys_api.Models.Dtos;
using ITKANSys_api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ITKANSys_api.Services
{
    public interface IReclamationService
    {
        Task<IEnumerable<Reclamation>> GetAllReclamationsAsync();
        Task<Reclamation> GetReclamationByIdAsync(int id);
        Task<Reclamation> CreateReclamationAsync(Reclamation reclamation);
        Task UpdateReclamationAsync(Reclamation reclamation);
        Task DeleteReclamationAsync(int id);
        Task<List<Reclamation>> SearchReclamationsAsync(string status, DateTime? creationdate);
        Task AddComiteeReclamationAsync(ComiteeReclamation comiteeReclamation);
        Task<List<Reclamation>> GetReclamationsWithReclamant();
        Task<Reclamant> CreateReclamantAsync(Reclamant reclamant);
        Task<Reclamation> AddReclamationAsync(ReclamationDto reclamationDto);

    }

    public class ReclamationService : IReclamationService
    {
        private readonly IReclamationRepository _reclamationRepository;
        private readonly ApplicationDbContext _context;

        public ReclamationService(IReclamationRepository reclamationRepository, ApplicationDbContext context)
        {
            _reclamationRepository = reclamationRepository;
            _context = context;
        }
        public async Task<Reclamation> AddReclamationAsync(ReclamationDto reclamationDto)
        {
            var reclamation = new Reclamation
            {
                Objet = reclamationDto.Objet,
                Détail = reclamationDto.Détail,
                Analyse = reclamationDto.Analyse,
                Status = reclamationDto.Status,
                ResponsableID = reclamationDto.ResponsableID,
                CreationDate = reclamationDto.CreationDate
            };

            return await _reclamationRepository.AddAsync(reclamation);
        }

        public async Task<Reclamant> CreateReclamantAsync(Reclamant reclamant)
        {
            _context.Reclamants.Add(reclamant);
            await _context.SaveChangesAsync();
            return reclamant;
        }

        public async Task<IEnumerable<Reclamation>> GetAllReclamationsAsync()
        {
            return await _reclamationRepository.GetAllAsync();
        }

        public async Task<Reclamation> GetReclamationByIdAsync(int id)
        {
            return await _reclamationRepository.GetByIdAsync(id);
        }

        public async Task<Reclamation> CreateReclamationAsync(Reclamation reclamation)
        {
            return await _reclamationRepository.AddAsync(reclamation);
        }

        public async Task UpdateReclamationAsync(Reclamation reclamation)
        {
            await _reclamationRepository.UpdateAsync(reclamation);
        }

        public async Task DeleteReclamationAsync(int id)
        {
            var reclamation = await _reclamationRepository.GetByIdAsync(id);
            if (reclamation == null)
            {
                throw new Exception($"Réclamation avec ID {id} non trouvée.");
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Delete related ComiteeReclamation entries
                    var comiteeEntries = await _context.ComiteeReclamation
                        .Where(cr => cr.ReclamationID == id)
                        .ToListAsync();

                    if (comiteeEntries != null && comiteeEntries.Any())
                    {
                        _context.ComiteeReclamation.RemoveRange(comiteeEntries);
                        await _context.SaveChangesAsync();
                    }

                    // Delete related HistoryReclamation entries if necessary
                    var historyEntries = await _context.HistoryReclamation
                        .Where(hr => hr.ReclamationID == id)
                        .ToListAsync();

                    if (historyEntries != null && historyEntries.Any())
                    {
                        _context.HistoryReclamation.RemoveRange(historyEntries);
                        await _context.SaveChangesAsync();
                    }

                    // Finally, delete the reclamation itself
                    await _reclamationRepository.DeleteAsync(reclamation);
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    // Handle errors here
                    throw new Exception($"Erreur lors de la suppression de la réclamation avec ID {id}: {ex.Message}");
                }
            }
        }



        public async Task AddComiteeReclamationAsync(ComiteeReclamation comiteeReclamation)
        {
            _context.ComiteeReclamation.Add(comiteeReclamation);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Reclamation>> GetReclamationsWithReclamant()
        {
            return await _context.Reclamations
                .Include(r => r.Reclamant)
                .ToListAsync();
        }

        async Task<List<Reclamation>> IReclamationService.SearchReclamationsAsync(string status, DateTime? creationdate)
        {
            var query = _context.Reclamations.Include(r => r.Reclamant).AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(r => r.Status == status);
            }

            if (creationdate.HasValue)
            {
                query = query.Where(r => r.CreationDate.Date == creationdate.Value.Date);
            }

            return await query.ToListAsync();
        }
    }
}

