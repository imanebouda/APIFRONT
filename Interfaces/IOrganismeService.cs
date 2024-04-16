using ITKANSys_api.Utility.ApiResponse;

namespace ITKANSys_api.Interfaces
{
    public interface IOrganismeService
    {
        Task<DataSourceResult> GetAll();
        Task<DataSourceResult> Search(Object record);
        Task<DataSourceResult> Load(Object record);
        Task<DataSourceResult> CurrentOrganisme();
        Task<DataSourceResult> Save(Object record);
        Task<DataSourceResult> Delete(Object record);
    }
}
