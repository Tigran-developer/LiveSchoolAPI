using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models.Entities;

namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ClassStudentController : Controller
    {
        private readonly ApplicationDBContext _dBContext;

        /*[HttpPost("book_class")]
        public async Task<ActionResult> BookClassForStudent([FromQuery] Guid studentId, [FromQuery] Guid classId)
        {
            var studentExist = await _dBContext.Pupils.AnyAsync(t => t.Id == studentId);
            var classExist = await _dBContext.Classes.AnyAsync(t => t.Id == classId);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            *//*if (userId == null)
            {
                return Unauthorized("UserId not found in token.");
            }
            if (!studentExist)
            {
                return NotFound("Student not found");
            }
            if (!classExist)
            {
                return NotFound("Class not found");
            }*//*
            DateTime now = DateTime.Now;

            var bookedClass = new ClassPupil
            {
                ClassId = classId,
                PupilId = studentId,
                JoinedAt = now
            };
            _dBContext.ClassPupils.Add(bookedClass);
            await _dBContext.SaveChangesAsync();

            return Ok("Class booked successfully");
        }*/
    }
}
