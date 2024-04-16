using ITKANSys_api.Utility.ApiResponse;

namespace ITKANSys_api.Interfaces
{
    public interface IResultatsIndicateurService
    {
        Task<DataSourceResult> GetAllResultByIdIndicateur(int Annee);
        Task<DataSourceResult> InsertResultIndicateur(object insertResultIndicateur);
        Task<DataSourceResult> UpdateResultIndicateur(object updateResultIndicateur);
        Task<DataSourceResult> SupprimerResultIndicateur(object ResultIndicateur);
        Task<DataSourceResult> RechercheResultatIndicateurs(string? periode, int annee, DateTime? dateDebut, DateTime? dateFin, int id, string? field, string? order, int take, int skip);
    }
}
