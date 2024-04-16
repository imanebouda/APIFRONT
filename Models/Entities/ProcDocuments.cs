using ITKANSys_api.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITKANSys_api.Models
{
    public class ProcDocuments
    {
        public int ID { get; set; }
        public string Libelle { get; set; }
        public int Perimé { get; set; }
        public DateTime CreationDate { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Type { get; set; }

        [Required]
        public DateTime created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }

        public int ProcID { get; set; }
 

        [ForeignKey("ProcID")] // Renommez la clé étrangère pour correspondre à la clé primaire de User
        [Column("ProcID")]
        public Procedures Procedure { get; set; }


    }
}
