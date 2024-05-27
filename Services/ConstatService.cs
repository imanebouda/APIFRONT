using ITKANSys_api.Interfaces;
using ITKANSys_api.Models.Entities;
using Microsoft.EntityFrameworkCore;

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
        private readonly ApplicationDbContext _context;

        public ConstatService(ApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<List<Constat>> AddConstat(Constat constat)
        {
            try
            {
                _context.Constat.Add(constat);
                await _context.SaveChangesAsync();

                return constats;
            }
            catch (Exception ex)
            {
                // Gérer l'exception ici
                // Vous pouvez enregistrer les détails de l'exception dans les journaux ou effectuer d'autres actions nécessaires.
                // Ne pas oublier de logger l'exception si nécessaire.
                Console.WriteLine($"Une exception s'est produite : {ex.Message}");
                throw; // Remonte l'exception pour la gestion centralisée des erreurs, si elle est configurée.
            }
            
        }

        public async Task<List<Constat>?> DeleteConstat(int id)
        {
            try
            {
                var constat = await _context.Constat.FindAsync(id);
                if (constat is null)
                    return null;


                _context.Constat.Remove(constat);
                await _context.SaveChangesAsync();
                return constats;
            }
            catch (Exception ex)
            {
                // Gérer l'exception ici
                // Vous pouvez enregistrer les détails de l'exception dans les journaux ou effectuer d'autres actions nécessaires.
                // Ne pas oublier de logger l'exception si nécessaire.
                Console.WriteLine($"Une exception s'est produite : {ex.Message}");
                throw; // Remonte l'exception pour la gestion centralisée des erreurs, si elle est configurée.
            }

        }

        public  async Task<List<Constat>> GetAllConstat()
        {

            try
            {
                var constats = await _context.Constat.ToListAsync();
                return constats;
            }
            catch (Exception ex)
            {
                // Gérer l'exception ici
                // Vous pouvez enregistrer les détails de l'exception dans les journaux ou effectuer d'autres actions nécessaires.
                // Ne pas oublier de logger l'exception si nécessaire.
                Console.WriteLine($"Une exception s'est produite : {ex.Message}");
                throw; // Remonte l'exception pour la gestion centralisée des erreurs, si elle est configurée.
            }
          
        }

        public async Task<Constat?> GetConstat(int id)
        {
            try
            {
                var constat = await _context.Constat.FindAsync(id);
                if (constat is null)
                    return null;

                return constat;
            }
            catch (Exception ex)
            {
                // Gérer l'exception ici
                // Vous pouvez enregistrer les détails de l'exception dans les journaux ou effectuer d'autres actions nécessaires.
                // Ne pas oublier de logger l'exception si nécessaire.
                Console.WriteLine($"Une exception s'est produite : {ex.Message}");
                throw; // Remonte l'exception pour la gestion centralisée des erreurs, si elle est configurée.
            }
           
        }

        public async Task<List<Constat>?> UpdateConstat(int id, Constat request)
        {

            try
            {
                var constat = await _context.Constat.FindAsync(id);
                if (constat is null)
                    return null;
                constat.EcartTitle = request.EcartTitle;
                constat.EcartType = request.EcartType;

                await _context.SaveChangesAsync();


                return constats;

            }
            catch (Exception ex)
            {
                // Gérer l'exception ici
                // Vous pouvez enregistrer les détails de l'exception dans les journaux ou effectuer d'autres actions nécessaires.
                // Ne pas oublier de logger l'exception si nécessaire.
                Console.WriteLine($"Une exception s'est produite : {ex.Message}");
                throw; // Remonte l'exception pour la gestion centralisée des erreurs, si elle est configurée.
            }
           
        }
    }
}
