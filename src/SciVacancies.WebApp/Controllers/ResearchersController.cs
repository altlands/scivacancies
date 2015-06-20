using System;
using AutoMapper;
using Microsoft.AspNet.Mvc;
using SciVacancies.Domain.Aggregates.Interfaces;
using SciVacancies.ReadModel;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Engine.CustomAttribute;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Controllers
{
    public class ResearchersController: Controller
    {
        private IResearcherService _researcherService;
        private IReadModelService _readModelService;

        public ResearchersController(IReadModelService readModelService, IResearcherService researcherService)
        {
            _researcherService = researcherService;
            _readModelService = readModelService;
        }

        [SiblingPage]
        [PageTitle("Информация")]
        [BindArgumentFromCookies(ConstTerms.CookieKeyForResearcherGuid, "researcherGuid")]
        public ViewResult Account(Guid researcherGuid)
        {
            if(researcherGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(researcherGuid));

            var model = Mapper.Map<ResearcherDetailsViewModel>(_readModelService.SingleResearcher(researcherGuid));

            return View(model);
        }

        [PageTitle("Изменить данные")]
        public ViewResult Edit(Guid id)
        {
            var model = new ResearcherEditViewModel {Guid = id };
            return View(model);
        }
        [HttpPut]
        [HttpPost]
        [PageTitle("Редактирование информации пользователя")]
        public RedirectToActionResult Edit(ResearcherEditViewModel model)
        {
            return RedirectToAction("account");
        }

        [SiblingPage]
        [PageTitle("Мои заявки")]
        public ViewResult Applications()
        {
            var model = new ResearcherDetailsViewModel();
            return View(model);
        }

        [SiblingPage]
        [PageTitle("Избранные вакансии")]
        public ActionResult Favorities()
        {
            var model = new ResearcherDetailsViewModel();
            return View(model);
        }

        [SiblingPage]
        [PageTitle("Подписки")]
        public ViewResult Subscriptions()
        {
            var model = new ResearcherDetailsViewModel();
            return View(model);
        }

        [SiblingPage]
        [PageTitle("Уведомления")]
        public ViewResult Notifications()
        {
            var model = new ResearcherDetailsViewModel();
            return View(model);
        }

    }
}
