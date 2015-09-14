using System;
using System.Net.Mail;
using SciVacancies.Services.Email.Vendors;

namespace SciVacancies.Services.Email
{
    public class SmtpNotificatorService: ISmtpNotificatorService
    {
        private readonly SmtpEmailService _smtpEmailService;
        //protected string Domain = "localhost:59075";
        public string Domain { get; } = "scivac.test.alt-lan.com";


        public SmtpNotificatorService()
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

        public void Send(MailMessage mailMessage)
        {
            _smtpEmailService.SendEmail(mailMessage);
        }

    }

    public interface ISmtpNotificatorService
    {
        void Send(MailMessage mailMessage);
        string Domain { get; }
    }
}
