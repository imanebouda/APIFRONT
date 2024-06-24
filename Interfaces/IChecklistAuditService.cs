using ITKANSys_api.Models.Entities;

namespace ITKANSys_api.Interfaces
{
    public interface IChecklistAuditService
    {

        // List<CheckListAudit> getAllCheckListAudit();
        Task<List<CheckListAudit>> GetAllCheckListAudit();
        Task<CheckListAudit?> GetCheckListAudit(int checkListId);
        Task<CheckListAudit> AddCheckListAudit(CheckListAudit checkListAudit);
        Task<List<CheckListAudit>?> UpdateCheckListAudit(int checkListId, CheckListAudit request);
        Task<List<CheckListAudit>?> DeleteCheckListAudit(int checkListId);
        Task<List<CheckListAudit>> SearchChecklistByType(int typeChecklistId);

        // Ajout de la méthode de recherche par type


        Task<List<CheckListAudit>> GetQuestionsForCheckListAudit(int checkListAuditId);
    }
}
