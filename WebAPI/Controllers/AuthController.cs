using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using WebAPI.Data;
using WebAPI.Models;
using WebAPI.Models.Entities;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailSender;
        private readonly ApplicationDBContext _dBContext;
        public AuthController(UserManager<User> userManager, ApplicationDBContext dBContext, IEmailService emailSender)
        {
            _userManager = userManager;
            this._dBContext = dBContext;
            _emailSender = emailSender;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            var user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Phone = request.Phone,
                isTeacher = request.IsTeacher,
                isAdmin = request.IsAdmin,
                isStudent = request.IsStudent,
                Initials = request.Initials               
            };

            using var transaction = await _dBContext.Database.BeginTransactionAsync();

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded){
                await transaction.RollbackAsync();
                return BadRequest(result.Errors);
            }

            IdentityResult roleResult;
            if (request.IsTeacher == true)
            {
                _dBContext.Teachers.Add(new Teacher
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Phone = request.Phone,
                    User = user,
                });

                roleResult = await _userManager.AddToRoleAsync(user, "Teacher");
            }
            else
            {
                _dBContext.Pupils.Add(new Pupil
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Phone = request.Phone,
                    User = user
                });

                roleResult = await _userManager.AddToRoleAsync(user, "Pupil");
            }

            if (!roleResult.Succeeded)
            {
                await transaction.RollbackAsync();
                return BadRequest(roleResult.Errors);
            }

            await _dBContext.SaveChangesAsync();
          

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var queryParams = new Dictionary<string, string?>
                {
                    { "token", token },
                    { "email", user.Email }
                };
            var callbackUrl = QueryHelpers.AddQueryString(request.ClientUrl!, queryParams);

            var message = new EmailMetadata(
                user.Email,
                "Confirm your email",
                $"Please confirm your account by <a href='{callbackUrl}'>clicking here</a>",
                null
            );

            await _emailSender.Send(message);
            await transaction.CommitAsync();

            return Ok();
        }


        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.EmailPhone);
            if (user is null)
                return NotFound("User not found.");

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return Unauthorized("Email not confirmed.");

            if (!await _userManager.CheckPasswordAsync(user, request.Password!))
                return Unauthorized("Invalid authenticate");

            var userData = new ResponseUserDTO
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                isTeacher = user.isTeacher,
                isAdmin = user.isAdmin,
                isStudent = user.isStudent,
            };

            var cookieValue = System.Text.Json.JsonSerializer.Serialize(userData);

            Response.Cookies.Append("currentUser", cookieValue, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });
            userData.Id = user.Id;

            return Ok(userData);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPassordDTO forgotPassord)
        {
            if(ModelState.IsValid == false)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(forgotPassord.Email);
            if (user is null)
                return NotFound("Invalid Request");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var queryParams = new Dictionary<string, string?>
                {
                    { "token", token },
                    { "email", user.Email }
                };
            var callbackUrl = QueryHelpers.AddQueryString(forgotPassord.ClientUrl, queryParams);
            var message = new EmailMetadata(
                user.Email,
                "Reset your password",
                $"Please reset your password by <a href='{callbackUrl}'>clicking here</a>",
                null
            );
            await _emailSender.Send(message);
            return Ok(new { success = true, message = "SUCCESS" });
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPassord)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(resetPassord.Email);
            if (user is null)
                return NotFound("Invalid Request");

            var result = await _userManager.ResetPasswordAsync(user, resetPassord.Token, resetPassord.Password);

            if(result.Succeeded == false)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new { Errors = errors });

            }
            return Ok(new { success = true, message = "SUCCESS" });
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest(new { success = false, message = "Invalid email address." });

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            return BadRequest(new { success = false, message = "Invalid or expired token." });
                
            return Ok(new { success = true, message = "Email confirmed successfully." });
        }

        [HttpPost("check-email-exist")]
        public async Task<IActionResult> CheckEmailExist([FromQuery] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Ok(new { isDuplicate = false });

            return Ok(new { isDuplicate = true });
        }

    }
}
