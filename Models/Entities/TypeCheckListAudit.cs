using ITKANSys_api.Models.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITKANSys_api.Models.Entities
{
    public class TypeCheckListAudit
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string type { get; set; }
        // Propriété de navigation
        //public ICollection<CheckListAudit> CheckListAudits { get; set; }
    }
}
