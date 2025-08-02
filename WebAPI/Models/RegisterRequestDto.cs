using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "FirstName is required")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The password and confirmation passoword do not match")]
        public string ConfirmPassword { get; set; }

        public string? Phone { get; set; }
        public bool? IsTeacher { get; set; }
        public string? Initials { get; set; }
        public string? ClientUrl { get; set; }
    }
}
