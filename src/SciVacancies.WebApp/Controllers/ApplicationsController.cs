using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using MediatR;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Razor;
using Microsoft.Framework.OptionsModel;
using Microsoft.Net.Http.Headers;
using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel.Core;
using SciVacancies.WebApp.Commands;
using SciVacancies.WebApp.Queries;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Controllers
{
    [ResponseCache(NoStore = true)]
    [Authorize]
    public class ApplicationsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IOptions<AttachmentSettings> _attachmentSettings;

        public ApplicationsController(IMediator mediator, IHostingEnvironment hostingEnvironment, IOptions<AttachmentSettings> attachmentSettings)
        {
            _mediator = mediator;
            _hostingEnvironment = hostingEnvironment;
            _attachmentSettings = attachmentSettings;
        }

        #region private VacancyApplicationCreateViewModelHelper
        private VacancyApplicationCreateViewModel VacancyApplicationCreateViewModelHelper(Guid researcherGuid, Vacancy vacancy, VacancyApplicationCreateViewModel model)
        {
            var researcher = _mediator.Send(new SingleResearcherQuery { ResearcherGuid = researcherGuid });
            model = model ?? new VacancyApplicationCreateViewModel();
            model.ResearcherGuid = researcherGuid;
            model.VacancyGuid = vacancy.guid;
            model.PositionName = vacancy.name;
            model.BirthDate = researcher.birthdate;
            model.ImageUrl = researcher.image_url;
            model.Email = researcher.email;
            model.Phone = researcher.phone;
            model.ResearcherFullName = $"{researcher.secondname} {researcher.firstname} {researcher.patronymic}";
            model.ResearcherFullNameEng = $"{researcher.firstname_eng} {researcher.patronymic_eng} {researcher.secondname_eng}";

            IEnumerable<Education> educations = _mediator.Send(new SelectResearcherEducationsQuery { ResearcherGuid = researcherGuid });
            if (educations != null && educations.Count() > 0)
            {
                model.Educations = Mapper.Map<List<EducationEditViewModel>>(educations);
            }
            IEnumerable<Publication> publications = _mediator.Send(new SelectResearcherPublicationsQuery { ResearcherGuid = researcherGuid });
            if (publications != null && publications.Count() > 0)
            {
                model.Publications = Mapper.Map<List<PublicationEditViewModel>>(publications);
            }

            model.ResearchActivity = researcher.research_activity;
            model.TeachingActivity = researcher.teaching_activity;
            model.OtherActivity = researcher.other_activity;
            model.ScienceDegree = researcher.science_degree;
            model.ScienceRank = researcher.science_rank;
            model.Rewards = researcher.rewards;
            model.Memberships = researcher.memberships;
            model.Conferences = researcher.conferences;
            model.Interests = researcher.interests;

            model.ReadId = vacancy.read_id;

            return model;
        }
        #endregion

        [Authorize(Roles = ConstTerms.RequireRoleResearcher)]
        [PageTitle("Новая заявка")]
        [BindResearcherIdFromClaims]
        public ViewResult Create(Guid researcherGuid, Guid vacancyGuid)
        {
            if (researcherGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(researcherGuid));
            if (vacancyGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(vacancyGuid));

            var vacancy = _mediator.Send(new SingleVacancyQuery { VacancyGuid = vacancyGuid });
            if (vacancy.status != VacancyStatus.Published)
                return View("Error", $"Вы не можете подать Заявку на Вакансию в статусе: {vacancy.status.GetDescriptionByResearcher()}");

            var appliedVacancyApplications = _mediator.Send(new SelectVacancyApplicationsByResearcherQuery
            {
                ResearcherGuid = researcherGuid
            });

            if (appliedVacancyApplications != null && appliedVacancyApplications.Any()
                && appliedVacancyApplications.Where(c => c.status == VacancyApplicationStatus.Applied).Select(c => c.researcher_guid).Distinct().ToList().Any(c => c == researcherGuid))
                return View("Error", "Вы не можете подать повторную Заявку на Вакансию");

            var model = VacancyApplicationCreateViewModelHelper(researcherGuid, vacancy, null);
            //TODO: Applications -> Create : вернуть добавление дополнительнительных публикаций
            return View(model);
        }



        [PageTitle("Новая заявка")]
        [Authorize(Roles = ConstTerms.RequireRoleResearcher)]
        [HttpPost]
        [BindResearcherIdFromClaims]
        public IActionResult Create(VacancyApplicationCreateViewModel model, Guid researcherGuid)
        {
            if (model.VacancyGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(model.VacancyGuid), "Не указан идентификатор Вакансии");

            var vacancy = _mediator.Send(new SingleVacancyQuery { VacancyGuid = model.VacancyGuid });
            if (vacancy.status != VacancyStatus.Published)
                return View("Error", $"Вы не можете подать Заявку на Вакансию в статусе: {vacancy.status.GetDescriptionByResearcher()}");

            var appliedVacancyApplications = _mediator.Send(new SelectVacancyApplicationsByResearcherQuery {
                    ResearcherGuid = researcherGuid
                });

            if (appliedVacancyApplications!=null && appliedVacancyApplications.Any()
                && appliedVacancyApplications.Where(c => c.status == VacancyApplicationStatus.Applied).Select(c => c.researcher_guid).Distinct().ToList().Any(c => c == researcherGuid))
                return View("Error", "Вы не можете подать повторную Заявку на Вакансию");

            //TODO: Application -> Attachments : как проверять безопасность, прикрепляемых файлов
            //todo: повторяющийся код
            if (model.Attachments != null && model.Attachments.Any())
            {
                //проверяем размеры файлов
                if (model.Attachments.Any(c => c.Length > _attachmentSettings.Value.VacancyApplication.MaxItemSize))
                    ModelState.AddModelError("Attachments", $"Размер одного из прикрепленных файлов превышает допустимый размер ({_attachmentSettings.Value.VacancyApplication.MaxItemSize / 1000}КБ).");

                //проверяем расширения файлов, если они указаны
                if (!string.IsNullOrWhiteSpace(_attachmentSettings.Value.VacancyApplication.AllowExtensions))
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
                    var allowedExtenionsInUpper = _attachmentSettings.Value.VacancyApplication.AllowExtensions.ToUpper();
                    if (fileExtensionsInUpper.Any(c => !allowedExtenionsInUpper.Contains(c)))
                        ModelState.AddModelError("Attachments", $"Расширение одного из прикрепленных файлов имеет недопустимое расширение. Допустимые типы файлов: {allowedExtenionsInUpper}");
                }

            }
            //с формы мы не получаем практически никаких данных, поэтому заново наполняем ViewModel
            model = VacancyApplicationCreateViewModelHelper(researcherGuid, vacancy, model);
            if (!ModelState.IsValid)
                return View(model);

            #region attachments
            var attachmentsList = new List<SciVacancies.Domain.Core.VacancyApplicationAttachment>();
            var newFolderName = Guid.NewGuid();
            //save attachments
            var fullDirectoryPath = $"{_hostingEnvironment.WebRootPath}{_attachmentSettings.Value.VacancyApplication.PhisicalPathPart}/{newFolderName}/";

            if (model.Attachments != null && model.Attachments.Any())
            {
                foreach (var file in model.Attachments)
                {
                    var fileName = System.IO.Path.GetFileName(ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"'));
                    var isExists = Directory.Exists(fullDirectoryPath);
                    //сценарий-А: сохранить файл на диск
                    try
                    {
                        //TODO: Application -> Attachments : как искать Текущую директорию при повторном добавлении(изменении текущего списка) файлов
                        //TODO: Application -> Attachments : можно ли редактировать список файлов, или Заявки создаются разово и для каждой создаётся новая папка с вложениями
                        if (!isExists)
                            Directory.CreateDirectory(fullDirectoryPath);
                        var filePath =
                            $"{_hostingEnvironment.WebRootPath}{_attachmentSettings.Value.VacancyApplication.PhisicalPathPart}/{newFolderName}/{fileName}";
                        file.SaveAs(filePath);
                        attachmentsList.Add(new SciVacancies.Domain.Core.VacancyApplicationAttachment
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

                //присваиваем прикреплённым файлам тип "Резюме" (для соответствущей выборки)
                attachmentsList.ForEach(c => c.TypeId = 2);

            }
            #endregion

            Guid vacancyApplicationGuid;
            try
            {
                var data = Mapper.Map<VacancyApplicationDataModel>(model);
                data.Attachments = attachmentsList;
                vacancyApplicationGuid = _mediator.Send(new CreateAndApplyVacancyApplicationCommand { ResearcherGuid = model.ResearcherGuid, VacancyGuid = model.VacancyGuid, Data = data });
            }
            catch (Exception e)
            {
                RemoveAttachmentDirectory(fullDirectoryPath);
                return View("Error", $"Что-то пошло не так при сохранении Заявки: {e.Message}");
            }
            return RedirectToAction("details", "applications", new { id = vacancyApplicationGuid });
        }

        private void RemoveAttachmentDirectory(string fullpath)
        {
            if (Directory.Exists(fullpath))
                Directory.Delete(fullpath);
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
                return HttpNotFound(); //throw new ObjectNotFoundException($"Не найдена Заявка c идентификатором: {id}");

            if (researcherGuid != Guid.Empty
                && User.IsInRole(ConstTerms.RequireRoleResearcher)
                && preModel.researcher_guid != researcherGuid)
                return View("Error", "Вы не можете просматривать Заявки других соискателей.");

            var model = Mapper.Map<VacancyApplicationDetailsViewModel>(preModel);
            model.Vacancy = Mapper.Map<VacancyDetailsViewModel>(_mediator.Send(new SingleVacancyQuery { VacancyGuid = preModel.vacancy_guid }));
            model.Attachments = _mediator.Send(new SelectAllVacancyApplicationAttachmentsQuery { VacancyApplicationGuid = id });
            model.FolderApplicationsAttachmentsUrl = _attachmentSettings.Value.VacancyApplication.UrlPathPart;
            return View(model);
        }

        /// <summary>
        /// организации рпосматривают присланные Заявки
        /// </summary>
        /// <param name="id"></param>
        /// <param name="organizationGuid"></param>
        /// <returns></returns>
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        [PageTitle("Детали заявки")]
        [BindOrganizationIdFromClaims]
        public IActionResult Preview(Guid id, Guid organizationGuid)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var preModel = _mediator.Send(new SingleVacancyApplicationQuery { VacancyApplicationGuid = id });

            if (preModel == null)
                return HttpNotFound(); //throw new ObjectNotFoundException($"Не найдена Заявка c идентификатором: {id}");

            if (preModel.status == VacancyApplicationStatus.InProcess
                || preModel.status == VacancyApplicationStatus.Cancelled
                || preModel.status == VacancyApplicationStatus.Lost
                || preModel.status == VacancyApplicationStatus.Removed)
                return View("Error", $"Вы не можете просматривать Заявку со статусом: {preModel.status.GetDescription()}");

            var vacancy = _mediator.Send(new SingleVacancyQuery { VacancyGuid = preModel.vacancy_guid });
            if (vacancy.organization_guid != organizationGuid)
                return View("Error", "Вы не можете просматривать Заявки, поданные на вакансии других организаций.");

            var model = Mapper.Map<VacancyApplicationDetailsViewModel>(preModel);
            model.Vacancy = Mapper.Map<VacancyDetailsViewModel>(vacancy);
            model.Attachments = _mediator.Send(new SelectAllVacancyApplicationAttachmentsQuery { VacancyApplicationGuid = id });
            model.FolderApplicationsAttachmentsUrl = _attachmentSettings.Value.VacancyApplication.UrlPathPart;
            return View(model);
        }


        private object OfferAcceptionPreValidation(Guid vacancyApplicationGuid, Guid researcherGuid, bool isWinner)
        {
            var vacancyApplicaiton = _mediator.Send(new SingleVacancyApplicationQuery { VacancyApplicationGuid = vacancyApplicationGuid });

            if (vacancyApplicaiton == null)
                return HttpNotFound(); //throw new ObjectNotFoundException($"Не найдена Заявка c идентификатором: {vacancyApplicationGuid}");

            if (isWinner && vacancyApplicaiton.status != VacancyApplicationStatus.Won)
                return View("Error", "Вы не можете принять предложение если Вы не Победитель");

            if (!isWinner && vacancyApplicaiton.status != VacancyApplicationStatus.Pretended)
                return View("Error", "Вы не можете принять предложение если Вы не Победитель");

            var vacancy = _mediator.Send(new SingleVacancyQuery { VacancyGuid = vacancyApplicaiton.vacancy_guid });

            if (vacancy == null)
                return HttpNotFound(); //throw new ObjectNotFoundException($"Не найдена Вакансия c идентификатором: {vacancyApplicaiton.vacancy_guid}");

            if (isWinner)
            {
                if (vacancy.status != VacancyStatus.OfferResponseAwaitingFromWinner)
                    return View("Error", $"Вы не можете принять предложение или отказаться от него если Вакансия имеет статус: {vacancy.status.GetDescriptionByResearcher()}");
                if (vacancy.winner_researcher_guid != researcherGuid)
                    return View("Error", "Вы не можете принять или отказаться от предложения, сделанного для другого заявителя.");
            }
            if (!isWinner)
            {
                if (vacancy.status != VacancyStatus.OfferResponseAwaitingFromPretender)
                    return View("Error", $"Вы не можете принять предложение или отказаться от него если Вакансия имеет статус: {vacancy.status.GetDescriptionByResearcher()}");
                if (vacancy.pretender_researcher_guid != researcherGuid)
                    return View("Error", "Вы не можете принять или отказаться от предложения, сделанного для другого заявителя.");
            }

            return vacancy;
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
                return HttpNotFound(); //throw new ObjectNotFoundException($"Не найдена Заявка c идентификатором: {id}");

            if (preModel.status == VacancyApplicationStatus.InProcess
                || preModel.status == VacancyApplicationStatus.Cancelled
                || preModel.status == VacancyApplicationStatus.Lost
                || preModel.status == VacancyApplicationStatus.Removed)
                return View("Error", $"Вы не можете просматривать Заявку со статусом: {preModel.status.GetDescription()}");

            var vacancy = _mediator.Send(new SingleVacancyQuery { VacancyGuid = preModel.vacancy_guid });
            if (vacancy.organization_guid != organizationGuid)
                return View("Error", "Вы не можете изменять Заявки, поданные на вакансии других организаций.");

            var model = Mapper.Map<VacancyApplicationDetailsViewModel>(preModel);
            model.Vacancy = Mapper.Map<VacancyDetailsViewModel>(vacancy);
            model.Attachments = _mediator.Send(new SelectAllVacancyApplicationAttachmentsQuery { VacancyApplicationGuid = id });
            model.FolderApplicationsAttachmentsUrl = _attachmentSettings.Value.VacancyApplication.UrlPathPart;
            return View(model);
        }

        [Authorize(Roles = ConstTerms.RequireRoleResearcher)]
        [BindResearcherIdFromClaims]
        public IActionResult OfferAcception(Guid id, Guid researcherGuid, bool isWinner, bool hasAccepted)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            if (researcherGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(researcherGuid));

            var result = OfferAcceptionPreValidation(id, researcherGuid, isWinner);
            if (result is HttpNotFoundResult) return (HttpNotFoundResult)result;
            var vacancy = (Vacancy)result;

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

            return RedirectToAction("details", "applications", new { id });

        }

    }
}
