using FluentEmail.Core;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class EmailService: IEmailService
    {
        private readonly IFluentEmail _fluentEmail;

        public EmailService(IFluentEmail fluentEmail)
        {
            _fluentEmail = fluentEmail;
        }
        public async Task Send(EmailMetadata metadata)
        {
           await _fluentEmail.To(metadata.ToAddress)
                .Subject(metadata.Subject)
                .Body(metadata.Body)
                .SendAsync();
        }
    }
}
