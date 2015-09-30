using System;
using System.Net.Mail;
using SciVacancies.Services.Email.Vendors;

namespace SciVacancies.Services.Email
{
    public class SmtpNotificatorService : ISmtpNotificatorService
    {
        private string p1;
        private string p2;

        //protected string Domain = "localhost:59075";
        //public string PortalLink { get; } = "<a target='_blank' href='http://localhost:59075'>http://localhost:59075</a>";

        public string Domain { get; } = "scivac.test.alt-lan.com";
        public string PortalLink { get; } = "<a target='_blank' href='http://scivac.test.alt-lan.com'>http://scivac.test.alt-lan.com</a>";


        public SmtpNotificatorService()
        {
            p1 = "mailer@alt-lan.com";
            p2 = "123456-mailer";

            if (string.IsNullOrWhiteSpace(p1))
                throw new Exception("Не указан логин для подключения в серверу рассылку email-уведомлений");
            if (string.IsNullOrWhiteSpace(p2))
                throw new Exception("Не указан пароль для подключения в серверу рассылку email-уведомлений");

            //_smtpEmailService = new OutlookEmailService(p1, p2);
        }

        public void Send(MailMessage mailMessage)
        {
            using (var emailService = new GmailEmailService(p1, p2))
            {
                emailService.SendEmail(mailMessage);
            }
        }

    }

    public interface ISmtpNotificatorService
    {
        void Send(MailMessage mailMessage);
        string Domain { get; }
        string PortalLink { get; }
    }
}
