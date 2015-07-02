using System.Net.Mail;

namespace SciVacancies.SmtpNotifications
{
    public interface IEmailService
    {
        void SendEmail(MailMessage message);
    }
}