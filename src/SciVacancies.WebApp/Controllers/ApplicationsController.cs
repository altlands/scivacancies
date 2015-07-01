using System;
using System.Data.Entity.Core;
using System.Linq;
using AutoMapper;
using MediatR;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel.Core;
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

            //TODO: оптимизировать запрос и его обработку
            var appliedVacancyApplications =
                _mediator.Send(new SelectPagedVacancyApplicationsByVacancyQuery
                {
                    PageSize = 1000,
                    VacancyGuid = vacancyGuid,
                    PageIndex = 1,
                    OrderBy = ConstTerms.OrderByDateAscending
                });

            if (appliedVacancyApplications.Items.Count > 0
                && appliedVacancyApplications.Items.Where(c => c.Status == VacancyApplicationStatus.Applied).Select(c => c.ResearcherGuid).Distinct().ToList().Any(c => c == researcherGuid))
                throw new Exception("Вы не можете подать повторную Заявку на Вакансию ");

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
        [BindResearcherIdFromClaims]
        public ActionResult Create(VacancyApplicationCreateViewModel model, Guid researcherGuid)
        {
            if (model.VacancyGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(model.VacancyGuid), "Не указан идентификатор Вакансии");

            var vacancyData = _mediator.Send(new SingleVacancyQuery { VacancyGuid = model.VacancyGuid });

            if (vacancyData.Status != VacancyStatus.AppliesAcceptance)
                throw new Exception($"Вы не можете подать Заявку на Вакансию в статусе: {vacancyData.Status.GetDescription()}");

            //TODO: оптимизировать запрос и его обработку
            var appliedVacancyApplications =
                _mediator.Send(new SelectPagedVacancyApplicationsByVacancyQuery
                {
                    PageSize = 1000,
                    VacancyGuid = model.VacancyGuid,
                    PageIndex = 1,
                    OrderBy = ConstTerms.OrderByDateAscending
                });

            if (appliedVacancyApplications.Items.Count > 0
                && appliedVacancyApplications.Items.Where(c => c.Status == VacancyApplicationStatus.Applied).Select(c => c.ResearcherGuid).Distinct().ToList().Any(c => c == researcherGuid))
                throw new Exception("Вы не можете подать повторную Заявку на Вакансию ");

            if (!ModelState.IsValid)
            {
                var researcher = _mediator.Send(new SingleResearcherQuery { ResearcherGuid = researcherGuid });
                model.PositionName = vacancyData.Name;
                model.Email = researcher.Email;
                model.Phone = researcher.Phone;
                model.ResearcherFullName = $"{researcher.SecondName} {researcher.FirstName} {researcher.Patronymic}";
                model.ScienceDegree = researcher.ScienceDegree;
                model.AcademicStatus = researcher.AcademicStatus;
                model.Rewards = researcher.Rewards;
                return View(model);
            }


            var data = Mapper.Map<VacancyApplicationDataModel>(model);
            var vacancyApplicationGuid = _mediator.Send(new CreateVacancyApplicationTemplateCommand { ResearcherGuid = model.ResearcherGuid, VacancyGuid = model.VacancyGuid, Data = data });
            //TODO: Application -> Publish : стоит ли делать отдельную команду Сохранить_И_Опубликовать
            _mediator.Send(new ApplyVacancyApplicationCommand
            {
                ResearcherGuid = researcherGuid,
                VacancyApplicationGuid = vacancyApplicationGuid
            });

            return RedirectToAction("details", "applications", new { id = vacancyApplicationGuid });
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

            var model = Mapper.Map<VacancyApplicationDetailsViewModel>(preModel);
            model.Researcher = Mapper.Map<ResearcherDetailsViewModel>(_mediator.Send(new SingleResearcherQuery { ResearcherGuid = researcherGuid }));

            return View(model);
        }

        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        [PageTitle("Детали заявки")]
        [BindOrganizationIdFromClaims]
        public ViewResult Preview(Guid id, Guid organizationGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var preModel = _mediator.Send(new SingleVacancyApplicationQuery { VacancyApplicationGuid = id });

            if (preModel == null)
                throw new ObjectNotFoundException($"Не найденая Заявка c идентификатором: {id}");

            if (preModel.Status == VacancyApplicationStatus.InProcess
                || preModel.Status == VacancyApplicationStatus.Cancelled
                || preModel.Status == VacancyApplicationStatus.Lost
                || preModel.Status == VacancyApplicationStatus.Removed)
                throw new Exception($"Вы не можете просматривать Заявку со статусом: {preModel.Status.GetDescription()}");

            var vacancy = _mediator.Send(new SingleVacancyQuery { VacancyGuid = preModel.VacancyGuid });
            if (vacancy.OrganizationGuid != organizationGuid)
                throw new Exception("Вы не можете изменять Заявки, поданные на вакансии других организаций.");

            var model = Mapper.Map<VacancyApplicationDetailsViewModel>(preModel);
            model.Researcher = Mapper.Map<ResearcherDetailsViewModel>(_mediator.Send(new SingleResearcherQuery { ResearcherGuid = preModel.ResearcherGuid }));
            model.Vacancy = Mapper.Map<VacancyDetailsViewModel>(vacancy);

            return View(model);
        }

        [PageTitle("Выбор победителя")]
        [BindOrganizationIdFromClaims]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        public IActionResult SetWinner(Guid id, Guid organizationGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var vacancyApplicaiton = _mediator.Send(new SingleVacancyApplicationQuery { VacancyApplicationGuid = id });

            if (vacancyApplicaiton == null)
                throw new ObjectNotFoundException($"Не найденая Заявка c идентификатором: {id}");

            if (vacancyApplicaiton.Status != VacancyApplicationStatus.Applied)
                throw new Exception($"Вы не можете выбрать в качестве одного из победителей Заявку со статусом: {vacancyApplicaiton.Status.GetDescription()}");

            var vacancy = _mediator.Send(new SingleVacancyQuery { VacancyGuid = vacancyApplicaiton.VacancyGuid });
            if (vacancy.OrganizationGuid != organizationGuid)
                throw new Exception("Вы не можете изменять Заявки, поданные на вакансии других организаций.");

            if (vacancy.Status != VacancyStatus.InCommittee)
                throw new Exception($"Вы не можете выбирать победителя для Заявки со статусом: {vacancy.Status.GetDescription()}");

            var model = Mapper.Map<VacancyApplicationSetWinnerViewModel>(vacancyApplicaiton);
            model.Vacancy = Mapper.Map<VacancyDetailsViewModel>(vacancy);
            model.Researcher = Mapper.Map<ResearcherDetailsViewModel>(_mediator.Send(new SingleResearcherQuery { ResearcherGuid = vacancyApplicaiton.ResearcherGuid }));

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PageTitle("Выбор победителя")]
        [BindOrganizationIdFromClaims]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        public IActionResult SetWinner(VacancyApplicationSetWinnerViewModel model, Guid organizationGuid)
        {
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var vacancyApplicaiton = _mediator.Send(new SingleVacancyApplicationQuery { VacancyApplicationGuid = model.Guid });
            var vacancy = _mediator.Send(new SingleVacancyQuery { VacancyGuid = vacancyApplicaiton.VacancyGuid });

            if (vacancyApplicaiton.Status != VacancyApplicationStatus.Applied)
                throw new Exception($"Вы не можете выбрать в качестве одного из победителей Заявку со статусом: {vacancyApplicaiton.Status.GetDescription()}");

            if (vacancy.OrganizationGuid != organizationGuid)
                throw new Exception("Вы не можете изменять Заявки, поданные на вакансии других организаций.");

            if (vacancy.Status != VacancyStatus.InCommittee)
                throw new Exception($"Вы не можете выбирать победителя для Заявки со статусом: {vacancy.Status.GetDescription()}");

            if (ModelState.IsValid)
            {
                if (model.IsWinner)
                    _mediator.Send(new SetVacancyWinnerCommand { VacancyGuid = model.VacancyGuid, ResearcherGuid = model.ResearcherGuid, OrganizationGuid = organizationGuid, VacancyApplicationGuid = model.Guid, Reason = model.Reason });
                else
                    _mediator.Send(new SetVacancyPretenderCommand { VacancyGuid = model.VacancyGuid, ResearcherGuid = model.ResearcherGuid, OrganizationGuid = organizationGuid, VacancyApplicationGuid = model.Guid, Reason = model.Reason });

                return RedirectToAction("preview", "applications", new { id = model.Guid });
            }
            //TODO - а эта часть для чего?
            model = Mapper.Map<VacancyApplicationSetWinnerViewModel>(vacancyApplicaiton);
            model.Vacancy = Mapper.Map<VacancyDetailsViewModel>(vacancy);
            model.Researcher = Mapper.Map<ResearcherDetailsViewModel>(_mediator.Send(new SingleResearcherQuery { ResearcherGuid = vacancyApplicaiton.ResearcherGuid }));

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

            var preModel = _mediator.Send(new SingleVacancyApplicationQuery { VacancyApplicationGuid = id });

            if (preModel.Status != VacancyApplicationStatus.Applied)
                throw new Exception($"Отменить заявку можно только в статусе: {VacancyApplicationStatus.InProcess.GetDescription()}.");

            var model = Mapper.Map<VacancyApplicationDetailsViewModel>(preModel);
            model.Researcher = Mapper.Map<ResearcherDetailsViewModel>(_mediator.Send(new SingleResearcherQuery { ResearcherGuid = model.ResearcherGuid }));
            model.Vacancy= Mapper.Map<VacancyDetailsViewModel>(_mediator.Send(new SingleVacancyQuery{VacancyGuid= model.VacancyGuid}));
            return View(model);
        }

        [HttpPost]
        [PageTitle("Удаление шаблона заявки")]
        [Authorize(Roles = ConstTerms.RequireRoleResearcher)]
        [BindResearcherIdFromClaims]
        public IActionResult DeletePost(Guid id, Guid researcherGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            if (researcherGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(researcherGuid));

            var preModel = _mediator.Send(new SingleVacancyApplicationQuery { VacancyApplicationGuid = id });

            if (preModel.Status != VacancyApplicationStatus.Applied)
                throw new Exception($"Отменить заявку можно только в статусе: {VacancyApplicationStatus.InProcess.GetDescription()}.");

            if (!ModelState.IsValid)
            {
                var model = Mapper.Map<VacancyApplicationDetailsViewModel>(preModel);
                model.Researcher = Mapper.Map<ResearcherDetailsViewModel>(_mediator.Send(new SingleResearcherQuery { ResearcherGuid = model.ResearcherGuid }));
                model.Vacancy = Mapper.Map<VacancyDetailsViewModel>(_mediator.Send(new SingleVacancyQuery { VacancyGuid = model.VacancyGuid }));
                return View("delete", model);
            }

            _mediator.Send(new CancelVacancyApplicationCommand{ ResearcherGuid = researcherGuid, VacancyApplicationGuid = id });
            return RedirectToAction("applications", "researchers");
        }
    }
}
