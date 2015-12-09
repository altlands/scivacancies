using SciVacancies.ReadModel.Core;
using SciVacancies.Services.Email;

using System;
using System.Globalization;
using Microsoft.Framework.Configuration;
using SciVacancies.Domain.Enums;

namespace SciVacancies.SmtpNotifications.SmtpNotificators
{
    public class SmtpNotificatorVacancyService : ISmtpNotificatorVacancyService
    {
        private readonly IEmailService EmailService;
        private readonly string From;
        private readonly string Domain;
        private readonly string PortalLink;

        public SmtpNotificatorVacancyService(IEmailService emailService, IConfiguration configuration)
        {
            EmailService = emailService;
            From = configuration["EmailSettings:Login"];
            Domain = configuration["EmailSettings:Domain"];
            PortalLink = configuration["EmailSettings:PortalLink"];

            if (string.IsNullOrEmpty(From)) throw new ArgumentNullException("From is null");
            if (string.IsNullOrEmpty(Domain)) throw new ArgumentNullException("Domain is null");
            if (string.IsNullOrEmpty(PortalLink)) throw new ArgumentNullException("PortalLink is null");
        }

        public void SendVacancyApplicationAppliedForOrganization(Organization organization, Vacancy vacancy, VacancyApplication vacancyApplication)
        {
            var body = $@"
<div style=''>

    Здравствуйте, {vacancy.contact_name}
    <br/>
    Для участия, в созданной вами вакансии <a target='_blank' href='http://{Domain}/vacancies/details/{vacancy.guid}'>{vacancy.fullname}</a>,
    <br/>
    подана новая <a target='_blank' href='http://{Domain}/applications/preview/{vacancyApplication.guid}'>заявка</a>
    <br/>
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
            EmailService.Send(new SciVacMailMessage(From, vacancy.contact_email, "Уведомление с портала вакансий", body));
        }
        public void SendVacancyApplicationAppliedForResearcher(Researcher researcher, Vacancy vacancy, VacancyApplication vacancyApplication)
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
            EmailService.Send(new SciVacMailMessage(From, researcher.email, "Уведомление с портала вакансий", body));
        }
        public void SendVacancyStatusChangedForOrganization(Vacancy vacancy, Organization organization, VacancyStatus status)
        {
            var body = $@"
<div style=''>
    Cообщаем, что вакансия
    <a target='_blank' href='http://{Domain}/vacancies/card/{vacancy.guid}'>{vacancy.fullname}</a> получила новый статус: {status.GetDescriptionByResearcher()}
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

            EmailService.Send(new SciVacMailMessage(From, vacancy.contact_email, "Уведомление с портала вакансий", body));
        }
        public void SendVacancyStatusChangedForResearcher(Vacancy vacancy, Researcher researcher, VacancyStatus status)
        {
            var body = $@"
<div style=''>
    Здравствуйте, {researcher.secondname} {researcher.firstname}
    <br/>
    У вакансии {vacancy.fullname}, на которую вы подали заявку, изменился статус ({status.GetDescriptionByResearcher()})
    <br/>
    Для просмотра обновлений перейдите, пожалуйста, по ссылке: <a target='_blank' href='http://{Domain}/vacancies/card/{vacancy.guid}'>http://{Domain}/vacancies/card/{vacancy.guid}</a>
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

            EmailService.Send(new SciVacMailMessage(From, researcher.email, "Уведомление с портала вакансий", body));
        }
        //TODO брать дни из конфига
        public void SendVacancyProlongedForResearcher(Vacancy vacancy, Researcher researcher)
        {
            var body = $@"
            <div style=''>
                Уважаемый(-ая), {researcher.secondname} {researcher.firstname}, сообщаем, что по вакансии
                <a target='_blank' href='http://{Domain}/vacancies/card/{vacancy.guid}'>{vacancy.fullname}</a> дата принятия решения комиссией переносится на 15 дней вперёд.
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

            EmailService.Send(new SciVacMailMessage(From, researcher.email, "Уведомление с портала вакансий", body));
        }
        public void SendWinnerSet(Researcher researcher, Guid applicationGuid, Guid vacancyGuid)
        {
            var body = $@"
            <div style=''>
    Здравствуйте, {researcher.secondname} {researcher.firstname}
    <br/>
    Вы выбраны в качестве победителя в конкурсе на <a target='_blank' href='http://{Domain}/vacancies/card/{vacancyGuid}'>вакансию</a>
    <br/>
    В течение 30-ти календарных дней (до {DateTime.Now.AddDays(30).ToString("d",new CultureInfo("ru-RU"))}) вам необходимо подтвердить свое согласие на замещение должности и подписать трудовой договор.

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

            EmailService.Send(new SciVacMailMessage(From, researcher.email, "Уведомление с портала вакансий", body));
        }

