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
        public ViewResult Account(Guid organizationGuid)
        {
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var model = Mapper.Map<OrganizationDetailsViewModel>(_readModelService.SingleOrganization(organizationGuid));
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
