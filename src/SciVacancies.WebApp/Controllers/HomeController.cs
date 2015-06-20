using Microsoft.AspNet.Mvc;
using SciVacancies.ReadModel;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Infrastructure;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IReadModelService _readModelService;
        public IFart Fart {get;set;}

        public HomeController(IFart fart, IReadModelService readModelService)
        {
            Fart = fart;
            _readModelService = readModelService;
        }
        
        [PageTitle("Главная страница")]
        public IActionResult Index()
        {
            var model = new IndexViewModel
            {
                OrganizationsList = _readModelService.SelectOrganizations(ConstTerms.OrderByCountDescending, 4, 1),
                VacanciesList = _readModelService.SelectVacancies(ConstTerms.OrderByDateDescending, 4, 1)
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
