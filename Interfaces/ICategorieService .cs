using ITKANSys_api.Utility.ApiResponse;

namespace ITKANSys_api.Interfaces
{
    public interface ICategorieService
    {
        Task<DataSourceResult> Search(Object record);
        Task<DataSourceResult> Insert(Object record);
        Task<DataSourceResult> Update(Object record);
        Task<DataSourceResult> Delete(Object record);
    }
}
