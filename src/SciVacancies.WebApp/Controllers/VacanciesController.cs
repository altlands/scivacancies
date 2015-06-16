using Microsoft.AspNet.Mvc;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Controllers
{
    /// <summary>
    /// страница с вакансиями (конкурсами)
    /// </summary>
    public class VacanciesController : Controller
    {
        [PageTitle("Карточка конкурса")]
        public ViewResult Show(/*Guid id*/)
        {
            return View();
        }

        [PageTitle("Карточка конкурса")]
        public ViewResult Details(/*Guid id*/)
        {
            return View();
        }

        [PageTitle("Завершенные конкурсы")]
        public ViewResult Closed() => View();

        [PageTitle("Новая вакансия")]
        public ViewResult Create() => View();


        [PageTitle("Новая вакансия")]
        [HttpPost]
        public RedirectToActionResult Create(VacancyCreateViewModel model)
        {
            return RedirectToAction("details");
        }
    }
}
