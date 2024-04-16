using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ITKANSys_api.Models.Dtos
{
    public class ProcesDocumentsDto
    {
        public int ID { get; set; }
        public string Libelle { get; set; }
        public int Perime { get; set; }
        public DateTime CreationDate { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int ProcessusID { get; set; }
        public string ExtensionFile { get; set; }
        public int Pilote { get; set; }
        public int CoPilote { get; set; }

        [Required]
        public DateTime created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }

        [ForeignKey("ProcessusID")] // Renommez la clé étrangère pour correspondre à la clé primaire de User
        public Processus Processus { get; set; }
    }
}
