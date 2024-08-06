using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ITKANSys_api.Models.Entities
{
    public class Reclamant
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Nom { get; set; }

        [Required]
        public string Prénom { get; set; }

        [Required]
        public string Email { get; set; }

        public string Mobile { get; set; }

        public string Adresse { get; set; }

        public string Ville { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }
        [JsonIgnore]
        public ICollection<Reclamation> Reclamations { get; set; } = new List<Reclamation>();
    }
}
