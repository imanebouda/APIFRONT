using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITKANSys_api.Models.Entities
{
    public class TypeAudit
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string type { get; set; }

        // Propriété de navigation
        // Décommentez cette section si vous avez une relation One-to-Many avec CheckListAudit
        // public ICollection<CheckListAudit> CheckListAudits { get; set; }
    }
}
