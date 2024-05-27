using ITKANSys_api.Models.Dtos;
using ITKANSys_api.Utility.ApiResponse;
using Microsoft.AspNetCore.Mvc;

namespace ITKANSys_api.Interfaces
{
    public interface IAuditService
    {
        Task<List<Audit>> GetAllAudit();
        
        Task<Audit?> GetAudit(int id);
        Task<DataSourceResult> AddAudit(object audit);
        Task<DataSourceResult> UpdateAudit(int id, object audit);
        //Task<List<Audit>?> UpdateAudit(int id, Audit request);

        //Task<DataSourceResult> DeleteAudit(object audit);
        //Task<DataSourceResult> DeleteAudit(int id);
        //Task<DataSourceResult> DeleteAudit(object audit);
        Task<DataSourceResult> DeleteAudit(string jsonString);
    }
}
