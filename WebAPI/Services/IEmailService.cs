
using WebAPI.Models;

namespace WebAPI.Services
{
    public interface IEmailService
    {
        Task Send(EmailMetadata metadata);
    }
}
