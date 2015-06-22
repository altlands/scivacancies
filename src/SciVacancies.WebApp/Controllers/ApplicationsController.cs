using System;
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
        [BindArgumentFromCookies(ConstTerms.CookiesKeyForResearcherGuid, "researcherGuid")]
        public ViewResult Create(Guid researcherGuid, Guid vacancyGuid)
        {
            if(researcherGuid==Guid.Empty)
                throw new ArgumentNullException(nameof(researcherGuid));
            if (vacancyGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(vacancyGuid));

            var researcher = _readModelService.SingleResearcher(researcherGuid);
            var vacancy = _readModelService.SingleVacancy(vacancyGuid);

            var model = new VacancyApplicationCreateViewModel
            {
                ResearcherGuid = researcherGuid,
                VacancyGuid = vacancyGuid,
                PositionName = vacancy.Name,
                Email = researcher.Email,
                Phone = researcher.Phone,
                ResearcherFullName = $"{researcher.SecondName} {researcher.FirstName} {researcher.Patronymic}",
                ResearchActivity =researcher.ResearchActivity,
                TeachingActivity = researcher.TeachingActivity,
                OtherActivity =  researcher.OtherActivity,
                ScienceDegree = researcher.ScienceDegree,
                AcademicStatus = researcher.AcademicStatus,
                Rewards = researcher.Rewards
            };


            return View(model);
        }


        [HttpPost]
        public ActionResult Create(VacancyApplicationCreateViewModel model)
        {
            var data = Mapper.Map<VacancyApplicationDataModel>(model);
            var vacancyApplicationGuid = _researcherService.CreateVacancyApplicationTemplate(model.ResearcherGuid, model.VacancyGuid, data);
            return RedirectToAction("applications", "researchers", new {id = vacancyApplicationGuid });
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
