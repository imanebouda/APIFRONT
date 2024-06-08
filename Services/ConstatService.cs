using ITKANSys_api.Interfaces;
using ITKANSys_api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITKANSys_api.Services
{
    public class ConstatService : IConstatService
    {
        Task<List<Constat>> IConstatService.AddConstat(Constat constat)
        {
            throw new NotImplementedException();
        }

        Task<List<Constat>?> IConstatService.DeleteConstat(int id)
        {
            throw new NotImplementedException();
        }

        Task<List<Constat>> IConstatService.GetAllConstat()
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
