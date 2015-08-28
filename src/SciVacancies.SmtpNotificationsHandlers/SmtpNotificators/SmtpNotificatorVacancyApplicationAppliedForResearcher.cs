using SciVacancies.ReadModel.Core;

namespace SciVacancies.SmtpNotificationsHandlers.SmtpNotificators
{
    public class SmtpNotificatorVacancyApplicationAppliedForResearcher : SmtpNotificator
    {
        public void Send(Researcher researcher, Vacancy vacancy, VacancyApplication vacancyApplication)
        {
            var body = $@"
<div style=''>
    Cообщаем, что на вакансию <a target='_blank' href='http://{Domain}/vacancies/card/{vacancy.guid}'>{vacancy.fullname}</a> 
    Вами подана новая <a target='_blank' href='http://{Domain}/applications/details/{vacancyApplication.guid}'>заявка</a> 
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
            Send(new SciVacMailMessage(researcher.email, researcher.extraemail, "Уведомление с портала вакансий", body));
        }
    }
}