        public void SendFirstCommitteeNotificationToOrganization(Organization organization, Vacancy vacancy)
        {
            #region неверный текст
            //            //TODO сколько осталось дней - брать из конфига
            //            var body = $@"
            //<div style=''>
            //    Здравствуйте, {vacancy.contact_name}
            //    <br/>
            //    Напоминаем вам, что ({vacancy.committee_start_date?.ToShortDateString()}) заканчивается прием заявок для участия, в
            //    <br/>
            //    созданной вами вакансии: <a target='_blank' href='http://{Domain}/vacancies/details/{vacancy.guid}'>{vacancy.fullname}</a>
            //    <br/>
            //    Конкурс на вакансию автоматически перейдет в статус «рассмотрение заявок комиссией». 
            //    <br/>
            //    Вам необходимо выбрать победителя конкурса в течение 15-ти рабочих дней (до {vacancy.committee_end_date?.ToShortDateString()}) и разместить в течение 3-х рабочих дней решение конкурсной комиссии.
            //    <br/>
            //    При необходимости вы можете продлить срок рассмотрения еще на 15-ть рабочих дней.            
            //            </div>

            //            <br/>
            //            <br/>
            //            <hr/>

            //            <div style='color: lightgray; font-size: smaller;'>
            //                Это письмо создано автоматически с 
            //                <a target='_blank' href='http://{Domain}'>Портала вакансий</a>.
            //                Чтобы не получать такие уведомления отключите их или смените email в 
            //                <a target='_blank' href='http://{Domain}/organizations/account/'>личном кабинете</a>.
            //            </div>
            //            ";
            //            EmailService.Send(new SciVacMailMessage(From, vacancy.contact_email, "Уведомление с портала вакансий", body)); 
            #endregion

            var body = $@"
            <div style=''>
    Здравствуйте, {vacancy.contact_name}
    <br/>
    Напоминаем вам, что  ({vacancy.committee_end_date?.ToLocalTime().ToShortDateString()} {vacancy.committee_end_date?.ToLocalTime().ToShortTimeString()}) заканчивается срок рассмотрения заявок, на созданную вами вакансию: <a target='_blank' href='http://{Domain}/vacancies/details/{vacancy.guid}'>{vacancy.fullname}</a>
    <br/>
    Вам необходимо в течение 3-х рабочих дней выбрать победителя и поместить решение конкурсной комиссии на портале {PortalLink}.
    <br/>

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
            EmailService.Send(new SciVacMailMessage(From, vacancy.contact_email, "Уведомление с портала вакансий", body));


        }
        public void SendSecondCommitteeNotificationToOrganization(Organization organization, Vacancy vacancy)
        {
            //TODO сколько осталось дней - брать из конфига

            var body = $@"
            <div style=''>
    Здравствуйте, {vacancy.contact_name}
    <br/>
    Напоминаем вам, что срок рассмотрения заявок ({vacancy.committee_end_date?.ToLocalTime().ToShortDateString()} {vacancy.committee_end_date?.ToLocalTime().ToShortTimeString()}), на созданную вами вакансию: <a target='_blank' href='http://{Domain}/vacancies/details/{vacancy.guid}'>{vacancy.fullname}</a>, уже прошел.
    <br/>
    Вам необходимо в течение 3-х рабочих дней выбрать победителя и поместить решение конкурсной комиссии на портале {PortalLink}.
    <br/>

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
            EmailService.Send(new SciVacMailMessage(From, vacancy.contact_email, "Уведомление с портала вакансий", body));
        }
        public void SendOfferResponseAwaitingNotificationToWinner(Researcher researcher, Guid applicationGuid, Guid vacancyGuid)
        {
            //TODO сколько осталось дней - брать из конфига

            var body = $@"
                <div style=''>
                    Уважаемый(-ая), {researcher.secondname} {researcher.firstname}, ваша <a target='_blank' href='http://{Domain}/applications/details/{applicationGuid}'>зявка</a>
                    победила в конкурсе на <a target='_blank' href='http://{Domain}/vacancies/details/{vacancyGuid}'>вакансию</a>.

                    Вам необходимо до конца этого дня подтвердить или отвергуть предложение контракта по этой вакансии.

                    Иначе сработает автоматический отказ от предложения.
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

            EmailService.Send(new SciVacMailMessage(From, researcher.email, "Уведомление с портала вакансий", body));
        }
        public void SendOfferResponseAwaitingNotificationToPretender(Researcher researcher, Guid applicationGuid, Guid vacancyGuid)
        {
            //TODO сколько осталось дней - брать из конфига

            var body = $@"
                <div style=''>
                    Уважаемый(-ая), {researcher.secondname} {researcher.firstname}, ваша <a target='_blank' href='http://{Domain}/applications/details/{applicationGuid}'>зявка</a>
                    заняла второе место в конкурсе на <a target='_blank' href='http://{Domain}/vacancies/details/{vacancyGuid}'>вакансию</a>.
                    
                    Вам необходимо до конца сего дня подтвердить или отвергуть предложение контракта по этой вакансии.
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

            EmailService.Send(new SciVacMailMessage(From, researcher.email, "Уведомление с портала вакансий", body));
        }

