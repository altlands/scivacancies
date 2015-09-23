using System;
using SciVacancies.ReadModel.Core;
using SciVacancies.Services;
using SciVacancies.Services.Email;

namespace SciVacancies.SmtpNotifications.SmtpNotificators
{
    //TODO: ntemnikov: move body generation to handlers
    public class SmtpNotificatorVacancyService : ISmtpNotificatorVacancyService
    {
        private readonly ISmtpNotificatorService _smtpNotificatorService;

        public SmtpNotificatorVacancyService(ISmtpNotificatorService smtpNotificatorService)
        {
            _smtpNotificatorService = smtpNotificatorService;
        }

        public void SendVacancyApplicationAppliedForOrganization(Organization organization, Vacancy vacancy, VacancyApplication vacancyApplication)
        {
            var body = $@"
<div style=''>
    Cообщаем, что на вакансию <a target='_blank' href='http://{_smtpNotificatorService.Domain}/vacancies/details/{vacancy.guid}'>{vacancy.fullname}</a> 
    подана новая <a target='_blank' href='http://{_smtpNotificatorService.Domain}/applications/preview/{vacancyApplication.guid}'>заявка</a> 
</div>

<br/>
<br/>
<hr/>

<div style='color: lightgray; font-size: smaller;'>
    Это письмо создано автоматически с 
    <a target='_blank' href='http://{_smtpNotificatorService.Domain}'>Портала вакансий</a>.
    Чтобы не получать такие уведомления отключите их или смените email в 
    <a target='_blank' href='http://{_smtpNotificatorService.Domain}/organizations/account/'>личном кабинете</a>.
</div>
";
            _smtpNotificatorService.Send(new SciVacMailMessage(organization.email, "Уведомление с портала вакансий", body));
        }
        public void SendVacancyApplicationAppliedForResearcher(Researcher researcher, Vacancy vacancy, VacancyApplication vacancyApplication)
        {
            var body = $@"
<div style=''>
    Cообщаем, что на вакансию <a target='_blank' href='http://{_smtpNotificatorService.Domain}/vacancies/card/{vacancy.guid}'>{vacancy.fullname}</a> 
    Вами подана новая <a target='_blank' href='http://{_smtpNotificatorService.Domain}/applications/details/{vacancyApplication.guid}'>заявка</a> 
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
        public void SendVacancyStatusChangedForOrganization(Vacancy vacancy, Organization organization)
        {
            var body = $@"
<div style=''>
    Cообщаем, что вакансия
    <a target='_blank' href='http://{_smtpNotificatorService.Domain}/vacancies/card/{vacancy.guid}'>{vacancy.fullname}</a> получила новый статус: {vacancy.status.GetDescriptionByResearcher()}
</div>

<br/>
<br/>
<hr/>

<div style='color: lightgray; font-size: smaller;'>
    Это письмо создано автоматически с 
    <a target='_blank' href='http://{_smtpNotificatorService.Domain}'>Портала вакансий</a>.
    Чтобы не получать такие уведомления отключите их или смените email в 
    <a target='_blank' href='http://{_smtpNotificatorService.Domain}/organizations/account/'>личном кабинете</a>.
</div>
";

            _smtpNotificatorService.Send(new SciVacMailMessage(organization.email, "Уведомление с портала вакансий", body));
        }
        public void SendVacancyStatusChangedForResearcher(Vacancy vacancy, Researcher researcher)
        {
            var body = $@"
<div style=''>
    Уважаемый(-ая), {researcher.secondname} {researcher.firstname}, сообщаем, что вакансия
    <a target='_blank' href='http://{_smtpNotificatorService.Domain}/vacancies/card/{vacancy.guid}'>{vacancy.fullname}</a> получила новый статус: {vacancy.status.GetDescriptionByResearcher()}
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
        //TODO брать дни из конфига
        public void SendVacancyProlongedForResearcher(Vacancy vacancy, Researcher researcher)
        {
            var body = $@"
            <div style=''>
                Уважаемый(-ая), {researcher.secondname} {researcher.firstname}, сообщаем, что по вакансии
                <a target='_blank' href='http://{_smtpNotificatorService.Domain}/vacancies/card/{vacancy.guid}'>{vacancy.fullname}</a> дата принятия решения комиссией переносится на 15 дней вперёд.
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
        public void SendWinnerSet(Researcher researcher, Guid applicationGuid, Guid vacancyGuid)
        {
            var body = $@"
<div style=''>
    Уважаемый(-ая), {researcher.secondname} {researcher.firstname}, ваша <a target='_blank' href='http://{_smtpNotificatorService.Domain}/applications/details/{applicationGuid}'>зявка</a>
    победила в конкурсе на <a target='_blank' href='http://{_smtpNotificatorService.Domain}/vacancies/details/{vacancyGuid}'>вакансию</a>.
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

        public void SendFirstCommitteeNotificationToOrganization(Organization organization, Vacancy vacancy)
        {
            //TODO сколько осталось дней - брать из конфига
            var body = $@"
            <div style=''>
                У вас остался один день до завершения комиссии по вакансии <a target='_blank' href='http://{_smtpNotificatorService.Domain}/vacancies/details/{vacancy.guid}'>{vacancy.fullname}</a>
            </div>

            <br/>
            <br/>
            <hr/>

            <div style='color: lightgray; font-size: smaller;'>
                Это письмо создано автоматически с 
                <a target='_blank' href='http://{_smtpNotificatorService.Domain}'>Портала вакансий</a>.
                Чтобы не получать такие уведомления отключите их или смените email в 
                <a target='_blank' href='http://{_smtpNotificatorService.Domain}/organizations/account/'>личном кабинете</a>.
            </div>
            ";
            _smtpNotificatorService.Send(new SciVacMailMessage(organization.email, "Уведомление с портала вакансий", body));
        }
        public void SendSecondCommitteeNotificationToOrganization(Organization organization, Vacancy vacancy)
        {
            //TODO сколько осталось дней - брать из конфига

            var body = $@"
            <div style=''>
                У вас остался один день на публикацию протокола комиссии и выбора победителей по вакансии <a target='_blank' href='http://{_smtpNotificatorService.Domain}/vacancies/details/{vacancy.guid}'>{vacancy.fullname}</a>
            </div>

            <br/>
            <br/>
            <hr/>

            <div style='color: lightgray; font-size: smaller;'>
                Это письмо создано автоматически с 
                <a target='_blank' href='http://{_smtpNotificatorService.Domain}'>Портала вакансий</a>.
                Чтобы не получать такие уведомления отключите их или смените email в 
                <a target='_blank' href='http://{_smtpNotificatorService.Domain}/organizations/account/'>личном кабинете</a>.
            </div>
            ";
            _smtpNotificatorService.Send(new SciVacMailMessage(organization.email, "Уведомление с портала вакансий", body));
        }
        public void SendOfferResponseAwaitingNotificationToWinner(Researcher researcher, Guid applicationGuid, Guid vacancyGuid)
        {
            //TODO сколько осталось дней - брать из конфига

            var body = $@"
                <div style=''>
                    Уважаемый(-ая), {researcher.secondname} {researcher.firstname}, ваша <a target='_blank' href='http://{_smtpNotificatorService.Domain}/applications/details/{applicationGuid}'>зявка</a>
                    победила в конкурсе на <a target='_blank' href='http://{_smtpNotificatorService.Domain}/vacancies/details/{vacancyGuid}'>вакансию</a>.

                    Вам необходимо до конца сего дня подтвердить или отвергуть предложение контракта по этой вакансии.
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
        public void SendOfferResponseAwaitingNotificationToPretender(Researcher researcher, Guid applicationGuid, Guid vacancyGuid)
        {
            //TODO сколько осталось дней - брать из конфига

            var body = $@"
                <div style=''>
                    Уважаемый(-ая), {researcher.secondname} {researcher.firstname}, ваша <a target='_blank' href='http://{_smtpNotificatorService.Domain}/applications/details/{applicationGuid}'>зявка</a>
                    заняла второе место в конкурсе на <a target='_blank' href='http://{_smtpNotificatorService.Domain}/vacancies/details/{vacancyGuid}'>вакансию</a>.
                    
                    Вам необходимо до конца сего дня подтвердить или отвергуть предложение контракта по этой вакансии.
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
