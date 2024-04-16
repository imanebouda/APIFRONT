
using ITKANSys_api.Data.Dtos;
using ITKANSys_api.Models;
using ITKANSys_api.Utility.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using static ITKANSys_api.Data.Services.ProcessusService;

namespace ITKANSys_api.Interfaces
{
    public interface IProcessusServices
    {
        Task<DataSourceResult> GetAll();
        Task<DataSourceResult> GetProcessusData();
        Task<DataSourceResult> GetProcessusByPiloteOrCoPilote(int pilote, int coPilote);
        Task<DataSourceResult> InsertProcessus(object insertProcessus);
        Task<DataSourceResult> RechercheProcessus(string code, string libelle, DateTime? dateDebut, DateTime? dateFin, int categorie, string field, string order,int take,int skip);
        Task<DataSourceResult> SupprimerProcessus(object processus);
        Task<GetDataResult> DetailProcessus(int ID);
        Task<DataSourceResult> UpdateProcessus(object processus);
        Task<DataSourceResult> GetAllSMQ();
        Task<DataSourceResult> GetAllCategories();
    }
}
