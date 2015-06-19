using Microsoft.AspNet.Mvc;
using System;
using SciVacancies.Domain.Aggregates.Interfaces;
using SciVacancies.ReadModel;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Controllers
{
    /// <summary>
    /// страница с вакансиями (конкурсами)
    /// </summary>
    public class VacanciesController : Controller
    {
        private readonly IReadModelService _readModelService;
        private readonly IOrganizationService _organizationService;

        public VacanciesController(IOrganizationService organizationService, IReadModelService readModelService)
        {
            _organizationService = organizationService;
            _readModelService = readModelService;
        }

        [PageTitle("Карточка конкурса")]
        public ViewResult Details(Guid id) => View();

        [PageTitle("Карточка конкурса")]
        public ViewResult Preview(Guid id) => View();

        [PageTitle("Новая вакансия")]
        public ViewResult Create() => View(new ApplicationCreateViewModel().InitDictionaries(_readModelService));


        [PageTitle("Новая вакансия")]
        [HttpPost]
        public RedirectToActionResult Create(VacancyCreateViewModel model) => RedirectToAction("vacancies", "organizations");
    }
}
