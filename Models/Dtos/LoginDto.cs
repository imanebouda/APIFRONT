using System.ComponentModel.DataAnnotations;

namespace ITKANSys_api.Core.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; } 

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
 