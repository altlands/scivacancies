using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using AutoMapper;
using MediatR;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.OptionsModel;
using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel.Core;
using SciVacancies.WebApp.Commands;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Queries;
using SciVacancies.WebApp.ViewModels;
using Microsoft.AspNet.Http;
using Microsoft.Net.Http.Headers;
using Microsoft.Framework.Logging;

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
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IOptions<AttachmentSettings> _attachmentSettings;
        private readonly IOptions<SagaSettings> _sagaSettings;
        private readonly ILogger _logger;
        private readonly IOptions<Holidays> _holidays;

        /// <summary>
        /// минимальное значнеие ЗП
        /// </summary>
        private int _salaryMinValue = 5965;


        public VacanciesController(IMediator mediator, IOptions<SagaSettings> sagaSettings, IOptions<Holidays> holidays, IHostingEnvironment hostingEnvironment, IOptions<AttachmentSettings> attachmentSettings, ILoggerFactory loggerFactory)
        {
            _sagaSettings = sagaSettings;
            _mediator = mediator;
            _hostingEnvironment = hostingEnvironment;
            _attachmentSettings = attachmentSettings;
            _logger = loggerFactory.CreateLogger<VacanciesController>();
            _holidays = holidays;
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
            return View(model);
        }

        [PageTitle("Новая вакансия")]
        [HttpPost]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        public ActionResult Create(VacancyCreateViewModel model)
        {
            if (model.ToPublish)
            {
                if (model.ContractType == ContractType.FixedTerm
                    && model.ContractTimeYears == 0
                    && model.ContractTimeMonths == 0)
                    ModelState.AddModelError("ContractTypeValue", $"Для договора типа \"{ContractType.FixedTerm.GetDescription()}\" укажите срок трудового договора");
                if (model.SalaryFrom < _salaryMinValue)
                    ModelState.AddModelError("SalaryFrom", $"Значение зарплаты \"от\" не может быть меньше {_salaryMinValue}");
                if (model.SalaryTo < _salaryMinValue)
                    ModelState.AddModelError("SalaryTo", $"Значение зарплаты \"до\" не может быть меньше {_salaryMinValue}");
                if (model.SalaryFrom > model.SalaryTo)
                    ModelState.AddModelError("SalaryFrom", "Значение зарплаты \"от\" не может быть выше значения \"до\"");
                if (!model.RequiredFilled())
                {
                    if (!model.PositionTypeId.HasValue)
                        ModelState.AddModelError("PositionTypeId", "Для публикации Требуется выбрать Должность");
                    if (!model.ResearchDirectionId.HasValue)
                        ModelState.AddModelError("ResearchDirectionId", "Для публикации Требуется выбрать Отрасль науки");
                    if (!model.RegionId.HasValue)
                        ModelState.AddModelError("RegionId", "Для публикации Требуется выбрать регион");
                    if (!model.SalaryFrom.HasValue)
                        ModelState.AddModelError("SalaryFrom", "Для публикации Укажите минимальную зарплату");
                    if (!model.SalaryTo.HasValue)
                        ModelState.AddModelError("SalaryTo", "Для публикации Укажите максимальную зарплату");
                    if (string.IsNullOrWhiteSpace(model.Tasks))
                        ModelState.AddModelError("Tasks", "Для публикации Требуется описать Задачи");
                    if (string.IsNullOrWhiteSpace(model.ContactName))
                        ModelState.AddModelError("ContactName", "Для публикации Укажите контактное лицо");
                    if (string.IsNullOrWhiteSpace(model.ContactEmail))
                        ModelState.AddModelError("ContactEmail", "Для публикации Укажите E-mail");
                    if (string.IsNullOrWhiteSpace(model.ContactPhone))
                        ModelState.AddModelError("ContactPhone", "Для публикации Укажите номер телефона");
                }

                if (ModelState.ErrorCount > 0)
                {
                    model.InitDictionaries(_mediator);
                    return View("Create", model);
                }
            }

            if (!ModelState.IsValid || ModelState.ErrorCount > 0)
            {
                model.InitDictionaries(_mediator);
                return View("Create", model);
            }

            var vacancyDataModel = Mapper.Map<VacancyDataModel>(model);

            Organization organization = _mediator.Send(new SingleOrganizationQuery { OrganizationGuid = model.OrganizationGuid });
            vacancyDataModel.OrganizationFoivId = organization.foiv_id;

            var vacancyGuid = _mediator.Send(new CreateVacancyCommand { OrganizationGuid = model.OrganizationGuid, Data = vacancyDataModel });
            if (model.ToPublish)
                return RedirectToAction("publish", new { id = vacancyGuid });

            return RedirectToAction("details", new { id = vacancyGuid });

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
        [BindOrganizationIdFromClaims("claimedUserGuid")]
        public IActionResult Edit(VacancyCreateViewModel model, Guid claimedUserGuid)
        {
            if (claimedUserGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(claimedUserGuid));

            if (model.OrganizationGuid != claimedUserGuid)
                return View("Error", "Вы не можете изменять вакансии других организаций");

            if (model.ToPublish)
            {
                if (model.ContractType == ContractType.FixedTerm
                    && model.ContractTimeYears == 0
                    && model.ContractTimeMonths == 0)
                    ModelState.AddModelError("ContractTypeValue", $"Для договора типа \"{ContractType.FixedTerm.GetDescription()}\" укажите срок трудового договора");
                if (model.SalaryFrom < _salaryMinValue)
                    ModelState.AddModelError("SalaryFrom", $"Значение зарплаты \"от\" не может быть меньше {_salaryMinValue}");
                if (model.SalaryTo < _salaryMinValue)
                    ModelState.AddModelError("SalaryTo", $"Значение зарплаты \"до\" не может быть меньше {_salaryMinValue}");
                if (model.SalaryFrom > model.SalaryTo)
                    ModelState.AddModelError("SalaryFrom", "Значение зарплаты \"от\" не может быть выше значения \"до\"");
                if (!model.RequiredFilled())
                {
                    if (!model.PositionTypeId.HasValue)
                        ModelState.AddModelError("PositionTypeId", "Для публикации Требуется выбрать Должность");
                    if (!model.ResearchDirectionId.HasValue)
                        ModelState.AddModelError("ResearchDirectionId", "Для публикации Требуется выбрать Отрасль науки");
                    if (!model.RegionId.HasValue)
                        ModelState.AddModelError("RegionId", "Для публикации Требуется выбрать регион");
                    if (!model.SalaryFrom.HasValue)
                        ModelState.AddModelError("SalaryFrom", "Для публикации Укажите минимальную зарплату");
                    if (!model.SalaryTo.HasValue)
                        ModelState.AddModelError("SalaryTo", "Для публикации Укажите максимальную зарплату");
                    if (string.IsNullOrWhiteSpace(model.Tasks))
                        ModelState.AddModelError("Tasks", "Для публикации Требуется описать Задачи");
                    if (string.IsNullOrWhiteSpace(model.ContactName))
                        ModelState.AddModelError("ContactName", "Для публикации Укажите контактное лицо");
                    if (string.IsNullOrWhiteSpace(model.ContactEmail))
                        ModelState.AddModelError("ContactEmail", "Для публикации Укажите E-mail");
                    if (string.IsNullOrWhiteSpace(model.ContactPhone))
                        ModelState.AddModelError("ContactPhone", "Для публикации Укажите номер телефона");
                }
            }

            if (!ModelState.IsValid || ModelState.ErrorCount > 0)
            {
                model.InitDictionaries(_mediator);
                return View("Create", model);
            }

            var vacancy = _mediator.Send(new SingleVacancyQuery { VacancyGuid = model.Guid });
            if (vacancy == null)
                return HttpNotFound(); //throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором {model.Guid}");

            if (vacancy.status != VacancyStatus.InProcess)
                return View("Error", $"Вы не можете изменить вакансию с текущим статусом: {vacancy.status.GetDescription()}");


            if (ModelState.ErrorCount > 0)
            {
                model.InitDictionaries(_mediator);
                return View(model);
            }

            var vacancyDataModel = Mapper.Map<VacancyDataModel>(model);
            var vacancyGuid = _mediator.Send(new UpdateVacancyCommand { VacancyGuid = model.Guid, Data = vacancyDataModel });

            if (model.ToPublish && model.RequiredFilled())
            { return RedirectToAction("publish", new { id = model.Guid }); }

            return RedirectToAction("details", new { id = model.Guid });

        }

        [PageTitle("Подробно о вакансии")]
        public IActionResult Preview(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var preModel = _mediator.Send(new SingleVacancyQuery { VacancyGuid = id });

            if (preModel == null)
                return HttpNotFound(); //throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            //TODO: вынести однотипную инициализацию в отдельный сервис
            var model = Mapper.Map<VacancyDetailsViewModel>(preModel);
            model.Winner =
                Mapper.Map<ResearcherDetailsViewModel>(_mediator.Send(new SingleResearcherQuery { ResearcherGuid = preModel.winner_researcher_guid }));
            model.Pretender =
                Mapper.Map<ResearcherDetailsViewModel>(_mediator.Send(new SingleResearcherQuery { ResearcherGuid = preModel.pretender_researcher_guid }));
            model.Criterias = _mediator.Send(new SelectVacancyCriteriasQuery { VacancyGuid = model.Guid });
            model.CriteriasHierarchy =
                    _mediator.Send(new SelectAllCriteriasQuery()).ToList().ToHierarchyCriteriaViewModelList(model.Criterias.ToList());
            model.Attachments = _mediator.Send(new SelectAllExceptCommitteeVacancyAttachmentsQuery { VacancyGuid = model.Guid }).ToList();
            model.AttachmentsCommittee = _mediator.Send(new SelectCommitteeVacancyAttachmentsQuery { VacancyGuid = model.Guid }).ToList();
            model.ResearchDirection = _mediator.Send(new SelectResearchDirectionQuery { Id = preModel.researchdirection_id });
            return View(model);
        }

        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        [PageTitle("Подробно о вакансии")]
        [BindOrganizationIdFromClaims]
        [ValidatePagerParameters]
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

            if (preModel.committee_start_date.HasValue)
                model.MaxProlongedDate = preModel.committee_start_date.Value.AddMinutesIncludingHolidays(
                    _sagaSettings.Value.Date.Committee.ProlongingMinutes + _sagaSettings.Value.Date.DeltaFromInCommitteeStartToEndMinutes
                    , _holidays.Value.Dates
                    );

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
                model.Applications.Items.Where(c => c.ResearcherGuid == Guid.Empty).ToList().ForEach(c => model.Applications.Items.Remove(c));

            model.Criterias = _mediator.Send(new SelectVacancyCriteriasQuery { VacancyGuid = model.Guid });
            model.CriteriasHierarchy =
                    _mediator.Send(new SelectAllCriteriasQuery()).ToList().ToHierarchyCriteriaViewModelList(model.Criterias.ToList());
            model.Attachments = _mediator.Send(new SelectAllExceptCommitteeVacancyAttachmentsQuery { VacancyGuid = model.Guid }).ToList();
            model.AttachmentsCommittee = _mediator.Send(new SelectCommitteeVacancyAttachmentsQuery { VacancyGuid = model.Guid }).ToList();
            model.ResearchDirection = _mediator.Send(new SelectResearchDirectionQuery { Id = preModel.researchdirection_id });
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
            if (model.Status != VacancyStatus.InProcess
                && model.Status != VacancyStatus.Removed
                && researcherGuid != Guid.Empty)
            {
                //если текущая вакансия есть в списке избранных
                ViewBag.VacancyInFavorites = _mediator.Send(new SelectFavoriteVacancyGuidsByResearcherQuery
                {
                    ResearcherGuid = researcherGuid
                }).Any(c => c == id);

                //найти поданные заявки
                model.AppliedByUserApplications =
                    _mediator.Send(new SelectVacancyApplicationsByResearcherQuery { ResearcherGuid = researcherGuid })
                        .Where(
                            c =>
                                c.status != VacancyApplicationStatus.InProcess &&
                                c.status != VacancyApplicationStatus.Removed)
                        .ToList();
            }
            model.Criterias = _mediator.Send(new SelectVacancyCriteriasQuery { VacancyGuid = model.Guid });
            model.CriteriasHierarchy =
                    _mediator.Send(new SelectAllCriteriasQuery()).ToList().ToHierarchyCriteriaViewModelList(model.Criterias.ToList());
            model.Attachments = _mediator.Send(new SelectAllExceptCommitteeVacancyAttachmentsQuery { VacancyGuid = model.Guid }).ToList();
            model.AttachmentsCommittee = _mediator.Send(new SelectCommitteeVacancyAttachmentsQuery { VacancyGuid = model.Guid }).ToList();
            model.ResearchDirection = _mediator.Send(new SelectResearchDirectionQuery { Id = preModel.researchdirection_id });
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

            if (preModel.status == VacancyStatus.InProcess
                || preModel.status == VacancyStatus.Closed
                || preModel.status == VacancyStatus.Cancelled
                || preModel.status == VacancyStatus.Removed
                || preModel.status == VacancyStatus.Published
                || preModel.status == VacancyStatus.InCommittee
                || preModel.status == VacancyStatus.OfferResponseAwaitingFromWinner
                || preModel.status == VacancyStatus.OfferResponseAwaitingFromPretender
                )
                return View("Error", $"Вы не можете отменить вакансию со статусом: {preModel.status.GetDescription()}");

            var model = Mapper.Map<VacancyDetailsViewModel>(preModel);

            return View(model);
        }
        [HttpPost]
        [BindOrganizationIdFromClaims]
        public IActionResult Cancel(Guid id, Guid organizationGuid, string reason)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));
            if (string.IsNullOrEmpty(reason))
                return View("Error", "Вы не указали причину отмены");

            var model = _mediator.Send(new SingleVacancyQuery { VacancyGuid = id });

            if (model == null)
                return HttpNotFound(); //throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            if (model.organization_guid != organizationGuid)
                return View("Error", "Вы не можете отменить вакансии других организаций");

            if (model.status == VacancyStatus.InProcess
              || model.status == VacancyStatus.Closed
              || model.status == VacancyStatus.Cancelled
              || model.status == VacancyStatus.Removed
              || model.status == VacancyStatus.Published
              || model.status == VacancyStatus.InCommittee
              || model.status == VacancyStatus.OfferResponseAwaitingFromWinner
              || model.status == VacancyStatus.OfferResponseAwaitingFromPretender
              )

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

            return View(model);
        }

        //TODO: удалить этот метод в действующем сайте
        //[BindOrganizationIdFromClaims]
        //[Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        //public IActionResult StartInCommittee(Guid id, Guid organizationGuid)
        //{
        //    if (id == Guid.Empty)
        //        throw new ArgumentNullException(nameof(id));

        //    if (organizationGuid == Guid.Empty)
        //        throw new ArgumentNullException(nameof(organizationGuid));

        //    var preModel = _mediator.Send(new SingleVacancyQuery { VacancyGuid = id });

        //    if (preModel == null)
        //        return HttpNotFound(); //throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

        //    if (preModel.organization_guid != organizationGuid)
        //        return View("Error", "Вы не можете менять Вакансии других организаций");

        //    if (preModel.status != VacancyStatus.Published)
        //        return View("Error", $"Вы не можете перевести Вакансию на рассмотрение комиссии со статусом: {preModel.status.GetDescription()}");

        //    //TODO: Saga -> реализовать эту проверку при запуске Саг с таймерами
        //    if ((DateTime.UtcNow - preModel.committee_start_date.Value.ToUniversalTime()).TotalMinutes < _sagaSettings.Value.Date.DeltaFromPublishToInCommitteeMinMinutes)
        //        return View("Error", $"Вы не можете начать перевести вакансию на рассмотрение комиссии раньше чем через {_sagaSettings.Value.Date.DeltaFromPublishToInCommitteeMinMinutes} мин. Текущее время сервера в UTC: {DateTime.UtcNow}, Время начала комиссии в UTC: {preModel.committee_start_date.Value.ToUniversalTime()}");

        //    var vacancyApplications = _mediator.Send(new CountVacancyApplicationInVacancyQuery
        //    {
        //        VacancyGuid = preModel.guid,
        //        Status = VacancyApplicationStatus.Applied
        //    });

        //    if (vacancyApplications == 0)
        //        //если нет заявок, то закрыть вакансию
        //        _mediator.Send(new CancelVacancyCommand { VacancyGuid = preModel.guid, Reason = "На Вакансию не подано ни одной Заявки." });
        //    else
        //        _mediator.Send(new SwitchVacancyInCommitteeCommand { VacancyGuid = id });

        //    return RedirectToAction("details", new { id });
        //}

        private object AddCommitteeReasonPreValidation(Guid vacancyGuid, Guid organizationGuid)
        {
            var vacancy = _mediator.Send(new SingleVacancyQuery { VacancyGuid = vacancyGuid });

            if (vacancy == null)
                return HttpNotFound();

            if (vacancy.organization_guid != organizationGuid)
                return View("Error", "Вы не можете изменять Вакансии, других организаций.");

            if (vacancy.status != VacancyStatus.InCommittee)
                return View("Error", $"Вы не можете указать конкурсное обоснование для Вакансии со статусом: {vacancy.status.GetDescription()}");

            var vacancyInCommitteeAttachments = _mediator.Send(new SelectCommitteeVacancyAttachmentsQuery { VacancyGuid = vacancyGuid });
            if (!string.IsNullOrWhiteSpace(vacancy.committee_resolution)
                || (vacancyInCommitteeAttachments != null && vacancyInCommitteeAttachments.Any()))
                return View("Error", "В вакансии УЖЕ УКАЗАНО конкурсное обоснование выбора победителя (и претендента).");


            var appliedVacancyApplications = _mediator.Send(new SelectVacancyApplicationInVacancyByStatusesQuery
            {
                VacancyGuid = vacancyGuid,
                Statuses = new List<VacancyApplicationStatus> { VacancyApplicationStatus.Applied, VacancyApplicationStatus.Won, VacancyApplicationStatus.Pretended }
            });
            if (appliedVacancyApplications == null || !appliedVacancyApplications.Any())
                return View("Error", "Для данной Вакансии не подано ни одной действующей заявки.");

            if (appliedVacancyApplications.Count() == 1) //подана всего 1 заявка
            {
                if (vacancy.winner_researcher_guid == Guid.Empty)
                    return View("Warning", "Для данной Вакансии еще не выбран Победитель.");
            }
            else
            {
                //if (appliedVacancyApplications.Count() > 1) // подано несколько заявок
                if (vacancy.winner_researcher_guid == Guid.Empty || vacancy.pretender_researcher_guid == Guid.Empty)
                    return View("Warning", "Для данной Вакансии еще не выбран Победитель и/или Претендент.");
            }

            return vacancy;
        }

        [PageTitle("Обоснование решения комиссии")]
        [BindOrganizationIdFromClaims]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        public IActionResult AddCommitteeReason(Guid id, Guid organizationGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var result = AddCommitteeReasonPreValidation(id, organizationGuid);
            if (result is HttpNotFoundResult) return (HttpNotFoundResult)result;
            if (result is ViewResult) return (ViewResult)result;
            var vacancy = (Vacancy)result;

            var model = new VacancySetReasonViewModel
            {
                Guid = id,
                Vacancy = Mapper.Map<VacancyDetailsViewModel>(vacancy)
            };

            return View(model);
        }

        [PageTitle("Обоснование решения комиссии")]
        [HttpPost]
        [BindOrganizationIdFromClaims]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        public IActionResult AddCommitteeReason(VacancySetReasonViewModel model, Guid organizationGuid)
        {
            if (model.Guid == Guid.Empty)
                throw new ArgumentNullException(nameof(model.Guid));
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var result = AddCommitteeReasonPreValidation(model.Guid, organizationGuid);
            if (result is HttpNotFoundResult) return (HttpNotFoundResult)result;
            if (result is ViewResult) return (ViewResult)result;
            var vacancy = (Vacancy)result;

            if (string.IsNullOrWhiteSpace(model.Reason) /* && (model.Attachments == null || !model.Attachments.Any())*/)
                ModelState.AddModelError("Reason", "Укажите обоснование решения комиссии" /*либо прикрепите протоколы решения */);


            //TODO: Application -> Attachments : как проверять безопасность, прикрепляемых файлов
            //todo: повторяющийся код
            if (model.Attachments != null && model.Attachments.Any())
            {
                //проверяем размеры файлов
                if (model.Attachments.Any(c => c.Length > _attachmentSettings.Value.Vacancy.MaxItemSize))
                    ModelState.AddModelError("Attachments", $"Размер одного из прикрепленных файлов превышает допустимый размер ({_attachmentSettings.Value.Vacancy.MaxItemSize / 1000}КБ).");

                //проверяем расширения файлов, если они указаны
                if (!string.IsNullOrWhiteSpace(_attachmentSettings.Value.Vacancy.AllowExtensions))
                {
                    var fileNames =
                        model.Attachments.Select(
                            c =>
                                System.IO.Path.GetFileName(
                                    ContentDispositionHeaderValue.Parse(c.ContentDisposition).FileName.Trim('"')));
                    var fileExtensionsInUpper = fileNames.Select(c =>
                        c.Split('.')
                        .Last()
                        .ToUpper()
                        );
                    var allowedExtenionsInUpper = _attachmentSettings.Value.Vacancy.AllowExtensions.ToUpper();
                    if (fileExtensionsInUpper.Any(c => !allowedExtenionsInUpper.Contains(c)))
                        ModelState.AddModelError("Attachments", $"Расширение одного из прикрепленных файлов имеет недопустимое расширение. Допустимые типы файлов: {allowedExtenionsInUpper}");
                }

            }

            if (!ModelState.IsValid)
            {
                model.Vacancy = Mapper.Map<VacancyDetailsViewModel>(vacancy);
                return View(model);
            }

            #region attachments
            var attachmentsList = new List<SciVacancies.Domain.Core.VacancyAttachment>();
            //save attachments
            if (model.Attachments != null && model.Attachments.Any())
            {
                var newFolderName = Guid.NewGuid();
                var fullDirectoryPath = $"{_hostingEnvironment.WebRootPath}{_attachmentSettings.Value.Vacancy.PhisicalPathPart}/{newFolderName}/";

                foreach (var file in model.Attachments)
                {
                    var fileName = System.IO.Path.GetFileName(ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"'));
                    var isExists = Directory.Exists(fullDirectoryPath);
                    //сценарий-А: сохранить файл на диск
                    try
                    {
                        //TODO: Application -> Attachments : как искать Текущую директорию при повторном добавлении(изменении текущего списка) файлов
                        //TODO: Application -> Attachments : можно ли редактировать список файлов, или Заявки создаются разово и для каждой генеиртся новая папка с вложениями
                        if (!isExists)
                            Directory.CreateDirectory(fullDirectoryPath);
                        var filePath =
                            $"{_hostingEnvironment.WebRootPath}{_attachmentSettings.Value.Vacancy.PhisicalPathPart}/{newFolderName}/{fileName}";
                        file.SaveAs(filePath);
                        attachmentsList.Add(new SciVacancies.Domain.Core.VacancyAttachment
                        {
                            Size = file.Length,
                            Extension = fileName.Split('.').Last(),
                            Name = fileName,
                            UploadDate = DateTime.Now,
                            Url = $"/{newFolderName}/{fileName}"
                        });

                    }
                    catch (Exception)
                    {
                        if (!isExists)
                            RemoveAttachmentDirectory(fullDirectoryPath);
                        return View("Error", "Ошибка при сохранении прикреплённых файлов");
                    }

                    //TODO: сохранение файл в БД (сделать)
                    //using (var memoryStream = new MemoryStream())
                    //{
                    //    файл в byte
                    //    byte[] byteData;
                    //    //сценарий-Б: сохранить файл в БД
                    //    //var openReadStream = file.OpenReadStream();
                    //    //var scale = (int)(500000 / file.Length);
                    //    //var resizedImage = new Bitmap(image, new Size(image.Width * scale, image.Height * scale));
                    //    //((Image)resizedImage).Save(memoryStream, ImageFormat.Jpeg);
                    //    //byteData = memoryStream.ToArray();
                    //    //memoryStream.SetLength(0);

                    //    //сценарий-В: сохранить файл в БД
                    //    //var openReadStream = file.OpenReadStream();
                    //    //openReadStream.CopyTo(memoryStream);
                    //    //byteData = memoryStream.ToArray();
                    //    //memoryStream.SetLength(0);
                    //}

                }

                //присваиваем прикреплённым файлам тип "Решение комиссии" (для соответствущей выборки)
                attachmentsList.ForEach(c => c.TypeId = 1);
            }
            #endregion

            //задать решение/обоснование комиссии
            _mediator.Send(new SetVacancyCommitteeResolutionCommand
            {
                Resolution = model.Reason,
                Attachments = attachmentsList,
                VacancyGuid = model.Guid
            });

            vacancy = _mediator.Send(new SingleVacancyQuery { VacancyGuid = vacancy.guid });

            //пометить Заявку как Победитель
            _mediator.Send(new MakeVacancyApplicationWinnerCommand
            {
                ResearcherGuid = vacancy.winner_researcher_guid,
                VacancyApplicationGuid = vacancy.winner_vacancyapplication_guid,
                Reason = vacancy.committee_resolution
            });
            //пометить Заявку как Претендент
            if (vacancy.pretender_researcher_guid != Guid.Empty)
            {
                _mediator.Send(new MakeVacancyApplicationPretenderCommand
                {
                    ResearcherGuid = vacancy.pretender_researcher_guid,
                    VacancyApplicationGuid = vacancy.pretender_vacancyapplication_guid,
                    Reason = vacancy.committee_resolution
                });
            }

            //отметить лузеров
            var stillAppliedApplications = _mediator.Send(new SelectVacancyApplicationInVacancyByStatusesQuery { VacancyGuid = model.Guid, Statuses = new List<VacancyApplicationStatus> { VacancyApplicationStatus.Applied } });
            stillAppliedApplications?.ToList().ForEach(c => _mediator.Send(new MakeVacancyApplicationLooserCommand
            {
                ResearcherGuid = c.researcher_guid,
                VacancyApplicationGuid = c.guid
            }));

            //переключить вакансию в статус ожидания ответа от победителя
            _mediator.Send(new SetVacancyToResponseAwaitingFromWinnerCommand
            {
                VacancyGuid = model.Guid
            });

            return RedirectToAction(actionName: "details", routeValues: new { id = model.Guid });
        }

        private object SetWinnerPreValidation(Guid vacancyApplicationGuid, Guid organizationGuid, bool isWinner, out Vacancy vacancy, out VacancyApplication vacancyApplication)
        {
            vacancyApplication = _mediator.Send(new SingleVacancyApplicationQuery { VacancyApplicationGuid = vacancyApplicationGuid });
            vacancy = null;

            if (vacancyApplication == null)
                return HttpNotFound(); //throw new ObjectNotFoundException($"Не найдена Заявка c идентификатором: {vacancyApplicationGuid}");

            if (vacancyApplication.status != VacancyApplicationStatus.Applied)
                return View("Error",
                    $"Вы не можете выбрать в качестве одного из победителей Заявку со статусом: {vacancyApplication.status.GetDescription()}");

            vacancy = _mediator.Send(new SingleVacancyQuery { VacancyGuid = vacancyApplication.vacancy_guid });

            if (vacancy == null)
                return HttpNotFound(); //throw new ObjectNotFoundException($"Не найдена Вакансия c идентификатором: {vacancyApplicaiton.vacancy_guid}");

            if (vacancy.organization_guid != organizationGuid)
                return View("Error", "Вы не можете изменять Заявки, поданные на вакансии других организаций.");

            if (isWinner && vacancy.winner_researcher_guid != Guid.Empty)
                return View("Error", "Для данной Вакансии уже выбран Победитель.");

            if (!isWinner && vacancy.winner_researcher_guid == Guid.Empty)
                return View("Error", "Для данной Вакансии еще не выбран Победитель.");

            if (vacancy.winner_researcher_guid != Guid.Empty && vacancy.pretender_researcher_guid != Guid.Empty)
                //return View("Error", "Для данной Вакансии уже выбраны Победитель и Претендент.");
                return RedirectToAction("addcommitteereason", new { id = vacancy.guid, applicationGuid = vacancyApplication.guid });

            if (vacancy.status != VacancyStatus.InCommittee)
                return View("Error",
                    $"Вы не можете выбирать победителя для Заявки со статусом: {vacancy.status.GetDescription()}");

            return null;
        }


        [PageTitle("Выбрать Победителя или Претендента")]
        [BindOrganizationIdFromClaims]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        public IActionResult SetWinner(Guid id, Guid organizationGuid, bool isWinner)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            Vacancy vacancy;
            VacancyApplication vacancyApplication;
            var result = SetWinnerPreValidation(id, organizationGuid, isWinner, out vacancy, out vacancyApplication);
            if (result is HttpNotFoundResult) return (HttpNotFoundResult)result;
            if (result is ViewResult) return (ViewResult)result;
            if (result is RedirectToActionResult) return (RedirectToActionResult)result;

            var model = Mapper.Map<VacancyApplicationSetWinnerViewModel>(vacancyApplication);
            model.Vacancy = Mapper.Map<VacancyDetailsViewModel>(vacancy);
            model.WinnerIsSetting = isWinner;

            return View(model);
        }

        [PageTitle("Выбрать Победителя или Претендента")]
        [HttpPost]
        [BindOrganizationIdFromClaims]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        public IActionResult SetWinner(VacancyApplicationSetWinnerViewModel model, Guid organizationGuid)
        {
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            Vacancy vacancy;
            VacancyApplication vacancyApplication;
            var result = SetWinnerPreValidation(model.Guid, organizationGuid, model.WinnerIsSetting, out vacancy, out vacancyApplication);
            if (result is HttpNotFoundResult) return (HttpNotFoundResult)result;
            if (result is ViewResult) return (ViewResult)result;
            if (result is RedirectToActionResult) return (RedirectToActionResult)result;

            if (!ModelState.IsValid)
            {
                model = Mapper.Map<VacancyApplicationSetWinnerViewModel>(vacancyApplication);
                model.Vacancy = Mapper.Map<VacancyDetailsViewModel>(vacancy);
                return View(model);
            }

            //количество поданнных заявок
            var appliedVacancyApplicationsCount = _mediator.Send(new CountVacancyApplicationInVacancyQuery
            {
                VacancyGuid = model.VacancyGuid,
                Status = VacancyApplicationStatus.Applied
            });

            if (model.WinnerIsSetting)
            {
                if (vacancy.winner_researcher_guid == Guid.Empty)
                {
                    //добавить инфу о победителе в вакансию
                    _mediator.Send(new SetVacancyWinnerCommand
                    {
                        VacancyGuid = model.VacancyGuid,
                        ResearcherGuid = model.ResearcherGuid,
                        VacancyApplicationGuid = model.Guid
                    });
                }
            }
            else
            {
                if (appliedVacancyApplicationsCount > 1 && vacancy.winner_researcher_guid != Guid.Empty && vacancy.pretender_researcher_guid == Guid.Empty)
                {
                    //добавить инфу о претенденте в вакансию
                    _mediator.Send(new SetVacancyPretenderCommand
                    {
                        VacancyGuid = model.VacancyGuid,
                        ResearcherGuid = model.ResearcherGuid,
                        VacancyApplicationGuid = model.Guid
                    });

                }
            }

            //если Победитель (и претендент) заданы, то переадресовать на страницу с Обоснованием
            if (
                (appliedVacancyApplicationsCount == 1 && vacancy.winner_researcher_guid != Guid.Empty)
                || (appliedVacancyApplicationsCount > 1 && vacancy.winner_researcher_guid != Guid.Empty && vacancy.pretender_researcher_guid != Guid.Empty)
                )
            {
                var vacancyInCommitteeAttachments = _mediator.Send(new SelectCommitteeVacancyAttachmentsQuery { VacancyGuid = vacancy.guid });
                if (string.IsNullOrWhiteSpace(vacancy.committee_resolution)
                    && (vacancyInCommitteeAttachments == null || !vacancyInCommitteeAttachments.Any()))
                    return RedirectToAction("addcommitteereason", new { id = vacancy.guid, applicationGuid = vacancyApplication.guid });
            }

            return RedirectToAction("preview", "applications", new { id = model.Guid });
        }

        [PageTitle("Предложить вакансию претенденту")]
        [BindOrganizationIdFromClaims]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        public IActionResult ReofferToPretender(Guid id, Guid organizationGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            Vacancy vacancy;
            var result = ReofferToPretenderPreValidation(id, organizationGuid, out vacancy);
            if (result is HttpNotFoundResult) return (HttpNotFoundResult)result;
            if (result is ViewResult) return (ViewResult)result;

            //передать ваканасию претенденту
            _mediator.Send(new SetVacancyToResponseAwaitingFromPretenderCommand { VacancyGuid = id });

            //obsolete - добавить статус для заявки
            //организация для победителя....

            return RedirectToAction("preview", "applications", new { id = vacancy.pretender_vacancyapplication_guid });
        }

        private object ReofferToPretenderPreValidation(Guid id, Guid organizationGuid, out Vacancy vacancy)
        {

            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            vacancy = _mediator.Send(new SingleVacancyQuery { VacancyGuid = id });

            if (vacancy == null)
                return HttpNotFound();

            if (vacancy.organization_guid != organizationGuid)
                return View("Error", "Вы не можете менять Вакансии других организаций");

            if (vacancy.winner_vacancyapplication_guid == Guid.Empty)
                return View("Error", "У вакансии не выбран Победитель");

            if (vacancy.pretender_vacancyapplication_guid == Guid.Empty)
                return View("Error", "У вакансии не выбран претендент");

            if (!vacancy.is_winner_accept.HasValue)
                return View("Error", "Победитель еще не принял решение");

            if (vacancy.is_pretender_accept.HasValue)
                return View("Error", "Победитель уже принял решение по вакансии");

            return null;
        }

        [PageTitle("Закрыть вакансию")]
        [BindOrganizationIdFromClaims]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        public IActionResult Close(Guid id, Guid organizationGuid)
        {
            Vacancy vacancy;
            var result = VacancyClosePrevalidation(id, organizationGuid, out vacancy);
            if (result is HttpNotFoundResult) return (HttpNotFoundResult)result;
            if (result is ViewResult) return (ViewResult)result;

            var model = Mapper.Map<VacancyDetailsViewModel>(vacancy);
            model.Winner = Mapper.Map<ResearcherDetailsViewModel>(_mediator.Send(new SingleResearcherQuery { ResearcherGuid = model.WinnerResearcherGuid }));
            if (model.PretenderResearcherGuid != Guid.Empty)
                model.Pretender = Mapper.Map<ResearcherDetailsViewModel>(_mediator.Send(new SingleResearcherQuery { ResearcherGuid = model.PretenderResearcherGuid }));
            //todo: ntemnikov -> показать кто принял предложение - Победитель или Претендент
            return View(model);
        }

        [PageTitle("Закрыть вакансию")]
        [HttpPost]
        [BindOrganizationIdFromClaims]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        public IActionResult ClosePost(Guid id, Guid organizationGuid)
        {
            Vacancy vacancy;
            var result = VacancyClosePrevalidation(id, organizationGuid, out vacancy);
            if (result is HttpNotFoundResult) return (HttpNotFoundResult)result;
            if (result is ViewResult) return (ViewResult)result;

            _mediator.Send(new CloseVacancyCommand
            {
                VacancyGuid = id
            });

            return RedirectToAction("vacancies", "organizations");
        }

        private object VacancyClosePrevalidation(Guid id, Guid organizationGuid, out Vacancy preModel)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            preModel = _mediator.Send(new SingleVacancyQuery { VacancyGuid = id });

            if (preModel == null)
                return HttpNotFound(); //throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            if (preModel.organization_guid != organizationGuid)
                return View("Error", "Вы не можете менять Вакансии других организаций");

            if (preModel.status != VacancyStatus.OfferAcceptedByWinner && preModel.status != VacancyStatus.OfferAcceptedByPretender)
                return View("Error", "Вы не можете закрыть вакансию, пока кто-то из победителей не даст согласится");
            return null;
        }

        private object PublishPreValidation(Guid id, Guid organizationGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var model = _mediator.Send(new SingleVacancyQuery { VacancyGuid = id });

            if (model == null)
                return HttpNotFound(); //throw new ObjectNotFoundException($"Не найдена вакансия с идентификатором: {id}");

            if (model.contract_type == ContractType.FixedTerm && (!model.contract_time.HasValue || model.contract_time.Value == 0.0m))
                return View("Error", $"Для договора типа \"{ContractType.FixedTerm.GetDescription()}\" укажите срок трудового договора");
            if (model.salary_from < _salaryMinValue)
                return View("Error", $"Значение зарплаты \"от\" не может быть меньше {_salaryMinValue}");
            if (model.salary_to < _salaryMinValue)
                return View("Error", $"Значение зарплаты \"до\" не может быть меньше {_salaryMinValue}");
            if (model.salary_from > model.salary_to)
                return View("Error", "Значение зарплаты \"от\" не может быть выше значения \"до\"");
            if (!model.positiontype_id.HasValue)
                return View("Error", "Для публикации Требуется выбрать Должность");
            if (!model.researchdirection_id.HasValue)
                return View("Error", "Для публикации Требуется выбрать Отрасль науки");
            if (!model.region_id.HasValue)
                return View("Error", "Для публикации Требуется выбрать регион");
            if (!model.salary_from.HasValue)
                return View("Error", "Для публикации Укажите минимальную зарплату");
            if (!model.salary_to.HasValue)
                return View("Error", "Для публикации Укажите максимальную зарплату");
            if (string.IsNullOrWhiteSpace(model.tasks))
                return View("Error", "Для публикации Требуется описать Задачи");
            if (string.IsNullOrWhiteSpace(model.contact_name))
                return View("Error", "Для публикации Укажите контактное лицо");
            if (string.IsNullOrWhiteSpace(model.contact_email))
                return View("Error", "Для публикации Укажите E-mail");
            if (string.IsNullOrWhiteSpace(model.contact_phone))
                return View("Error", "Для публикации Укажите номер телефона");

            if (model.organization_guid != organizationGuid)
                return View("Error", "Вы не можете публиковать вакансии других организаций");

            if (model.status != VacancyStatus.InProcess)
                return View("Error", $"Вы не можете опубликовать вакансию со статусом: {model.status.GetDescription()}");



            return model;
        }

        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        [BindOrganizationIdFromClaims]
        [PageTitle("Публикация вакансии")]
        public IActionResult Publish(Guid id, Guid organizationGuid)
        {
            var result = PublishPreValidation(id, organizationGuid);
            if (result is HttpNotFoundResult) return (HttpNotFoundResult)result;
            if (result is ViewResult) return (ViewResult)result;
            var preModel = (Vacancy)result;

            var model = new VacancyPublishModel
            {
                Guid = id,
                Details = Mapper.Map<VacancyDetailsViewModel>(preModel)
            };
            model.Details.Criterias = _mediator.Send(new SelectVacancyCriteriasQuery { VacancyGuid = model.Details.Guid });
            model.Details.CriteriasHierarchy = _mediator.Send(new SelectAllCriteriasQuery()).ToList().ToHierarchyCriteriaViewModelList(model.Details.Criterias.ToList());
            model.Details.Attachments = _mediator.Send(new SelectAllExceptCommitteeVacancyAttachmentsQuery { VacancyGuid = model.Details.Guid }).ToList();
            model.Details.ResearchDirection = _mediator.Send(new SelectResearchDirectionQuery { Id = preModel.researchdirection_id });

            return View(model);
        }

        [PageTitle("Публикация вакансии")]
        [HttpPost]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        [BindOrganizationIdFromClaims]
        public IActionResult Publish(Guid id, string inCommitteeDateString, Guid organizationGuid)
        {
            var result = PublishPreValidation(id, organizationGuid);
            if (result is HttpNotFoundResult) return (HttpNotFoundResult)result;
            if (result is ViewResult) return (ViewResult)result;
            var preModel = (Vacancy)result;

            DateTime inCommitteeDateValue;
            if (!DateTime.TryParse(inCommitteeDateString, new CultureInfo("ru-RU"), DateTimeStyles.NoCurrentDateDefault, out inCommitteeDateValue))
                ModelState.AddModelError("InCommitteeDateString", "Мы не смогли распознать дату перевода вакансии на рассмотрение комиссии");

            //make date correction
            inCommitteeDateValue = new DateTime(inCommitteeDateValue.Year, inCommitteeDateValue.Month, inCommitteeDateValue.Day, inCommitteeDateValue.Hour, inCommitteeDateValue.Minute, inCommitteeDateValue.Second, DateTimeKind.Utc);

            if (inCommitteeDateValue < DateTime.UtcNow)
                ModelState.AddModelError("InCommitteeDateString", "Вы установили дату ранее текущей");

            if ((inCommitteeDateValue - DateTime.UtcNow).TotalMinutes < _sagaSettings.Value.Date.DeltaFromPublishToInCommitteeMinMinutes)
                ModelState.AddModelError("InCommitteeDateString", $"Вы не можете установить дату перевода вакансии на рассмотрение комиссии раньше, чем через {_sagaSettings.Value.Date.DeltaFromPublishToInCommitteeMinMinutes / (24 * 60)} календарных дн.");

            if (!ModelState.IsValid)
            {
                var model = new VacancyPublishModel
                {
                    Guid = id,
                    Details = Mapper.Map<VacancyDetailsViewModel>(preModel)
                };
                model.Details.Criterias = _mediator.Send(new SelectVacancyCriteriasQuery { VacancyGuid = model.Details.Guid });
                model.Details.CriteriasHierarchy = _mediator.Send(new SelectAllCriteriasQuery()).ToList().ToHierarchyCriteriaViewModelList(model.Details.Criterias.ToList());
                model.Details.Attachments = _mediator.Send(new SelectAllExceptCommitteeVacancyAttachmentsQuery { VacancyGuid = model.Details.Guid }).ToList();
                model.Details.ResearchDirection = _mediator.Send(new SelectResearchDirectionQuery { Id = preModel.researchdirection_id });

                return View(model);
            }

            _mediator.Send(new PublishVacancyCommand
            {
                VacancyGuid = id,
                InCommitteeStartDate = inCommitteeDateValue,
                InCommitteeEndDate = inCommitteeDateValue.AddMinutesIncludingHolidays(_sagaSettings.Value.Date.DeltaFromInCommitteeStartToEndMinutes, _holidays.Value.Dates)
            });

            return RedirectToAction("details", new { id });
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
                return View("Error", "Копировать можно только свои Вакансии");

            var model = Mapper.Map<VacancyCreateViewModel>(preModel);
            model.Guid = Guid.Empty;
            model.InitDictionaries(_mediator);

            return View("Create", model);
        }

        [PageTitle("Новая вакансия")]
        [HttpPost]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        public IActionResult Copy(VacancyCreateViewModel model)
        {
            return Create(model);
        }

        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        [PageTitle("Подробно о вакансии")]
        [BindOrganizationIdFromClaims]
        [ValidatePagerParameters]
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

            //TODO: оптимизировать запрос и его обработку.
            var page = _mediator.Send(new SelectPagedVacancyApplicationsByVacancyQuery
            {
                PageSize = 1,
                PageIndex = 1,
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
                model.Applications.Items.Where(c => c.ResearcherGuid == Guid.Empty).ToList().ForEach(c => model.Applications.Items.Remove(c));

            return View(model);
        }

        [HttpGet]
        [PageTitle("Продление срока рассмотрения вакансии")]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        [BindOrganizationIdFromClaims]
        public IActionResult ProlongInCommittee(Guid id, Guid organizationGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var preModel = _mediator.Send(new SingleVacancyQuery { VacancyGuid = id });

            if (preModel == null)
                return HttpNotFound();

            if (preModel.organization_guid != organizationGuid)
                return View("Error", "Продлять рассмотрение можно только для своих вакансий");

            if (preModel.status != VacancyStatus.InCommittee)
                return View("Error", "Продлять рассмотрение можно только для вакансий, находящихся на рассмотрении");

            if (preModel.status != VacancyStatus.InCommittee)
                return View("Error", "Продлять рассмотрение можно только для вакансий, находящихся на рассмотрении");

            if (!preModel.committee_start_date.HasValue)
                return View("Error", "Не указано начало периода рассмотрения вакансии");

            var maxProlongedDate = preModel.committee_start_date.Value.AddMinutesIncludingHolidays(
                _sagaSettings.Value.Date.Committee.ProlongingMinutes + _sagaSettings.Value.Date.DeltaFromInCommitteeStartToEndMinutes
                , _holidays.Value.Dates
                );

            if (preModel.committee_end_date.HasValue
                && preModel.committee_end_date.Value.Date >= maxProlongedDate.Date) //проверяем, что заданная дата окончания Рассмотрения находится в пределах максимального значения
                return View("Error", $"Вы не можете продлить рассмотрение до даты позднее чем 30 рабочих дней с начала рассмотрения. Т.е. с {maxProlongedDate.ToLocalMoscowVacancyDateString()} по {maxProlongedDate.ToLocalMoscowVacancyDateString()}");

            var model = Mapper.Map<VacancyDetailsViewModel>(preModel);

            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        [BindOrganizationIdFromClaims]
        public IActionResult ProlongInCommitteeSubmit(Guid id, string reason, Guid organizationGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));
            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentNullException(nameof(reason));

            var preModel = _mediator.Send(new SingleVacancyQuery { VacancyGuid = id });

            if (preModel == null)
                return HttpNotFound();

            if (preModel.organization_guid != organizationGuid)
                return View("Error", "Продлять рассмотрение можно только для своих вакансий");

            if (preModel.status != VacancyStatus.InCommittee)
                return View("Error", "Продлять рассмотрение можно только для вакансий, находящихся на рассмотрении");

            if (!preModel.committee_start_date.HasValue)
                return View("Error", "Не указано начало периода рассмотрения вакансии");

            var maxProlongedDate = preModel.committee_start_date.Value.AddMinutesIncludingHolidays(
                _sagaSettings.Value.Date.Committee.ProlongingMinutes + _sagaSettings.Value.Date.DeltaFromInCommitteeStartToEndMinutes
                , _holidays.Value.Dates
                );

            if (preModel.committee_end_date.HasValue
                && preModel.committee_end_date.Value.Date >= maxProlongedDate.Date) //проверяем, что заданная дата окончания Рассмотрения находится в пределах максимального значения
                return View("Error", $"Вы не можете продлить рассмотрение до даты позднее чем 30 рабочих дней с начала рассмотрения. Т.е. с {maxProlongedDate.ToLocalMoscowVacancyDateString()} по {maxProlongedDate.ToLocalMoscowVacancyDateString()}");

            try
            {
                //if (preModel.committee_end_date.HasValue)
                //    _mediator.Send(new ProlongVacancyInCommitteeCommand { VacancyGuid = id, Reason = reason, InCommitteeEndDate = preModel.committee_end_date.Value.AddMinutesIncludingHolidays(_sagaSettings.Value.Date.Committee.ProlongingMinutes, _holidays.Value.Dates) });
                //else
                //    _mediator.Send(new ProlongVacancyInCommitteeCommand { VacancyGuid = id, Reason = reason, InCommitteeEndDate = DateTime.UtcNow.AddMinutesIncludingHolidays(_sagaSettings.Value.Date.Committee.ProlongingMinutes, _holidays.Value.Dates) });
                _mediator.Send(new ProlongVacancyInCommitteeCommand { VacancyGuid = id, Reason = reason, InCommitteeEndDate = maxProlongedDate });
            }
            catch (Exception exception)
            {

            }

            return RedirectToAction("details", new { id });
        }



        private void RemoveAttachmentDirectory(string fullpath)
        {
            if (Directory.Exists(fullpath))
                Directory.Delete(fullpath);
        }

    }
}
