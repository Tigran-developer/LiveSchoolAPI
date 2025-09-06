using System.Security.Claims;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models;
using WebAPI.Models.Entities;
using WebAPI.Services;
using WebAPI.Attributes;

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

        [HttpGet("browse_classes")]
        [Permission("Classes", "Read")]
        public async Task<IActionResult> GetAvailableClasses()
        {
            Console.WriteLine($"[DEBUG] browse_classes - User.Identity.IsAuthenticated: {User.Identity?.IsAuthenticated}");
            Console.WriteLine($"[DEBUG] browse_classes - User.Identity.Name: {User.Identity?.Name}");
            
            // Get user data from authentication cookie claims
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var firstName = User.FindFirst("FirstName")?.Value;
            var lastName = User.FindFirst("LastName")?.Value;
            
            Console.WriteLine($"[DEBUG] browse_classes - UserId: {userId}, Email: {userEmail}, Name: {firstName} {lastName}");
            
            if (string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("[DEBUG] browse_classes - UserId not found in claims, returning Unauthorized");
                return Unauthorized("User not authenticated");
            }

            // Find the pupil record for this user
            var student = await _dBContext.Pupils.FirstOrDefaultAsync(t => t.UserId == userId);
            if (student == null) return NotFound("Student not found");

            var classes = await _dBContext.Classes
                .Where(c => !c.IsDeleted && !_dBContext.ClassPupils
                    .Any(cp => cp.ClassId == c.Id && cp.PupilId == student.Id))
                .Include(c => c.Teacher)
                .Include(c => c.Admin)
                .Include(c => c.Difficulty)
                .Include(c => c.Status)
                .Select(c => new ClassDTO
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    Subject = c.Subject,
                    StartTime = c.StartTime,
                    EndTime = c.EndTime,
                    DurationInMinutes = c.DurationInMinutes,
                    IsRecurring = c.IsRecurring,
                    RecurrencePattern = c.RecurrencePattern,
                    ZoomLink = c.ZoomLink,
                    MaxParticipants = c.MaxParticipants,
                    ParticipantsCount = c.ParticipantsCount,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                    NotifyBeforeMinutes = c.NotifyBeforeMinutes,
                    IsDeleted = c.IsDeleted,
                    Price = c.Price,
                    DifficultyId = c.DifficultyId,
                    DifficultyName = c.Difficulty.Name,
                    Tags = c.Tags,
                    Materials = c.Materials,
                    Notes = c.Notes,
                    Location = c.Location,
                    IsOnline = c.IsOnline,
                    RecordingUrl = c.RecordingUrl,
                    Status = c.Status == null ? null : new LessonStatusDTO
                    {
                        Id = c.Status.Id,
                        Name = c.Status.Name
                    },
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

        [HttpGet("booked_classes")]
        [Permission("Classes", "Read")]
        public async Task<IActionResult> GetClassesForStudentAsync()
        {
            Console.WriteLine($"[DEBUG] booked_classes - User.Identity.IsAuthenticated: {User.Identity?.IsAuthenticated}");
            Console.WriteLine($"[DEBUG] booked_classes - User.Identity.Name: {User.Identity?.Name}");
            
            // Get user data from authentication cookie claims
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var firstName = User.FindFirst("FirstName")?.Value;
            var lastName = User.FindFirst("LastName")?.Value;
            
            Console.WriteLine($"[DEBUG] booked_classes - UserId: {userId}, Email: {userEmail}, Name: {firstName} {lastName}");
            
            if (string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("[DEBUG] booked_classes - UserId not found in claims, returning Unauthorized");
                return Unauthorized("User not authenticated");
            }

            // Find the pupil record for this user
            var pupil = await _dBContext.Pupils.FirstOrDefaultAsync(p => p.UserId == userId);
            if (pupil == null)
                return NotFound("Student record not found");

            var classes = await _dBContext.Classes
                .Where(c => !c.IsDeleted &&
                            _dBContext.ClassPupils.Any(cp => cp.ClassId == c.Id && cp.PupilId == pupil.Id))
                .Include(c => c.Teacher)
                .Include(c => c.Admin)
                .Include(c => c.Difficulty)
                .Include(c => c.Status)
                .Select(c => new ClassDTO
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    Subject = c.Subject,
                    StartTime = c.StartTime,
                    EndTime = c.EndTime,
                    DurationInMinutes = c.DurationInMinutes,
                    IsRecurring = c.IsRecurring,
                    RecurrencePattern = c.RecurrencePattern,
                    ZoomLink = c.ZoomLink,
                    MaxParticipants = c.MaxParticipants,
                    ParticipantsCount = c.ParticipantsCount,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                    NotifyBeforeMinutes = c.NotifyBeforeMinutes,
                    IsDeleted = c.IsDeleted,
                    Price = c.Price,
                    DifficultyId = c.DifficultyId,
                    DifficultyName = c.Difficulty.Name,
                    Tags = c.Tags,
                    Materials = c.Materials,
                    Notes = c.Notes,
                    Location = c.Location,
                    IsOnline = c.IsOnline,
                    RecordingUrl = c.RecordingUrl,
                    Status = c.Status == null ? null : new LessonStatusDTO
                    {
                        Id = c.Status.Id,
                        Name = c.Status.Name
                    },
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
        [Permission("Classes", "Read")]
        public async Task<IActionResult> GetClassById(Guid id)
        {
            var classEntity = await _dBContext.Classes
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (classEntity == null)
                return NotFound("Class not found");

            return Ok(classEntity);
        }

        [HttpPost("add_class")]
        [Permission("Classes", "Write")]
        public async Task<IActionResult> AddClass([FromBody] AddClassDTO dto)
        {
            var teacherExists = await _dBContext.Teachers.AnyAsync(t => t.Id == dto.TeacherId);
            if (!teacherExists)
                return BadRequest($"Teacher with ID '{dto.TeacherId}' not found.");

            var adminExists = await _dBContext.Admins.AnyAsync(a => a.Id == dto.CreatedBy);
            if (!adminExists)
                return BadRequest($"Admin with ID '{dto.CreatedBy}' not found.");

            var difficultyExists = await _dBContext.ClassDifficulties.AnyAsync(d => d.Id == dto.DifficultyId);
            if (!difficultyExists)
                return BadRequest($"Difficulty with ID '{dto.DifficultyId}' not found.");

            var newClass = new Class
            {
                Title = dto.Title,
                Description = dto.Description,
                Subject = dto.Subject,
                TeacherId = dto.TeacherId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                DurationInMinutes = dto.DurationInMinutes,
                IsRecurring = dto.IsRecurring,
                RecurrencePattern = dto.RecurrencePattern,
                ZoomLink = dto.ZoomLink,
                MaxParticipants = dto.MaxParticipants,
                CreatedBy = dto.CreatedBy,
                NotifyBeforeMinutes = dto.NotifyBeforeMinutes ?? 15,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Price = dto.Price,
                DifficultyId = dto.DifficultyId,
                Tags = dto.Tags,
                Materials = dto.Materials,
                Notes = dto.Notes,
                Location = dto.Location,
                IsOnline = dto.IsOnline,
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
        [Permission("Classes", "Write")]
        public async Task<IActionResult> UpdateClass(int id, [FromBody] UpdateClassDto dto)
        {
            var classEntity = await _dBContext.Classes.FindAsync(id);

            if (classEntity == null || classEntity.IsDeleted)
                return NotFound("Class not found");

            if (dto.Title != null) classEntity.Title = dto.Title;
            if (dto.Description != null) classEntity.Description = dto.Description;
            if (dto.Subject != null) classEntity.Subject = dto.Subject;
            if (dto.StartTime.HasValue) classEntity.StartTime = dto.StartTime.Value;
            if (dto.EndTime.HasValue) classEntity.EndTime = dto.EndTime.Value;
            if (dto.DurationInMinutes.HasValue) classEntity.DurationInMinutes = dto.DurationInMinutes.Value;
            if (dto.IsRecurring.HasValue) classEntity.IsRecurring = dto.IsRecurring.Value;
            if (dto.RecurrencePattern != null) classEntity.RecurrencePattern = dto.RecurrencePattern;
            if (dto.ZoomLink != null) classEntity.ZoomLink = dto.ZoomLink;
            if (dto.MaxParticipants.HasValue) classEntity.MaxParticipants = dto.MaxParticipants.Value;
            if (dto.Price.HasValue) classEntity.Price = dto.Price.Value;
            if (dto.DifficultyId.HasValue) classEntity.DifficultyId = dto.DifficultyId.Value;
            if (dto.Tags != null) classEntity.Tags = dto.Tags;
            if (dto.Materials != null) classEntity.Materials = dto.Materials;
            if (dto.Notes != null) classEntity.Notes = dto.Notes;
            if (dto.Location != null) classEntity.Location = dto.Location;
            if (dto.IsOnline.HasValue) classEntity.IsOnline = dto.IsOnline.Value;

            classEntity.UpdatedAt = DateTime.UtcNow;

            await _dBContext.SaveChangesAsync();

            return Ok(new { Message = "Class updated successfully" });
        }

        [HttpDelete("{id}")]
        [Permission("Classes", "Delete")]
        public async Task<IActionResult> DeleteClass(int id)
        {
            var classEntity = await _dBContext.Classes.FindAsync(id);

            if (classEntity == null || classEntity.IsDeleted)
                return NotFound("Class not found");

            classEntity.IsDeleted = true;
            await _dBContext.SaveChangesAsync();

            return Ok(new { Message = "Class deleted successfully" });
        }

        [HttpPost("book_class")]
        [Permission("Classes", "Write")]
        public async Task<ActionResult> BookClassForStudent([FromBody] BookClassDto dto)
        {
            // Get the authenticated user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized("User not authenticated");

            // Find the pupil record for this user
            var student = await _dBContext.Pupils.FirstOrDefaultAsync(t => t.UserId == user.Id);
            if (student == null) return NotFound("Student not found");

            var classExist = await _dBContext.Classes.AnyAsync(t => t.Id == dto.ClassId);
            if (!classExist) return NotFound("Class not found");

            var alreadyBooked = await _dBContext.ClassPupils
                .AnyAsync(cp => cp.ClassId == dto.ClassId && cp.PupilId == student.Id);

            if (alreadyBooked) return BadRequest("Class already booked for this student.");

            var bookedClass = new ClassPupil
            {
                ClassId = dto.ClassId,
                PupilId = student.Id,
                JoinedAt = DateTime.UtcNow
            };

            _dBContext.ClassPupils.Add(bookedClass);
            await _dBContext.SaveChangesAsync();

            return Ok("Class booked successfully");
        }

    }
}
