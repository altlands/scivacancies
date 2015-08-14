using System;
using System.Net.Mail;
using SciVacancies.SmtpNotifications;
using SciVacancies.SmtpNotifications.Vendors;

namespace SciVacancies.SmtpNotificationsHandlers.SmtpNotificators
{
    public class SmtpNotificator
    {
        private readonly SmtpEmailService _smtpEmailService;
        //protected string Domain = "localhost:59075";
        protected string Domain = "scivac.test.alt-lan.com";


        public SmtpNotificator()
        {
            var p1 = "mailer@alt-lan.com";
            var p2 = "123456-mailer";

            if (string.IsNullOrWhiteSpace(p1))
                throw new Exception("Не указан логин для подключения в серверу рассылку email-уведомлений");
            if (string.IsNullOrWhiteSpace(p2))
                throw new Exception("Не указан пароль для подключения в серверу рассылку email-уведомлений");

            _smtpEmailService = new GmailEmailService(p1, p2);
            //_smtpEmailService = new OutlookEmailService(p1, p2);
        }

        protected void Send(MailMessage mailMessage)
        {
            _smtpEmailService.SendEmail(mailMessage);
        }

    }
}
