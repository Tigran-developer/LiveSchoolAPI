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

        public RequirePermissionAttribute(string[] allowedRoles = null)
        {
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

            // Check role-based permissions
            if (_allowedRoles.Length > 0)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                if (userRoles == null || !_allowedRoles.Any(role => userRoles.Contains(role)))
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }
        }
    }
}
