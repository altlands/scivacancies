
namespace SciVacancies.SmtpNotificationsHandlers.SmtpNotificators
{
    /// <summary>
    /// регистрация пользователя на протале
    /// </summary>
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

            Send(new SciVacMailMessage(mailTo, extraMailTo, "Вы зарегистрированы на портале вакансий", body));
        }
    }
}
