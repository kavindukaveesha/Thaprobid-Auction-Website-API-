using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;

namespace api.repository
{
    public class EmailSender : IEmailSender
    {
        private readonly string _emailAddress;
        private readonly string _emailPassword;
        private readonly string _smtpServer;
        private readonly int _smtpPort;

        public EmailSender(IConfiguration configuration)
        {
            _emailAddress = configuration["EmailSettings:EmailAddress"];
            _emailPassword = configuration["EmailSettings:EmailPassword"];
            _smtpServer = configuration["EmailSettings:SmtpServer"];
            _smtpPort = int.Parse(configuration["EmailSettings:SmtpPort"]);
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var fromAddress = new MailAddress(_emailAddress, "Your App Name");
            var toAddress = new MailAddress(email);

            using (var smtp = new SmtpClient
            {
                Host = _smtpServer,
                Port = _smtpPort,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, _emailPassword)
            })
            {
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
    }
}