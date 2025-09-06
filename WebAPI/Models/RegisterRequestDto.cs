using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "FirstName is required")]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "LastName is required")]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "ConfirmPassword is required")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Phone { get; set; }
        
        [Required(ErrorMessage = "User type is required")]
        public string UserType { get; set; } = string.Empty; // "Teacher" or "Pupil"
        
        [MaxLength(15)]
        public string? Initials { get; set; }
        
        [Required(ErrorMessage = "ClientUrl is required")]
        public string ClientUrl { get; set; } = string.Empty;
    }
}
