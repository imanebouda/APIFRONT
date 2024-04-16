using System.ComponentModel.DataAnnotations;

namespace ITKANSys_api.Models.Entities.Param
{
    public class Parametrage
    {
        public int id { get; set; }
        public string label { get; set; }
        public string value { get; set; }

        [Required]
        public DateTime created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
    }
}
