namespace ITKANSys_api.Interfaces
{
    public interface IPreuve
    {


        Task<List<Models.Entities.Preuve>> GetAllPreuves();
        Task<Models.Entities.Preuve?> GetPreuve(int preuveId);
        Task<Models.Entities.Preuve> AddPreuve(Models.Entities.Preuve preuve);
        Task<Models.Entities.Preuve?> UpdatePreuve(int preuveId, Models.Entities.Preuve request);
        Task<List<Models.Entities.Preuve>?> DeletePreuve(int preuveId);
    }
}
