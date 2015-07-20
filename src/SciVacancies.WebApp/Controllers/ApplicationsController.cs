using System;
using System.Collections.Generic;
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
    [ResponseCache(NoStore = true)]
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
            researcher.educations = _mediator.Send(new SelectResearcherEducationsQuery { ResearcherGuid = researcherGuid }).ToList();
            researcher.publications = _mediator.Send(new SelectResearcherPublicationsQuery { ResearcherGuid = researcherGuid }).ToList();
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
                && appliedVacancyApplications.Items.Where(c => c.status == VacancyApplicationStatus.Applied).Select(c => c.researcher_guid).Distinct().ToList().Any(c => c == researcherGuid))
                throw new Exception("Вы не можете подать повторную Заявку на Вакансию ");

            var model = new VacancyApplicationCreateViewModel
            {
                ResearcherGuid = researcherGuid,
                VacancyGuid = vacancyGuid,
                PositionName = vacancy.name,
                Email = researcher.email,
                Phone = researcher.phone,
                ResearcherFullName = $"{researcher.secondname} {researcher.firstname} {researcher.patronymic}",
                Educations = researcher.educations,
                Publications = researcher.publications,
                ResearchActivity = researcher.research_activity,
                TeachingActivity = researcher.teaching_activity,
                OtherActivity = researcher.other_activity,
                ScienceDegree = researcher.science_degree,
                ScienceRank = researcher.science_rank,
                Rewards = researcher.rewards
            };
            //TODO: Applications -> Create : вернуть добавление дополнительнительных публикаций
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

            if (vacancyData.status != VacancyStatus.Published)
                throw new Exception($"Вы не можете подать Заявку на Вакансию в статусе: {vacancyData.status.GetDescription()}");

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
                && appliedVacancyApplications.Items.Where(c => c.status == VacancyApplicationStatus.Applied).Select(c => c.researcher_guid).Distinct().ToList().Any(c => c == researcherGuid))
                throw new Exception("Вы не можете подать повторную Заявку на Вакансию ");

            //с формы мы не оплучам практически никакие данные, поэтоу заново наполняем ViewModel
            var researcher = _mediator.Send(new SingleResearcherQuery { ResearcherGuid = researcherGuid });
            researcher.educations = _mediator.Send(new SelectResearcherEducationsQuery { ResearcherGuid = researcherGuid }).ToList();
            researcher.publications = _mediator.Send(new SelectResearcherPublicationsQuery { ResearcherGuid = researcherGuid }).ToList();
            model.Conferences = researcher.conferences;
            model.Educations = researcher.educations;
            model.Email = researcher.email;
            model.ExtraEmail = researcher.extraemail;
            model.ExtraPhone = researcher.extraphone;
            model.OtherActivity = researcher.other_activity;
            model.Phone = researcher.phone;
            model.Publications = researcher.publications;
            model.PositionName = vacancyData.name;
            model.ResearchActivity = researcher.research_activity;
            model.ResearcherFullName = $"{researcher.secondname} {researcher.firstname} {researcher.patronymic}";
            model.ResearcherGuid = researcherGuid;
            model.Rewards = researcher.rewards;
            model.Educations = researcher.educations;
            model.Publications = researcher.publications;
            model.ScienceDegree = researcher.science_degree;
            model.ScienceRank = researcher.science_rank;
            model.TeachingActivity = researcher.teaching_activity;
            model.VacancyGuid = vacancyData.guid;

            if (!ModelState.IsValid)
                return View(model);

            var data = Mapper.Map<VacancyApplicationDataModel>(model);
            var vacancyApplicationGuid = _mediator.Send(new CreateAndApplyVacancyApplicationCommand { ResearcherGuid = model.ResearcherGuid, VacancyGuid = model.VacancyGuid, Data = data });

            return RedirectToAction("details", "applications", new { id = vacancyApplicationGuid });
        }

        [Authorize(Roles = ConstTerms.RequireRoleResearcher)]
        [PageTitle("Детали заявки")]
        [BindResearcherIdFromClaims]
        public IActionResult Details(Guid id, Guid researcherGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var preModel = _mediator.Send(new SingleVacancyApplicationQuery { VacancyApplicationGuid = id });

            if (preModel == null)
                throw new ObjectNotFoundException($"Не найденая Заявка c идентификатором: {id}");

            if (researcherGuid != Guid.Empty
                && User.IsInRole(ConstTerms.RequireRoleResearcher)
                && preModel.researcher_guid != researcherGuid)
                throw new Exception("Вы не можете просматривать Заявки других соискателей.");

            var model = Mapper.Map<VacancyApplicationDetailsViewModel>(preModel);
            model.Researcher = Mapper.Map<ResearcherDetailsViewModel>(_mediator.Send(new SingleResearcherQuery { ResearcherGuid = researcherGuid }));
            model.Researcher.Publications = Mapper.Map<List<PublicationEditViewModel>>(_mediator.Send(new SelectResearcherPublicationsQuery { ResearcherGuid = researcherGuid }));
            model.Researcher.Educations= Mapper.Map<List<EducationEditViewModel>>(_mediator.Send(new SelectResearcherEducationsQuery { ResearcherGuid = researcherGuid }));
            model.Vacancy = Mapper.Map<VacancyDetailsViewModel>(_mediator.Send(new SingleVacancyQuery { VacancyGuid = preModel.vacancy_guid }));
            //TODO: ntemnikov : показать Приложенные файлы
            //TODO: ntemnikov : показать Научные интересы
            return View(model);
        }

        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        [PageTitle("Детали заявки")]
        [BindOrganizationIdFromClaims]
        public IActionResult Preview(Guid id, Guid organizationGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var preModel = _mediator.Send(new SingleVacancyApplicationQuery { VacancyApplicationGuid = id });

            if (preModel == null)
                throw new ObjectNotFoundException($"Не найденая Заявка c идентификатором: {id}");

            if (preModel.status == VacancyApplicationStatus.InProcess
                || preModel.status == VacancyApplicationStatus.Cancelled
                || preModel.status == VacancyApplicationStatus.Lost
                || preModel.status == VacancyApplicationStatus.Removed)
                throw new Exception($"Вы не можете просматривать Заявку со статусом: {preModel.status.GetDescription()}");

            var vacancy = _mediator.Send(new SingleVacancyQuery { VacancyGuid = preModel.vacancy_guid });
            if (vacancy.organization_guid != organizationGuid)
                throw new Exception("Вы не можете изменять Заявки, поданные на вакансии других организаций.");

            var model = Mapper.Map<VacancyApplicationDetailsViewModel>(preModel);
            model.Researcher = Mapper.Map<ResearcherDetailsViewModel>(_mediator.Send(new SingleResearcherQuery { ResearcherGuid = preModel.researcher_guid }));
            model.Researcher.Publications = Mapper.Map<List<PublicationEditViewModel>>(_mediator.Send(new SelectResearcherPublicationsQuery { ResearcherGuid = preModel.researcher_guid }));
            model.Researcher.Educations = Mapper.Map<List<EducationEditViewModel>>(_mediator.Send(new SelectResearcherEducationsQuery { ResearcherGuid = preModel.researcher_guid }));
            model.Vacancy = Mapper.Map<VacancyDetailsViewModel>(vacancy);
            //TODO: ntemnikov : показать Приложенные файлы
            //TODO: ntemnikov : показать Научные интересы
            return View(model);
        }


        private VacancyApplication SetWinnerPreValidation(Guid vacancyApplicationGuid, Guid organizationGuid, out Vacancy vacancy, bool isWinner)
        {
            var vacancyApplicaiton = _mediator.Send(new SingleVacancyApplicationQuery { VacancyApplicationGuid = vacancyApplicationGuid });

            if (vacancyApplicaiton == null)
                throw new ObjectNotFoundException($"Не найденая Заявка c идентификатором: {vacancyApplicationGuid}");

            if (vacancyApplicaiton.status != VacancyApplicationStatus.Applied)
                throw new Exception(
                    $"Вы не можете выбрать в качестве одного из победителей Заявку со статусом: {vacancyApplicaiton.status.GetDescription()}");

            vacancy = _mediator.Send(new SingleVacancyQuery { VacancyGuid = vacancyApplicaiton.vacancy_guid });

            if (vacancy == null)
                throw new ObjectNotFoundException($"Не найденая Вакансия c идентификатором: {vacancyApplicaiton.vacancy_guid}");

            if (vacancy.organization_guid != organizationGuid)
                throw new Exception("Вы не можете изменять Заявки, поданные на вакансии других организаций.");

            if (isWinner && vacancy.winner_researcher_guid != Guid.Empty)
                throw new Exception("Для данной Вакансии уже выбран Победитель.");

            if (!isWinner && vacancy.winner_researcher_guid == Guid.Empty)
                throw new Exception("Для данной Вакансии еще не выбран Победитель.");

            if (vacancy.winner_researcher_guid != Guid.Empty && vacancy.pretender_researcher_guid != Guid.Empty)
                throw new Exception("Для данной Вакансии уже выбраны Победитель и Претендент.");

            if (vacancy.status != VacancyStatus.InCommittee)
                throw new Exception(
                    $"Вы не можете выбирать победителя для Заявки со статусом: {vacancy.status.GetDescription()}");
            return vacancyApplicaiton;
        }

        [PageTitle("Выбор победителя")]
        [BindOrganizationIdFromClaims]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        public IActionResult SetWinner(Guid id, Guid organizationGuid, bool isWinner)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            Vacancy vacancy;
            var vacancyApplication = SetWinnerPreValidation(id, organizationGuid, out vacancy, isWinner);

            var model = Mapper.Map<VacancyApplicationSetWinnerViewModel>(vacancyApplication);
            model.Vacancy = Mapper.Map<VacancyDetailsViewModel>(vacancy);
            model.Researcher = Mapper.Map<ResearcherDetailsViewModel>(_mediator.Send(new SingleResearcherQuery { ResearcherGuid = vacancyApplication.researcher_guid }));
            model.IsWinner = isWinner;
            model.SetWinner = isWinner;

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

            Vacancy vacancy;
            var vacancyApplicaiton = SetWinnerPreValidation(model.Guid, organizationGuid, out vacancy, model.SetWinner);

            if (ModelState.IsValid)
            {
                if (model.IsWinner)
                    _mediator.Send(new SetVacancyWinnerCommand
                    {
                        VacancyGuid = model.VacancyGuid,
                        ResearcherGuid = model.ResearcherGuid,
                        VacancyApplicationGuid = model.Guid,
                        Reason = model.Reason
                    });
                else
                    _mediator.Send(new SetVacancyPretenderCommand
                    {
                        VacancyGuid = model.VacancyGuid,
                        ResearcherGuid = model.ResearcherGuid,
                        VacancyApplicationGuid = model.Guid,
                        Reason = model.Reason
                    });

                return RedirectToAction("preview", "applications", new { id = model.Guid });
            }

            model = Mapper.Map<VacancyApplicationSetWinnerViewModel>(vacancyApplicaiton);
            model.Vacancy = Mapper.Map<VacancyDetailsViewModel>(vacancy);
            model.Researcher = Mapper.Map<ResearcherDetailsViewModel>(_mediator.Send(new SingleResearcherQuery { ResearcherGuid = vacancyApplicaiton.researcher_guid }));

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

            if (preModel.status != VacancyApplicationStatus.Applied)
                throw new Exception($"Отменить заявку можно только в статусе: {VacancyApplicationStatus.InProcess.GetDescription()}.");

            var model = Mapper.Map<VacancyApplicationDetailsViewModel>(preModel);
            model.Researcher = Mapper.Map<ResearcherDetailsViewModel>(_mediator.Send(new SingleResearcherQuery { ResearcherGuid = model.ResearcherGuid }));
            model.Vacancy = Mapper.Map<VacancyDetailsViewModel>(_mediator.Send(new SingleVacancyQuery { VacancyGuid = model.VacancyGuid }));
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

            if (preModel.status != VacancyApplicationStatus.Applied)
                throw new Exception($"Отменить заявку можно только в статусе: {VacancyApplicationStatus.InProcess.GetDescription()}.");

            if (!ModelState.IsValid)
            {
                var model = Mapper.Map<VacancyApplicationDetailsViewModel>(preModel);
                model.Researcher = Mapper.Map<ResearcherDetailsViewModel>(_mediator.Send(new SingleResearcherQuery { ResearcherGuid = model.ResearcherGuid }));
                model.Vacancy = Mapper.Map<VacancyDetailsViewModel>(_mediator.Send(new SingleVacancyQuery { VacancyGuid = model.VacancyGuid }));
                return View("delete", model);
            }

            _mediator.Send(new CancelVacancyApplicationCommand { ResearcherGuid = researcherGuid, VacancyApplicationGuid = id });
            return RedirectToAction("applications", "researchers");
        }

        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        [PageTitle("Детали заявки")]
        [BindOrganizationIdFromClaims]
        public IActionResult Print(Guid id, Guid organizationGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var preModel = _mediator.Send(new SingleVacancyApplicationQuery { VacancyApplicationGuid = id });

            if (preModel == null)
                throw new ObjectNotFoundException($"Не найденая Заявка c идентификатором: {id}");

            if (preModel.status == VacancyApplicationStatus.InProcess
                || preModel.status == VacancyApplicationStatus.Cancelled
                || preModel.status == VacancyApplicationStatus.Lost
                || preModel.status == VacancyApplicationStatus.Removed)
                throw new Exception($"Вы не можете просматривать Заявку со статусом: {preModel.status.GetDescription()}");

            var vacancy = _mediator.Send(new SingleVacancyQuery { VacancyGuid = preModel.vacancy_guid });
            if (vacancy.organization_guid != organizationGuid)
                throw new Exception("Вы не можете изменять Заявки, поданные на вакансии других организаций.");

            var model = Mapper.Map<VacancyApplicationDetailsViewModel>(preModel);
            model.Researcher = Mapper.Map<ResearcherDetailsViewModel>(_mediator.Send(new SingleResearcherQuery { ResearcherGuid = preModel.researcher_guid }));
            model.Researcher.Publications = Mapper.Map<List<PublicationEditViewModel>>(_mediator.Send(new SelectResearcherPublicationsQuery { ResearcherGuid = preModel.researcher_guid }));
            model.Researcher.Educations = Mapper.Map<List<EducationEditViewModel>>(_mediator.Send(new SelectResearcherEducationsQuery { ResearcherGuid = preModel.researcher_guid }));
            model.Vacancy = Mapper.Map<VacancyDetailsViewModel>(vacancy);

            return View(model);
        }

        private Vacancy OfferAcceptionPreValidation(Guid vacancyApplicationGuid, Guid organizationGuid, bool isWinner)
        {
            var vacancyApplicaiton = _mediator.Send(new SingleVacancyApplicationQuery { VacancyApplicationGuid = vacancyApplicationGuid });

            if (vacancyApplicaiton == null)
                throw new ObjectNotFoundException($"Не найденая Заявка c идентификатором: {vacancyApplicationGuid}");

            if (isWinner && vacancyApplicaiton.status != VacancyApplicationStatus.Won)
                throw new Exception($"Вы не можете зафиксировать принятие предложения от Победителя если Заявка имеет статус: {vacancyApplicaiton.status.GetDescription()}");

            if (!isWinner && vacancyApplicaiton.status != VacancyApplicationStatus.Pretended)
                throw new Exception($"Вы не можете зафиксировать принятие предложения от Претендента если Заявка имеет статус: {vacancyApplicaiton.status.GetDescription()}");

            var vacancy = _mediator.Send(new SingleVacancyQuery { VacancyGuid = vacancyApplicaiton.vacancy_guid });

            if (vacancy == null)
                throw new ObjectNotFoundException($"Не найденая Вакансия c идентификатором: {vacancyApplicaiton.vacancy_guid}");

            if (vacancy.organization_guid != organizationGuid)
                throw new Exception("Вы не можете изменять Заявки, поданные на вакансии других организаций.");

            if (vacancy.status != VacancyStatus.OfferResponseAwaiting)
                throw new Exception(
                    $"Вы не можете зафиксироватиьо принятие или отказ от предложения если Заявка имеет статус: {vacancy.status.GetDescription()}");
            return vacancy;
        }

        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        [BindOrganizationIdFromClaims]
        public IActionResult OfferAcception(Guid id, Guid organizationGuid, bool isWinner, bool hasAccepted)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var vacancy = OfferAcceptionPreValidation(id, organizationGuid, isWinner);

            if (isWinner)
                if (hasAccepted)
                    _mediator.Send(new SetWinnerAcceptOfferCommand
                    {
                        VacancyGuid = vacancy.guid
                    });
                else
                    _mediator.Send(new SetWinnerRejectOfferCommand
                    {
                        VacancyGuid = vacancy.guid
                    });
            else
                if (hasAccepted)
                    _mediator.Send(new SetPretenderAcceptOfferCommand
                    {
                        VacancyGuid = vacancy.guid
                    });
                else
                    _mediator.Send(new SetPretenderRejectOfferCommand
                    {
                        VacancyGuid = vacancy.guid
                    });

            return RedirectToAction("preview", "applications", new { id });

        }

    }
}
