using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;
using WebAPI.Models.Entities;
using WebAPI.Constants;

namespace WebAPI.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class PermissionAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        private readonly string _resource;
        private readonly string _action;
        private readonly string[] _allowedRoles;

        public PermissionAttribute(string resource, string action = "Read", string[] allowedRoles = null)
        {
            _resource = resource;
            _action = action;
            _allowedRoles = allowedRoles ?? new string[0];
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context == null)
                return;

            var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<User>>();
            var user = await userManager.GetUserAsync(context.HttpContext.User);

            if (user == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            System.Diagnostics.Debug.WriteLine(context);
            // Get user roles
            var userRoles = await userManager.GetRolesAsync(user);

            // Check if user has permission based on role and resource
            if (!HasPermission(user, userRoles, _resource, _action))
            {
                context.Result = new ForbidResult();
                return;
            }
        }

        private bool HasPermission(User user, IList<string> userRoles, string resource, string action)
        {
            Console.WriteLine($"[DEBUG] HasPermission called - User: {user?.Email}, Resource: {resource}, Action: {action}");
            Console.WriteLine($"[DEBUG] User roles: [{string.Join(", ", userRoles ?? new List<string>())}]");
            
            if (user == null || userRoles == null || string.IsNullOrEmpty(resource) || string.IsNullOrEmpty(action))
            {
                Console.WriteLine($"[DEBUG] HasPermission - Invalid parameters. User: {user != null}, Roles: {userRoles != null}, Resource: '{resource}', Action: '{action}'");
                return false;
            }

            // Admin has access to everything
            if (userRoles.Contains(Permissions.Roles.Admin))
            {
                Console.WriteLine($"[DEBUG] HasPermission - Admin access granted for {resource}:{action}");
                return true;
            }

            // Check resource-specific permissions
            switch (resource)
            {
                case Permissions.Resources.Classes:
                    return HasClassPermission(user, userRoles, action);
                
                case Permissions.Resources.Pupils:
                    return HasPupilPermission(user, userRoles, action);
                
                case Permissions.Resources.Teachers:
                    return HasTeacherPermission(user, userRoles, action);
                
                case Permissions.Resources.Lessons:
                    return HasLessonPermission(user, userRoles, action);
                
                case Permissions.Resources.Users:
                    return HasUserPermission(user, userRoles, action);
                
                case Permissions.Resources.System:
                    return HasSystemPermission(user, userRoles, action);
                
                default:
                    return false;
            }
        }

        private bool HasClassPermission(User user, IList<string> userRoles, string action)
        {
            Console.WriteLine($"[DEBUG] HasClassPermission - User: {user?.Email}, Action: {action}");
            Console.WriteLine($"[DEBUG] HasClassPermission - User roles: [{string.Join(", ", userRoles)}]");
            
            // Admin has access to everything (already handled in main HasPermission method)
            
            if (userRoles.Contains(Permissions.Roles.Teacher))
            {
                bool hasAccess = action == Permissions.Levels.Read || action == Permissions.Levels.Write;
                Console.WriteLine($"[DEBUG] HasClassPermission - Teacher access: {hasAccess} for action: {action}");
                // Teachers can read and manage classes they teach
                return hasAccess;
            }

            if (userRoles.Contains(Permissions.Roles.Pupil))
            {
                bool hasAccess = action == Permissions.Levels.Read;
                Console.WriteLine($"[DEBUG] HasClassPermission - Pupil access: {hasAccess} for action: {action}");
                // Pupils can read classes (including their booked classes)
                return hasAccess;
            }

            Console.WriteLine($"[DEBUG] HasClassPermission - No matching role found, access denied");
            return false;
        }

        private bool HasPupilPermission(User user, IList<string> userRoles, string action)
        {
            if (userRoles.Contains(Permissions.Roles.Teacher))
            {
                // Teachers can read pupil information
                return action == Permissions.Levels.Read;
            }

            if (userRoles.Contains(Permissions.Roles.Pupil))
            {
                // Pupils can only read their own information
                return action == Permissions.Levels.Read;
            }

            return false;
        }

        private bool HasTeacherPermission(User user, IList<string> userRoles, string action)
        {
            if (userRoles.Contains(Permissions.Roles.Teacher))
            {
                // Teachers can read other teacher information
                return action == Permissions.Levels.Read;
            }

            return false;
        }

        private bool HasLessonPermission(User user, IList<string> userRoles, string action)
        {
            if (userRoles.Contains(Permissions.Roles.Teacher))
            {
                // Teachers can manage lessons
                return action == Permissions.Levels.Read || action == Permissions.Levels.Write || action == Permissions.Levels.Delete;
            }

            if (userRoles.Contains(Permissions.Roles.Pupil))
            {
                // Pupils can only read lesson information
                return action == Permissions.Levels.Read;
            }

            return false;
        }

        private bool HasUserPermission(User user, IList<string> userRoles, string action)
        {
            // Only admins can manage users
            return userRoles.Contains(Permissions.Roles.Admin);
        }

        private bool HasSystemPermission(User user, IList<string> userRoles, string action)
        {
            // Only admins have system access
            return userRoles.Contains(Permissions.Roles.Admin);
        }
    }
}
