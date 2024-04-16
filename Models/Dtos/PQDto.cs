using ITKANSys_api.Models.Entities.Param;

namespace ITKANSys_api.Models.Dtos
{
    public class PQDto
    {
        public int ID { get; set; }
        public int SMQID { get; set; }
        public string Libelle { get; set; }
        public string Version { get; set; }
        public DateTime DateApplication { get; set; }
        public int Perime { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime CreationDate { get; set; }
        public string Description { get; set; }
        public string ExtensionFile { get; set; }
        public string SQMLibelle { get; set; }
        public SMQ SMQ { get; set; }
        public string TypeDocument { get; set; }
    }
}
