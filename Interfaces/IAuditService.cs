namespace ITKANSys_api.Interfaces
{
    public interface IAuditService
    {
         List<Audit> GetAllAudit();
        Audit? GetAudit(int id);
        List<Audit> AddAudit(Audit audit);
        List<Audit>? UpdateAudit(int id, Audit request);
        List<Audit>? DeleteAudit(int id);
    }
}
