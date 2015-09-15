using System.Net.Mail;

namespace SciVacancies.Services.Email
{
    public interface IEmailService
    {
        void SendEmail(MailMessage message);
    }
}