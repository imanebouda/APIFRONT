using System.ComponentModel.DataAnnotations;
using ITKANSys_api.Models.Entities;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations.Schema;

namespace ITKANSys_api.Models
{
    public class SiteAudit
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string address { get; set; }

        [Required]
        public string city { get; set; }
    }
}
