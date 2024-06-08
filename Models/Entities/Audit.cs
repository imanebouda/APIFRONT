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

        [Required]
        public DateTime DateAudit { get; set; }

        [Required]
        public string status { get; set; }

        [Required]
        public string description { get; set; }

        // Propriété de clé étrangère
        public int typeAuditId { get; set; }

        // Propriété de navigation
        [ForeignKey("typeAuditId")]
        public TypeAudit typeAudit { get; set; }

        // Association One-to-Many avec la classe Constat
        //  [InverseProperty("Audit")]
        //  public ICollection<Constat> Constats { get; set; }

        // Association One-to-Many avec la classe Processus
        // [InverseProperty("Audit")]
        //  public ICollection<Processus> Processus { get; set; }
    }
}