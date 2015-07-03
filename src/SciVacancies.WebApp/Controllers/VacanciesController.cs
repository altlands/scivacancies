using System;
using System.Data.Entity.Core;
using System.Linq;
using AutoMapper;
using MediatR;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using NPoco;
using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel.Core;
using SciVacancies.WebApp.Commands;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Queries;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Controllers
{
    /// <summary>
    /// страница с вакансиями (конкурсами)
    /// </summary>
    [Authorize]
    public class VacanciesController : Controller
    {
        private readonly IMediator _mediator;

        public VacanciesController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [PageTitle("Новая вакансия")]
        [BindOrganizationIdFromClaims]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        public ViewResult Create(Guid organizationGuid)
        {
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException($"{nameof(organizationGuid)}");

            var model = new PositionCreateViewModel(organizationGuid);
            model.InitDictionaries(_mediator);
            return View(model);
        }


        [PageTitle("Новая вакансия")]
        [HttpPost]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PositionCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var positionDataModel = Mapper.Map<PositionDataModel>(model);
                var positionGuid = _mediator.Send(new CreatePositionCommand { OrganizationGuid = model.OrganizationGuid, Data = positionDataModel });

                if (!model.ToPublish)
                    return RedirectToAction("details", "positions", new { id = positionGuid });

                var vacancyDataModel = Mapper.Map<VacancyDataModel>(positionDataModel);
                vacancyDataModel.OrganizationName = _mediator.Send(new SingleOrganizationQuery { OrganizationGuid = model.OrganizationGuid }).Name;

                var vacancyGuid = _mediator.Send(new PublishVacancyCommand
                {
                    OrganizationGuid = model.OrganizationGuid,
                    PositionGuid = positionGuid,
                    Data = vacancyDataModel
                });

                return RedirectToAction("details", "vacancies", new { id = vacancyGuid });
            }
            model.InitDictionaries(_mediator);
            return View(model);

        }

        [PageTitle("Подробно о вакансии")]
        public ViewResult Preview(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var preModel = _mediator.Send(new SingleVacancyQuery { VacancyGuid = id });

            var model = Mapper.Map<VacancyDetailsViewModel>(preModel);

            if (model == null)
                throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            model.Winner =
                Mapper.Map<ResearcherDetailsViewModel>(_mediator.Send(new SingleResearcherQuery { ResearcherGuid = preModel.WinnerGuid }));
            model.Pretender =
                Mapper.Map<ResearcherDetailsViewModel>(_mediator.Send(new SingleResearcherQuery { ResearcherGuid = preModel.PretenderGuid }));

            return View(model);
        }

        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        [PageTitle("Подробно о вакансии")]
        [BindOrganizationIdFromClaims]
        public ViewResult Details(Guid id, Guid organizationGuid, int pageSize = 10, int currentPage = 1)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var preModel = _mediator.Send(new SingleVacancyQuery { VacancyGuid = id });

            var model = Mapper.Map<VacancyDetailsViewModel>(preModel);

            if (model == null)
                throw new ObjectNotFoundException($"Не найдена вакансия с Guid: {id}");

            ViewBag.VacancyInFavorites = false;

            if (organizationGuid != model.OrganizationGuid)
                throw new Exception("Вы не можете просматривать детальную информацию о Вакансиях других организаций");


            //если есть Заявки, загрузить информацию о заявителях
            if (model.Status != VacancyStatus.AppliesAcceptance && model.Status != VacancyStatus.InCommittee && model.Status != VacancyStatus.Closed)
                return View(model);

            model.Applications =
                 _mediator.Send(new SelectPagedVacancyApplicationsByVacancyQuery
                 {
                     PageSize = pageSize,
                     PageIndex = currentPage,
                     VacancyGuid = id,
                     OrderBy = "Guid"
                 }).MapToPagedList<VacancyApplication, VacancyApplicationDetailsViewModel>();

            if (model.Applications.Items.Count > 0)
                //TODO: оптимизировать запрос и его обработку
                model.Applications.Items.ForEach(
                    c =>
                        c.Researcher =
                            Mapper.Map<ResearcherDetailsViewModel>(
                                _mediator.Send(new SingleResearcherQuery { ResearcherGuid = c.ResearcherGuid })));

            return View(model);
        }

        [AllowAnonymous]
        [PageTitle("Карточка вакансии")]
        [BindResearcherIdFromClaims]
        public ViewResult Card(Guid id, Guid researcherGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var preModel = _mediator.Send(new SingleVacancyQuery { VacancyGuid = id });

            var model = Mapper.Map<VacancyDetailsViewModel>(preModel);

            if (model == null)
                throw new ObjectNotFoundException($"Не найдена вакансия с Guid: {id}");

            ViewBag.VacancyInFavorites = false;

            //если Вакансия Опубликована или Принимает Заявки
            if ((model.Status == VacancyStatus.AppliesAcceptance || model.Status == VacancyStatus.Published)
                && researcherGuid != Guid.Empty)
            {
                //TODO: оптимизировать запрос и его обработку
                var favoritesVacancies = _mediator.Send(new SelectPagedFavoriteVacanciesByResearcherQuery { PageSize = 1000, PageIndex = 1, ResearcherGuid = researcherGuid, OrderBy = ConstTerms.OrderByDateAscending });

                //если текущая вакансия есть в списке избранных
                ViewBag.VacancyInFavorites = favoritesVacancies != null && favoritesVacancies.TotalItems != 0 && favoritesVacancies.Items.Select(c => c.Guid).ToList().Contains(id);
            }
            return View(model);
        }

        [PageTitle("Отменить вакансию")]
        [BindOrganizationIdFromClaims]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        public IActionResult Cancel(Guid id, Guid organizationGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var preModel = _mediator.Send(new SingleVacancyQuery { VacancyGuid = id });

            if (preModel.OrganizationGuid != organizationGuid)
                throw new Exception("Вы не можете отменить вакансии других организаций");

            if (preModel.Status != VacancyStatus.Published && preModel.Status != VacancyStatus.AppliesAcceptance && preModel.Status != VacancyStatus.InCommittee)
                throw new Exception($"Вы не можете отменить вакансию со статусом: {preModel.Status.GetDescription()}");

            var model = Mapper.Map<VacancyDetailsViewModel>(preModel);

            return View(model);
        }

        [HttpPost]
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

            if (model.Status != VacancyStatus.Published && model.Status != VacancyStatus.AppliesAcceptance && model.Status != VacancyStatus.InCommittee)
                throw new Exception($"Вы не можете отменить вакансию со статусом: {model.Status.GetDescription()}");

            _mediator.Send(new CancelVacancyCommand { VacancyGuid = id, OrganizationGuid = organizationGuid, Reason = reason });

            return RedirectToAction("vacancies", "organizations");
        }

        [BindOrganizationIdFromClaims]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        public IActionResult StartAcceptApplications(Guid id, Guid organizationGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var preModel = _mediator.Send(new SingleVacancyQuery { VacancyGuid = id });

            if (preModel == null)
                throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            if (preModel.OrganizationGuid != organizationGuid)
                throw new Exception("Вы не можете менять Вакансии других организаций");

            if (preModel.Status != VacancyStatus.Published)
                throw new Exception($"Вы не можете начать включить режим приём Заявок для Вакансии со статусом: {preModel.Status.GetDescription()}");

            _mediator.Send(new SwitchVacancyToAcceptApplicationsCommand
            {
                OrganizationGuid = organizationGuid,
                VacancyGuid = id
            });

            return RedirectToAction("vacancies", "organizations");
        }

        [BindOrganizationIdFromClaims]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        public IActionResult StartInCommittee(Guid id, Guid organizationGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var preModel = _mediator.Send(new SingleVacancyQuery { VacancyGuid = id });

            if (preModel == null)
                throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            if (preModel.OrganizationGuid != organizationGuid)
                throw new Exception("Вы не можете менять Вакансии других организаций");

            if (preModel.Status != VacancyStatus.AppliesAcceptance)
                throw new Exception($"Вы не можете Вакансию на рассмотрение комиссии со статусом: {preModel.Status.GetDescription()}");

            //TODO: оптимизировать запрос и его обработку
            var vacancyApplications = _mediator.Send(new SelectPagedVacancyApplicationsByVacancyQuery
            {
                VacancyGuid = preModel.Guid,
                PageSize = 1000,
                PageIndex = 1,
                OrderBy = ConstTerms.OrderByDateDescending
            });

            //TODO: Vacancy -> InCommitte : нужно ли проверять минимальное (какое) количество заявок, поданных на вакансию
            if (vacancyApplications.TotalItems == 0
                || vacancyApplications.Items.Count(c => c.Status == VacancyApplicationStatus.Applied) < 2)
                throw new Exception("Подано недостаточно Заявок для перевода Вакансии на рассмотрение комиссии");

            _mediator.Send(new SwitchVacancyInCommitteeCommand
            {
                OrganizationGuid = organizationGuid,
                VacancyGuid = id
            });

            return RedirectToAction("vacancies", "organizations");
        }

        private Vacancy VacancyClosePrevalidation(Guid id, Guid organizationGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var preModel = _mediator.Send(new SingleVacancyQuery { VacancyGuid = id });

            if (preModel == null)
                throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            if (preModel.OrganizationGuid != organizationGuid)
                throw new Exception("Вы не можете менять Вакансии других организаций");

            if (preModel.WinnerGuid!= Guid.Empty)
                throw new Exception("Вы не можете закрыть Вакансию: не выбран победитель");

            if (preModel.PretenderGuid!= Guid.Empty)
                throw new Exception("Вы не можете закрыть Вакансию: не выбран претендент");

            if (preModel.Status != VacancyStatus.InCommittee)
                throw new Exception($"Вы не можете выбрать победителя по Вакансии со статусом: {preModel.Status.GetDescription()}");

            return preModel;
        }

        [PageTitle("Закрыть вакансию")]
        [BindOrganizationIdFromClaims]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        public IActionResult Close(Guid id, Guid organizationGuid)
        {
            var model = Mapper.Map<VacancyDetailsViewModel>(VacancyClosePrevalidation(id, organizationGuid));
            model.Winner = Mapper.Map<ResearcherDetailsViewModel>(_mediator.Send(new SingleResearcherQuery { ResearcherGuid = model.WinnerGuid }));
            model.Pretender = Mapper.Map<ResearcherDetailsViewModel>(_mediator.Send(new SingleResearcherQuery { ResearcherGuid = model.PretenderGuid }));
            return View(model);
        }

        [PageTitle("Закрыть вакансию")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        [BindOrganizationIdFromClaims]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        public IActionResult ClosePost(Guid id, Guid organizationGuid)
        {
            VacancyClosePrevalidation(id, organizationGuid);

            _mediator.Send(new CloseVacancyCommand
            {
                OrganizationGuid = organizationGuid,
                VacancyGuid = id
            });

            return RedirectToAction("vacancies", "organizations");
        }

    }
}
