using SciVacancies.ReadModel.Core;
using SciVacancies.Services;
using SciVacancies.Services.Email;

namespace SciVacancies.SmtpNotifications.SmtpNotificators
{
    public class SmtpNotificatorAccountService : ISmtpNotificatorAccountService
    {
        private readonly ISmtpNotificatorService _smtpNotificatorService;

        public SmtpNotificatorAccountService(ISmtpNotificatorService smtpNotificatorService)
        {
            _smtpNotificatorService = smtpNotificatorService;
        }

        public void SendPasswordRestore(Researcher researcher, string username, string mailTo, string token)
        {
            var researcherFullName = $"{researcher.secondname} {researcher.firstname} {researcher.patronymic}";
            var body = $@"
<div style=''>
    Уважаемый(-ая), {researcherFullName}, 
    <br/>
    для сброса пароля перейдите, пожалуйста, по <a target='_blank' href='http://{_smtpNotificatorService.Domain}/account/restorepassword/?username={username}&token={token}'> ссылке</a>.
    <br/>
</div>
<br/>
<div style='color: red;'>
    Если вы не отправляли запрос на сброс пароля, то не открывайте указанные ссылки.
</div>

<br/>
<br/>
<hr/>

<div style='color: lightgray; font-size: smaller;'>
    Это письмо создано автоматически с 
    <a target='_blank' href='http://{_smtpNotificatorService.Domain}'>Портала вакансий</a>.
    Чтобы не получать такие уведомления отключите их или смените email в 
    <a target='_blank' href='http://{_smtpNotificatorService.Domain}/researchers/account/'>личном кабинете</a>.
</div>
";

            _smtpNotificatorService.Send(new SciVacMailMessage(mailTo, "Сброс пароля для портала вакансий", body));
        }



        public void SendUserActivation(Researcher researcher, string username, string mailTo, string token)
        {
            var researcherFullName = $"{researcher.secondname} {researcher.firstname} {researcher.patronymic}";
            var body = $@"
<div style=''>
    Уважаемый(-ая), {researcherFullName}, 
    <br/>
    для активации вашей учётной записи перейдите, пожалуйста, по <a target='_blank' href='http://{_smtpNotificatorService.Domain}/account/activateaccount/?username={username}&token={token}'> ссылке</a>.
    <br/>
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
    <a target='_blank' href='http://{_smtpNotificatorService.Domain}'>Портала вакансий</a>.
    Чтобы не получать такие уведомления отключите их или смените email в 
    <a target='_blank' href='http://{_smtpNotificatorService.Domain}/researchers/account/'>личном кабинете</a>.
</div>
";

            _smtpNotificatorService.Send(new SciVacMailMessage(mailTo, "Подтверждение с портала вакансий", body));
        }



        public void SendUserRegistered(string fullName, string mailTo, string login)
        {
            var body =
                $@"
<div style=''>
    Здравствуйте, {fullName}
    <br/>
    Вы успешно прошли регистрацию на портале http://scivac.ru/
    <br/>
    Ваш логин: {login}
</div>

<br/>
<br/>
<hr/>

<div style='color: lightgray; font-size: smaller;'>
    Это письмо создано автоматически с 
    <a target='_blank' href='http://{_smtpNotificatorService.Domain}'>Портала вакансий</a>.
    Чтобы не получать такие уведомления отключите их или смените email в 
    <a target='_blank' href='http://{_smtpNotificatorService.Domain}/researchers/account/'>личном кабинете</a>.
</div>
";

            _smtpNotificatorService.Send(new SciVacMailMessage(mailTo, "Вы зарегистрированы на портале вакансий", body));
        }


    }

}
