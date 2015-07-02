namespace SciVacancies.SmtpNotifications.Vendors
{
    public class GmailEmailService : SmtpEmailService
    {
        public GmailEmailService(string login, string password)
            : base(login, password, "smtp.gmail.com", 587, true, false)
        {
        }
    }
}
