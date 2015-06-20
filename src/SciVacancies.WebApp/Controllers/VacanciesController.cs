using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNet.Mvc;
using SciVacancies.Domain.Aggregates.Interfaces;
using SciVacancies.Domain.DataModels;
using SciVacancies.ReadModel;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Engine.CustomAttribute;
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
        [BindArgumentFromCookies(ConstTerms.CookieKeyForOrganizationGuid, "organizationGuid")]
        public ViewResult Create(Guid organizationGuid)
        {
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException($"{nameof(organizationGuid)}");

            var model = new PositionCreateViewModel(organizationGuid).InitDictionaries(_readModelService);
            return View(model);
        }


        [PageTitle("Новая вакансия")]
        [HttpPost]
        public ActionResult Create(PositionCreateViewModel model)
        {
            var errors = ModelState.Values.Where(c => c.Errors.Count > 0).ToList();
            var tempCount = errors.Count;

            if (true /*ModelState.IsValid*/)
            {
                _organizationService.CreatePosition(model.OrganizationGuid, Mapper.Map<PositionDataModel>(model));
                return RedirectToAction("vacancies", "organizations");
            }
            model.InitDictionaries(_readModelService);
            return View(model);

        }
    }
}
