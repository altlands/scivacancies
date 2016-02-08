using SciVacancies.ReadModel.Core;
using SciVacancies.Services.Email;

using Microsoft.Extensions.Configuration;
using System;
using Microsoft.Extensions.Logging;

namespace SciVacancies.SmtpNotifications.SmtpNotificators
{
    public class SmtpNotificatorAccountService : ISmtpNotificatorAccountService
    {
        readonly IEmailService EmailService;

        private readonly string From;
        private readonly string Domain;
        private readonly string PortalLink;
        private readonly ILogger _logger;

        public SmtpNotificatorAccountService(IEmailService emailService, IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            EmailService = emailService;
            _logger = loggerFactory.CreateLogger<SmtpNotificatorAccountService>();

            From = configuration["EmailSettings:Login"];
            Domain = configuration["EmailSettings:Domain"];
            PortalLink = configuration["EmailSettings:PortalLink"];

            if (string.IsNullOrEmpty(From)) throw new ArgumentNullException("From is null");
            if (string.IsNullOrEmpty(Domain)) throw new ArgumentNullException("Domain is null");
            if (string.IsNullOrEmpty(PortalLink)) throw new ArgumentNullException("PortalLink is null");
        }

        public void SendPasswordRestore(Researcher researcher, string username, string mailTo, string token)
        {
            var researcherFullName = $"{researcher.secondname} {researcher.firstname} {researcher.patronymic}";
            var body = $@"
<div style=''>

    Здравствуйте, {researcherFullName}
    <br/>
    Был получен ваш запрос на восстановление доступа к порталу {PortalLink}.
    <br/>
    Ваш логин: {username}
    <br/>
    Для создания нового пароля для вашей учетной записи вам необходимо пройти по <a target='_blank' href='http://{Domain}/account/restorepassword/?username={username}&token={token}'> ссылке</a>
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

            EmailService.Send(new SciVacMailMessage(From, mailTo, "Сброс пароля для портала вакансий", body));
        }



        public void SendUserActivation(Researcher researcher, string username, string mailTo, string token)
        {
            var researcherFullName = $"{researcher.secondname} {researcher.firstname} {researcher.patronymic}";
            var body = $@"
<div style=''>
    Уважаемый(-ая), {researcherFullName}, 
    <br/>
    для активации вашей учётной записи перейдите, пожалуйста, по <a target='_blank' href='http://{Domain}/account/activateaccount/?username={username}&token={token}'> ссылке</a>. 
    Не забудьте выполнить вход на сайте 
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
    <a target='_blank' href='http://{Domain}'>Портала вакансий</a>.
    Чтобы не получать такие уведомления отключите их или смените email в 
    <a target='_blank' href='http://{Domain}/researchers/account/'>личном кабинете</a>.
</div>
";

            EmailService.Send(new SciVacMailMessage(From, mailTo, "Подтверждение с портала вакансий", body));
        }



        public void SendUserRegistered(string fullName, string mailTo, string login)
        {
            var body =
                $@"
<div style=''>
    Здравствуйте, {fullName}
    <br/>
    Вы успешно прошли регистрацию на портале {PortalLink}
    <br/>
    Ваш логин: {login}
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

            EmailService.Send(new SciVacMailMessage(From, mailTo, "Вы зарегистрированы на портале вакансий", body));
        }


    }

}
