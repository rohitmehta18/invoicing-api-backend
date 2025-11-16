
using MailKit.Net.Smtp;
using MimeKit;
using Invoicing.Api.Models;

namespace Invoicing.Api.Services
{
    public class EmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IConfiguration config)
        {
            _settings = config.GetSection("EmailSettings").Get<EmailSettings>();
        }

        public async Task SendInvoiceEmail(string toEmail, string subject, string htmlBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Invoicing System", _settings.From));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = htmlBody
            };

            message.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.Host, _settings.Port, false);
            await smtp.AuthenticateAsync(_settings.Username, _settings.Password);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
    }
}
