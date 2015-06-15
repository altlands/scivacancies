using Microsoft.AspNet.Mvc;

namespace SciVacancies.WebApp.Controllers
{
    /// <summary>
    /// страница с вакансиями (конкурсами)
    /// </summary>
    public class VacanciesController : Controller
    {

        public ViewResult Details(string id) => View();

        public ViewResult Closed() =>View();

    }
}
