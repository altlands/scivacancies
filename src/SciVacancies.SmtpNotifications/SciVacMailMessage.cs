using System;
using System.Net.Mail;
using Microsoft.Framework.Configuration;

namespace SciVacancies.SmtpNotifications
{
    public class SciVacMailMessage : MailMessage
    {
        public SciVacMailMessage(string from, string mailTo, string subject, string body) : base(@from: new MailAddress(from), to: new MailAddress(mailTo))
        {
            Subject = subject;
            IsBodyHtml = true;
            Body = body;
        }

    }
}
