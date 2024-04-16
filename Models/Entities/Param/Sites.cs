using System.ComponentModel.DataAnnotations;

namespace ITKANSys_api.Models.Entities.Param
{
    public class Sites
    {
        public int ID { get; set; }
        public string Libelle { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
    }
}
