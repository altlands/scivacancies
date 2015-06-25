using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel;
using SciVacancies.ReadModel.Core;
using SciVacancies.WebApp.Commands;
using SciVacancies.WebApp.Queries;

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
        [PageTitle("Подробно о вакансии")]
        [BindResearcherIdFromClaims]
        [BindOrganizationIdFromClaims]
        public ViewResult Details(Guid id, Guid researcherGuid, Guid organizationGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var model = _readModelService.SingleVacancy(id);

            ViewBag.ShowAddFavorite = false;
            ViewBag.VacancyInFavorites = false;
            if (organizationGuid != Guid.Empty)
                ViewBag.CurrentOrganizationGuid = organizationGuid;

            //если заявка на готовится к открытию или открыта
            if ((model.Status == VacancyStatus.AppliesAcceptance || model.Status == VacancyStatus.Published)
                && researcherGuid != Guid.Empty)
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

            return View(model);
        }

        [AllowAnonymous]
        [PageTitle("Подробно о вакансии")]
        public ViewResult Preview(Guid id) => View();

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

    }
}
