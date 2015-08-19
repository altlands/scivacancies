using System.Net.Mail;

namespace SciVacancies.SmtpNotificationsHandlers.SmtpNotificators
{
    public class SmtpNotificatorUserRegistered : SmtpNotificator
    {
        public void Send(string fullName, string mailTo, string extraMailTo)
        {
            var body =
                $@"
<div style=''>
    Уважаемый(-ая), {fullName}, Вы зарегистрированы на портале Вакансий.
    <br/>
</div>

<br/>
<br/>
<hr/>

<div style='color: lightgray; font-size: smaller;'>
    Это письмо создано автоматически с 
    <a target='_blank' href='http://{Domain}'>Портала вакансий</a>.
    Чтобы не получать такие уведомления отключите их или смените email в 
    <a target='_blank' href='http://{Domain}/researchers/account/'>личном кабинете</a>.
</div>
";
            var mailToAddress = string.IsNullOrWhiteSpace(extraMailTo) ? new MailAddress(mailTo) : new MailAddress(mailTo, extraMailTo);

            var mailMessage = new MailMessage(from: new MailAddress("mailer@alt-lan.com"), to: mailToAddress)
            {
                Body = body,
                Subject = "Вы зарегистрированы на портале вакансий",
                IsBodyHtml = true
            };

            Send(mailMessage);
        }
    }
}
