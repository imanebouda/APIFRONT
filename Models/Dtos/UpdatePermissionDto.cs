using System.ComponentModel.DataAnnotations;

namespace ITKANSys_api.Core.Dtos
{
    public class UpdatePermissionDto
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; } 
      
    }
}
