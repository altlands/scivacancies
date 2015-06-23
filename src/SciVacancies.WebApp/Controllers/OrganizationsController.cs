using System;
using AutoMapper;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using SciVacancies.ReadModel;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Engine.CustomAttribute;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Controllers
{
    [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
    public class OrganizationsController : Controller
    {
        private readonly IReadModelService _readModelService;

        public OrganizationsController(IReadModelService readModelService)
        {
            _readModelService = readModelService;
        }

        [AllowAnonymous]
        [PageTitle("Карточка организации")]
        public ViewResult Card() => View();

        [SiblingPage]
        [PageTitle("Информация")]
        [BindOrganizationIdFromClaims]
        public ViewResult Account(Guid organizationId)
        {
            if (organizationId == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationId));

            var model = Mapper.Map<OrganizationDetailsViewModel>(_readModelService.SingleOrganization(organizationId));
            return View(model);
        }

        [SiblingPage]
        [PageTitle("Вакансии")]
        [BindOrganizationIdFromClaims]
        public ViewResult Vacancies(Guid organizationId)
        {
            if (organizationId == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationId));

            var model = new VacanciesInOrganizationIndexViewModel
            {
                OrganizationGuid = organizationId,
                Positions = _readModelService.SelectPositions(organizationId),
                Vacancies = _readModelService.SelectVacancies(organizationId)
            };

            return View(model);
        }

        [PageTitle("Закрытые вакансии")]
        [SiblingPage]
        public ViewResult Closed()
        {
            var model = new OrganizationDetailsViewModel();
            return View(model);
        }

        [SiblingPage]
        [PageTitle("Уведомления")]
        public ViewResult Notifications()
        {
            var model = new OrganizationDetailsViewModel();
            return View(model);
        }
    }
}
