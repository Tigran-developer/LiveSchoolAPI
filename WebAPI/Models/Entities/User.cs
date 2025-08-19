using Microsoft.AspNetCore.Identity;

namespace WebAPI.Models.Entities
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public bool? isAdmin { get; set; }
        public bool? isStudent { get; set; }
        public bool? isTeacher { get; set; }
        public string? Initials { get; set; }
    }
}
