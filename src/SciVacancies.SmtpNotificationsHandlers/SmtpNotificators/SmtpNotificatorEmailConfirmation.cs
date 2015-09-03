
using SciVacancies.ReadModel.Core;

namespace SciVacancies.SmtpNotificationsHandlers.SmtpNotificators
{
    public class SmtpNotificatorEmailConfirmation : SmtpNotificator
    {
        public void Send(Researcher researcher, string mailTo, string token)
        {
            var researcherFullName = $"{researcher.secondname} {researcher.firstname} {researcher.patronymic}";
            var body = $@"
<div style=''>
    Уважаемый(-ая), {researcherFullName}, 
    <br/>
    для подтверждения указанной электронной почты ({mailTo}) предлагаем перейти по <a target='_blank' href='http://{Domain}/account/confirmemail/?token={token}'> ссылке</a>.
    <br/>
    (Чтобы подтвеждение Email прошло успешно, вы должны быть авторизованы (выполнить вход) на <a target='_blank' href='http://{Domain}'>портале вакансий</a>)
</div>
<br/>
<div style='color: red;'>
    Если вы не отправляли запрос на подтверждение своей электронной почты, то не открывайте указанные ссылки.
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

            Send(new SciVacMailMessage(mailTo, "Подтверждение с портала вакансий", body));
        }
    }
}
