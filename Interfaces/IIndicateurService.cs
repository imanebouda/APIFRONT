using ITKANSys_api.Utility.ApiResponse;

namespace ITKANSys_api.Interfaces
{
    public interface  IIndicateurService
    {
        Task<DataSourceResult> GetAllIndicateurs(int ID);
        Task<DataSourceResult> InsertIndicateurs(object insertIndicateurs);
        Task<DataSourceResult> RechercheIndicateurs(int id, string titre, string? frequence, DateTime? dateDebut, DateTime? dateFin, string field, string order, int take, int skip);
        Task<DataSourceResult> UpdateIndicateurs(object Indicateurs);
        Task<DataSourceResult> SupprimerIndicateurs(object Indicateurs);
        Task<GetDataResult> DetailIndicateur(int ID);
    }
}
