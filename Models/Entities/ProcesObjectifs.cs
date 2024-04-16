﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITKANSys_api.Models.Entities
{
    public class ProcesObjectifs
    {
        public int ID { get; set; }
        public int ProcessusID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [Required]// Renommez la clé étrangère en CreatedById
        public DateTime created_at { get; set; }
        public DateTime? deleted_at { get; set; }
        public DateTime? updated_at { get; set; }


        [ForeignKey("ProcessusID")] // Renommez la clé étrangère
        [Column("ProcessusID")]
        public Processus Processus { get; set; }

    }
}
