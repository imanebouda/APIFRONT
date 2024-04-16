using ITKANSys_api.Utility.ApiResponse;

namespace ITKANSys_api.Interfaces
{
    public interface IPermissionsService
    {
        Task<DataSourceResult> GetAll();
        Task<DataSourceResult> GetAllByRole(Object record);
        Task<DataSourceResult> Save(Object record);
        Task<DataSourceResult> Delete(Object record);
    }
}
