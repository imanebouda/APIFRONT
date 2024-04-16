using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITKANSys_api.Models.Entities
{
    public class ProcObjectifs
    {
        public int ID { get; set; }
        public int ProcID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [Required]// Renommez la clé étrangère en CreatedById
        public DateTime created_at { get; set; }
        public DateTime? deleted_at { get; set; }
        public DateTime? updated_at { get; set; }

        [ForeignKey("ProcID")] // Renommez la clé étrangère
        [Column("ProcID")]
        public Procedures Procedures { get; set; }
    }
}
