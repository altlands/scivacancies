using System.Linq;
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

            //заполнить названия организаций
            var organizationGuids = model.VacanciesList.Items.Select(c => c.OrganizationGuid).Distinct().ToList();
            var organizations=  _mediator.Send(new SelectOrganizationsByGuidsQuery {OrganizationGuids = organizationGuids});
            model.VacanciesList.Items.ForEach(c =>
            {
                var organization = organizations.SingleOrDefault(d => d.guid == c.OrganizationGuid);
                if (organization != null)
                {
                    c.OrganizationName = organization.name;
                }
            });

            return View(model);
        }

        [PageTitle("Информация о системе")]
        public ActionResult About()
        {
            return View();
        }
       
       
    }
}
