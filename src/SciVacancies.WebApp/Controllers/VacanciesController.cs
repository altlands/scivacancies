using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using AutoMapper;
using MediatR;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using NPoco;
using SciVacancies.Domain.Enums;
using SciVacancies.WebApp.Commands;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Queries;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Controllers
{
    /// <summary>
    /// страница с вакансиями (конкурсами)
    /// </summary>
    [Authorize]
    public class VacanciesController : Controller
    {
        private readonly IMediator _mediator;

        public VacanciesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [PageTitle("Подробно о вакансии")]
        [BindResearcherIdFromClaims]
        [BindOrganizationIdFromClaims]
        public ViewResult Details(Guid id, Guid researcherGuid, Guid organizationGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var preModel = _mediator.Send(new SingleVacancyQuery { VacancyGuid = id });

            var model = Mapper.Map<VacancyDetailsViewModel>(preModel);

            if (model == null)
                throw new ObjectNotFoundException($"Не найдена вакансия с Guid: {id}");

            ViewBag.ShowAddFavorite = false;
            ViewBag.VacancyInFavorites = false;
            if (organizationGuid != Guid.Empty)
                ViewBag.CurrentOrganizationGuid = organizationGuid;

            //если заявка на готовится к открытию или открыта
            if ((model.Status == VacancyStatus.AppliesAcceptance || model.Status == VacancyStatus.Published)
                && researcherGuid != Guid.Empty)
            {
                //если есть GUID Исследователя
                var favoritesVacancies = _mediator.Send(new SelectPagedFavoriteVacanciesByResearcherQuery { PageSize = 500, PageIndex = 1, ResearcherGuid = researcherGuid, OrderBy = ConstTerms.OrderByDateAscending });
                //если текущей вакансии нет в списке избранных
                if (favoritesVacancies == null
                    || favoritesVacancies.TotalItems == 0
                    || !favoritesVacancies.Items.Select(c => c.Guid).ToList().Contains(id))
                    ViewBag.ShowAddFavorite = true;
                else
                    ViewBag.VacancyInFavorites = true;
            }

            if (model.Status != VacancyStatus.Published) //Если статус Опубликовано - пройден
                model.Applications = _mediator.Send(new SelectPagedVacancyApplicationsByVacancyQuery
                {
                    PageIndex = 1,
                    PageSize = 3000,
                    VacancyGuid = id,
                    OrderBy = "Guid"
                });

            return View(model);
        }

        [AllowAnonymous]
        [PageTitle("Подробно о вакансии")]
        public ViewResult Preview(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var preModel = _mediator.Send(new SingleVacancyQuery { VacancyGuid = id });

            var model = Mapper.Map<VacancyDetailsViewModel>(preModel);

            if (model == null)
                throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            model.Winner= 
                Mapper.Map<ResearcherDetailsViewModel>(_mediator.Send(new SingleResearcherQuery { ResearcherGuid = preModel.WinnerGuid }));
            model.Pretender = 
                Mapper.Map<ResearcherDetailsViewModel>(_mediator.Send(new SingleResearcherQuery { ResearcherGuid = preModel.PretenderGuid}));

            return View(model);
        }

        [PageTitle("Отменить вакансию")]
        [BindOrganizationIdFromClaims]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        public IActionResult Cancel(Guid id, Guid organizationGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));
            
            var preModel = _mediator.Send(new SingleVacancyQuery { VacancyGuid = id });

            if (preModel.OrganizationGuid != organizationGuid)
                throw new Exception("Вы не можете отменить вакансии других организаций");

            if (preModel.Status != VacancyStatus.Published && preModel.Status != VacancyStatus.AppliesAcceptance)
                throw new Exception($"Вы не можете отменить вакансию со статусом: {preModel.Status.GetDescription()}");

            var model = Mapper.Map<VacancyDetailsViewModel>(preModel);

            return View(model);
        }

        [HttpPost]
        [PageTitle("Вакансия отменена")]
        [ValidateAntiForgeryToken]
        [BindOrganizationIdFromClaims]
        public IActionResult Cancel(Guid id, Guid organizationGuid, string reason)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var model = _mediator.Send(new SingleVacancyQuery { VacancyGuid = id });

            if (model.OrganizationGuid != organizationGuid)
                throw new Exception("Вы не можете отменить вакансии других организаций");

            if (model.Status != VacancyStatus.Published && model.Status != VacancyStatus.AppliesAcceptance)
                throw new Exception($"Вы не можете отменить вакансию со статусом: {model.Status.GetDescription()}");

            _mediator.Send(new CancelVacancyCommand { VacancyGuid = id, OrganizationGuid = organizationGuid, Reason = reason });

            return RedirectToAction("vacancies", "organizations");
        }

        [PageTitle("Начать приём заявок")]
        [BindOrganizationIdFromClaims]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        public IActionResult StartAcceptApplications(Guid id, Guid organizationGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var preModel = _mediator.Send(new SingleVacancyQuery { VacancyGuid = id });

            if (preModel == null)
                throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            if (preModel.OrganizationGuid != organizationGuid)
                throw new Exception("Вы не можете менять Вакансии других организаций");

            if (preModel.Status != VacancyStatus.Published)
                throw new Exception($"Вы не можете начать включить режим приём Заявок для Вакансии со статусом: {preModel.Status.GetDescription()}");

            _mediator.Send(new SwitchVacancyToAcceptApplicationsCommand
            {
                OrganizationGuid = organizationGuid,
                VacancyGuid = id
            });

            return View();
        }

        [PageTitle("На рассмотрении")]
        [BindOrganizationIdFromClaims]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        public IActionResult StartInCommittee(Guid id, Guid organizationGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var preModel = _mediator.Send(new SingleVacancyQuery { VacancyGuid = id });

            if (preModel == null)
                throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            if (preModel.OrganizationGuid != organizationGuid)
                throw new Exception("Вы не можете менять Вакансии других организаций");

            if (preModel.Status != VacancyStatus.AppliesAcceptance)
                throw new Exception($"Вы не можете Вакансию на рассмотрение комиссии со статусом: {preModel.Status.GetDescription()}");

            _mediator.Send(new SwitchVacancyInCommitteeCommand
            {
                OrganizationGuid = organizationGuid,
                VacancyGuid = id
            });

            return View();
        }

        [PageTitle("Выбор победителя")]
        [BindOrganizationIdFromClaims]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        public IActionResult SetWinners(Guid id, Guid organizationGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var preModel = _mediator.Send(new SingleVacancyQuery { VacancyGuid = id });

            if (preModel == null)
                throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            if (preModel.OrganizationGuid != organizationGuid)
                throw new Exception("Вы не можете менять Вакансии других организаций");

            if (preModel.Status != VacancyStatus.InCommittee)
                throw new Exception($"Вы не можете выбрать победителя по Заявке со статусом: {preModel.Status.GetDescription()}");

            _mediator.Send(new SwitchVacancyToAcceptApplicationsCommand
            {
                OrganizationGuid = organizationGuid,
                VacancyGuid = id
            });

            return View();
        }

    }
}
