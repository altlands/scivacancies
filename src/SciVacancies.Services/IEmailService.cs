using System.Net.Mail;

namespace SciVacancies.Services
{
    public interface IEmailService
    {
        void SendEmail(MailMessage message);
    }
}