using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
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
        private readonly IOptions<VacancyLifeCycleSettings> _vacancyLifeCycleSettings;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IOptions<AttachmentSettings> _attachmentSettings;

        public VacanciesController(IMediator mediator, IOptions<VacancyLifeCycleSettings> vacancyLifeCycleSettings, IHostingEnvironment hostingEnvironment, IOptions<AttachmentSettings> attachmentSettings)
        {
            _mediator = mediator;
            _vacancyLifeCycleSettings = vacancyLifeCycleSettings;
            _hostingEnvironment = hostingEnvironment;
            _attachmentSettings = attachmentSettings;
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
            model.InCommitteeDate = DateTime.Now.AddDays(_vacancyLifeCycleSettings.Options.DeltaFromPublishToInCommitteeMinDays);
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


                if (model.InCommitteeDate < DateTime.Now)
                    ModelState.AddModelError("InCommitteeDate", "Вы установили дату ранее текущей");

                if (model.ToPublish)
                    if ((model.InCommitteeDate - DateTime.Now).Days < _vacancyLifeCycleSettings.Options.DeltaFromPublishToInCommitteeMinDays)
                        ModelState.AddModelError("InCommitteeDate", $"Вы не можете установить дату перевода вакансии на рассмотрение комиссии раньше, чем через {_vacancyLifeCycleSettings.Options.DeltaFromPublishToInCommitteeMinDays} дн.");

                if (ModelState.ErrorCount > 0)
                {
                    model.InitDictionaries(_mediator);
                    return View("create", model);
                }
                // раскоментировать когда будут прикреплённые файлы для вакансии
                //присваиваем прикреплённым файлам тип "Прочее" (для соответствущей выборки)
                //attachmentsList.ForEach(c => c.TypeId = 3);

                var vacancyGuid = _mediator.Send(new CreateVacancyCommand { OrganizationGuid = model.OrganizationGuid, Data = vacancyDataModel });
                if (model.ToPublish)
                {
                    _mediator.Send(new PublishVacancyCommand
                    {
                        VacancyGuid = vacancyGuid,
                        InCommitteeStartDate = model.InCommitteeDate,
                        InCommitteeEndDate = model.InCommitteeDate.AddHours(25) /*TODO: вынести в конфиг: количество дней до окончания комиссии*/
                    });
                }

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

                if (model.ToPublish)
                {
                    if (vacancy.status != VacancyStatus.InProcess)
                        ModelState.AddModelError("Status", $"Вы не можете публиковать вакансии в статусе {vacancy.status.GetDescription()}");

                    if ((DateTime.Now - model.InCommitteeDate).Days < _vacancyLifeCycleSettings.Options.DeltaFromPublishToInCommitteeMinDays)
                        ModelState.AddModelError("InCommitteeDate", $"Вы не можете установить дату перевода вакансии на рассмотрение комиссии раньше, чем через {_vacancyLifeCycleSettings.Options.DeltaFromPublishToInCommitteeMinDays} дн.");
                }

                if (ModelState.ErrorCount > 0)
                {
                    model.InitDictionaries(_mediator);
                    return View(model);
                }

                var vacancyDataModel = Mapper.Map<VacancyDataModel>(model);
                var vacancyGuid = _mediator.Send(new UpdateVacancyCommand { VacancyGuid = model.Guid, Data = vacancyDataModel });

                if (model.ToPublish)
                    _mediator.Send(new PublishVacancyCommand
                    {
                        VacancyGuid = model.Guid,
                        InCommitteeStartDate = model.InCommitteeDate, /*TODO: уточнять эту дату через модальное окно в UI перед отправкой данных на сервер (подтверждение пользователем)*/
                        InCommitteeEndDate = model.InCommitteeDate.AddHours(25) /*TODO: вынести в конфиг: количество дней до окончания комиссии*/
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

            //TODO: показать отрасль науки, направление исследований

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
            model.Criterias = _mediator.Send(new SelectVacancyCriteriasQuery { VacancyGuid = model.Guid });
            model.CriteriasHierarchy =
                    _mediator.Send(new SelectAllCriteriasQuery()).ToList().ToHierarchyCriteriaViewModelList(model.Criterias.ToList());
            model.Attachments = _mediator.Send(new SelectAllExceptCommitteeVacancyAttachmentsQuery { VacancyGuid = model.Guid }).ToList();
            model.AttachmentsCommittee = _mediator.Send(new SelectCommitteeVacancyAttachmentsQuery { VacancyGuid = model.Guid }).ToList();

            //TODO: показать отрасль науки, направление исследований

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

            //TODO: показать отрасль науки, направление исследований

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

            if ( model.status == VacancyStatus.InProcess
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
                return View("Error", $"Вы не можете перевести Вакансию на рассмотрение комиссии со статусом: {preModel.status.GetDescription()}");

            //TODO: Saga -> реализовать эту проверку при замуске Саг с таймерами
            //if ((DateTime.Now - preModel.committee_date).Days < _vacancyLifeCycleSettings.Options.DeltaFromPublishToInCommitteeMinDays)
            //    return View("Error", $"Вы не можете начать перевести вакансию на рассмотрение комиссии раньше чем через {_vacancyLifeCycleSettings.Options.DeltaFromPublishToInCommitteeMinDays} дн.");

            var vacancyApplications = _mediator.Send(new CountVacancyApplicationInVacancyQuery
            {
                VacancyGuid = preModel.guid,
                Status = VacancyApplicationStatus.Applied
            });

            if (vacancyApplications == 0)
                //если нет заявок, то закрыть вакансию
                _mediator.Send(new CancelVacancyCommand { VacancyGuid = preModel.guid, Reason = "На Вакансию не подано ни одной Заявки." });
            else
                _mediator.Send(new SwitchVacancyInCommitteeCommand { VacancyGuid = id });

            return RedirectToAction("details", new { id });
        }

        private object SetCommitteeReasonPreValidation(Guid vacancyGuid, Guid organizationGuid)
        {
            var vacancy = _mediator.Send(new SingleVacancyQuery { VacancyGuid = vacancyGuid });

            if (vacancy == null)
                return HttpNotFound();

            if (vacancy.organization_guid != organizationGuid)
                return View("Error", "Вы не можете изменять Заявки, поданные на вакансии других организаций.");

            if (vacancy.status != VacancyStatus.InCommittee)
                return View("Error", $"Вы не можете выбирать победителя для Заявки со статусом: {vacancy.status.GetDescription()}");

            var vacancyInCommitteeAttachments = _mediator.Send(new SelectCommitteeVacancyAttachmentsQuery { VacancyGuid = vacancyGuid });
            if (!string.IsNullOrWhiteSpace(vacancy.committee_resolution)
                || (vacancyInCommitteeAttachments != null && vacancyInCommitteeAttachments.Any()))
                return View("Error", "В вакансии УЖЕ УКАЗАНО конкурсное обоснование выбора победителя (и претендента).");

            return vacancy;
        }

        [PageTitle("Обоснование решения комиссии")]
        [BindOrganizationIdFromClaims]
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        public IActionResult SetCommitteeReason(Guid id, Guid organizationGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var result = SetCommitteeReasonPreValidation(id, organizationGuid);
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
        [ValidateAntiForgeryToken]
        public IActionResult SetCommitteeReason(VacancySetReasonViewModel model, Guid organizationGuid)
        {
            if (model.Guid == Guid.Empty)
                throw new ArgumentNullException(nameof(model.Guid));
            if (organizationGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationGuid));

            var result = SetCommitteeReasonPreValidation(model.Guid, organizationGuid);
            if (result is HttpNotFoundResult) return (HttpNotFoundResult)result;
            if (result is ViewResult) return (ViewResult)result;
            var vacancy = (Vacancy)result;

            if (string.IsNullOrWhiteSpace(model.Reason) /* && (model.Attachments == null || !model.Attachments.Any())*/)
                ModelState.AddModelError("Reason", "Укажите обоснование решения комиссии" /*либо прикрепите протоколы решения */);

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
                var fullDirectoryPath = $"{_hostingEnvironment.WebRootPath}{_attachmentSettings.Options.Vacancy.PhisicalPathPart}\\{newFolderName}\\";

                foreach (var file in model.Attachments)
                {
                    var fileName = System.IO.Path.GetFileName(ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"'));

                    //сценарий-А: сохранить файл на диск
                    try
                    {
                        //TODO: Application -> Attachments : что делать с Директорией при удалении(отмене, отклонении) Заявки
                        //TODO: Application -> Attachments : как искать Текущую директорию при повторном добавлении(изменении текущего списка) файлов
                        //TODO: Application -> Attachments : можно ли редактировать список файлов, или Заявки создаются разово и для каждой генеиртся новая папка с вложениями
                        Directory.CreateDirectory(fullDirectoryPath);
                        var filePath =
                            $"{_hostingEnvironment.WebRootPath}{_attachmentSettings.Options.Vacancy.PhisicalPathPart}\\{newFolderName}\\{fileName}";
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

            _mediator.Send(new SetVacancyCommitteeResolutionCommand
            {
                Resolution = model.Reason,
                Attachments = attachmentsList,
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
                return View("Error", "Для данной Вакансии уже выбраны Победитель и Претендент.");

            if (vacancy.status != VacancyStatus.InCommittee)
                return View("Error",
                    $"Вы не можете выбирать победителя для Заявки со статусом: {vacancy.status.GetDescription()}");

            var vacancyInCommitteeAttachments = _mediator.Send(new SelectCommitteeVacancyAttachmentsQuery { VacancyGuid = vacancy.guid });
            if (string.IsNullOrWhiteSpace(vacancy.committee_resolution)
                && (vacancyInCommitteeAttachments == null || !vacancyInCommitteeAttachments.Any()))
                return RedirectToAction("setcommitteereason", new { id = vacancy.guid });

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
            model.Researcher = Mapper.Map<ResearcherDetailsViewModel>(_mediator.Send(new SingleResearcherQuery { ResearcherGuid = vacancyApplication.researcher_guid }));
            model.WinnerIsSetting = isWinner;

            return View(model);
        }

        [PageTitle("Выбрать Победителя или Претендента")]
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                model.Researcher = Mapper.Map<ResearcherDetailsViewModel>(_mediator.Send(new SingleResearcherQuery { ResearcherGuid = vacancyApplication.researcher_guid }));

                return View(model);
            }

            if (model.WinnerIsSetting)
            {
                //добавить инфу о победителе в вакансию
                _mediator.Send(new SetVacancyWinnerCommand
                {
                    VacancyGuid = model.VacancyGuid,
                    ResearcherGuid = model.ResearcherGuid,
                    VacancyApplicationGuid = model.Guid
                });


                //пометить Заявку как Победитель
                _mediator.Send(new MakeVacancyApplicationWinnerCommand
                {
                    ResearcherGuid = model.ResearcherGuid,
                    VacancyApplicationGuid = model.Guid,
                    Reason = vacancy.committee_resolution
                });

                //если не осталось не обработанных заявок, т.е. Заявка была всего одна...
                var appliedVacancyApplications = _mediator.Send(new CountVacancyApplicationInVacancyQuery
                {
                    VacancyGuid = model.VacancyGuid,
                    Status = VacancyApplicationStatus.Applied
                });
                //...то переключить вакансию в статус ожидания ответа от победителя
                if (appliedVacancyApplications == 0)
                    GoOutFromInCommittee(model.VacancyGuid);
            }
            else
            {
                //добавить инфу о претенденте в вакансию
                _mediator.Send(new SetVacancyPretenderCommand
                {
                    VacancyGuid = model.VacancyGuid,
                    ResearcherGuid = model.ResearcherGuid,
                    VacancyApplicationGuid = model.Guid
                });


                //пометить Заявку как Претендент
                _mediator.Send(new MakeVacancyApplicationPretenderCommand
                {
                    ResearcherGuid = model.ResearcherGuid,
                    VacancyApplicationGuid = model.Guid,
                    Reason = vacancy.committee_resolution
                });

                //после выбора победителя
                GoOutFromInCommittee(model.VacancyGuid);
            }

            return RedirectToAction("preview", "applications", new { id = model.Guid });
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

        /// <summary>
        /// обработать выход из Рассмотрения комиссии
        /// </summary>
        /// <param name="vacancyGuid"></param>
        private void GoOutFromInCommittee(Guid vacancyGuid)
        {
            //переключить вакансию в статус ожидания ответа от победителя
            _mediator.Send(new SetVacancyToResponseAwaitingFromWinnerCommand
            {
                VacancyGuid = vacancyGuid
            });

            //отметить лузеров
            var stillAppliedApplications = _mediator.Send(new SelectVacancyApplicationInVacancyByStatusQuery { VacancyGuid = vacancyGuid, Status = VacancyApplicationStatus.Applied });
            stillAppliedApplications?.ToList().ForEach(c => _mediator.Send(new MakeVacancyApplicationLooserCommand
            {
                ResearcherGuid = c.researcher_guid,
                VacancyApplicationGuid = c.guid
            }));
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
        [ValidateAntiForgeryToken]
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


        private void RemoveAttachmentDirectory(string fullpath)
        {
            if (Directory.Exists(fullpath))
                Directory.Delete(fullpath);
        }

    }
}
