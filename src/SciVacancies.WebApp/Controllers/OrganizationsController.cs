using System;
using System.Data.Entity.Core;
using AutoMapper;
using MediatR;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Engine.CustomAttribute;
using SciVacancies.WebApp.Queries;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Controllers
{
    [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
    public class OrganizationsController : Controller
    {
        private readonly IMediator _mediator;

        public OrganizationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [PageTitle("Карточка организации")]
        public ViewResult Card(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var preModel = _mediator.Send(new SingleOrganizationQuery { OrganizationGuid = id });

            if (preModel == null)
                throw new ObjectNotFoundException($"Не найдена организация по идентификатору: {id}");

            var model = Mapper.Map<OrganizationDetailsViewModel>(preModel);

            model.VacanciesInOrganization = new VacanciesInOrganizationIndexViewModel
            {
                OrganizationGuid = id,
                PagedPositions = _mediator.Send(new SelectPagedPositionsByOrganizationQuery { OrganizationGuid = id, PageSize = 500, PageIndex = 1 }),
                PagedVacancies = _mediator.Send(new SelectPagedVacanciesByOrganizationQuery { OrganizationGuid = id, PageSize = 500, PageIndex = 1 })
            };

            return View(model);
        }

        [SiblingPage]
        [PageTitle("Информация")]
        [BindOrganizationIdFromClaims]
        public IActionResult Account(Guid organizationGuid)
        {
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var model = Mapper.Map<OrganizationDetailsViewModel>(_mediator.Send(new SingleOrganizationQuery { OrganizationGuid = organizationGuid }));

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

            var preModel = _mediator.Send(new SingleOrganizationQuery {OrganizationGuid = organizationGuid});

            var model = new VacanciesInOrganizationIndexViewModel
            {
                OrganizationGuid = organizationGuid,
                PagedPositions = _mediator.Send(new SelectPagedPositionsByOrganizationQuery { OrganizationGuid = organizationGuid, PageSize = 500, PageIndex = 1 }),
                PagedVacancies = _mediator.Send(new SelectPagedVacanciesByOrganizationQuery { OrganizationGuid = organizationGuid, PageSize = 500, PageIndex = 1 }),
                Name = preModel.Name
            };

            return View(model);
        }

        [PageTitle("Закрытые вакансии")]
        [SiblingPage]
        [BindOrganizationIdFromClaims]
        public ViewResult Closed(Guid organizationGuid)
        {
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var preModel = _mediator.Send(new SingleOrganizationQuery { OrganizationGuid = organizationGuid });

            var model = new VacanciesInOrganizationIndexViewModel
            {
                PagedVacancies = _mediator.Send(new SelectPagedClosedVacanciesByOrganizationQuery
                {
                    OrganizationGuid = organizationGuid,
                    PageIndex = 1,
                    PageSize = 500
                }),
                Name = preModel.Name
            };
            return View(model);
        }

        [SiblingPage]
        [PageTitle("Уведомления")]
        [BindOrganizationIdFromClaims]
        public ViewResult Notifications(Guid organizationGuid)
        {
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var preModel = _mediator.Send(new SingleOrganizationQuery { OrganizationGuid = organizationGuid });

            var model = new NotificationsInOrganizationIndexViewModel
            {
                PagedNotifications = _mediator.Send(new SelectPagedNotificationsByOrganizationQuery
                {
                    OrganizationGuid = organizationGuid,
                    PageIndex = 1,
                    PageSize = 500
                }),
                Name = preModel.Name
            };
            return View(model);
        }


    }
}
