using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ITKANSys_api.Models.Entities
{
    public class Action
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment primary key
        public int ID { get; set; }

        [Required]
        public string libelle { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

      

        [Required]
        public string description { get; set; }


        public int UserId { get; set; }

        // Navigation property for the auditor
        [ForeignKey("UserId")]
        public User? user { get; set; }



        // Foreign key property
        public int typeActionId { get; set; }

        // Navigation property
        [ForeignKey("typeActionId ")]
        public TypeAction? typeAction { get; set; }



        public int statusActionId { get; set; }

        // Navigation property
        [ForeignKey("statusActionId ")]
        public StatusAction? statusAction { get; set; }

    }
}
