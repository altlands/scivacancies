using Microsoft.AspNet.Mvc;

namespace SciVacancies.WebApp.Controllers
{
    public class VacanciesController : Controller
    {
        public ViewResult Details(string id) => View();
    }
}
