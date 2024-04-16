using ITKANSys_api.Utility.ApiResponse;

namespace ITKANSys_api.Interfaces
{
    public interface ITypeOrganismeService
    {
        Task<DataSourceResult> GetAll();
        Task<DataSourceResult> Search(Object record);
        Task<DataSourceResult> Insert(Object record);
    }
}
