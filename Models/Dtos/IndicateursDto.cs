﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ITKANSys_api.Models.Dtos
{
    public class IndicateursDto
    {
        public int ID { get; set; }
        public int ProcessusID { get; set; }
        public string Libelle { get; set; }
        public double Cible { get; set; }
        public double Tolerance { get; set; }
        public string Formule { get; set; }
        public string Frequence { get; set; }


        [Required]// Renommez la clé étrangère en CreatedById
        public DateTime created_at { get; set; }
        public DateTime? deleted_at { get; set; }
        public DateTime? updated_at { get; set; }

        [ForeignKey("ProcessusID")] // Renommez la clé étrangère
        public Processus Processus { get; set; }
    }
}
