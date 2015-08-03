using System.Net.Mail;

namespace SciVacancies.WebApp.Engine.SmtpNotificators
{
    internal class RegisterSmtpNotificator : SmtpNotificator
    {
        public void Send(string researcherFullName, string login, string password)
        {

            var domain = "localhost:59075";

            var body = $@"
<div style=''>
    Уважаемый(-ая), {researcherFullName}, Вы зарегистрированы на портале Вакансий.
    <br/>
    Ваш логин для входа в систему: {login}
    <br/>
    Пароль: {password}
</div>

<br/>
<br/>
<hr/>

<div style='color: lightgray; font-size: smaller;'>
    Это письмо создано автоматически с 
    <a target='_blank' href='http://{domain}'>Портала вакансий</a>.
    Чтобы не получать такие уведомления отключите их или смените email в 
    <a target='_blank' href='http://{domain}/researchers/account/'>личном кабинете</a>.
</div>
";
            var mailMessage = new MailMessage(from: "mailer@alt-lan.com", to: "mail@mail.ru", body: body, subject: "Вы зарегистрированы на портале вакансий")
            {
                IsBodyHtml = true
            };

            Send(mailMessage);
        }
    }
}
