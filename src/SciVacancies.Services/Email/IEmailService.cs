using System.Net.Mail;

namespace SciVacancies.Services.Email
{
    public interface IEmailService
    {
        void Send(MailMessage message);
    }
}