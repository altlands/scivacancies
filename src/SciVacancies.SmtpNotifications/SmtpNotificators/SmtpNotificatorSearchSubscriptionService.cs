using System.Collections.Generic;
using System.Linq;
using SciVacancies.ReadModel.Core;
using SciVacancies.Services;
using SciVacancies.Services.Email;

namespace SciVacancies.SmtpNotifications.SmtpNotificators
{
    public class SmtpNotificatorSearchSubscriptionService : ISmtpNotificatorSearchSubscriptionService
    {

        private readonly ISmtpNotificatorService _smtpNotificatorService;

        public SmtpNotificatorSearchSubscriptionService(ISmtpNotificatorService smtpNotificatorService)
        {
            _smtpNotificatorService = smtpNotificatorService;
        }
        //TODO: ntemnikov: move body generation to Services project
        public void Notify(SearchSubscription searchSubscription, Researcher researcher, List<SciVacancies.ReadModel.ElasticSearchModel.Model.Vacancy> vacancies)
        {
            var researcherFullName = $"{researcher.secondname} {researcher.firstname} {researcher.patronymic}";
            var body = $@"
<div style=''>
    Уважаемый(-ая), {researcherFullName}, по одной из ваших
    <a target='_blank' href='http://{_smtpNotificatorService.Domain}/researcher/subscriptions/'>подписок</a>
     ('{searchSubscription.title}') подобраны следующие вакансии: <br/>
    {vacancies.Aggregate(string.Empty, (current, vacancy) => current + $"<a target='_blank' href='http://{_smtpNotificatorService.Domain}/vacancies/card/{vacancy.Id}'>{vacancy.FullName}</a> <br/>")}
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

            _smtpNotificatorService.Send(new SciVacMailMessage(researcher.email, "Уведомление с портала вакансий", body));
        }
     
    }
}
