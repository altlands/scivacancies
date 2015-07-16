using System;
using System.Net.Mail;
using SciVacancies.SmtpNotifications.Vendors;

namespace SciVacancies.WebApp.Engine.SmtpNotificators
{
    public class SmtpNotificator
    {
        private readonly GmailEmailService _gmailEmailService;

        public SmtpNotificator()
        {
            var p1 = "mailer@alt-lan.com";
            var p2 = "";
            if (string.IsNullOrWhiteSpace(p1))
                throw new Exception("Не указан логин для подключения в серверу рассылку email-уведомлений");
            if (string.IsNullOrWhiteSpace(p2))
                throw new Exception("Не указан пароль для подключения в серверу рассылку email-уведомлений");

            _gmailEmailService = new GmailEmailService(p1,p2 );
        }

        protected void Send(MailMessage mailMessage)
        {
            _gmailEmailService.SendEmail(mailMessage);
        }

    }
}
