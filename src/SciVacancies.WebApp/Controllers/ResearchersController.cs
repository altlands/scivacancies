using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNet.Mvc;
using SciVacancies.Domain.Aggregates.Interfaces;
using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel;
using SciVacancies.ReadModel.Core;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Engine.CustomAttribute;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Controllers
{
    public class ResearchersController : Controller
    {
        private IResearcherService _researcherService;
        private readonly IReadModelService _readModelService;

        public ResearchersController(IReadModelService readModelService, IResearcherService researcherService)
        {
            _researcherService = researcherService;
            _readModelService = readModelService;
        }

        [SiblingPage]
        [PageTitle("Информация")]
        [BindArgumentFromCookies(ConstTerms.CookiesKeyForResearcherGuid, "researcherGuid")]
        public ViewResult Account(Guid researcherGuid)
        {
            if (researcherGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(researcherGuid));

            var model = Mapper.Map<ResearcherDetailsViewModel>(_readModelService.SingleResearcher(researcherGuid));

            return View(model);
        }

        [PageTitle("Изменить данные")]
        public ViewResult Edit(Guid id)
        {
            var model = new ResearcherEditViewModel { Guid = id };
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
        [BindArgumentFromCookies(ConstTerms.CookiesKeyForResearcherGuid, "researcherGuid")]
        public ViewResult Applications(Guid researcherGuid)
        {
            if (researcherGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(researcherGuid));

            var model = new VacancyApplicationsInResearcherIndexViewModel
            {
                Vacancies = _readModelService.SelectVacancyApplicationsByResearcher(researcherGuid)
            };
            return View(model);
        }

        [SiblingPage]
        [PageTitle("Избранные вакансии")]
        [BindArgumentFromCookies(ConstTerms.CookiesKeyForResearcherGuid, "researcherGuid")]
        public ActionResult Favorities(Guid researcherGuid)
        {
            if(researcherGuid==Guid.Empty)
                throw new ArgumentNullException(nameof(researcherGuid));

            var model = new VacanciesFavoritiesInResearcherIndexViewModel
            {
                Vacancies = _readModelService.SelectFavoriteVacancies(researcherGuid)
            };
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

        [BindArgumentFromCookies(ConstTerms.CookiesKeyForResearcherGuid, "researcherGuid")]
        public ActionResult AddToFavorite(Guid researcherGuid, Guid vacancyGuid)
        {
            if (researcherGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(researcherGuid));
            if (vacancyGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(vacancyGuid));


            var model = _readModelService.SingleVacancy(vacancyGuid);
            //если пользователь - Исследователь
            if (Context.Request.Cookies.ContainsKey(ConstTerms.CookiesKeyForUserRole)
                && bool.Parse(Context.Request.Cookies.Get(ConstTerms.CookiesKeyForUserRole))
                //если заявка на готовится к открытию или открыта
                && (model.Status == VacancyStatus.AppliesAcceptance || model.Status == VacancyStatus.Published)
                )
            {
                //если есть GUID Исследователя
                if (Context.Request.Cookies.ContainsKey(ConstTerms.CookiesKeyForResearcherGuid))
                {
                    var userGuid = Guid.Parse(Context.Request.Cookies.Get(ConstTerms.CookiesKeyForResearcherGuid));
                    List<Vacancy> favoritesVacancies = null;
                    try
                    {
                        favoritesVacancies = _readModelService.SelectFavoriteVacancies(userGuid);
                    }
                    catch (Exception)
                    {
                    }
                    //если текущей вакансии нет в списке избранных
                    if (favoritesVacancies == null
                        || !favoritesVacancies.Select(c => c.Guid).ToList().Contains(vacancyGuid))
                        _researcherService.AddVacancyToFavorites(researcherGuid, vacancyGuid);
                }
            }

            return Redirect(Context.Request.Headers["referer"]);
        }

    }
}
