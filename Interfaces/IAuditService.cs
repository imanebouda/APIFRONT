using ITKANSys_api.Models.Dtos;
using ITKANSys_api.Utility.ApiResponse;
using Microsoft.AspNetCore.Mvc;

namespace ITKANSys_api.Interfaces
{
    public interface IAuditService
    {
        Task<List<Audit>> GetAllAudit();

        Task<Audit?> GetAudit(int id);
        Task<Audit> AddAudit(Audit audit);
        Task<List<Audit>?> UpdateAudit(int auditId, Audit request);
        //Task<List<Audit>?> UpdateAudit(int id, Audit request);

        //Task<DataSourceResult> DeleteAudit(object audit);
        //Task<DataSourceResult> DeleteAudit(int id);
        //Task<DataSourceResult> DeleteAudit(object audit);
        Task<List<Audit>?> DeleteAudit(int auditId);
        /* Task<List<CheckListAudit>?> UpdateCheckListAudit(int checkListId, CheckListAudit request);
        Task<List<CheckListAudit>?> DeleteCheckListAudit(int checkListId);*/
        Task<List<Audit>> searchAuditByType(int typeAuditId); // Ajout de la méthode de recherche par type
    }
}
