using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace SellingBook.Services.Email
{
    public class EmailSender : IEmailSender
    {
        // Các thông số cấu hình lấy từ appsettings.json
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPassword;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailSender(IConfiguration configuration)
        {
            // Cấu hình thông qua appsettings.json
            _smtpHost = configuration["EmailSettings:SMTPHost"];
            _smtpPort = int.Parse(configuration["EmailSettings:SMTPPort"]);
            _smtpUser = configuration["EmailSettings:SMTPUser"];
            _smtpPassword = configuration["EmailSettings:SMTPPassword"];
            _fromEmail = configuration["EmailSettings:FromEmail"];
            _fromName = configuration["EmailSettings:FromName"];
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            using (var client = new SmtpClient(_smtpHost, _smtpPort))
            {
                // Thiết lập thông tin đăng nhập SMTP
                client.Credentials = new NetworkCredential(_smtpUser, _smtpPassword);
                client.EnableSsl = true; // Sử dụng SSL để bảo mật

                // Tạo email
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_fromEmail, _fromName),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true // Cấu hình nội dung email là HTML
                };
                mailMessage.To.Add(toEmail); // Thêm email người nhận

                // Gửi email
                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
