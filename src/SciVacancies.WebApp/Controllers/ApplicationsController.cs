using System;
using Microsoft.AspNet.Mvc;
using SciVacancies.Domain.Aggregates.Interfaces;
using SciVacancies.Domain.DataModels;
using SciVacancies.ReadModel;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.Controllers
{
    public class ApplicationsController: Controller
    {
        private readonly IResearcherService _researcherService;
        private readonly IReadModelService _readModelService;

        public ApplicationsController(IResearcherService researcherService, IReadModelService readModelService)
        {
            _researcherService = researcherService;
            _readModelService = readModelService;
        }

        [PageTitle("Новая заявка")]
        public ViewResult Create(Guid researcherGuid, Guid vacancyGuid)
        {
            return View(new PageViewModelBase());
        }


        [HttpPost]
        public ActionResult Create(PageViewModelBase model)
        {
            //var data = new VacancyApplicationDataModel();
            //_researcherService.CreateVacancyApplicationTemplate(, , data);

            return RedirectToAction("applications", "researchers", new {id = Guid.NewGuid()});
        }

        [PageTitle("Детали заявки")]
        public ViewResult Details(Guid id)
        {
            _readModelService.SingleResearcher(id);
            return View();
        }

        [PageTitle("Детали заявки")]
        public ViewResult ApplicationInVacancy(Guid id) => View();
    }
}
