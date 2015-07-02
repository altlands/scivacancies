using MediatR;
using Microsoft.AspNet.Mvc;
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
                OrganizationsList =_mediator.Send(new SelectPagedOrganizationsQuery {PageSize = 4, PageIndex = 1, OrderBy =ConstTerms.OrderByVacancyCountDescending}),
                VacanciesList = _mediator.Send(new SelectPagedVacanciesQuery {PageSize = 4, PageIndex = 1, OrderBy = ConstTerms.OrderByDateStartDescending, PublishedOnly = true})
            };

            return View(model);
        }

        public IActionResult About()
        {
            ViewBag.Message = "Your application description page. SSSSSSSSS";

            return View();
        }

        public IActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
