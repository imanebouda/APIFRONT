namespace ITKANSys_api.Interfaces
{
    public interface IConstatService
    {
        Task<List<Constat>> GetAllConstat();
        Task<Constat?> GetConstat(int id);
        Task<List<Constat>> AddConstat(Constat constat);
        Task<List<Constat>?> UpdateConstat(int id, Constat request);
        Task<List<Constat>?> DeleteConstat(int id);
    }
}
