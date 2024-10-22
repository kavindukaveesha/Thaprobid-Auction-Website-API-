using api.Dto.EmailDto;
using api.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging; // For logging
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Threading.Tasks;

public class EmailService : IEmailRepository
{
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<EmailService> _logger; // Inject ILogger

    public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
    {
        _emailSettings = emailSettings.Value;
        _logger = logger; // Initialize the logger
    }

    public async Task SendEmailAsync(EmailConfigDto emailConfigDto)
    {
        try
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_emailSettings.Username));
            email.To.Add(MailboxAddress.Parse(emailConfigDto.RecieverEmail));
            email.Subject = emailConfigDto.Subject;
            email.Body = new TextPart("html") { Text = emailConfigDto.Body };

            using (var client = new SmtpClient())
            {
                client.Connect(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
                client.Authenticate(_emailSettings.Username, _emailSettings.Password);
                await client.SendAsync(email); // Use await here
                await client.DisconnectAsync(true); // Use await here
            }

            _logger.LogInformation($"Email sent successfully to: {emailConfigDto.RecieverEmail}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error sending email to {emailConfigDto.RecieverEmail}: {ex.Message}");
            // Consider re-throwing the exception or handling it based on your application's needs. 
        }
    }
}