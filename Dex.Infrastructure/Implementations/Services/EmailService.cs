using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Dex.Infrastructure.Contracts.IServices;
using Microsoft.Extensions.Configuration;

namespace Dex.Infrastructure.Implementations.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            if (string.IsNullOrEmpty(email)) { throw new ArgumentNullException(nameof(email));}
            if (string.IsNullOrEmpty(subject)) { throw new ArgumentNullException(nameof(subject));}

            return Task.Run(() =>
            {
                using var client = BuildClient();
                var message = BuildMessage(email, subject, htmlMessage);
                client.Send(message);
            });
        }

        private SmtpClient BuildClient()
        {
            var email = _configuration.GetSection("Data").GetSection("Email").Value;
            var password = _configuration.GetSection("Data").GetSection("Password").Value;

            return new SmtpClient("smtp.gmail.com", 587)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(email, password),
                EnableSsl = true
            };
        }

        private MailMessage BuildMessage(string toEmail, string subject, string htmlMessage)
        {
            var email = _configuration.GetSection("Data").GetSection("Email").Value;

            var mailMessage = new MailMessage
            {
                From = new MailAddress(email),
                Subject = subject,
                IsBodyHtml = true,
                Body = htmlMessage
            };

            mailMessage.To.Add(toEmail);
            return mailMessage;
        }
    }
}
