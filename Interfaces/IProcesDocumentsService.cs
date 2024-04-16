using ITKANSys_api.Data.OtherObjects;
using ITKANSys_api.Models;
using ITKANSys_api.Utility.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace ITKANSys_api.Interfaces
{
    public interface IProcesDocumentsService
    {
        Task<DataSourceResult> UpdateProcesDocuments(string Libelle, int ID);
        Task<GetDataResult> DetailProcesDocuments(int processusID);
        Task<DataSourceResult> SupprimerProcesDocuments(object processus);
        Task<DataSourceResult> UpdatePerimeState(object processusDocuments);
        Task<DataSourceResult> InsertProcesDocuments(IFormFile file, string Libelle, int ProcessusID, CancellationToken cancellationtoken);
        Task<DataSourceResult> RechercheDocumentsPerime(int procedureID, int processusID, bool MQ, bool PQ, string? code, string? version, string? libelle, DateTime? dateDebut, DateTime? dateFin, string field, string order, int take, int skip);
        Task<bool> DocumentContainsValidDocument(int documentId, int id, string documentType);
    }
}
