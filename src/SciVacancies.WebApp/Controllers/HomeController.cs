using System;
using System.Net.Mail;
using MediatR;
using Microsoft.AspNet.Mvc;
using NPoco;
using SciVacancies.ReadModel.Core;
using SciVacancies.SmtpNotifications.Vendors;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Queries;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ResponseCache(NoStore = true)]
        [PageTitle("Главная")]
        public IActionResult Index()
        {
            var model = new IndexViewModel
            {
                VacanciesList =
                    _mediator.Send(new SelectPagedVacanciesQuery
                    {
                        PageSize = 4,
                        PageIndex = 1,
                        OrderBy = ConstTerms.OrderByDateStartDescending,
                        PublishedOnly = true
                    }).MapToPagedList<Vacancy, VacancyDetailsViewModel>(),
                OrganizationsList =
                    _mediator.Send(new SelectPagedOrganizationsQuery
                    {
                        PageSize = 4,
                        PageIndex = 1,
                        OrderBy = ConstTerms.OrderByVacancyCountDescending
                    })
                        .MapToPagedList<Organization, OrganizationDetailsViewModel>(),
                ResearchDirections = new VacanciesFilterSource(_mediator).ResearchDirections
            };



            return View(model);
        }

        [PageTitle("Error")]
        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }

        [PageTitle("Информация о системе")]
        public ActionResult About()
        {
            return View();
        }

        public IActionResult Email()
        {
            var gmailEmailService = new GmailEmailService("mailer@alt-lan.com","" );

            var domain = "localhost:59075";
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
            var mailMessage = new MailMessage(from: "mailer@alt-lan.com", to: "nistoc@gmail.com", body: body, subject: "Уведомление с портала вакансий")
            {
                IsBodyHtml = true,
            
            };
            gmailEmailService.SendEmail(mailMessage);

            return Content("index");
        }
    }
}
