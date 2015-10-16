using SciVacancies.ReadModel.Core;
using SciVacancies.Services.Email;

using System;

namespace SciVacancies.SmtpNotifications.SmtpNotificators
{
    public class SmtpNotificatorSearchSubscriptionService : ISmtpNotificatorSearchSubscriptionService
    {
        private readonly ISmtpNotificatorService _smtpNotificatorService;

        public SmtpNotificatorSearchSubscriptionService(ISmtpNotificatorService smtpNotificatorService)
        {
            _smtpNotificatorService = smtpNotificatorService;
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
    <a target='_blank' href='http://{_smtpNotificatorService.Domain}/researchers/subscriptions/'>Здесь доступно управление подписками</a>.
    <br/>
    <a target='_blank' href='http://{_smtpNotificatorService.Domain}/subscriptions/details/{searchSubscriptionGuid}'>Обработать</a> подписку (выполнить поиск).
    <br/>
</div>
<br/>

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

            _smtpNotificatorService.Send(new SciVacMailMessage(researcher.email, "Уведомление с портала вакансий", body));
        }
    }

}
