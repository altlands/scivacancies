using SciVacancies.Services.Email.Vendors;

using System;
using System.Net.Mail;
using Microsoft.Framework.Configuration;

namespace SciVacancies.Services.Email
{
    public class SmtpNotificatorService : ISmtpNotificatorService
    {
        private string p1;
        private string p2;

        public string Domain { get; } 
        public string PortalLink { get; }

        public SmtpNotificatorService(IConfiguration configuration)
        {
            p1 = "mailer@alt-lan.com";
            p2 = "123456-mailer";

            var smtpSettings = configuration.Get<SmtpSettings>("SmtpSettings");
            Domain = smtpSettings.Domain;
            PortalLink = smtpSettings.PortalLink;

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
