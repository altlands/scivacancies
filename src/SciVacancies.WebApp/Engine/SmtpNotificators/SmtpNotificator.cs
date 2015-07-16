using System.Net.Mail;
using SciVacancies.SmtpNotifications.Vendors;

namespace SciVacancies.WebApp.Engine.SmtpNotificators
{
    public class SmtpNotificator
    {
        private readonly GmailEmailService _gmailEmailService;

        public SmtpNotificator()
        {
            _gmailEmailService = new GmailEmailService("mailer@alt-lan.com", "");
        }

        protected void Send(MailMessage mailMessage)
        {
            _gmailEmailService.SendEmail(mailMessage);
        }

    }
}
