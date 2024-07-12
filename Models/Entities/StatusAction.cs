using System.ComponentModel.DataAnnotations;

namespace ITKANSys_api.Models.Entities
{
    public class StatusAction
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string Status { get; set; }

    }
}
