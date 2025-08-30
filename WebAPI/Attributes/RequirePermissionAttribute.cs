using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;
using WebAPI.Models.Entities;

namespace WebAPI.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class RequirePermissionAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        private readonly string[] _allowedRoles;
        private readonly bool _requireAdmin;
        private readonly bool _requireTeacher;
        private readonly bool _requireStudent;

        public RequirePermissionAttribute(string[] allowedRoles = null, bool requireAdmin = false, bool requireTeacher = false, bool requireStudent = false)
        {
            _allowedRoles = allowedRoles ?? new string[0];
            _requireAdmin = requireAdmin;
            _requireTeacher = requireTeacher;
            _requireStudent = requireStudent;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<User>>();
            var user = await userManager.GetUserAsync(context.HttpContext.User);

            if (user == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Check role-based permissions
            if (_allowedRoles.Length > 0)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                if (!_allowedRoles.Any(role => userRoles.Contains(role)))
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }

            // Check property-based permissions
            if (_requireAdmin && user.isAdmin != true)
            {
                context.Result = new ForbidResult();
                return;
            }

            if (_requireTeacher && user.isTeacher != true)
            {
                context.Result = new ForbidResult();
                return;
            }

            if (_requireStudent && user.isStudent != true)
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}
