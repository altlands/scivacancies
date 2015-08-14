using System.Net.Mail;
using SciVacancies.ReadModel.Core;

namespace SciVacancies.SmtpNotificationsHandlers.SmtpNotificators
{
    public class SmtpNotificatorVacancyApplicationApplied : SmtpNotificator
    {
        public void Send(Organization organization, Vacancy vacancy, VacancyApplication vacancyApplication)
        {
            var body = $@"
<div style=''>
    Cообщаем, что на вакансию <a target='_blank' href='http://{Domain}/vacancies/details/{vacancy.guid}'>{vacancy.fullname}</a> 
    подана новая <a target='_blank' href='http://{Domain}/vacancies/preview/{vacancyApplication.guid}'>заявка</a> 
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
            var mailMessage = new MailMessage(@from: "mailer@alt-lan.com", to: organization.email, body: body, subject: "Уведомление с портала вакансий")
            {
                IsBodyHtml = true
            };

            Send(mailMessage);
        }
    }
}
