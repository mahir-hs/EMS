using api.Services.IServices;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace api.Services
{
    public class EmailService(ILogger<EmailService> logger, IConfiguration config) : IEmailService
    {
        private readonly ILogger<EmailService> _logger= logger;
        private readonly IConfiguration _config = config;
        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress("Employee Management System", "mahirhasan333@gmail.com"));
                emailMessage.To.Add(new MailboxAddress("Recipient", toEmail));
                emailMessage.Subject = subject;

                var bodyBuilder = new BodyBuilder { HtmlBody = body };
                emailMessage.Body = bodyBuilder.ToMessageBody();

                var smtpHost = _config["Mailtrap:Host"];
                var smtpPort = int.Parse(_config["Mailtrap:Port"]);
                var smtpUsername = _config["Mailtrap:Username"];
                var smtpPassword = _config["Mailtrap:Password"];

                using var smtp = new SmtpClient();
                try
                {
                    await smtp.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);
                    await smtp.AuthenticateAsync(smtpUsername, smtpPassword);
                    await smtp.SendAsync(emailMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email: {ex.Message}");
                }
                finally
                {
                    await smtp.DisconnectAsync(true);
                }

                _logger.LogInformation("Email sent to {ToEmail}", toEmail);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {ToEmail}", toEmail);
                return false;
            }
        }
    }
}
