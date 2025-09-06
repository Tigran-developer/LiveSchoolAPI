using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;
using WebAPI.Data;
using WebAPI.Models;
using WebAPI.Models.Entities;
using WebAPI.Services;
using WebAPI.Attributes;
using WebAPI.Extensions;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailSender;
        private readonly ApplicationDBContext _dBContext;
        private readonly SignInManager<User> _signInManager;
        
        public AuthController(UserManager<User> userManager, ApplicationDBContext dBContext, IEmailService emailSender, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            this._dBContext = dBContext;
            _emailSender = emailSender;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            if (request == null)
                return BadRequest("Request cannot be null");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrEmpty(request.ClientUrl))
                return BadRequest("Client URL is required");

            if (string.IsNullOrEmpty(request.UserType) || (request.UserType != "Teacher" && request.UserType != "Student"))
                return BadRequest("UserType must be either 'Teacher' or 'Pupil'");

            var user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Phone = request.Phone ?? string.Empty,
                Initials = request.Initials ?? string.Empty               
            };

            using var transaction = await _dBContext.Database.BeginTransactionAsync();

            try
            {
                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(result.Errors);
                }

                IdentityResult roleResult;
                if (request.UserType == "Teacher")
                {
                    _dBContext.Teachers.Add(new Teacher
                    {
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Email = request.Email,
                        Phone = request.Phone ?? string.Empty,
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
                        Phone = request.Phone ?? string.Empty,
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
                var callbackUrl = QueryHelpers.AddQueryString(request.ClientUrl, queryParams);

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
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "An error occurred during registration");
            }
        }


        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequestDto request)
        {
            if (request == null)
                return BadRequest("Request cannot be null");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrEmpty(request.EmailPhone) || string.IsNullOrEmpty(request.Password))
                return BadRequest("Email/Phone and Password are required");

            var user = await _userManager.FindByEmailAsync(request.EmailPhone);
            if (user is null)
                return NotFound("User not found.");

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return Unauthorized("Email not confirmed.");

            if (!await _userManager.CheckPasswordAsync(user, request.Password))
                return Unauthorized("Invalid credentials");

            // Get user roles from UserRole table
            var userRoles = await _userManager.GetUserRolesAsync(user);

            var userData = new ResponseUserDTO
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone ?? string.Empty,
                Roles = userRoles
            };

            // Sign in user with cookie authentication (this will trigger OnSigningIn event)
            await _signInManager.SignInAsync(user, isPersistent: true);

            return Ok(userData);
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPassordDTO forgotPassord)
        {
            if (forgotPassord == null)
                return BadRequest("Request cannot be null");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrEmpty(forgotPassord.Email))
                return BadRequest("Email is required");

            if (string.IsNullOrEmpty(forgotPassord.ClientUrl))
                return BadRequest("Client URL is required");

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
            if (resetPassord == null)
                return BadRequest("Request cannot be null");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrEmpty(resetPassord.Email) || string.IsNullOrEmpty(resetPassord.Token) || string.IsNullOrEmpty(resetPassord.Password))
                return BadRequest("Email, Token, and Password are required");

            var user = await _userManager.FindByEmailAsync(resetPassord.Email);
            if (user is null)
                return NotFound("Invalid Request");

            var result = await _userManager.ResetPasswordAsync(user, resetPassord.Token, resetPassord.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new { Errors = errors });
            }
            return Ok(new { success = true, message = "SUCCESS" });
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
                return BadRequest("Email and token are required");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest(new { success = false, message = "Invalid email address." });

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
                return BadRequest(new { success = false, message = "Invalid or expired token." });
                
            return Ok(new { success = true, message = "Email confirmed successfully." });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            // Sign out user and clear authentication cookie
            await _signInManager.SignOutAsync();
            
            return Ok(new { message = "Logged out successfully" });
        }

        [HttpGet("user-info")]
        public IActionResult GetUserInfo()
        {
            if (!User.Identity?.IsAuthenticated == true)
            {
                return Ok(new { IsAuthenticated = false });
            }

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var firstName = User.FindFirst("FirstName")?.Value;
            var lastName = User.FindFirst("LastName")?.Value;
            var phone = User.FindFirst("Phone")?.Value;
            var roles = User.FindAll(System.Security.Claims.ClaimTypes.Role).Select(c => c.Value).ToList();

            return Ok(new
            {
                IsAuthenticated = true,
                User = new
                {
                    Id = userId,
                    Email = userEmail,
                    FirstName = firstName,
                    LastName = lastName,
                    Phone = phone,
                    Roles = roles
                },
                Claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList()
            });
        }

        [HttpPost("check-email-exist")]
        [RequireRole("Admin", "Teacher")]
        public async Task<IActionResult> CheckEmailExist([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("Email parameter is required");

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Ok(new { isDuplicate = false });

            return Ok(new { isDuplicate = true });
        }

    }
}
