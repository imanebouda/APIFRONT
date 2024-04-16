using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITKANSys_api.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        public int? CreatedBy { get; set; } // Renommez la clé étrangère en CreatedBy

        [Required]
        public DateTime Created_at { get; set; }


        public DateTime? Updated_at { get; set; }

        public DateTime? Deleted_at { get; set; }

        [ForeignKey("CreatedBy")] // Renommez la clé étrangère pour correspondre à la clé primaire de User
        public User Users { get; set; }

        

    }
}
