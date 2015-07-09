using System;
using AutoMapper;
using MediatR;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Enums;
using SciVacancies.WebApp.Commands;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Queries;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Controllers
{
    [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
    public class PositionsController : Controller
    {
        private readonly IMediator _mediator;

        public PositionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [PageTitle("Подробно о вакансии")]
        [BindOrganizationIdFromClaims]
        public IActionResult Details(Guid id, Guid organizationGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var model = _mediator.Send(new SinglePositionQuery { PositionGuid = id });

            if (model.Status == PositionStatus.Published)
                RedirectToAction("details", "vacancies");

            if (model.OrganizationGuid != organizationGuid)
                throw new Exception("Вы не можете просматривать вакансии других организаций");

            return View(model);
        }

        [PageTitle("Вакансия удалена")]
        [BindOrganizationIdFromClaims]
        public IActionResult Delete(Guid id, Guid organizationGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var model = _mediator.Send(new SinglePositionQuery { PositionGuid = id });

            if (model.OrganizationGuid != organizationGuid)
                throw new Exception("Вы не можете отменить удаление вакансии других организаций");

            if (model.Status == PositionStatus.Removed)
                throw new Exception("Вакансия уже удалена");

            if (model.Status == PositionStatus.Published)
                throw new Exception($"Вы не можете удалить вакансию с текущим статусом: {model.Status.GetDescription()}");

            _mediator.Send(new RemovePositionCommand { OrganizationGuid = organizationGuid, PositionGuid = id });

            return View();
        }

        [BindOrganizationIdFromClaims]
        public IActionResult Publish(Guid id, Guid organizationGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var model = _mediator.Send(new SinglePositionQuery { PositionGuid = id });

            if (model.OrganizationGuid != organizationGuid)
                throw new Exception("Вы не можете отменить удаление вакансии других организаций");

            if (model.Status == PositionStatus.Removed)
                throw new Exception("Вакансия уже удалена");

            if (model.Status == PositionStatus.Published)
                throw new Exception("Вакансия уже опубликована");

            var vacancyDataModel = Mapper.Map<VacancyDataModel>(model);
            vacancyDataModel.OrganizationName = _mediator.Send(new SingleOrganizationQuery { OrganizationGuid = organizationGuid }).Name;

            var vacancyGuid = _mediator.Send(new PublishVacancyCommand
            {
                PositionGuid = id,
                OrganizationGuid = model.OrganizationGuid,
                Data = vacancyDataModel
            });

            return RedirectToAction("vacancies", "organizations");
        }

        [PageTitle("Копировать вакансию")]
        public IActionResult Copy(Guid id, Guid oreganizationGuid)
        {
            //TODO: Positions -> Copy : реализовать копирование вакансии

            return null;
        }
    }

}
