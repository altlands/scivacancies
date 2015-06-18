using System;
using Microsoft.AspNet.Mvc;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.Controllers
{
    public class ApplicationsController: Controller
    {
        [PageTitle("Новая заявка")]
        public ViewResult Create(Guid researcherGuid, Guid vacancyGuid)
        {
            return View(new PageViewModelBase());
        }


        [HttpPost]
        public ActionResult Create(PageViewModelBase model)
        {
            return RedirectToAction("applications", "researchers", new {id = Guid.NewGuid()});
        }

        [PageTitle("Детали заявки")]
        public ViewResult Details(Guid id) => View();

        [PageTitle("Детали заявки")]
        public ViewResult ApplicationInVacancy(Guid id) => View();
    }
}
