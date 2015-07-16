using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using SciVacancies.ReadModel.Core;
using Vacancy = SciVacancies.ReadModel.ElasticSearchModel.Model.Vacancy;

namespace SciVacancies.WebApp.Engine.SmtpNotificators
{
    public class WinnerSetSmtpNotificator : SmtpNotificator
    {
        public void Send(SearchSubscription searchSubscription, Researcher researcher, List<Vacancy> vacancies)
        {

            var domain = "localhost:59075";
            var researcherFullName = $"{researcher.secondname} {researcher.firstname} {researcher.patronymic}";
            var applicationGuid = Guid.NewGuid();

            var body = $@"
<div style=''>
    Уважаемый(-ая), {researcherFullName}, по вашей
    <a target='_blank' href='http://{domain}/researcher/subscriptions/{applicationGuid}'>подписке</a>
    подобраны следующие вакансии: <br/>
    {vacancies.Aggregate(string.Empty, (current, vacancy) => current + $"<a target='_blank' href='http://{domain}/vacancies/details/{vacancy.Id}'>{vacancy.FullName}</a> <br/>")}
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
            var mailMessage = new MailMessage(from: "mailer@alt-lan.com", to: "nistoc@gmail.com", body: body, subject: "Уведомление с портала вакансий")
            {
                IsBodyHtml = true,

            };


            Send(mailMessage);

        }
    }
}
