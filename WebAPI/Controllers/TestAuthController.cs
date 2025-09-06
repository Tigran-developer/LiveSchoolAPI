using Microsoft.AspNetCore.Mvc;
using WebAPI.Attributes;
using WebAPI.Constants;
using Microsoft.AspNetCore.Identity;
using WebAPI.Models.Entities;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestAuthController : ControllerBase
    {
        [HttpGet("public")]
        public IActionResult PublicEndpoint()
        {
            return Ok("This endpoint is public - no authentication required");
        }

        [HttpGet("admin-only")]
        [RequireRole("Admin")]
        public IActionResult AdminOnly()
        {
            return Ok("This endpoint is only accessible to Admin users");
        }

        [HttpGet("teacher-or-admin")]
        [RequireRole("Admin", "Teacher")]
        public IActionResult TeacherOrAdmin()
        {
            return Ok("This endpoint is accessible to both Admin and Teacher users");
        }

        [HttpGet("admin-property")]
        [RequirePermission(new[] { "Admin" })]
        public IActionResult AdminProperty()
        {
            return Ok("This endpoint requires Admin role");
        }

        [HttpGet("teacher-property")]
        [RequirePermission(new[] { "Teacher" })]
        public IActionResult TeacherProperty()
        {
            return Ok("This endpoint requires Teacher role");
        }

        [HttpGet("classes-read")]
        [Permission("Classes", "Read")]
        public IActionResult ClassesRead()
        {
            return Ok("This endpoint requires read permission to Classes resource");
        }

        [HttpGet("classes-write")]
        [Permission("Classes", "Write")]
        public IActionResult ClassesWrite()
        {
            return Ok("This endpoint requires write permission to Classes resource");
        }

        [HttpGet("pupils-manage")]
        [Permission("Pupils", "Manage")]
        public IActionResult PupilsManage()
        {
            return Ok("This endpoint requires manage permission to Pupils resource");
        }

        [HttpGet("system-access")]
        [Permission("System", "Read")]
        public IActionResult SystemAccess()
        {
            return Ok("This endpoint requires system access (Admin only)");
        }

        [HttpGet("complex-permission")]
        [RequirePermission(new[] { "Admin", "Teacher" })]
        public IActionResult ComplexPermission()
        {
            return Ok("This endpoint requires Admin role OR Teacher role");
        }

        [HttpGet("user-info")]
        public IActionResult GetCurrentUser()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return Ok(new
                {
                    IsAuthenticated = true,
                    Username = User.Identity.Name,
                    Claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList()
                });
            }
            
            return Ok(new { IsAuthenticated = false });
        }

        [HttpGet("debug-permission")]
        public async Task<IActionResult> DebugPermission([FromQuery] string resource = "Classes", [FromQuery] string action = "Read")
        {
            var userManager = HttpContext.RequestServices.GetRequiredService<UserManager<User>>();
            var user = await userManager.GetUserAsync(User);
            
            if (user == null)
            {
                return Ok(new
                {
                    IsAuthenticated = false,
                    Message = "User not authenticated",
                    Resource = resource,
                    Action = action
                });
            }

            var userRoles = await userManager.GetRolesAsync(user);
            
            // Test the permission logic manually
            bool hasPermission = false;
            string permissionResult = "";
            
            if (userRoles.Contains("Admin"))
            {
                hasPermission = true;
                permissionResult = "Admin has full access";
            }
            else if (resource == "Classes")
            {
                if (userRoles.Contains("Teacher"))
                {
                    hasPermission = action == "Read" || action == "Write";
                    permissionResult = $"Teacher access: {hasPermission} for action: {action}";
                }
                else if (userRoles.Contains("Pupil"))
                {
                    hasPermission = action == "Read";
                    permissionResult = $"Pupil access: {hasPermission} for action: {action}";
                }
                else
                {
                    permissionResult = "No matching role found";
                }
            }

            return Ok(new
            {
                IsAuthenticated = true,
                User = new
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                },
                Roles = userRoles,
                Resource = resource,
                Action = action,
                HasPermission = hasPermission,
                PermissionResult = permissionResult,
                DebugInfo = new
                {
                    IsAdmin = userRoles.Contains("Admin"),
                    IsTeacher = userRoles.Contains("Teacher"),
                    IsPupil = userRoles.Contains("Pupil"),
                    RequestedResource = resource,
                    RequestedAction = action
                }
            });
        }
    }
}
