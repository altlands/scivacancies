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
        [ResponseCache(NoStore = true)]
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
        public IActionResult Card(Guid id, int pageSize = 10, int currentPage = 1)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var preModel = _mediator.Send(new SingleOrganizationQuery { OrganizationGuid = id });

            if (preModel == null)
                return HttpNotFound(); //throw new ObjectNotFoundException($"Не найдена организация по идентификатору: {id}");

            var model = Mapper.Map<OrganizationDetailsViewModel>(preModel);

            model.VacanciesInOrganization = new VacanciesInOrganizationIndexViewModel
            {
                OrganizationGuid = id,
                PagedVacancies = _mediator.Send(new SelectPagedVacanciesByOrganizationQuery { OrganizationGuid = id, PageSize = pageSize, PageIndex = currentPage }).MapToPagedList()
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

        [HttpGet]
        [HttpPost]
        [SiblingPage]
        [PageTitle("Вакансии")]
        [BindOrganizationIdFromClaims]
        public ViewResult Vacancies(Guid organizationGuid, int pageSize = 10, int currentPage = 1)
        {
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var preModel = _mediator.Send(new SingleOrganizationQuery {OrganizationGuid = organizationGuid});

            var model = new VacanciesInOrganizationIndexViewModel
            {
                OrganizationGuid = organizationGuid,
                PagedVacancies = _mediator.Send(new SelectPagedVacanciesByOrganizationQuery { OrganizationGuid = organizationGuid, PageSize = pageSize, PageIndex = currentPage }).MapToPagedList(),
                Name = preModel.name
            };

            return View(model);
        }

        [PageTitle("Закрытые вакансии")]
        [SiblingPage]
        [BindOrganizationIdFromClaims]
        public ViewResult Closed(Guid organizationGuid, int pageSize = 10, int currentPage = 1)
        {
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var preModel = _mediator.Send(new SingleOrganizationQuery { OrganizationGuid = organizationGuid });

            var model = new VacanciesInOrganizationIndexViewModel
            {
                PagedVacancies = _mediator.Send(new SelectPagedClosedVacanciesByOrganizationQuery { OrganizationGuid = organizationGuid, PageSize = pageSize, PageIndex = currentPage }).MapToPagedList(),
                Name = preModel.name
            };
            return View(model);
        }

        [SiblingPage]
        [PageTitle("Уведомления")]
        [BindOrganizationIdFromClaims]
        public ViewResult Notifications(Guid organizationGuid, int pageSize = 10, int currentPage = 1)
        {
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var preModel = _mediator.Send(new SingleOrganizationQuery { OrganizationGuid = organizationGuid });

            var model = new NotificationsInOrganizationIndexViewModel
            {
                PagedItems = _mediator.Send(new SelectPagedOrganizationNotificationsQuery
                {
                    OrganizationGuid = organizationGuid,
                    PageSize = pageSize,
                    PageIndex = currentPage
                }).MapToPagedList(),
                Name = preModel.name
            };
            return View(model);
        }


    }
}
