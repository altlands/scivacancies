using System;
using System.Data.Entity.Core;
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
    [Authorize]
    public class ApplicationsController : Controller
    {
        private readonly IMediator _mediator;

        public ApplicationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = ConstTerms.RequireRoleResearcher)]
        [PageTitle("Новая заявка")]
        [BindResearcherIdFromClaims]
        public ViewResult Create(Guid researcherGuid, Guid vacancyGuid)
        {
            if (researcherGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(researcherGuid));
            if (vacancyGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(vacancyGuid));

            var researcher = _mediator.Send(new SingleResearcherQuery { ResearcherGuid = researcherGuid });
            var vacancy = _mediator.Send(new SingleVacancyQuery { VacancyGuid = vacancyGuid });

            var model = new VacancyApplicationCreateViewModel
            {
                ResearcherGuid = researcherGuid,
                VacancyGuid = vacancyGuid,
                PositionName = vacancy.Name,
                Email = researcher.Email,
                Phone = researcher.Phone,
                ResearcherFullName = $"{researcher.SecondName} {researcher.FirstName} {researcher.Patronymic}",
                ResearchActivity = researcher.ResearchActivity,
                TeachingActivity = researcher.TeachingActivity,
                OtherActivity = researcher.OtherActivity,
                ScienceDegree = researcher.ScienceDegree,
                AcademicStatus = researcher.AcademicStatus,
                Rewards = researcher.Rewards
            };

            return View(model);
        }


        [Authorize(Roles = ConstTerms.RequireRoleResearcher)]
        [HttpPost]
        public ActionResult Create(VacancyApplicationCreateViewModel model)
        {
            var data = Mapper.Map<VacancyApplicationDataModel>(model);
            var vacancyApplicationGuid = _mediator.Send(new CreateVacancyApplicationTemplateCommand
            {
                ResearcherGuid = model.ResearcherGuid,
                VacancyGuid = model.VacancyGuid,
                Data = data
            });
            return RedirectToAction("applications", "researchers", new { id = vacancyApplicationGuid });
        }

        [Authorize(Roles = ConstTerms.RequireRoleResearcher)]
        [PageTitle("Детали заявки")]
        [BindResearcherIdFromClaims]
        public ViewResult Details(Guid id, Guid researcherGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var preModel = _mediator.Send(new SingleVacancyApplicationQuery { VacancyApplicationGuid = id });

            if (preModel == null)
                throw new ObjectNotFoundException($"Не найденая Заявка c идентификатором: {id}");

            if (researcherGuid != Guid.Empty
                && User.IsInRole(ConstTerms.RequireRoleResearcher)
                && preModel.ResearcherGuid != researcherGuid)
                throw new Exception("Вы не можете просматривать Заявки других соискателей.");

            var model = Mapper.Map<ApplicationDetailsViewModel>(preModel);

            return View(model);
        }

        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        [PageTitle("Детали заявки")]
        [BindOrganizationIdFromClaims]
        public ViewResult ApplicationInVacancy(Guid id, Guid organizationGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var preModel = _mediator.Send(new SingleVacancyApplicationQuery { VacancyApplicationGuid = id });

            if (preModel == null)
                throw new ObjectNotFoundException($"Не найденая Заявка c идентификатором: {id}");

            if (organizationGuid != Guid.Empty
                && User.IsInRole(ConstTerms.RequireRoleOrganizationAdmin))
                if (_mediator.Send(new SingleVacancyQuery { VacancyGuid = preModel.VacancyGuid }).OrganizationGuid != organizationGuid)
                    throw new Exception("Вы не можете просматривать Заявки, поданные на вакансии других организаций.");

            var model = Mapper.Map<ApplicationDetailsViewModel>(preModel);

            return View(model);
        }

        [PageTitle("Удаление шаблона заявки")]
        [Authorize(Roles = ConstTerms.RequireRoleResearcher)]
        [BindResearcherIdFromClaims]
        public IActionResult Delete(Guid id, Guid researcherGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            if (researcherGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(researcherGuid));

            var vacancyApplication = _mediator.Send(new SingleVacancyApplicationQuery { VacancyApplicationGuid = id });

            if (vacancyApplication.ResearcherGuid != researcherGuid)
                throw new Exception("Можно удалять только свои шаблоны заявок");

            if (vacancyApplication.Status != VacancyApplicationStatus.InProcess)
                throw new Exception($"Отменить заявку можно только в статусе: {VacancyApplicationStatus.InProcess.GetDescription()}.");

            _mediator.Send(new RemoveVacancyApplicationTemplateCommand { ResearcherGuid = researcherGuid, VacancyApplicationGuid = id });

            return View();
        }

        [PageTitle("Отмена заявки")]
        [Authorize(Roles = ConstTerms.RequireRoleAdministrator)]
        public IActionResult Cancel(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var vacancyApplication = _mediator.Send(new SingleVacancyApplicationQuery { VacancyApplicationGuid = id });

            if (vacancyApplication.Status != VacancyApplicationStatus.Applied)
                throw new Exception($"Отменить заявку можно только в статусе: {VacancyApplicationStatus.Applied.GetDescription()}.");

            _mediator.Send(new CancelVacancyApplicationCommand { ResearcherGuid = vacancyApplication.ResearcherGuid, VacancyApplicationGuid = id });

            return View();
        }
    }
}
