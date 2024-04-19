using ITKANSys_api.Models;
using ITKANSys_api.Models.Entities.Param;
using ITKANSys_api.Models.Dtos;
using System.ComponentModel.DataAnnotations;
using static ITKANSys_api.Data.Services.ProcessusService;
using ITKANSys_api.Models.Entities;

namespace ITKANSys_api.Data.Dtos
{
    public class ProcessusDto
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Version { get; set; }
        public string Libelle { get; set; }
        public string Description { get; set; }
        public string NamePilote { get; set; }
        public string NameCoPilote { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        [Required]
        public DateTime created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }

        public int SMQ_ID { get; set; }
        public int Categorie_ID { get; set; }
        public int USER_ID { get; set; }
        public int Pilote { get; set; }
        public int CoPilote { get; set; }

        public SMQ SMQ { get; set; }
        public Categories Categories { get; set; }
        public User Users { get; set; }

        public User PiloteUser { get; set; }

        public User CoPiloteUser { get; set; }
        public Audit Audit { get; set; }
    }
}
