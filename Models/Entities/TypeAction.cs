﻿using System.ComponentModel.DataAnnotations;

namespace ITKANSys_api.Models.Entities
{
    public class TypeAction
    {

        [Key]
        public int id { get; set; }

        [Required]
        public string type { get; set; }

    }
}
