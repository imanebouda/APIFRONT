using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITKANSys_api.Models
{
    public class ProcesDocuments
    {
        public int ID { get; set; }
        public string Libelle { get; set; }
        public int Perime { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime CreationDate { get; set; }
        public int ProcessusID { get; set; }

        [Required]
        public DateTime created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }

        [ForeignKey("ProcessusID")] // Renommez la clé étrangère pour correspondre à la clé primaire de User
        public Processus Processus { get; set; } 
    }
}
