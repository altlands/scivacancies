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

            var model = new VacancyCreateViewModel(organizationGuid);
            model.InitDictionaries(_mediator);
            //TODO: ntemnikov: Vacancies -> Create : вернуть редактирование Критериев во View
            return View(model);
        }


        [PageTitle("Новая вакансия")]
        [HttpPost]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VacancyCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.ContractType == ContractType.FixedTerm
                    && model.ContractTimeYears == 0
                    && model.ContractTimeYears == 0)
                {
                    ModelState.AddModelError("ContractTypeValue", $"Для договора типа \"{ContractType.FixedTerm.GetDescription()}\" укажите срок трудового договора");
                    model.InitDictionaries(_mediator);
                    return View(model);
                }

                var vacancyDataModel = Mapper.Map<VacancyDataModel>(model);
                var vacancyGuid = _mediator.Send(new CreateVacancyCommand { OrganizationGuid = model.OrganizationGuid, Data = vacancyDataModel });

                if (!model.ToPublish)
                    return RedirectToAction("details", new { id = vacancyGuid });

                _mediator.Send(new PublishVacancyCommand
                {
                    VacancyGuid = vacancyGuid
                });

                return RedirectToAction("details", new { id = vacancyGuid });
            }
            model.InitDictionaries(_mediator);
            return View("create", model);

        }

        [PageTitle("Изменить вакансию")]
        [BindOrganizationIdFromClaims]
        public IActionResult Edit(Guid id, Guid organizationGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var preModel = _mediator.Send(new SingleVacancyQuery { VacancyGuid = id });

            if (preModel == null)
                throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            if (preModel.organization_guid != organizationGuid)
                throw new Exception("Вы не можете изменять вакансии других организаций");

            if (preModel.status != VacancyStatus.InProcess)
                throw new Exception($"Вы не можете изменить вакансию с текущим статусом: {preModel.status.GetDescription()}");

            var model = Mapper.Map<VacancyCreateViewModel>(preModel);
            model.InitDictionaries(_mediator);

            return View(model);
        }

        [PageTitle("Изменить вакансию")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [BindOrganizationIdFromClaims("claimedUserGuid")]
        public IActionResult Edit(VacancyCreateViewModel model, Guid claimedUserGuid)
        {
            if (claimedUserGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(claimedUserGuid));

            if (model.OrganizationGuid != claimedUserGuid)
                throw new Exception("Вы не можете изменять вакансии других организаций");

            if (ModelState.IsValid)
            {
                if (model.ContractType == ContractType.FixedTerm
                    && model.ContractTimeYears == 0
                    && model.ContractTimeYears == 0)
                {
                    ModelState.AddModelError("ContractTypeValue", $"Для договора типа \"{ContractType.FixedTerm.GetDescription()}\" укажите срок трудового договора");
                    model.InitDictionaries(_mediator);
                    return View(model);
                }

                var vacancy = _mediator.Send(new SingleVacancyQuery { VacancyGuid = model.Guid });
                if (vacancy == null)
                    throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором {model.Guid}");

                if (vacancy.status != VacancyStatus.InProcess)
                    throw new Exception($"Вы не можете изменить вакансию с текущим статусом: {vacancy.status.GetDescription()}");

                var vacancyDataModel = Mapper.Map<VacancyDataModel>(model);
                var vacancyGuid = _mediator.Send(new UpdateVacancyCommand { VacancyGuid = model.Guid, Data = vacancyDataModel });

                if (!model.ToPublish)
                    return RedirectToAction("details", new { id = model.Guid });

                _mediator.Send(new PublishVacancyCommand
                {
                    VacancyGuid = model.Guid
                });
                return RedirectToAction("details", new { id = vacancyGuid });
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

            if (preModel == null)
                throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            var model = Mapper.Map<VacancyDetailsViewModel>(preModel);

            if (model == null)
                throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            model.Winner =
                Mapper.Map<ResearcherDetailsViewModel>(_mediator.Send(new SingleResearcherQuery { ResearcherGuid = preModel.winner_researcher_guid }));
            model.Pretender =
                Mapper.Map<ResearcherDetailsViewModel>(_mediator.Send(new SingleResearcherQuery { ResearcherGuid = preModel.pretender_researcher_guid }));

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

            if (preModel == null)
                throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            var model = Mapper.Map<VacancyDetailsViewModel>(preModel);

            if (model == null)
                throw new ObjectNotFoundException($"Не найдена вакансия с Guid: {id}");

            ViewBag.VacancyInFavorites = false;

            if (organizationGuid != model.OrganizationGuid)
                throw new Exception("Вы не можете просматривать детальную информацию о Вакансиях других организаций");

            model.Applications =
                 _mediator.Send(new SelectPagedVacancyApplicationsByVacancyQuery
                 {
                     PageSize = pageSize,
                     PageIndex = currentPage,
                     VacancyGuid = id,
                     OrderBy = "Guid"
                 }).MapToPagedList<VacancyApplication, VacancyApplicationDetailsViewModel>();

            if (model.Applications?.Items != null && model.Applications.Items.Count > 0)
                //TODO: оптимизировать запрос и его обработку.
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

            if (preModel == null)
                throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            var model = Mapper.Map<VacancyDetailsViewModel>(preModel);

            if (model == null)
                throw new ObjectNotFoundException($"Не найдена вакансия с Guid: {id}");

            ViewBag.VacancyInFavorites = false;

            //если Вакансия Опубликована или Принимает Заявки
            if (model.Status == VacancyStatus.Published
                && researcherGuid != Guid.Empty)
            {
                //TODO: оптимизировать запрос и его обработку
                var favoritesVacancies = _mediator.Send(new SelectPagedFavoriteVacanciesByResearcherQuery { PageSize = 1000, PageIndex = 1, ResearcherGuid = researcherGuid, OrderBy = ConstTerms.OrderByDateAscending });

                //если текущая вакансия есть в списке избранных
                ViewBag.VacancyInFavorites = favoritesVacancies != null && favoritesVacancies.TotalItems != 0 && favoritesVacancies.Items.Select(c => c.guid).ToList().Contains(id);
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

            if (preModel == null)
                throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            if (preModel.organization_guid != organizationGuid)
                throw new Exception("Вы не можете отменить вакансии других организаций");

            if (preModel.status != VacancyStatus.Published && preModel.status != VacancyStatus.InCommittee)
                throw new Exception($"Вы не можете отменить вакансию со статусом: {preModel.status.GetDescription()}");

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

            if (model == null)
                throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            if (model.organization_guid != organizationGuid)
                throw new Exception("Вы не можете отменить вакансии других организаций");

            if (model.status != VacancyStatus.Published && model.status != VacancyStatus.InCommittee)
                throw new Exception($"Вы не можете отменить вакансию со статусом: {model.status.GetDescription()}");

            _mediator.Send(new CancelVacancyCommand { VacancyGuid = id, Reason = reason });

            return RedirectToAction("vacancies", "organizations");
        }

        [PageTitle("Вакансия удалена")]
        [BindOrganizationIdFromClaims]
        public IActionResult Delete(Guid id, Guid organizationGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var model = _mediator.Send(new SingleVacancyQuery { VacancyGuid = id });

            if (model == null)
                throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            if (model.organization_guid != organizationGuid)
                throw new Exception("Вы не можете отменить удаление вакансии других организаций");

            if (model.status == VacancyStatus.Removed)
                throw new Exception("Вакансия уже удалена");

            if (model.status != VacancyStatus.InProcess)
                throw new Exception($"Вы не можете удалить вакансию с текущим статусом: {model.status.GetDescription()}");

            _mediator.Send(new RemoveVacancyCommand { VacancyGuid = id });

            return View();
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

            if (preModel.organization_guid != organizationGuid)
                throw new Exception("Вы не можете менять Вакансии других организаций");

            if (preModel.status != VacancyStatus.Published)
                throw new Exception($"Вы не можете Вакансию на рассмотрение комиссии со статусом: {preModel.status.GetDescription()}");

            //TODO: оптимизировать запрос и его обработку
            var vacancyApplications = _mediator.Send(new SelectPagedVacancyApplicationsByVacancyQuery
            {
                VacancyGuid = preModel.guid,
                PageSize = 1000,
                PageIndex = 1,
                OrderBy = ConstTerms.OrderByDateDescending
            });

            //TODO: Vacancy -> InCommitte : нужно ли проверять минимальное (какое) количество заявок, поданных на вакансию
            if (vacancyApplications.TotalItems == 0
                || vacancyApplications.Items.Count(c => c.status == VacancyApplicationStatus.Applied) < 2)
                throw new Exception("Подано недостаточно Заявок для перевода Вакансии на рассмотрение комиссии");

            _mediator.Send(new SwitchVacancyInCommitteeCommand
            {
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

            if (preModel.organization_guid != organizationGuid)
                throw new Exception("Вы не можете менять Вакансии других организаций");

            if (preModel.status != VacancyStatus.OfferAccepted && preModel.status != VacancyStatus.OfferRejected)
                throw new Exception("Вы не можете закрыть вакансию, пока кто-то из победителей не даст согласится или оба не откажутся");

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
                VacancyGuid = id
            });

            return RedirectToAction("vacancies", "organizations");
        }

        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        [BindOrganizationIdFromClaims]
        public IActionResult Publish(Guid id, Guid organizationGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var model = _mediator.Send(new SingleVacancyQuery { VacancyGuid = id });

            if (model == null)
                throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            if (model.organization_guid != organizationGuid)
                throw new Exception("Вы не можете отменить удаление вакансии других организаций");

            if (model.status != VacancyStatus.InProcess)
                throw new Exception($"Вы не можете опубликовать вакансию со статусом: {model.status.GetDescription()}");

            var vacancyGuid = _mediator.Send(new PublishVacancyCommand
            {
                VacancyGuid = id
            });

            return RedirectToAction("vacancies", "organizations");
        }

        [PageTitle("Новая вакансия")]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        [BindOrganizationIdFromClaims]
        public IActionResult Copy(Guid id, Guid organizationGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var preModel = _mediator.Send(new SingleVacancyQuery { VacancyGuid = id });

            if (preModel == null)
                throw new ObjectNotFoundException($"Не найдена заявка с идентификатором {id}");

            if (preModel.organization_guid != organizationGuid)
                throw new Exception("Копировать можно только свои заявки");

            var model = Mapper.Map<VacancyCreateViewModel>(preModel);
            model.Guid = Guid.Empty;
            model.InitDictionaries(_mediator);

            return View("create", model);
        }

        [PageTitle("Новая вакансия")]
        [HttpPost]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        [ValidateAntiForgeryToken]
        public IActionResult Copy(VacancyCreateViewModel model) { return Create(model); }

        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        [BindOrganizationIdFromClaims]
        public IActionResult WinnerDecision(Guid id, bool decision, Guid organizationGuid, bool isWinner)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var preModel = _mediator.Send(new SingleVacancyQuery { VacancyGuid = id });

            if (preModel == null)
                throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            if (preModel.organization_guid != organizationGuid)
                throw new Exception("Вы не можете изменять вакансии других организаций");

            if (preModel.status != VacancyStatus.InCommittee)
                throw new Exception($"Вы не можете изменить вакансию с текущим статусом: {preModel.status.GetDescription()}");

            var model = Mapper.Map<VacancyCreateViewModel>(preModel);
            model.InitDictionaries(_mediator);

            if (isWinner)
                if (decision)
                    _mediator.Send(new SetWinnerAcceptOfferCommand { VacancyGuid = id });
                else
                    _mediator.Send(new SetWinnerRejectOfferCommand { VacancyGuid = id });
            else
            {
                if (decision)
                {
                    _mediator.Send(new SetPretenderAcceptOfferCommand { VacancyGuid = id });
                }
                else
                    _mediator.Send(new SetPretenderRejectOfferCommand { VacancyGuid = id });
            }

            return RedirectToAction("vacancies", "organizations");

        }

    }
}
