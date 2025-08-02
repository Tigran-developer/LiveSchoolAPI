using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class ForgotPassordDTO
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? ClientUrl { get; set; }
    }
}
