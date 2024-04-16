using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITKANSys_api.Models.Entities.Param
{
    public class SMQ
    {
        public int ID { get; set; }
        public string Libelle { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }

        // Ajoutez la clé étrangère vers le modèle Role ici
        public int SiteID { get; set; }
        [ForeignKey("SiteID")]
        public Sites Sites { get; set; }
    }
}
