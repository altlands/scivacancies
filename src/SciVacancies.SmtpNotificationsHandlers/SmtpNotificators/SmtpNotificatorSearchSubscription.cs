using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using SciVacancies.ReadModel.Core;

namespace SciVacancies.SmtpNotificationsHandlers.SmtpNotificators
{
    public class SmtpNotificatorSearchSubscription : SmtpNotificator
    {
        public void Send(SearchSubscription searchSubscription, Researcher researcher, List<SciVacancies.ReadModel.ElasticSearchModel.Model.Vacancy> vacancies)
        {
            //var domain = "localhost:59075";
            var domain = "scivac.test.alt-lan.com";
            var researcherFullName = $"{researcher.secondname} {researcher.firstname} {researcher.patronymic}";
            var body = $@"
<div style=''>
    Уважаемый(-ая), {researcherFullName}, по одной из ваших
    <a target='_blank' href='http://{domain}/researcher/subscriptions/'>подписок</a>
     ('{searchSubscription.title}') подобраны следующие вакансии: <br/>
    {vacancies.Aggregate(string.Empty, (current, vacancy) => current + $"<a target='_blank' href='http://{domain}/vacancies/card/{vacancy.Id}'>{vacancy.FullName}</a> <br/>")}
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

            var mailMessage = new MailMessage(@from: "mailer@alt-lan.com", to: researcher.email, body: body, subject: "Уведомление с портала вакансий")
            {
                IsBodyHtml = true,
            };

            Send(mailMessage);
            if (!string.IsNullOrWhiteSpace(researcher.extraemail))
            {
                mailMessage = new MailMessage(@from: "mailer@alt-lan.com", to: researcher.extraemail, body: body, subject: "Уведомление с портала вакансий")
                {
                    IsBodyHtml = true,
                };

                Send(mailMessage);
            }
        }
     
    }
}
