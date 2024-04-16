using ITKANSys_api.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITKANSys_api.Models
{
    public class Procedures
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Version { get; set; }
        public string Libelle { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }

        [Required]
        public DateTime created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }

        // Clé étrangère pour représenter la relation avec Processus
        public int ProcessusID { get; set; }

        [ForeignKey("ProcessusID")]
        public Processus Processus { get; set; }
    }

}
