using ITKANSys_api.Models;
using ITKANSys_api.Models.Dtos;
using ITKANSys_api.Utility.ApiResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITKANSys_api.Interfaces
{
    public interface IAuditService
    {
        Task<List<Audit>?> GetAllAudit();
        Task<Audit?> GetAudit(int id);
        Task<Audit> AddAudit(Audit audit);
        Task<List<Audit>?> UpdateAudit(int auditId, Audit request);
        Task<List<Audit>?> DeleteAudit(int auditId);
        Task<List<Audit>?> SearchAuditByType(int typeAuditId); // Method to search audits by type
    }
}
