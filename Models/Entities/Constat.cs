using System.ComponentModel.DataAnnotations.Schema;

namespace ITKANSys_api.Models.Entities
{
    public class Constat
    {
        public int ID { get; set; }
        public string EcartTitle { get; set; }
        public string EcartType { get; set; }

        [ForeignKey("Audit")] // Spécifie la clé étrangère
        public int AuditID { get; set; }

        public Audit Audit { get; set; } // Association Many-to-One avec la classe Audit
    }
}
