using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmailController : Controller
{
    private readonly IEmailService _emailService;
    private readonly ILogger<EmailController> _logger;

    public EmailController(IEmailService emailService, ILogger<EmailController> logger)
    {
        _emailService = emailService;
    }

    [HttpGet("singleemail")]
    public async Task<IActionResult> sendSingleEmail()
    {
        EmailMetadata emailMetadata = new EmailMetadata("tigransargsyan998@gmail.com",
            "Fluent email test",
            "This is a test email from FluentEmail",
            null
            );
        await _emailService.Send(emailMetadata);

        return Ok("Email sent successfully");
    }
}

