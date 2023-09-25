using System.Net.Mail;
using System.Net;
using ProfilesAPI.Context;
using JuliaChistyakovaPackage;

namespace ProfilesAPI.Services
{
    public interface IEmailService
    {
        public Task SendCredentialsToEmail(string email, string pw);
        public Task SendConfirmationLink(string email, int accountId);
    }

    public class EmailService : IEmailService
    {
        private const string SENDER_EMAIL = "help.medgift@mail.ru";
        private const string PASSWORD = "NXqQyh2QWhkByQmBJqa5"; //Cvthnmyfgj7
        private const int PORT = 587;
        
        public async Task SendCredentialsToEmail(string email, string pw)
        {
            MailAddress from = new MailAddress(SENDER_EMAIL, "InnoClinic");
            MailAddress to = new MailAddress(email);
            MailMessage m = new MailMessage(from, to);
            m.Subject = "InnoClinic account confirmation";
            m.IsBodyHtml = true;
            m.Body = $"<h3>Hello, dear user!</h3>" +
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

        public async Task SendConfirmationLink(string email, int accountId)
        {
            MailAddress from = new MailAddress(SENDER_EMAIL, "InnoClinic");
            MailAddress to = new MailAddress(email);
            MailMessage m = new MailMessage(from, to);
            m.Subject = "InnoClinic account confirmation";
            m.IsBodyHtml = true;
            m.Body = $"<h3>Hello, dear user!</h3>" +
                    $"<p>Congratulations! Now you have an account.<br>" +
                    $"To confirm your email follow this :" +
                    $"<a target=\"_self\" href=\"{Ports.ProfilesAPI}/Account/ConfirmEmail/{accountId}\">link</a></p>" +
                    $"Thanks!";
            SmtpClient smtp = new SmtpClient("smtp.mail.ru", PORT);
            smtp.Credentials = new NetworkCredential(SENDER_EMAIL, PASSWORD);
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);
            return;
        }
    }
}
