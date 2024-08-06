using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ITKANSys_api.Models.Entities
{
    public class HistoryReclamation
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Clé primaire auto-incrémentée
        public int ID { get; set; }
        public int ReclamationID { get; set; }

        // Navigation property
        [ForeignKey("ReclamationID")]
        public Reclamation reclamation { get; set; }

        public string Commentaire { get; set; }

        //public string CreationBy { get; set; }
        //[ForeignKey("CreationBy")] // Renommez la clé étrangère pour correspondre à la clé primaire de User
        //public User Users { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        public Reclamation Reclamation { get; set; }
    }
}
