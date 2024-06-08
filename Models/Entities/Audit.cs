using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITKANSys_api.Models.Entities
{
    public class Audit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Clé primaire auto-incrémentée
        public int ID { get; set; }

        [Required]
        public string NomAudit { get; set; }
<<<<<<< HEAD
 
=======

        [Required]
>>>>>>> 7d731497bc5de5f582f9c84fecac832e8e0f1223
        public DateTime DateAudit { get; set; }

        [Required]
        public string status { get; set; }
<<<<<<< HEAD
        public string  description { get; set; }
        public string typeAudit { get; set; }
       
=======

        [Required]
        public string description { get; set; }

        // Propriété de clé étrangère
        public int typeAuditId { get; set; }

        // Propriété de navigation
        [ForeignKey("typeAuditId")]
        public TypeAudit typeAudit { get; set; }
>>>>>>> 7d731497bc5de5f582f9c84fecac832e8e0f1223

        // Association One-to-Many avec la classe Constat
      //  [InverseProperty("Audit")]
      //  public ICollection<Constat> Constats { get; set; }

        // Association One-to-Many avec la classe Processus
<<<<<<< HEAD
        [InverseProperty("Audit")]
        public ICollection<Processus> Processus { get; set; }



=======
       // [InverseProperty("Audit")]
      //  public ICollection<Processus> Processus { get; set; }
>>>>>>> 7d731497bc5de5f582f9c84fecac832e8e0f1223
    }
}
