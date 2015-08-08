﻿using System;
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
    /// <summary>
    /// страница с вакансиями
    /// </summary>
    [ResponseCache(NoStore = true)]
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
                    return View("create", model);
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
                return HttpNotFound(); //throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            if (preModel.organization_guid != organizationGuid)
                return View("Error", "Вы не можете изменять вакансии других организаций");

            if (preModel.status != VacancyStatus.InProcess)
                return View("Error", $"Вы не можете изменить вакансию с текущим статусом: {preModel.status.GetDescription()}");

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
                return View("Error", "Вы не можете изменять вакансии других организаций");

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
                    return HttpNotFound(); //throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором {model.Guid}");

                if (vacancy.status != VacancyStatus.InProcess)
                    return View("Error", $"Вы не можете изменить вакансию с текущим статусом: {vacancy.status.GetDescription()}");

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
        public IActionResult Preview(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var preModel = _mediator.Send(new SingleVacancyQuery { VacancyGuid = id });

            if (preModel == null)
                return HttpNotFound(); //throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            var model = Mapper.Map<VacancyDetailsViewModel>(preModel);

            model.Winner =
                Mapper.Map<ResearcherDetailsViewModel>(_mediator.Send(new SingleResearcherQuery { ResearcherGuid = preModel.winner_researcher_guid }));
            model.Pretender =
                Mapper.Map<ResearcherDetailsViewModel>(_mediator.Send(new SingleResearcherQuery { ResearcherGuid = preModel.pretender_researcher_guid }));
            model.Criterias = _mediator.Send(new SelectVacancyCriteriasQuery { VacancyGuid = model.Guid });
            model.CriteriasHierarchy =
                    _mediator.Send(new SelectAllCriteriasQuery()).ToList().ToHierarchyCriteriaViewModelList(model.Criterias.ToList());

            return View(model);
        }

        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        [PageTitle("Подробно о вакансии")]
        [BindOrganizationIdFromClaims]
        public IActionResult Details(Guid id, Guid organizationGuid, int pageSize = 10, int currentPage = 1,
            string sortField = ConstTerms.OrderByFieldApplyDate, string sortDirection = ConstTerms.OrderByDescending)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var preModel = _mediator.Send(new SingleVacancyQuery { VacancyGuid = id });

            if (preModel == null)
                return HttpNotFound(); //throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            var model = Mapper.Map<VacancyDetailsViewModel>(preModel);

            ViewBag.VacancyInFavorites = false;

            if (organizationGuid != model.OrganizationGuid)
                return View("Error", "Вы не можете просматривать детальную информацию о Вакансиях других организаций");

            model.Applications =
                 _mediator.Send(new SelectPagedVacancyApplicationsByVacancyQuery
                 {
                     PageSize = pageSize,
                     PageIndex = currentPage,
                     VacancyGuid = id,
                     OrderBy = new SortFilterHelper().GetSortField<VacancyApplication>(sortField),
                     OrderDirection = sortDirection
                 }).MapToPagedList<VacancyApplication, VacancyApplicationDetailsViewModel>();

            if (model.Applications?.Items != null && model.Applications.Items.Count > 0)
            {
                model.Applications.Items.Where(c => c.ResearcherGuid == Guid.Empty).ToList().ForEach(c => model.Applications.Items.Remove(c));
                //TODO: оптимизировать запрос и его обработку.
                model.Applications.Items.ForEach(
                    c =>
                        c.Researcher =
                            Mapper.Map<ResearcherDetailsViewModel>(
                                _mediator.Send(new SingleResearcherQuery { ResearcherGuid = c.ResearcherGuid })));
            }
            model.Criterias = _mediator.Send(new SelectVacancyCriteriasQuery {VacancyGuid = model.Guid});
            model.CriteriasHierarchy =
                    _mediator.Send(new SelectAllCriteriasQuery()).ToList().ToHierarchyCriteriaViewModelList(model.Criterias.ToList());

            return View(model);
        }

        [AllowAnonymous]
        [PageTitle("Карточка вакансии")]
        [BindResearcherIdFromClaims]
        public IActionResult Card(Guid id, Guid researcherGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var preModel = _mediator.Send(new SingleVacancyQuery { VacancyGuid = id });

            if (preModel == null)
                return HttpNotFound();

            var model = Mapper.Map<VacancyDetailsViewModel>(preModel);

            ViewBag.VacancyInFavorites = false;

            //если Вакансия Опубликована или Принимает Заявки
            if (model.Status == VacancyStatus.Published
                && researcherGuid != Guid.Empty)
            {
                var favoritesVacancies = _mediator.Send(new SelectFavoriteVacancyGuidsByResearcherQuery
                {
                    ResearcherGuid = researcherGuid
                }).ToList();
                //если текущая вакансия есть в списке избранных
                ViewBag.VacancyInFavorites = favoritesVacancies.Any(c => c == id);
            }
            model.Criterias = _mediator.Send(new SelectVacancyCriteriasQuery { VacancyGuid = model.Guid });
            model.CriteriasHierarchy =
                    _mediator.Send(new SelectAllCriteriasQuery()).ToList().ToHierarchyCriteriaViewModelList(model.Criterias.ToList());
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
                return HttpNotFound(); //throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            if (preModel.organization_guid != organizationGuid)
                return View("Error", "Вы не можете отменить вакансии других организаций");

            if (preModel.status != VacancyStatus.Published && preModel.status != VacancyStatus.InCommittee)
                return View("Error", $"Вы не можете отменить вакансию со статусом: {preModel.status.GetDescription()}");

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
                return HttpNotFound(); //throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            if (model.organization_guid != organizationGuid)
                return View("Error", "Вы не можете отменить вакансии других организаций");

            if (model.status != VacancyStatus.Published && model.status != VacancyStatus.InCommittee)
                return View("Error", $"Вы не можете отменить вакансию со статусом: {model.status.GetDescription()}");

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
                return HttpNotFound(); //throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            if (model.organization_guid != organizationGuid)
                return View("Error", "Вы не можете отменить удаление вакансии других организаций");

            if (model.status == VacancyStatus.Removed)
                return View("Error", "Вакансия уже удалена");

            if (model.status != VacancyStatus.InProcess)
                return View("Error", $"Вы не можете удалить вакансию с текущим статусом: {model.status.GetDescription()}");

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
                return HttpNotFound(); //throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            if (preModel.organization_guid != organizationGuid)
                return View("Error", "Вы не можете менять Вакансии других организаций");

            if (preModel.status != VacancyStatus.Published)
                return View("Error", $"Вы не можете Вакансию на рассмотрение комиссии со статусом: {preModel.status.GetDescription()}");

            //TODO: оптимизировать запрос и его обработку
            var vacancyApplications = _mediator.Send(new SelectPagedVacancyApplicationsByVacancyQuery
            {
                VacancyGuid = preModel.guid,
                PageSize = 1000,
                PageIndex = 1,
                OrderBy = ConstTerms.OrderByFieldDate
            });

            if (vacancyApplications.TotalItems == 0
                || vacancyApplications.Items.Count(c => c.status == VacancyApplicationStatus.Applied) < 2)
                return View("Error", "Подано недостаточно Заявок для перевода Вакансии на рассмотрение комиссии");

            _mediator.Send(new SwitchVacancyInCommitteeCommand
            {
                VacancyGuid = id
            });

            return RedirectToAction("vacancies", "organizations");
        }

        private object VacancyClosePrevalidation(Guid id, Guid organizationGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var preModel = _mediator.Send(new SingleVacancyQuery { VacancyGuid = id });

            if (preModel == null)
                return HttpNotFound(); //throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            if (preModel.organization_guid != organizationGuid)
                return View("Error", "Вы не можете менять Вакансии других организаций");

            if (preModel.status != VacancyStatus.OfferAccepted && preModel.status != VacancyStatus.OfferRejected)
                return View("Error", "Вы не можете закрыть вакансию, пока кто-то из победителей не даст согласится или оба не откажутся");

            return preModel;
        }

        [PageTitle("Закрыть вакансию")]
        [BindOrganizationIdFromClaims]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        public IActionResult Close(Guid id, Guid organizationGuid)
        {
            var model = Mapper.Map<VacancyDetailsViewModel>(VacancyClosePrevalidation(id, organizationGuid));
            model.Winner = Mapper.Map<ResearcherDetailsViewModel>(_mediator.Send(new SingleResearcherQuery { ResearcherGuid = model.WinnerResearcherGuid }));
            model.Pretender = Mapper.Map<ResearcherDetailsViewModel>(_mediator.Send(new SingleResearcherQuery { ResearcherGuid = model.PretenderResearcherGuid }));
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
                return HttpNotFound(); //throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            if (model.organization_guid != organizationGuid)
                return View("Error", "Вы не можете отменить удаление вакансии других организаций");

            if (model.status != VacancyStatus.InProcess)
                return View("Error", $"Вы не можете опубликовать вакансию со статусом: {model.status.GetDescription()}");

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
                return HttpNotFound(); //throw new ObjectNotFoundException($"Не найдена заявка с идентификатором {id}");

            if (preModel.organization_guid != organizationGuid)
                return View("Error", "Копировать можно только свои заявки");

            var model = Mapper.Map<VacancyCreateViewModel>(preModel);
            model.Guid = Guid.Empty;
            model.InitDictionaries(_mediator);

            return View("create", model);
        }

        [PageTitle("Новая вакансия")]
        [HttpPost]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        [ValidateAntiForgeryToken]
        public IActionResult Copy(VacancyCreateViewModel model)
        {
            return Create(model);
        }

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
                return HttpNotFound(); //throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            if (preModel.organization_guid != organizationGuid)
                return View("Error", "Вы не можете изменять вакансии других организаций");

            if (preModel.status != VacancyStatus.InCommittee)
                return View("Error", $"Вы не можете изменить вакансию с текущим статусом: {preModel.status.GetDescription()}");

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

        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        [PageTitle("Подробно о вакансии")]
        [BindOrganizationIdFromClaims]
        public IActionResult Print(Guid id, Guid organizationGuid, int pageSize = 10, int currentPage = 1)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var preModel = _mediator.Send(new SingleVacancyQuery { VacancyGuid = id });

            if (preModel == null)
                return HttpNotFound(); //throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            var model = Mapper.Map<VacancyDetailsViewModel>(preModel);

            ViewBag.VacancyInFavorites = false;

            if (organizationGuid != model.OrganizationGuid)
                return View("Error", "Вы не можете просматривать детальную информацию о Вакансиях других организаций");

            var page = _mediator.Send(new SelectPagedVacancyApplicationsByVacancyQuery
            {
                PageSize = pageSize,
                PageIndex = currentPage,
                VacancyGuid = id,
                OrderBy = "Guid"
            });
            page = _mediator.Send(new SelectPagedVacancyApplicationsByVacancyQuery
            {
                PageSize = page.TotalItems,
                PageIndex = 1,
                VacancyGuid = id,
                OrderBy = "Guid"
            });

            model.Applications =
                 page.MapToPagedList<VacancyApplication, VacancyApplicationDetailsViewModel>();

            if (model.Applications?.Items != null && model.Applications.Items.Count > 0)
            {
                model.Applications.Items.Where(c => c.ResearcherGuid == Guid.Empty).ToList().ForEach(c => model.Applications.Items.Remove(c));
                //TODO: оптимизировать запрос и его обработку.
                model.Applications.Items.ForEach(
                    c =>
                        c.Researcher =
                            Mapper.Map<ResearcherDetailsViewModel>(
                                _mediator.Send(new SingleResearcherQuery { ResearcherGuid = c.ResearcherGuid })));
            }

            return View(model);
        }


    }
}
