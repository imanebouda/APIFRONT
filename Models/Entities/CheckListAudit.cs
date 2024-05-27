using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITKANSys_api.Models.Entities
{
    public class CheckListAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ajoutez cet attribut pour rendre l'id auto-incrémenté
        public int id { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string niveau { get; set; }

        [Required]
        public string code { get; set; }

        [Required]
        public string description { get; set; }

        // Propriété de clé étrangère
        public int typechecklist_id { get; set; }

        // Propriété de navigation
        [ForeignKey("typechecklist_id")]
        public TypeCheckListAudit TypeCheckListAudit { get; set; }
    }
}
