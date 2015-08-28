using SciVacancies.ReadModel.Core;

namespace SciVacancies.SmtpNotificationsHandlers.SmtpNotificators
{
    public class SmtpNotificatorVacancyApplicationAppliedForOrganization : SmtpNotificator
    {
        public void Send(Organization organization, Vacancy vacancy, VacancyApplication vacancyApplication)
        {
            var body = $@"
<div style=''>
    Cообщаем, что на вакансию <a target='_blank' href='http://{Domain}/vacancies/details/{vacancy.guid}'>{vacancy.fullname}</a> 
    подана новая <a target='_blank' href='http://{Domain}/applications/preview/{vacancyApplication.guid}'>заявка</a> 
</div>

<br/>
<br/>
<hr/>

<div style='color: lightgray; font-size: smaller;'>
    Это письмо создано автоматически с 
    <a target='_blank' href='http://{Domain}'>Портала вакансий</a>.
    Чтобы не получать такие уведомления отключите их или смените email в 
    <a target='_blank' href='http://{Domain}/organizations/account/'>личном кабинете</a>.
</div>
";
            Send(new SciVacMailMessage(organization.email, "Уведомление с портала вакансий", body));
        }
    }
}
