using MediatR;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Mvc;
using SciVacancies.ReadModel.Core;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Queries;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Controllers
{
    [ResponseCache(NoStore = true)]
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

        [PageTitle("Информация о системе")]
        public ActionResult About()
        {
            return View();
        }
       
       
    }
}
