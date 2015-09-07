using System.Net.Mail;

namespace SciVacancies.SmtpNotificationsHandlers
{
    public class SciVacMailMessage : MailMessage
    {
        public SciVacMailMessage(string mailTo, string subject,string body) : base(from: new MailAddress("mailer@alt-lan.com"), to: new MailAddress(mailTo))
        {
            Subject = subject;
            IsBodyHtml = true;
            Body = body;
        }

        public SciVacMailMessage(string mailTo, string extraMailTo, string subject, string body) : base(from: new MailAddress("mailer@alt-lan.com"), to: new MailAddress(mailTo, extraMailTo))
        {
            Subject = subject;
            IsBodyHtml = true;
            Body = body;
        }
    }
}
