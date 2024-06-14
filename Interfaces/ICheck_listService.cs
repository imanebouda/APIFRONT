


namespace ITKANSys_api.Interfaces
{
    public interface ICheck_listService
    {


        // List<CheckListAudit> getAllCheckListAudit();
        Task<List<Check_list>> GetAllCheckListAudit();
        Task<Check_list?> GetCheckListAudit(int checkListId);
        Task<Check_list> AddCheckListAudit(Check_list checkListAudit);
        Task<List<Check_list>?> UpdateCheckListAudit(int checkListId, Check_list request);
        Task<List<Check_list>?> DeleteCheckListAudit(int checkListId);

    }
}
