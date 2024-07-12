using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ITKANSys_api.Models.Entities
{
    public class Preuve
    {



        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment primary key
        public int ID { get; set; }



        [Required]
        public string filename { get; set; }


        [Required]
        public string filepath { get; set; }


        [Required]
        public DateTime CreationDate { get; set; }


        public int ActionId { get; set; }

        // Navigation property
        [ForeignKey("ActionId ")]
        public Action? Action { get; set; }


    }
}
