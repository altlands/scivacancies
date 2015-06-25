using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel;
using SciVacancies.ReadModel.Core;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Engine.CustomAttribute;
using SciVacancies.WebApp.ViewModels;
using SciVacancies.WebApp.Commands;
using SciVacancies.WebApp.Queries;

using MediatR;

namespace SciVacancies.WebApp.Controllers
{
    [Authorize(Roles = ConstTerms.RequireRoleResearcher)]
    public class ResearchersController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IReadModelService _readModelService;

        public ResearchersController(IMediator mediator, IReadModelService readModelService)
        {
            _mediator = mediator;
            _readModelService = readModelService;
        }

        [SiblingPage]
        [PageTitle("Информация")]
        [BindResearcherIdFromClaims]
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
        [BindResearcherIdFromClaims]
        public ViewResult Applications(Guid researcherGuid)
        {
            if (researcherGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(researcherGuid));

            var model = new VacancyApplicationsInResearcherIndexViewModel
            {
                Applications = _readModelService.SelectVacancyApplicationsByResearcher(researcherGuid)
            };
            return View(model);
        }

        [SiblingPage]
        [PageTitle("Избранные вакансии")]
        [BindResearcherIdFromClaims]
        public ActionResult Favorities(Guid researcherGuid)
        {
            if (researcherGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(researcherGuid));

            var model = new VacanciesFavoritiesInResearcherIndexViewModel
            {
                Vacancies = _readModelService.SelectFavoriteVacancies(researcherGuid)
            };
            return View(model);
        }

        [SiblingPage]
        [PageTitle("Подписки")]
        public ViewResult Subscriptions(Guid researcherGuid)
        {
            var searchSubscriptions = _mediator.Send(new SelectPagedSearchSubscriptionsQuery
            {
                ResearcherGuid = researcherGuid,
                PageIndex = 1,
                PageSize = 10
            });

            var model = new ResearcherDetailsViewModel();
            return View(model);
        }

        [SiblingPage]
        [PageTitle("Уведомления")]
        public ViewResult Notifications()
        {
            var notifications = _mediator.Send(new SelectPagedNotificationsByResearcherQuery
            {
                ResearcherGuid = Guid.NewGuid(),
                PageIndex = 1,
                PageSize = 10
            });

            var model = new ResearcherDetailsViewModel();
            return View(model);
        }

        [BindResearcherIdFromClaims]
        public ActionResult AddToFavorite(Guid researcherGuid, Guid vacancyGuid)
        {
            if (researcherGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(researcherGuid));
            if (vacancyGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(vacancyGuid));


            var model = _readModelService.SingleVacancy(vacancyGuid);
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
                    || !favoritesVacancies.Select(c => c.Guid).ToList().Contains(vacancyGuid))
                    _mediator.Send(new AddVacancyToFavoritesCommand { ResearcherGuid = researcherGuid, VacancyGuid = vacancyGuid });
            }

            return Redirect(Context.Request.Headers["referer"]);
        }

    }
}
