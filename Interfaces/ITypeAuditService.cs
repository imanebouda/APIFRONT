using ITKANSys_api.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITKANSys_api.Interfaces
{
    public interface ITypeAuditService
    {
        Task<List<TypeAudit>> GetAllTypeAudits();
        Task<TypeAudit> GetTypeAudit(int id);
        Task<TypeAudit> AddTypeAudit(TypeAudit typeAudit);
        Task<TypeAudit> UpdateTypeAudit(int id, TypeAudit typeAudit);
        Task<bool> DeleteTypeAudit(int id);
    }
}
