using Microsoft.AspNetCore.Mvc;
using WebAPI.Attributes;
using WebAPI.Constants;

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
        [RequirePermission(requireAdmin: true)]
        public IActionResult AdminProperty()
        {
            return Ok("This endpoint requires isAdmin = true");
        }

        [HttpGet("teacher-property")]
        [RequirePermission(requireTeacher: true)]
        public IActionResult TeacherProperty()
        {
            return Ok("This endpoint requires isTeacher = true");
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
        [RequirePermission(
            allowedRoles: new[] { "Admin", "Teacher" }, 
            requireTeacher: true
        )]
        public IActionResult ComplexPermission()
        {
            return Ok("This endpoint requires Admin role OR Teacher role with isTeacher = true");
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
    }
}
