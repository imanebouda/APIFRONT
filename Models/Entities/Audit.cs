using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITKANSys_api.Models.Entities
{
    public class Audit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment primary key
        public int ID { get; set; }

        [Required]
        public string NomAudit { get; set; }

        [Required]
        public DateTime DateAudit { get; set; }

        [Required]
        public string status { get; set; }

        [Required]
        public string description { get; set; }

        // Foreign key property
        public int typeAuditId { get; set; }

        // Navigation property
        [ForeignKey("typeAuditId")]
        public TypeAudit typeAudit { get; set; }

        // Foreign key property for the auditor (user)
        public int UserId { get; set; }

        // Navigation property for the auditor
        [ForeignKey("UserId")]
        public User? Auditor { get; set; }

        

    }
}
