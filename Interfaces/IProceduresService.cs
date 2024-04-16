using ITKANSys_api.Models.Dtos;
using ITKANSys_api.Utility.ApiResponse;

namespace ITKANSys_api.Interfaces
{
    public interface IProceduresService
    {
        Task<DataSourceResult> GetAllByIdProcessus(int ProcessusID);
        Task<DataSourceResult> RechercheProcedures(string code, string libelle, DateTime? dateDebut, DateTime? dateFin, int id, string field, string order, int take, int skip);
        Task<DataSourceResult> InsertProcedure(object insertProcedure);
        Task<DataSourceResult> UpdateProcedure(object updateProcedure);
        Task<DataSourceResult> SupprimerProcedure(object procedure);
        Task<GetDataResult> DetailProcedure(int ID);

    }
}