        public void SendOfferRejectedByPretender(Vacancy vacancy, Organization organization)
        {
            var body = $@"
<div style=''>
    Здравствуйте, {vacancy.contact_name}
    <br />
    Претендент отклонил ваше предложение по вакансии <a target='_blank' href='http://{Domain}/vacancies/card/{vacancy.guid}'>{vacancy.fullname}</a>
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
            EmailService.Send(new SciVacMailMessage(From, vacancy.contact_email, "Уведомление с портала вакансий", body));
        }

        public void SendOfferRejectedByWinner(Vacancy vacancy, Organization organization)
        {
            var body = $@"
<div style=''>
    Здравствуйте, {vacancy.contact_name}
    <br />
    Победитель отклонил ваше предложение по вакансии <a target='_blank' href='http://{Domain}/vacancies/card/{vacancy.guid}'>{vacancy.fullname}</a>
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
            EmailService.Send(new SciVacMailMessage(From, vacancy.contact_email, "Уведомление с портала вакансий", body));
        }

        public void SendOfferAcceptedByPretender(Vacancy vacancy, Organization organization)
        {
            var body = $@"
<div style=''>
    Здравствуйте, {vacancy.contact_name}
    <br />
    Претендент принял ваше предложение по вакансии <a target='_blank' href='http://{Domain}/vacancies/card/{vacancy.guid}'>{vacancy.fullname}</a>
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
            EmailService.Send(new SciVacMailMessage(From, vacancy.contact_email, "Уведомление с портала вакансий", body));
        }

        public void SendOfferAcceptedByWinner(Vacancy vacancy, Organization organization)
        {
            var body = $@"
<div style=''>
    Здравствуйте, {vacancy.contact_name}
    <br />
    Победитель принял ваше предложение по вакансии <a target='_blank' href='http://{Domain}/vacancies/card/{vacancy.guid}'>{vacancy.fullname}</a>
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
            EmailService.Send(new SciVacMailMessage(From, vacancy.contact_email, "Уведомление с портала вакансий", body));
        }
    }
}
