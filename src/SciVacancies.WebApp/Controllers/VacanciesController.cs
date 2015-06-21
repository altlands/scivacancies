using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.CodeGeneration;
using SciVacancies.Domain.Aggregates.Interfaces;
using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel;
using SciVacancies.ReadModel.Core;
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
        public ViewResult Details(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var model = _readModelService.SingleVacancy(id);

            ViewBag.ShowAddFavorite = false;
            ViewBag.VacancyInFavorites = false;

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
                        || !favoritesVacancies.Select(c => c.Guid).ToList().Contains(id))
                        ViewBag.ShowAddFavorite = true;
                    else
                        ViewBag.VacancyInFavorites = true;
                }
            }

            return View(model);
        }

        [PageTitle("Карточка конкурса")]
        public ViewResult Preview(Guid id) => View();

        [PageTitle("Новая вакансия")]
        [BindArgumentFromCookies(ConstTerms.CookiesKeyForOrganizationGuid, "organizationGuid")]
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
            if (ModelState.IsValid)
            {
                var positionDataModel = Mapper.Map<PositionDataModel>(model);
                var positionGuid = _organizationService.CreatePosition(model.OrganizationGuid, positionDataModel);

                if (model.ToPublish)
                {
                    var vacancyGuid = _organizationService.PublishVacancy(model.OrganizationGuid, positionGuid, Mapper.Map<VacancyDataModel>(positionDataModel));
                }

                return RedirectToAction("vacancies", "organizations");
            }
            model.InitDictionaries(_readModelService);
            return View(model);

        }
    }
}
