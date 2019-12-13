using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Dex.Infrastructure.Contracts.IServices;

namespace Dex.Infrastructure.Implementations.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;

        public EmailService(SmtpClient smtpClient)
        {
            _smtpClient = smtpClient ?? throw new ArgumentNullException(nameof(smtpClient));
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            if (string.IsNullOrEmpty(email)) { throw new ArgumentNullException(nameof(email));}
            if (string.IsNullOrEmpty(subject)) { throw new ArgumentNullException(nameof(subject));}

            return Task.Run(() =>
            {
                var credentials = (NetworkCredential) _smtpClient.Credentials;
                var message = BuildMessage(credentials.UserName, email, subject, htmlMessage);
                _smtpClient.Send(message);
            });
        }

        private MailMessage BuildMessage(string fromEmail, string toEmail, string subject, string htmlMessage)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = subject,
                IsBodyHtml = true,
                Body = htmlMessage
            };

            mailMessage.To.Add(toEmail);
            return mailMessage;
        }
    }
}
