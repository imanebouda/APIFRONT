namespace ITKANSys_api.Interfaces
{
    public class FileInfo
    {
        public int ProcessusID { get; set; } // Identifiant unique
        public string Libelle { get; set; } // Nom du fichier
        public IFormFile file { get; set; } // Chemin du fichier
    }
}
