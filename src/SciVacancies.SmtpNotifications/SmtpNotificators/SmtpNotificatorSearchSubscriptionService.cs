using SciVacancies.ReadModel.Core;
using SciVacancies.Services.Email;

using System;
using Microsoft.Extensions.Configuration;

namespace SciVacancies.SmtpNotifications.SmtpNotificators
{
    public class SmtpNotificatorSearchSubscriptionService : ISmtpNotificatorSearchSubscriptionService
    {
        readonly IEmailService EmailService;

        readonly string From;
        readonly string Domain;
        readonly string PortalLink;

        public SmtpNotificatorSearchSubscriptionService(IEmailService emailService, IConfiguration configuration)
        {
            EmailService = emailService;
            From = configuration["EmailSettings:Login"];
            Domain = configuration["EmailSettings:Domain"];
            PortalLink = configuration["EmailSettings:PortalLink"];

            if (string.IsNullOrEmpty(From)) throw new ArgumentNullException("From is null");
            if (string.IsNullOrEmpty(Domain)) throw new ArgumentNullException("Domain is null");
            if (string.IsNullOrEmpty(PortalLink)) throw new ArgumentNullException("PortalLink is null");
        }

        public void SendCreated(Researcher researcher, Guid searchSubscriptionGuid, string title)
        {
            var researcherFullName = $"{researcher.secondname} {researcher.firstname} {researcher.patronymic}";
            var body = $@"
<div style=''>
    Здравствуйте, {researcherFullName}, 
    <br/>
    Сообщаем, что Вы добавили Поисковую подписку '{title}'. 
    <br/>
    <a target='_blank' href='http://{Domain}/researchers/subscriptions/'>Здесь доступно управление подписками</a>.
    <br/>
    <a target='_blank' href='http://{Domain}/subscriptions/details/{searchSubscriptionGuid}'>Обработать</a> подписку (выполнить поиск).
    <br/>
</div>
<br/>

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

            EmailService.Send(new SciVacMailMessage(From, researcher.email, "Уведомление с портала вакансий", body));
        }
    }

}
