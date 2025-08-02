using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Email is required")]
        public string EmailPhone { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
