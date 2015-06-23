using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel;
using SciVacancies.ReadModel.Core;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.ViewModels;

using SciVacancies.WebApp.Commands;

using MediatR;

namespace SciVacancies.WebApp.Controllers
{
    /// <summary>
    /// страница с вакансиями (конкурсами)
    /// </summary>
    [Authorize]
    public class VacanciesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IReadModelService _readModelService;

        public VacanciesController(IMediator mediator, IReadModelService readModelService)
        {
            _mediator = mediator;
            _readModelService = readModelService;
        }

        [AllowAnonymous]
        [PageTitle("Карточка конкурса")]
        [BindResearcherIdFromClaims]
        public ViewResult Details(Guid id, Guid researcherGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var model = _readModelService.SingleVacancy(id);

            ViewBag.ShowAddFavorite = false;
            ViewBag.VacancyInFavorites = false;

            //если заявка на готовится к открытию или открыта
            if (model.Status == VacancyStatus.AppliesAcceptance || model.Status == VacancyStatus.Published)
            {
                //если есть GUID Исследователя
                List<Vacancy> favoritesVacancies = null;
                try
                {
                    favoritesVacancies = _readModelService.SelectFavoriteVacancies(researcherGuid);
                }
                catch (Exception) { }
                //если текущей вакансии нет в списке избранных
                if (favoritesVacancies == null
                    || !favoritesVacancies.Select(c => c.Guid).ToList().Contains(id))
                    ViewBag.ShowAddFavorite = true;
                else
                    ViewBag.VacancyInFavorites = true;
            }

            //если заявка на готовится к открытию или открыта
            if (model.Status == VacancyStatus.AppliesAcceptance || model.Status == VacancyStatus.Published)
            {
                //если есть GUID Исследователя
                if (researcherGuid != Guid.Empty)
                {
                    List<Vacancy> favoritesVacancies = null;
                    try
                    {
                        favoritesVacancies = _readModelService.SelectFavoriteVacancies(researcherGuid);
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

        [AllowAnonymous]
        [PageTitle("Карточка конкурса")]
        public ViewResult Preview(Guid id) => View();

        [PageTitle("Новая вакансия")]
        [BindOrganizationIdFromClaims]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        public ViewResult Create(Guid organizationGuid)
        {
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException($"{nameof(organizationGuid)}");

            var model = new PositionCreateViewModel(organizationGuid).InitDictionaries(_readModelService);
            return View(model);
        }


        [PageTitle("Новая вакансия")]
        [HttpPost]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        public ActionResult Create(PositionCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var positionDataModel = Mapper.Map<PositionDataModel>(model);
                var positionGuid = _mediator.Send(new CreatePositionCommand { OrganizationGuid = model.OrganizationGuid, Data = positionDataModel });

                if (model.ToPublish)
                {
                    var vacancyGuid = _mediator.Send(new PublishVacancyCommand
                    {
                        OrganizationGuid = model.OrganizationGuid,
                        PositionGuid = positionGuid,
                        Data = Mapper.Map<VacancyDataModel>(positionDataModel)
                    });
                }

                return RedirectToAction("vacancies", "organizations");
            }
            model.InitDictionaries(_readModelService);
            return View(model);

        }
    }
}
