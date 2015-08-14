using System;
using System.Net.Mail;
using SciVacancies.ReadModel.Core;

namespace SciVacancies.SmtpNotificationsHandlers.SmtpNotificators
{
    public class SmtpNotificatorWinnerSet : SmtpNotificator
    {
        public void Send(Researcher researcher,Guid applicationGuid, Guid vacancyGuid)
        {
            var body = $@"
<div style=''>
    Уважаемый(-ая), {researcher.secondname} {researcher.firstname}, ваша <a target='_blank' href='http://{Domain}/applications/details/{applicationGuid}'>зявка</a>
    победила в конкурсе на <a target='_blank' href='http://{Domain}/vacancies/details/{vacancyGuid}'>вакансию</a>.
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
            var mailMessage = new MailMessage(@from: "mailer@alt-lan.com", to: researcher.email, body: body, subject: "Уведомление с портала вакансий")
            {
                IsBodyHtml = true
            };

            Send(mailMessage);
        }
    }
}
