using System;
using FluentEmail.Core;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class EmailService: IEmailService
    {
        private readonly IFluentEmail _fluentEmail;

        public EmailService(IFluentEmail fluentEmail)
        {
            _fluentEmail = fluentEmail ?? throw new ArgumentNullException(nameof(fluentEmail));
        }
        
        public async Task Send(EmailMetadata metadata)
        {
            if (metadata == null)
                throw new ArgumentNullException(nameof(metadata));

            if (string.IsNullOrEmpty(metadata.ToAddress))
                throw new ArgumentException("ToAddress cannot be null or empty", nameof(metadata));

            if (string.IsNullOrEmpty(metadata.Subject))
                throw new ArgumentException("Subject cannot be null or empty", nameof(metadata));

            try
            {
                await _fluentEmail.To(metadata.ToAddress)
                    .Subject(metadata.Subject)
                    .Body(metadata.Body)
                    .SendAsync();
            }
            catch (Exception ex)
            {
                // Log the error here if you have logging configured
                throw new InvalidOperationException($"Failed to send email to {metadata.ToAddress}", ex);
            }
        }
    }
}
