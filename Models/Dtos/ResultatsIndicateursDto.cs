using ITKANSys_api.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ITKANSys_api.Models.Dtos
{
    public class ResultatsIndicateursDto
    {
        public int ID { get; set; }
        public int IndicateurID { get; set; }
        public string Periode { get; set; }
        public int Annee { get; set; }
        public decimal Resultat { get; set; }

        [Required]// Renommez la clé étrangère en CreatedById
        public DateTime created_at { get; set; }
        public DateTime? deleted_at { get; set; }
        public DateTime? updated_at { get; set; }

        [ForeignKey("IndicateurID")] // Renommez la clé étrangère
        public Indicateurs Indicateurs { get; set; }
    }
}
