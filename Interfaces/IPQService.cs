using ITKANSys_api.Utility.ApiResponse;

namespace ITKANSys_api.Interfaces
{
    public interface IPQService
    {
        Task<DataSourceResult> InsertPQ(IFormFile file, string Libelle, int SmqID, string version, string Description, DateTime dateApplication, CancellationToken cancellationtoken);
        Task<DataSourceResult> UpdatePQ(string Libelle, int ID, int SmqID, string version, string Description, DateTime dateApplication);
        Task<GetDataResult> DetailPQ();
        Task<DataSourceResult> SupprimerPQ(object pq);
        Task<DataSourceResult> UpdatePerimeState(object PQdocument);
        Task<bool> DocumentContainsValidDocument(int documentId, string documentType);
    }
}
