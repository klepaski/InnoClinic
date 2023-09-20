using ProfilesAPI.Models;
using System.Net.Mail;
using System.Net;

namespace ProfilesAPI.Services
{
    public interface IEmailService
    {
        public Task SendCredentialsToEmail(string email, string pw);
    }

    public class EmailService : IEmailService
    {
        private readonly ProfilesDbContext _db;
        private const string SENDER_EMAIL = "help.medgift@mail.ru";
        private const string PASSWORD = "NXqQyh2QWhkByQmBJqa5"; //Cvthnmyfgj7
        private const int PORT = 587;

        public EmailService(ProfilesDbContext db)
        {
            _db = db;
        }

        public async Task SendCredentialsToEmail(string email, string pw)
        {
            MailAddress from = new MailAddress(SENDER_EMAIL, "InnoClinic");
            MailAddress to = new MailAddress(email);
            MailMessage m = new MailMessage(from, to);
            m.Subject = "InnoClinic account confirmation";
            m.IsBodyHtml = true;
            m.Body = $"<h3>Hello, dear doctor!</h3>" +
                    $"<p>Congratulations! Now you have an account.<br>" +
                    $"Your credentials:</p>" +
                    $"<h3>email: {email}<br/>" +
                    $"password: {pw}</h3>" +
                    $"Thanks!";
            SmtpClient smtp = new SmtpClient("smtp.mail.ru", PORT);
            smtp.Credentials = new NetworkCredential(SENDER_EMAIL, PASSWORD);
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);
            return;
        }
    }
}
