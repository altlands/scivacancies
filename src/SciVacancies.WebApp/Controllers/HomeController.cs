using System.Linq;
using MediatR;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using SciVacancies.ReadModel.Core;
using SciVacancies.WebApp.Infrastructure.Identity;
using SciVacancies.WebApp.Models;
using SciVacancies.WebApp.Queries;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Controllers
{
    [ResponseCache(NoStore = true)]
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly SciVacUserManager _userManager;

        public HomeController(SciVacUserManager userManager, IMediator mediator)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [ResponseCache(NoStore = true)]
        [PageTitle("Главная")]
        public IActionResult Index()
        {
            //проверяем не инициализированную БД
            var user = _userManager.FindByName("researcher1");
            if (user == null)
                //return Content("инициализация уже проходила");
                return RedirectToAction("index", "initialize");

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
            if (organizationGuids.Any())
            {
                var organizations =
                    _mediator.Send(new SelectOrganizationsByGuidsQuery { OrganizationGuids = organizationGuids });
                model.VacanciesList.Items.ForEach(c =>
                {
                    var organization = organizations.SingleOrDefault(d => d.guid == c.OrganizationGuid);
                    if (organization != null)
                    {
                        c.OrganizationName = organization.name;
                    }
                });
            }

            //todo: ntemnikov -> переименовать в IsMainPage
            ViewBag.HideSearchPanel = true;

            return View(model);
        }

        [PageTitle("Информация о системе")]
        public ActionResult About()
        {
            return View();
        }


    }
}
