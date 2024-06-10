using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITKANSys_api.Models.Entities
{
    public class Constat
    {
        [Key]
        public int ID { get; set; }

        [Required]
      
        public string constat { get; set; }
       

        public int typeConstatId { get; set; }
        public int ChecklistId { get; set; }

        // Propriété de navigation
        [ForeignKey("typeConstatId")]
        public TypeContat typeConstat { get; set; }


        // Propriété de navigation
        [ForeignKey("ChecklistId")]
        public CheckListAudit Checklist { get; set; }
    }
}
