using System;
using System.Linq;
using AutoMapper;
using MediatR;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using NPoco;
using SciVacancies.Domain.Enums;
using SciVacancies.WebApp.Commands;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Engine.CustomAttribute;
using SciVacancies.WebApp.Queries;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Controllers
{
    [Authorize(Roles = ConstTerms.RequireRoleResearcher)]
    public class ResearchersController : Controller
    {
        private readonly IMediator _mediator;

        public ResearchersController(IMediator mediator) { _mediator = mediator; }

        [ResponseCache(NoStore = true)]
        [SiblingPage]
        [PageTitle("Информация")]
        [BindResearcherIdFromClaims]
        public IActionResult Account(Guid researcherGuid)
        {
            if (researcherGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(researcherGuid));

            var model = Mapper.Map<ResearcherDetailsViewModel>(_mediator.Send(new SingleResearcherQuery { ResearcherGuid = researcherGuid }));

            if (model == null)
                return RedirectToAction("logout", "account");

            return View(model);
        }

        [ResponseCache(NoStore = true)]
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

            var source =
                _mediator.Send(new SelectPagedVacancyApplicationsByResearcherQuery
                {
                    PageSize = 500,
                    PageIndex = 1,
                    OrderBy = ConstTerms.OrderByCreationDateDescending,
                    ResearcherGuid = researcherGuid
                });

            var model = new VacancyApplicationsInResearcherIndexViewModel();

            if (source.TotalItems > 0)
            {
                model.Applications = Mapper.Map<Page<VacancyApplicationDetailsViewModel>>(source);
                var innerObjects = _mediator.Send(new SelectPagedVacanciesByGuidsQuery
                {
                    VacanciesGuids = model.Applications.Items.Select(c => c.VacancyGuid).Distinct(),
                    PageSize = 500,
                    PageIndex = 1,
                    OrderBy = ConstTerms.OrderByDateDescending
                });
                model.Applications.Items.ForEach(
                    c =>
                        c.Vacancy = Mapper.Map<VacancyDetailsViewModel>(innerObjects.Items.Single(d => d.Guid == c.VacancyGuid)));
            }

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
                Vacancies = _mediator.Send(new SelectPagedFavoriteVacanciesByResearcherQuery { PageSize = 500, PageIndex = 1, OrderBy = ConstTerms.OrderByCreationDateDescending, ResearcherGuid = researcherGuid })
            };
            return View(model);
        }

        [SiblingPage]
        [PageTitle("Подписки")]
        [BindResearcherIdFromClaims]
        public ViewResult Subscriptions(Guid researcherGuid)
        {
            var model = new SearchSubscriptionsInResearcherIndexViewModel
            {
                PagedItems = _mediator.Send(new SelectPagedSearchSubscriptionsQuery
                {
                    ResearcherGuid = researcherGuid,
                    PageIndex = 1,
                    PageSize = 10
                })
            };
            return View(model);
        }


        [SiblingPage]
        [PageTitle("Уведомления")]
        [BindResearcherIdFromClaims]
        public ViewResult Notifications(Guid researcherGuid)
        {
            var model = new NotificationsInResearcherIndexViewModel
            {
                PagedItems = _mediator.Send(new SelectPagedNotificationsByResearcherQuery
                {
                    ResearcherGuid = researcherGuid,
                    PageIndex = 1,
                    PageSize = 10
                })
            };
            return View(model);
        }

        [BindResearcherIdFromClaims]
        public ActionResult AddToFavorite(Guid researcherGuid, Guid vacancyGuid)
        {
            if (researcherGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(researcherGuid));
            if (vacancyGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(vacancyGuid));


            var model = _mediator.Send(new SingleVacancyQuery { VacancyGuid = vacancyGuid });
            //если заявка на готовится к открытию или открыта
            if (model.Status == VacancyStatus.AppliesAcceptance || model.Status == VacancyStatus.Published)
            {
                //если есть GUID Исследователя
                var favoritesVacancies = _mediator.Send(new SelectPagedFavoriteVacanciesByResearcherQuery { PageSize = 500, PageIndex = 1, ResearcherGuid = researcherGuid, OrderBy = ConstTerms.OrderByDateAscending });
                //если текущей вакансии нет в списке избранных
                if (favoritesVacancies == null
                    || favoritesVacancies.TotalItems == 0
                    || !favoritesVacancies.Items.Select(c => c.Guid).ToList().Contains(vacancyGuid))
                    _mediator.Send(new AddVacancyToFavoritesCommand { ResearcherGuid = researcherGuid, VacancyGuid = vacancyGuid });
            }

            return Redirect(Context.Request.Headers["referer"]);
        }

        [BindResearcherIdFromClaims]
        [PageTitle("Управление избранными вакансиями")]
        public IActionResult RemoveFavorite(Guid vacancyGuid, Guid researcherGuid)
        {
            if (researcherGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(researcherGuid));
            if (vacancyGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(vacancyGuid));

            var favoritesVacancies = _mediator.Send(new SelectPagedFavoriteVacanciesByResearcherQuery { PageSize = 500, PageIndex = 1, ResearcherGuid = researcherGuid, OrderBy = ConstTerms.OrderByDateAscending });
            if (favoritesVacancies == null)
                throw new Exception("У вас нет избранных вакансий");
            if (favoritesVacancies.Items.All(c => c.Guid != vacancyGuid))
                throw new Exception($"У вас нет избранной вакансии с идентификатором {vacancyGuid}");

            _mediator.Send(new RemoveVacancyFromFavoritesCommand { ResearcherGuid = researcherGuid, VacancyGuid = vacancyGuid });

            return Redirect(Context.Request.Headers["referer"]);
        }


    }
}
