using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Threading.Tasks;
using api.Dto.EmailDto;
using api.Interfaces;

namespace api.repository
{
    public class EmailSender : IEmailRepository
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = htmlMessage };

            using var client = new SmtpClient();
            try
            {
                // Connect to the SMTP server
                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);

                // Authenticate
                await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.Password);

                // Send the email
                await client.SendAsync(message);
            }
            catch (Exception ex)
            {
                // Log the exception (you may want to use a logging framework)
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw; // Rethrow the exception for higher-level handling
            }
            finally
            {
                // Disconnect from the server
                await client.DisconnectAsync(true);
            }
        }
    }
}