namespace ITKANSys_api.Interfaces
{
    public interface ITypeCheckListAuditService
    {
        Task<List<TypeCheckListAudit>> getAllTypeCheckListAudit();
        Task<TypeCheckListAudit?> getTypeCheckListAudit(int typeCheckListId);
        List<TypeCheckListAudit> addTypeCheckListAudit(TypeCheckListAudit nTypeCheckListAudit);
        Task<List<TypeCheckListAudit>?> updateTypeCheckListAudit(int checkListId, TypeCheckListAudit request);
        Task<List<TypeCheckListAudit>?> deleteTypeCheckListAudit(int checkListId);
    }
}
