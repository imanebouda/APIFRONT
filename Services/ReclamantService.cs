using ITKANSys_api.Interfaces;

namespace ITKANSys_api.Services
{
    public class ReclamantService
    {

        private readonly ReclamantRepository _repository;

        public ReclamantService(ReclamantRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Reclamant>> GetAllReclamantsAsync()
        {
            return await _repository.GetAllAsync();

        }
       
        public async Task<Reclamant> GetReclamantByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task CreateReclamantAsync(Reclamant reclamant)
        {
            reclamant.CreationDate = DateTime.Now;
            await _repository.CreateAsync(reclamant);
        }

        public async Task UpdateReclamantAsync(Reclamant reclamant)
        {
            await _repository.UpdateAsync(reclamant);
        }

        public async Task DeleteReclamantAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
