using ITKANSys_api.Interfaces;
using ITKANSys_api.Models.Entities;

namespace ITKANSys_api.Services
{
    public class ConstatService : IConstatService
    {
        private static List<Constat> constats = new List<Constat>
    { new Constat
        {
            ID = 1,
            EcartTitle  = "constat d audit informatique",
            EcartType = "CC",
            
        },
        new Constat{
            ID = 2,
            EcartTitle  = "constat d audit informatique",
            EcartType = "REMAMRQUE",
            }

    };
        public List<Constat> AddConstat(Constat constat)
        {
            constats.Add(constat);

            return constats;
        }

        public List<Constat>? DeleteConstat(int id)
        {
            var constat = constats.Find(x => x.ID == id);
            if (constat is null)
                return null;


            constats.Remove(constat);
            return constats;
        }

        public List<Constat> GetAllConstat()
        {
            return constats;
        }

        public Constat? GetConstat(int id)
        {
            var constat = constats.Find(x => x.ID == id);
            if (constat is null)
                return null;

            return constat;
        }

        public List<Constat>? UpdateConstat(int id, Constat request)
        {
            var constat = constats.Find(x => x.ID == id);
            if (constat is null)
                return null;
            constat.EcartTitle = request.EcartTitle;
            constat.EcartType= request.EcartType;
            


            return constats;
        }
    }
}
