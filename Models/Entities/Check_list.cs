using ITKANSys_api.Models.Entities.Param;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ITKANSys_api.Models.Entities;


namespace ITKANSys_api.Models.Entities
{
    public class Check_list
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment primary key
        public int Id { get; set; }

      
        // Foreign key property
        public int typeAuditId { get; set; }

        // Navigation property
        [ForeignKey("typeAuditId")]
        public TypeAudit typeAudit { get; set; }



        public int SMQ_ID { get; set; }
     

        [ForeignKey("SMQ_ID")]
        public SMQ SMQ { get; set; }


        // Clé étrangère pour représenter la relation avec Processus
        public int ProcessusID { get; set; }

        [ForeignKey("ProcessusID")]
        public Processus Processus { get; set; }

    }
}
