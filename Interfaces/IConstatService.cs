namespace ITKANSys_api.Interfaces
{
    public interface IConstatService
    {
        List<Constat> GetAllConstat();
        Constat? GetConstat(int id);
        List<Constat> AddConstat(Constat constat);
        List<Constat>? UpdateConstat(int id, Constat request);
        List<Constat>? DeleteConstat(int id);
    }
}
