using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models;
using WebAPI.Models.Entities;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailSender;
        private readonly ApplicationDBContext _dBContext;
        public ClassesController(UserManager<User> userManager, ApplicationDBContext dBContext, IEmailService emailSender)
        {
            _userManager = userManager;
            this._dBContext = dBContext;
            _emailSender = emailSender;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClasses()
        {
            var classes = await _dBContext.Classes
                .Where(c => !c.IsDeleted)
                .Include(c => c.Teacher)
                .Include(c => c.Admin)
                .Select(c => new ClassDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    StartTime = c.StartTime,
                    EndTime = c.EndTime,
                    IsRecurring = c.IsRecurring,
                    RecurrencePattern = c.RecurrencePattern,
                    ZoomLink = c.ZoomLink,
                    MaxParticipants = c.MaxParticipants,
                    CreatedAt = c.CreatedAt,
                    Status = c.Status,
                    NotifyBeforeMinutes = c.NotifyBeforeMinutes,
                    IsDeleted = c.IsDeleted,
                    Teacher = c.Teacher == null ? null : new SimplePrivateUserDTO
                    {
                        FirstName = c.Teacher.FirstName,
                        LastName = c.Teacher.LastName
                    },
                    Admin = c.Admin == null ? null : new SimplePrivateUserDTO
                    {
                        FirstName = c.Admin.FirstName,
                        LastName = c.Admin.LastName
                    }
                })
                .ToListAsync();

            return Ok(classes);
        }

        [HttpGet("class/{id}")]
        public async Task<IActionResult> GetClassById(int id)
        {
            var classEntity = await _dBContext.Classes
                .FirstOrDefaultAsync(c => c.Id.ToString() == id.ToString() && !c.IsDeleted);

            if (classEntity == null)
                return NotFound("Class not found");

            return Ok(classEntity);
        }

        [HttpPost]
        public async Task<IActionResult> AddClass([FromBody] AddClassDTO dto)
        {
            var teacherExists = await _dBContext.Teachers.AnyAsync(t => t.Id == dto.TeacherId);
            if (!teacherExists)
                return BadRequest($"Teacher with ID '{dto.TeacherId}' not found.");

            var adminExists = await _dBContext.Admins.AnyAsync(a => a.Id == dto.CreatedBy);
            if (!adminExists)
                return BadRequest($"Admin with ID '{dto.CreatedBy}' not found.");

            var newClass = new Class
            {
                Title = dto.Title,
                Description = dto.Description,
                TeacherId = dto.TeacherId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                IsRecurring = dto.IsRecurring,
                RecurrencePattern = dto.RecurrencePattern,
                ZoomLink = dto.ZoomLink,
                MaxParticipants = dto.MaxParticipants,
                CreatedBy = dto.CreatedBy,
                NotifyBeforeMinutes = dto.NotifyBeforeMinutes ?? 15,
                CreatedAt = DateTime.UtcNow,
                Status = "Scheduled",
                IsDeleted = false
            };

            _dBContext.Classes.Add(newClass);
            await _dBContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClassById), new { id = newClass.Id }, new
            {
                newClass.Id,
                Message = "Class created successfully"
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClass(int id, [FromBody] AddClassDTO dto)
        {
            var classEntity = await _dBContext.Classes.FindAsync(id);

            if (classEntity == null || classEntity.IsDeleted)
                return NotFound("Class not found");

            classEntity.Title = dto.Title;
            classEntity.Description = dto.Description;
            classEntity.TeacherId = dto.TeacherId;
            classEntity.StartTime = dto.StartTime;
            classEntity.EndTime = dto.EndTime;
            classEntity.IsRecurring = dto.IsRecurring;
            classEntity.RecurrencePattern = dto.RecurrencePattern;
            classEntity.ZoomLink = dto.ZoomLink;
            classEntity.MaxParticipants = dto.MaxParticipants;
            classEntity.NotifyBeforeMinutes = dto.NotifyBeforeMinutes ?? 15;
            classEntity.CreatedBy = dto.CreatedBy; // Optional, or omit if set automatically

            await _dBContext.SaveChangesAsync();

            return Ok(new { Message = "Class updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClass(int id)
        {
            var classEntity = await _dBContext.Classes.FindAsync(id);

            if (classEntity == null || classEntity.IsDeleted)
                return NotFound("Class not found");

            classEntity.IsDeleted = true;
            await _dBContext.SaveChangesAsync();

            return Ok(new { Message = "Class deleted successfully" });
        }
    }
}
