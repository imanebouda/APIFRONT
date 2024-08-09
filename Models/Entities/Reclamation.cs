using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ITKANSys_api.Models.Entities
{
    public class Reclamation
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int ID { get; set; }

        [Required]
        public string Objet { get; set; }

        [Required]
        public string Détail { get; set; }

        public string Analyse { get; set; }

        public string Status { get; set; }

        [Required]
        public int ReclamantID { get; set; }

        [Required]
        public int ResponsableID { get; set; }


        [ForeignKey("ResponsableID")]
        public User userId { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [ForeignKey("ReclamantID")]
        public Reclamant Reclamant { get; set; }

        public ICollection<HistoryReclamation> HistoryReclamations { get; set; }
        public ICollection<ComiteeReclamation> ComiteeReclamation { get; set; }
    }
}
