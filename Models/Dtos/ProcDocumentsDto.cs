using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ITKANSys_api.Models.Dtos
{
    public class ProcDocumentsDto
    {
        public int ID { get; set; }
        public string Libelle { get; set; }
        public int Perimé { get; set; }
        public DateTime CreationDate { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ExtensionFile { get; set; }

        [Required]
        public DateTime created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }

        public int ProcID { get; set; }


        [ForeignKey("ProcID")] // Renommez la clé étrangère pour correspondre à la clé primaire de User
        public Procedures Procedure { get; set; }
    }
}
