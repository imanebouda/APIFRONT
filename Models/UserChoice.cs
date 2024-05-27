using ITKANSys_api.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITKANSys_api.Models
{
    public class UserChoice
    {
        [Key]
        public int Id { get; set; }

        public string Choice { get; set; }
        [ForeignKey("CheckListAudit")]
        public int CheckListAuditId { get; set; }

        public CheckListAudit CheckListAudit { get; set; }
    }
}