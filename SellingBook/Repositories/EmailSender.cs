using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace SellingBook.Repositories
{
    public class EmailSender : IEmailSender
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailSender(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string Message)
        {
            try
            {
                using (var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
                {
                    client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                    client.EnableSsl = true;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_smtpSettings.FromEmail, _smtpSettings.FromName),
                        Subject = subject,
                        Body = Message, 
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(email);

                    try
                    {
                        await client.SendMailAsync(mailMessage);

                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException("Failed to send email.", ex);
                    }
                }
            }
            catch (SmtpException smtpEx)
            {
                throw new InvalidOperationException("SMTP error occurred while sending email.", smtpEx);
            }
        }


    }
}
