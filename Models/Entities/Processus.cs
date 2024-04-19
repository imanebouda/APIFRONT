using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ITKANSys_api.Models.Entities;
using ITKANSys_api.Models.Entities.Param;

namespace ITKANSys_api.Models
{
    public class Processus
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Version { get; set; }
        public string Libelle { get; set; }
        public string Description { get; set; }
        public int Pilote { get; set; }
        public int CoPilote { get; set; }
        public DateTime CreationDate { get; set; }

        [Required]
        public DateTime created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }

        public int SMQ_ID { get; set; }
        public int Categorie_ID { get; set; }
        public int USER_ID { get; set; }

        [ForeignKey("SMQ_ID")]
        public SMQ SMQ { get; set; }

        [ForeignKey("Categorie_ID")]
        public Categories Categories { get; set; }

        [ForeignKey("USER_ID")]
        public User Users { get; set; }

        [ForeignKey("Pilote")]
        public User PiloteUser { get; set; }

        [ForeignKey("CoPilote")]
        public User CoPiloteUser { get; set; }

        [ForeignKey("Audit")] // Spécifie la clé étrangère
        public int AuditID { get; set; }

        public Audit Audit { get; set; } // Association Many-to-One avec la classe Audit
    }
}

