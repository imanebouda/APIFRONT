using ITKANSys_api.Models;
using ITKANSys_api.Models.Dtos;
using ITKANSys_api.Utility.ApiResponse;
using System.Collections.Generic;
using System.Threading.Tasks;



namespace ITKANSys_api.Interfaces
{
    public interface IActionService
    {

        Task<List<Models.Entities.Action>> GetAllActions();
        Task<Models.Entities.Action?> GetAction(int actionId);
        Task<Models.Entities.Action> AddAction(Models.Entities.Action action);
        Task<Models.Entities.Action?> UpdateAction(int actionId, Models.Entities.Action request);
        Task<List<Models.Entities.Action>?> DeleteAction(int actionId);
    }
}
