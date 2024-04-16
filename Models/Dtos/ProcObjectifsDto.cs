using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ITKANSys_api.Models.Dtos
{
    public class ProcObjectifsDto
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
        public Procedures Procedures { get; set; }
    }
}
