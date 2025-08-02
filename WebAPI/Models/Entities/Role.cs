using Microsoft.AspNetCore.Identity;

namespace WebAPI.Models.Entities
{
    public class Role: IdentityRole
    {
        public string? Description { get; set; }
    }
}
