using ITKANSys_api.Models.Entities.Param;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITKANSys_api.Models.Entities
{
    public class Organisme
    {
        public int id { get; set; }
        public int id_type_organisme { get; set; }
        public string code { get; set; }
        public string brand { get; set; }
        public string social_reason { get; set; }
        public string approval { get; set; }
        public string address { get; set; }
        public string zip_code { get; set; }
        public string city { get; set; }
        public string phone { get; set; }
        public string gsm { get; set; }
        public string email { get; set; }
        public string logo { get; set; }
        [Column(TypeName = "date")]
        public DateTime? open_date { get; set; } // Modifié en DateTime?


        [ForeignKey("id_type_organisme")]
        public TypeOrganisme TypeOrganisme { get; set; }

        [Required]
        public DateTime created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
    }
}
