using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using WebAPI.Data;
using WebAPI.Models.Entities;

namespace WebAPI.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            
            using ApplicationDBContext context = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();

            context.Database.Migrate();
        }
    }

    public static class UserExtensions
    {
        public static async Task<List<string>> GetUserRolesAsync(this UserManager<User> userManager, User user)
        {
            if (user == null)
                return new List<string>();

            var roles = await userManager.GetRolesAsync(user);
            return roles?.ToList() ?? new List<string>();
        }

        public static async Task<bool> HasRoleAsync(this UserManager<User> userManager, User user, string role)
        {
            if (user == null || string.IsNullOrEmpty(role))
                return false;

            var roles = await userManager.GetRolesAsync(user);
            return roles?.Contains(role) ?? false;
        }

        public static async Task<bool> HasAnyRoleAsync(this UserManager<User> userManager, User user, params string[] roles)
        {
            if (user == null || roles == null || roles.Length == 0)
                return false;

            var userRoles = await userManager.GetRolesAsync(user);
            return userRoles?.Any(r => roles.Contains(r)) ?? false;
        }
    }
}
