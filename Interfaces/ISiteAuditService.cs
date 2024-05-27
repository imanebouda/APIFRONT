using ITKANSys_api.Models;
using ITKANSys_api.Models.Entities;
using ITKANSys_api.Utility.ApiResponse;

namespace ITKANSys_api.Interfaces
{
    public interface ISiteAuditService
    {
        Task<List<SiteAudit>> getAllSiteAudits();
        Task<SiteAudit?> getSiteAudit(int siteId);
        Task<List<SiteAudit>?> addSiteAudit(SiteAudit nSiteAudit); // Modification ici
        Task<List<SiteAudit>?> updateSiteAudit(int siteId, SiteAudit request);
        Task<List<SiteAudit>?> deleteSiteAudit(int siteId);
    }
}
