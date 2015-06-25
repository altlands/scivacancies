using System;
using AutoMapper;
using MediatR;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using SciVacancies.ReadModel;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Engine.CustomAttribute;
using SciVacancies.WebApp.Queries;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Controllers
{
    [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
    public class OrganizationsController : Controller
    {
        private readonly IReadModelService _readModelService;
        private readonly IMediator _mediator;

        public OrganizationsController(IReadModelService readModelService, IMediator mediator)
        {
            _readModelService = readModelService;
            _mediator = mediator;
        }

        [AllowAnonymous]
        [PageTitle("Карточка организации")]
        public ViewResult Card() => View();

        [SiblingPage]
        [PageTitle("Информация")]
        [BindOrganizationIdFromClaims]
        public IActionResult Account(Guid organizationGuid)
        {
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var model = Mapper.Map<OrganizationDetailsViewModel>(_mediator.Send(new SingleOrganizationQuery {OrganizationGuid = organizationGuid }));

            if (model == null)
                return RedirectToAction("logout", "account");

            return View(model);
        }

        [SiblingPage]
        [PageTitle("Вакансии")]
        [BindOrganizationIdFromClaims]
        public ViewResult Vacancies(Guid organizationGuid)
        {
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var model = new VacanciesInOrganizationIndexViewModel
            {
                OrganizationGuid = organizationGuid,
                Positions = _readModelService.SelectPositions(organizationGuid),
                Vacancies = _readModelService.SelectVacancies(organizationGuid)
            };

            return View(model);
        }

        [PageTitle("Закрытые вакансии")]
        [SiblingPage]
        public ViewResult Closed(Guid organizationGuid)
        {
            var closedVacacies = _mediator.Send(new SelectPagedClosedVacanciesByOrganizationQuery
            {
                OrganizationGuid = organizationGuid,
                PageIndex = 1,
                PageSize = 10
            });

            var model = new OrganizationDetailsViewModel();
            return View(model);
        }

        [SiblingPage]
        [PageTitle("Уведомления")]
        public ViewResult Notifications(Guid organizationGuid)
        {
            var notifications = _mediator.Send(new SelectPagedNotificationsByOrganizationQuery
            {
                OrganizationGuid = organizationGuid,
                PageIndex = 1,
                PageSize = 0
            });

            var model = new OrganizationDetailsViewModel();
            return View(model);
        }
    }
}
