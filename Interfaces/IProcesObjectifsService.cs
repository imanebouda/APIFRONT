using ITKANSys_api.Utility.ApiResponse;
namespace ITKANSys_api.Interfaces
{
   public interface IProcesObjectifsService
   {
       Task<DataSourceResult> GetAllProcesObjectifs(int ID);
       Task<DataSourceResult> RechercheProcesObjectifs(int id,string titre, DateTime? dateDebut, DateTime? dateFin, string field, string order, int take, int skip);
       Task<DataSourceResult> InsertProcesObjectifs(object insertProcessus);
       Task<DataSourceResult> UpdateProcesObjectifs(object processus);
        Task<DataSourceResult> SupprimerProcesObjectifs(object processus);
    }
}
