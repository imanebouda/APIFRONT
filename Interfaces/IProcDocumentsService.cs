using ITKANSys_api.Utility.ApiResponse;

namespace ITKANSys_api.Interfaces
{
    public interface IProcDocumentsService
    {
        Task<DataSourceResult> GetAll(int procID);
        Task<DataSourceResult> InsertProcDocuments(IFormFile file, string Libelle,string  type, int ProcID, CancellationToken cancellationtoken);
        Task<DataSourceResult> UpdateProcDocuments(string Libelle, int ID);
        Task<GetDataResult> DetailProcDocuments(int procID);
        Task<DataSourceResult> SupprimerProcDocuments(object procedure);
        Task<DataSourceResult> UpdatePeriméState(object procedureDocuments);
        Task<DataSourceResult> RechercheProcDocumentsPerime(string libelle, DateTime? dateDebut, DateTime? dateFin, string? field, string? order, int take, int skip);
    }
}
