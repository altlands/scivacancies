using SciVacancies.ReadModel.Core;

namespace SciVacancies.SmtpNotificationsHandlers.SmtpNotificators
{
    public class SmtpNotificatorVacancyStatusChangedForOrganization : SmtpNotificator
    {
        public void Send(Vacancy vacancy, Organization organization)
        {
            var body = $@"
<div style=''>
    Cообщаем, что вакансия
    <a target='_blank' href='http://{Domain}/vacancies/card/{vacancy.guid}'>{vacancy.fullname}</a> получила новый статус: {vacancy.status.GetDescriptionByResearcher()}
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
