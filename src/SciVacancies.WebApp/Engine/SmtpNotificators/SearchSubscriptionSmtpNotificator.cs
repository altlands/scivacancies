using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SciVacancies.WebApp.Engine.SmtpNotificators
{
    internal class SearchSubscriptionSmtpNotificator : SmtpNotificator
    {
        public void Send()
        {
            //var domain = "localhost:59075";
            var domain = "scivac.test.alt-lan.com";
            var researcherFullName = "Фамилько Имён Отчествович";
            var applicationGuid = Guid.NewGuid();
            var vacancyGuid = Guid.NewGuid();

            var body = $@"
<div style=''>
    Уважаемый(-ая), {researcherFullName}, ваша
    <a target='_blank' href='http://{domain}/applications/details/{applicationGuid}'>зявка</a>
    победила в конкурсе на 
    <a target='_blank' href='http://{domain}/vacancies/details/{vacancyGuid}'>вакансию</a>.
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
            var mailMessage = new MailMessage(from: "mailer@alt-lan.com", to: "mail@mail.ru", body: body, subject: "Уведомление с портала вакансий")
            {
                IsBodyHtml = true
            };

            Send(mailMessage);
        }
    }
}
