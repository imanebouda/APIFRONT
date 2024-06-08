using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ITKANSys_api.Models.Entities
{
    public class ProgrammeAudit
    {
        public int ID { get; set; }
        public int AuditID { get; set; }
        public string Title { get; set; }
        public DateTime DateAudit { get; set; }
        public string Description { get; set; }

       


        [ForeignKey("AuditID")] // Renommez la clé étrangère
        [Column("AuditID")]
        public  Audit Audit { get; set; }
    }
}
