namespace WebAPI.Extensions
{
    public static class FluentEmailExtensions
    {
        public static void AddFluentEmail(this IServiceCollection services, ConfigurationManager configuration)
        {
            var emailSettings = configuration.GetSection("EmailSettings");
            var defaultFromEmail = emailSettings["DefaultFromEmail"];
            var host = emailSettings["SMTPSetting:Host"];
            var port = emailSettings.GetValue<int>("SMTPSetting:Port");

            if (string.IsNullOrEmpty(defaultFromEmail) || string.IsNullOrEmpty(host))
            {
                throw new InvalidOperationException("Email configuration is incomplete. Please check DefaultFromEmail and SMTPSetting:Host in appsettings.json");
            }

            services.AddFluentEmail(defaultFromEmail)
                .AddSmtpSender(host, port);
        }
    }
}
