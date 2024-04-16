using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITKANSys_api.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public string Controller { get; set; }
        public string Methode { get; set; }
        public int? CreatedById { get; set; }
        [Required]// Renommez la clé étrangère en CreatedById
        public DateTime created_at { get; set; }
        public DateTime? deleted_at { get; set; }
        public DateTime? updated_at { get; set; }

        [ForeignKey("CreatedById")] // Renommez la clé étrangère
        public User CreatedByUser { get; set; }
    }
}
