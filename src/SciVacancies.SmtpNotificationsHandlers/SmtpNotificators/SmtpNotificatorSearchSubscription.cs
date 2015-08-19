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
            var researcherFullName = $"{researcher.secondname} {researcher.firstname} {researcher.patronymic}";
            var body = $@"
<div style=''>
    Уважаемый(-ая), {researcherFullName}, по одной из ваших
    <a target='_blank' href='http://{Domain}/researcher/subscriptions/'>подписок</a>
     ('{searchSubscription.title}') подобраны следующие вакансии: <br/>
    {vacancies.Aggregate(string.Empty, (current, vacancy) => current + $"<a target='_blank' href='http://{Domain}/vacancies/card/{vacancy.Id}'>{vacancy.FullName}</a> <br/>")}
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

            var mailToAddress = string.IsNullOrWhiteSpace(researcher.extraemail) ? new MailAddress(researcher.email) : new MailAddress(researcher.email, researcher.extraemail);

            var mailMessage = new MailMessage(from: new MailAddress("mailer@alt-lan.com"), to: mailToAddress)
            {
                Body = body,
                Subject = "Вы зарегистрированы на портале вакансий",
                IsBodyHtml = true
            };

            Send(mailMessage);
        }
     
    }
}
