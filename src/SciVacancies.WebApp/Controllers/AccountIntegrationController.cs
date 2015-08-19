using System;
using MediatR;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using SciVacancies.WebApp.Engine;

namespace SciVacancies.WebApp.Controllers
{
    [Authorize]
    public class AccountIntegrationController: Controller
    {
        private readonly IMediator _mediator;

        public AccountIntegrationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Обновить профиль ислледователя
        /// </summary>
        /// <param name="researcherGuid"></param>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        [Authorize(Roles = ConstTerms.RequireRoleResearcher)]
        [BindResearcherIdFromClaims]
        [PageTitle("Обновление данных профиля")]
        public ViewResult UpdateResearcherAccountFromOutside(Guid researcherGuid, AuthorizeResourceTypes dataSource)
        {
            var model = "Подтвердите обновление данных профиля с внешнего ресурса";
            return View(model: model);
        }

        /// <summary>
        /// Обновить профиль ислледователя
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = ConstTerms.RequireRoleResearcher)]
        [BindResearcherIdFromClaims]
        [PageTitle("Обновление данных профиля")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ViewResult UpdateResearcherAccountFromOutside()
        {
            var model = "Подтвердите обновление данных профиля с внешнего ресурса";
            return View(model: model);
        }

        /// <summary>
        /// Обновить профиль организации
        /// </summary>
        /// <param name="organizationGuid"></param>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        [BindResearcherIdFromClaims]
        [PageTitle("Обновление данных профиля")]
        [BindOrganizationIdFromClaims]
        public ViewResult UpdateOrganizationAccountFromOutside(Guid organizationGuid, AuthorizeResourceTypes dataSource)
        {
            var model = "Подтвердите обновление данных профиля с внешнего ресурса";
            return View(model: model);
        }

        /// <summary>
        /// Обновить профиль организации
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = ConstTerms.RequireRoleResearcher)]
        [BindResearcherIdFromClaims]
        [PageTitle("Обновление данных профиля")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ViewResult UpdateOrganizationAccountFromOutside()
        {
            var model = "Подтвердите обновление данных профиля с внешнего ресурса";
            return View(model: model);
        }


    }
}
