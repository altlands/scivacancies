namespace SciVacancies.Services.Email.Vendors
{
    public class OutlookEmailService : SmtpEmailService
    {
        public OutlookEmailService(string login, string password)
            : base(login, password, "smtp-mail.outlook.com", 587, true, false)
        {
        }
    }
}