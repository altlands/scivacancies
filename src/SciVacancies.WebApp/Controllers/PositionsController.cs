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

        [PageTitle("Изменить вакансию")]
        [BindOrganizationIdFromClaims]
        public IActionResult Edit(Guid id, Guid organizationGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var preModel = _mediator.Send(new SinglePositionQuery { PositionGuid = id });

            if (preModel.OrganizationGuid != organizationGuid)
                throw new Exception("Вы не можете изменять вакансии других организаций");

            if (preModel.Status != PositionStatus.InProcess)
                throw new Exception($"Вы не можете изменить вакансию с текущим статусом: {preModel.Status.GetDescription()}");

            var model = Mapper.Map<PositionEditViewModel>(preModel);
            model.InitDictionaries(_mediator);

            return View(model);
        }

        [PageTitle("Изменить вакансию")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [BindOrganizationIdFromClaims("claimedUserGuid")]
        public IActionResult Edit(PositionEditViewModel model, Guid claimedUserGuid)
        {
            if (claimedUserGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(claimedUserGuid));

            if (model.OrganizationGuid != claimedUserGuid)
                throw new Exception("Вы не можете изменять вакансии других организаций");

            if (ModelState.IsValid)
            {
                var positionDataModel = Mapper.Map<PositionDataModel>(model);

                if (positionDataModel.Status != PositionStatus.InProcess)
                    throw new Exception($"Вы не можете изменить вакансию с текущим статусом: {model.Status.GetDescription()}");

                var positionGuid = _mediator.Send(new UpdatePositionCommand { PositionGuid = model.Guid, OrganizationGuid = model.OrganizationGuid, Data = positionDataModel });

                if (!model.ToPublish)
                    return RedirectToAction("details", "positions", new {id = positionGuid});

                var vacancyDataModel = Mapper.Map<VacancyDataModel>(positionDataModel);
                vacancyDataModel.OrganizationName = _mediator.Send(new SingleOrganizationQuery { OrganizationGuid = model.OrganizationGuid }).Name;

                var vacancyGuid = _mediator.Send(new PublishVacancyCommand
                {
                    OrganizationGuid = model.OrganizationGuid,
                    PositionGuid = model.Guid,
                    Data = vacancyDataModel
                });
                return RedirectToAction("details", "vacancies", new { id = vacancyGuid });
            }
            model.InitDictionaries(_mediator);
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
    }

}
