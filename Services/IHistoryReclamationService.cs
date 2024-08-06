using ITKANSys_api.Interfaces;

namespace ITKANSys_api.Services
{
    public interface IHistoryReclamationService
    {

        Task<IEnumerable<HistoryReclamation>> GetAllHistoryReclamationsAsync();
        Task<HistoryReclamation> GetHistoryReclamationByIdAsync(int id);
        Task<HistoryReclamation> CreateHistoryReclamationAsync(HistoryReclamation historyReclamation);
        Task<bool> UpdateHistoryReclamationAsync(int id, HistoryReclamation historyReclamation);
        Task DeleteHistoryReclamationAsync(int id);
    }


    public class HistoryReclamationService : IHistoryReclamationService
    {
        private readonly IHistoryReclamationRepository _historyReclamationRepository;

        public HistoryReclamationService(IHistoryReclamationRepository historyReclamationRepository)
        {
            _historyReclamationRepository = historyReclamationRepository;
        }

        public async Task<IEnumerable<HistoryReclamation>> GetAllHistoryReclamationsAsync()
        {
            return await _historyReclamationRepository.GetAllAsync();
        }

        public async Task<HistoryReclamation> GetHistoryReclamationByIdAsync(int id)
        {
            return await _historyReclamationRepository.GetByIdAsync(id);
        }

        public async Task<HistoryReclamation> CreateHistoryReclamationAsync(HistoryReclamation historyReclamation)
        {
            return await _historyReclamationRepository.AddAsync(historyReclamation);
        }

        public async Task<bool> UpdateHistoryReclamationAsync(int id, HistoryReclamation historyReclamation)
        {
            var existingHistoryReclamation = await _historyReclamationRepository.GetByIdAsync(id);
            if (existingHistoryReclamation == null)
            {
                return false;
            }

            existingHistoryReclamation.ReclamationID = historyReclamation.ReclamationID;
            existingHistoryReclamation.Commentaire = historyReclamation.Commentaire;
            existingHistoryReclamation.CreationDate = historyReclamation.CreationDate;

            await _historyReclamationRepository.UpdateAsync(existingHistoryReclamation);
            return true;
        }

        public async Task DeleteHistoryReclamationAsync(int id)
        {
            var historyReclamation = await _historyReclamationRepository.GetByIdAsync(id);
            if (historyReclamation != null)
            {
                await _historyReclamationRepository.DeleteAsync(historyReclamation);
            }
        }
    }
}
