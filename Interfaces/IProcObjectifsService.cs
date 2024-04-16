using ITKANSys_api.Utility.ApiResponse;

namespace ITKANSys_api.Interfaces
{
    public interface IProcObjectifsService
    {
        Task<DataSourceResult> GetAllProcObjectifs(int ID);
        Task<DataSourceResult> InsertProcObjectifs(object insertProcessus);
        Task<DataSourceResult> RechercheProcObjectifs(int id, string titre, DateTime? dateDebut, DateTime? dateFin, string field, string order, int take, int skip);
        Task<DataSourceResult> UpdateProcObjectifs(object processus);
        Task<DataSourceResult> SupprimerProcObjectifs(object processus);
    }
}
