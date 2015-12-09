using System.Net.Mail;

namespace SciVacancies.SmtpNotifications
{
    public class SciVacMailMessage : MailMessage
    {
        public SciVacMailMessage(string from, string mailTo, string subject, string body) : base(new MailAddress(from), new MailAddress(mailTo))
        {
            Subject = subject;
            IsBodyHtml = true;
            Body = body;
        }

    }
}
