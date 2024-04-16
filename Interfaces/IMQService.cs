using ITKANSys_api.Utility.ApiResponse;

namespace ITKANSys_api.Interfaces
{
    public interface IMQService
    {
        Task<DataSourceResult> InsertMQ(IFormFile file, string Libelle, int SmqID, string version, string Description, DateTime dateApplication, CancellationToken cancellationtoken);
        Task<DataSourceResult> UpdateMQ(string Libelle, int ID, int SmqID, string version, string Description, DateTime dateApplication);
        Task<GetDataResult> DetailMQ();
        Task<DataSourceResult> SupprimerMQ(object pq);
        Task<DataSourceResult> UpdatePerimeState(object PQdocument);
        Task<bool> DocumentContainsValidDocument(int documentId, string documentType);
    }
}
