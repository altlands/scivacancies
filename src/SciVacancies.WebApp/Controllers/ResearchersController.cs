using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using AutoMapper;
using MediatR;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.OptionsModel;
using Microsoft.Net.Http.Headers;
using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel.Core;
using SciVacancies.WebApp.Commands;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Engine.CustomAttribute;
using SciVacancies.WebApp.Queries;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Controllers
{
    [ResponseCache(NoStore = true)]
    [Authorize]
    public class ResearchersController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IOptions<AttachmentSettings> _attachmentSettings;

        public ResearchersController(IMediator mediator, IHostingEnvironment hostingEnvironment, IOptions<AttachmentSettings> attachmentSettings)
        {
            _mediator = mediator;
            _hostingEnvironment = hostingEnvironment;
            _attachmentSettings = attachmentSettings;
        }

        [ResponseCache(NoStore = true)]
        [SiblingPage]
        [Authorize]
        [PageTitle("Информация")]
        [BindResearcherIdFromClaims]
        [NonActivatedUser]
        public IActionResult Account(Guid researcherGuid)
        {
            if (researcherGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(researcherGuid));

            var preModel = _mediator.Send(new SingleResearcherQuery { ResearcherGuid = researcherGuid });
            if (preModel == null)
                return RedirectToAction("logout", "account");

            var model = Mapper.Map<ResearcherDetailsViewModel>(preModel);
            model.Educations = Mapper.Map<List<EducationEditViewModel>>(_mediator.Send(new SelectResearcherEducationsQuery { ResearcherGuid = researcherGuid }));
            model.Publications = Mapper.Map<List<PublicationEditViewModel>>(_mediator.Send(new SelectResearcherPublicationsQuery { ResearcherGuid = researcherGuid }));

            return View(model);
        }

        [Authorize(Roles = ConstTerms.RequireRoleResearcher)]
        [ResponseCache(NoStore = true)]
        [PageTitle("Изменить данные")]
        [BindResearcherIdFromClaims]
        public IActionResult Edit(Guid researcherGuid)
        {
            if (researcherGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(researcherGuid));

            var preModel = _mediator.Send(new SingleResearcherQuery { ResearcherGuid = researcherGuid });
            if (preModel == null)
                return HttpNotFound(); //throw new ObjectNotFoundException();

            var model = Mapper.Map<ResearcherEditViewModel>(preModel);
            model.Educations = Mapper.Map<List<EducationEditViewModel>>(_mediator.Send(new SelectResearcherEducationsQuery { ResearcherGuid = researcherGuid }));
            model.Publications = Mapper.Map<List<PublicationEditViewModel>>(_mediator.Send(new SelectResearcherPublicationsQuery { ResearcherGuid = researcherGuid }));

            return View(model);
        }
        [Authorize(Roles = ConstTerms.RequireRoleResearcher)]
        [HttpPost]
        [PageTitle("Редактирование информации пользователя")]
        [BindResearcherIdFromClaims("authorizedUserGuid")]
        public IActionResult Edit(ResearcherEditViewModel model, Guid authorizedUserGuid)
        {
            if (model.Guid == Guid.Empty)
                throw new ArgumentNullException(nameof(model), "Отсутствует идентификатор исследователя");

            if (authorizedUserGuid != model.Guid)
                return View("Error", "Вы не можете изменять чужой профиль");

            if (ModelState.ErrorCount > 0)
                return View(model);

            var preModel = _mediator.Send(new SingleResearcherQuery { ResearcherGuid = authorizedUserGuid });
            if (preModel == null)
                return HttpNotFound(); //throw new ObjectNotFoundException();

            var dataModel = Mapper.Map<ResearcherDataModel>(preModel);
            var data = Mapper.Map(model, dataModel);
            _mediator.Send(new UpdateResearcherCommand
            {
                Data = data,
                ResearcherGuid = authorizedUserGuid
            });

            return RedirectToAction("account");
        }

        [Authorize(Roles = ConstTerms.RequireRoleResearcher)]
        [ResponseCache(NoStore = true)]
        [PageTitle("Изменить данные")]
        [BindResearcherIdFromClaims]
        public IActionResult EditPhoto(Guid researcherGuid)
        {
            if (researcherGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(researcherGuid));

            var preModel = _mediator.Send(new SingleResearcherQuery { ResearcherGuid = researcherGuid });
            if (preModel == null)
                return HttpNotFound(); //throw new ObjectNotFoundException();

            var model = Mapper.Map<ResearcherEditPhotoViewModel>(preModel);
            return View(model);
        }
        [Authorize(Roles = ConstTerms.RequireRoleResearcher)]
        [HttpPost]
        [PageTitle("Редактирование информации пользователя")]
        [BindResearcherIdFromClaims("authorizedUserGuid")]
        public IActionResult EditPhoto(ResearcherEditPhotoViewModel model, Guid authorizedUserGuid)
        {
            if (model.Guid == Guid.Empty)
                throw new ArgumentNullException(nameof(model), "Отсутствует идентификатор исследователя");

            if (authorizedUserGuid != model.Guid)
                return View("Error", "Вы не можете изменять чужой профиль");

            if (ModelState.ErrorCount > 0)
                return View(model);

            if (model.PhotoFile != null)
            {
                var file = model.PhotoFile;
                var fileName = System.IO.Path.GetFileName(ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"'));
                
                var fileExtension = fileName
                    .Split('.')
                    .Last()
                    .ToUpper();

                if (!_attachmentSettings.Options.Researcher.AllowExtensions.ToUpper().Contains(fileExtension.ToUpper()))
                    ModelState.AddModelError("PhotoFile",
                        $"Можно добавить только изображение. Допустимые типы файлов: {_attachmentSettings.Options.Researcher.AllowExtensions}");

                if (ModelState.ErrorCount > 0)
                    return View(model);

                if (file.Length > 0)
                {
                    var newFileName = $"{authorizedUserGuid}.{fileExtension}";
                    var filePath =
                        $"{_hostingEnvironment.WebRootPath}{_attachmentSettings.Options.Researcher.PhisicalPathPart}\\{newFileName}";
                    Directory.CreateDirectory(
                        $"{_hostingEnvironment.WebRootPath}{_attachmentSettings.Options.Researcher.PhisicalPathPart}\\");

                    using (var image = Image.FromStream(file.OpenReadStream()))
                    {
                        Image newImage = null;
                        if (file.Length > _attachmentSettings.Options.Researcher.MaxItemSize)
                        {
                            var scale = ((float)_attachmentSettings.Options.Researcher.MaxItemSize / file.Length);
                            var newWidth = image.Width * scale;
                            var newHeight = image.Height * scale;
                            newImage = ScaleImage(image, (int)newWidth, (int)newHeight);
                        }
                        else
                            newImage = image;

                        //сценарий-А: сохранить фото на диск
                        newImage.Save(filePath);
                        model.ImageName = newFileName;
                        model.ImageSize = file.Length;
                        model.ImageExtension = fileExtension;
                        model.ImageUrl = $"/{newFileName}";

                        ////TODO: сохранение фото в БД (сделать)
                        //using (var memoryStream = new MemoryStream())
                        //{
                        //    //фотографии в byte
                        //    byte[] byteData;

                        //    //сценарий-Б: сохранить фото в БД
                        //    ((Image)newImage).Save(memoryStream, ImageFormat.Jpeg);
                        //    byteData = memoryStream.ToArray();
                        //}

                        newImage.Dispose();
                    }
                }

            }



            var preModel = _mediator.Send(new SingleResearcherQuery { ResearcherGuid = authorizedUserGuid });
            if (preModel == null)
                return HttpNotFound(); //throw new ObjectNotFoundException();

            var dataModel = Mapper.Map<ResearcherDataModel>(preModel);
            var data = Mapper.Map(model, dataModel);
            _mediator.Send(new UpdateResearcherCommand
            {
                Data = data,
                ResearcherGuid = authorizedUserGuid
            });

            return RedirectToAction("account");
        }

        private Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var newImage = new Bitmap(maxWidth, maxHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, maxWidth, maxHeight);

            return newImage;
        }

        [Authorize(Roles = ConstTerms.RequireRoleResearcher)]
        [SiblingPage]
        [PageTitle("Мои заявки")]
        [BindResearcherIdFromClaims]
        public ViewResult Applications(Guid researcherGuid, int pageSize = 10, int currentPage = 1,
            string sortField = ConstTerms.OrderByFieldApplyDate, string sortDirection = ConstTerms.OrderByDescending)
        {
            if (researcherGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(researcherGuid));

            var source =
                _mediator.Send(new SelectPagedVacancyApplicationsByResearcherQuery
                {
                    PageSize = pageSize,
                    PageIndex = currentPage,
                    ResearcherGuid = researcherGuid,
                    OrderBy = new SortFilterHelper().GetSortField<VacancyApplication>(sortField),
                    OrderDirection = sortDirection
                });

            var model = new VacancyApplicationsInResearcherIndexViewModel();

            if (source.TotalItems > 0)
            {
                model.Applications = source.MapToPagedList<VacancyApplication, VacancyApplicationDetailsViewModel>();

                var innerObjects = _mediator.Send(new SelectPagedVacanciesByGuidsQuery
                {
                    VacancyGuids = model.Applications.Items.Select(c => c.VacancyGuid).Distinct(),
                    PageSize = 1000,
                    PageIndex = 1,
                    OrderBy = ConstTerms.OrderByFieldDate
                });
                model.Applications.Items.ForEach(
                    c =>
                        c.Vacancy = Mapper.Map<VacancyDetailsViewModel>(innerObjects.Items.Single(d => d.guid == c.VacancyGuid)));
            }

            return View(model);
        }

        [Authorize(Roles = ConstTerms.RequireRoleResearcher)]
        [SiblingPage]
        [PageTitle("Избранные вакансии")]
        [BindResearcherIdFromClaims]
        public ActionResult Favorities(Guid researcherGuid, int pageSize = 10, int currentPage = 1, string sortField = ConstTerms.OrderByFieldPublishDate, string sortDirection = ConstTerms.OrderByDescending)
        {
            if (researcherGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(researcherGuid));
            var model = new VacanciesFavoritiesInResearcherIndexViewModel
            {
                Vacancies = _mediator.Send(new SelectPagedFavoriteVacanciesByResearcherQuery
                {
                    PageSize = pageSize,
                    PageIndex = currentPage,
                    ResearcherGuid = researcherGuid,
                    OrderBy = new SortFilterHelper().GetSortField<Vacancy>(sortField),
                    OrderDirection = sortDirection
                }).MapToPagedList(),
                AppliedApplications = _mediator.Send(new SelectVacancyApplicationsByResearcherQuery { ResearcherGuid = researcherGuid }).Where(c => c.status != VacancyApplicationStatus.InProcess && c.status != VacancyApplicationStatus.Removed).ToList()
            };
            return View(model);
        }

        [Authorize(Roles = ConstTerms.RequireRoleResearcher)]
        [SiblingPage]
        [PageTitle("Подписки")]
        [BindResearcherIdFromClaims]
        public ViewResult Subscriptions(Guid researcherGuid, int pageSize = 10, int currentPage = 1)
        {
            var model = new SearchSubscriptionsInResearcherIndexViewModel
            {
                PagedItems = _mediator.Send(new SelectPagedSearchSubscriptionsQuery
                {
                    ResearcherGuid = researcherGuid,
                    PageSize = pageSize,
                    PageIndex = currentPage
                }).MapToPagedList()
            };
            return View(model);
        }


        [Authorize(Roles = ConstTerms.RequireRoleResearcher)]
        [SiblingPage]
        [PageTitle("Уведомления")]
        [BindResearcherIdFromClaims]
        public ViewResult Notifications(Guid researcherGuid, int pageSize = 10, int currentPage = 1,
            string sortField = ConstTerms.OrderByFieldCreationDate, string sortDirection = ConstTerms.OrderByDescending)
        {
            var model = new NotificationsInResearcherIndexViewModel
            {
                PagedItems = _mediator.Send(new SelectPagedResearcherNotificationsQuery
                {
                    ResearcherGuid = researcherGuid,
                    PageSize = pageSize,
                    PageIndex = currentPage,
                    OrderBy = new SortFilterHelper().GetSortField<ResearcherNotification>(sortField),
                    OrderDirection = sortDirection
                }).MapToPagedList()
            };
            return View(model);
        }

        [Authorize(Roles = ConstTerms.RequireRoleResearcher)]
        [BindResearcherIdFromClaims]
        public ActionResult AddToFavorite(Guid researcherGuid, Guid vacancyGuid)
        {
            if (researcherGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(researcherGuid));
            if (vacancyGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(vacancyGuid));


            var model = _mediator.Send(new SingleVacancyQuery { VacancyGuid = vacancyGuid });
            //если заявка на готовится к открытию или открыта
            if (model.status == VacancyStatus.Published)
            {
                var favoritesVacancies = _mediator.Send(new SelectFavoriteVacancyGuidsByResearcherQuery
                {
                    ResearcherGuid = researcherGuid
                }).ToList();
                //если текущей вакансии нет в списке избранных
                if (!favoritesVacancies.Contains(vacancyGuid))
                    _mediator.Send(new AddVacancyToFavoritesCommand { ResearcherGuid = researcherGuid, VacancyGuid = vacancyGuid });
            }

            return Redirect(Context.Request.Headers["referer"]);
        }

        [Authorize(Roles = ConstTerms.RequireRoleResearcher)]
        [BindResearcherIdFromClaims]
        [PageTitle("Избранная вакансия")]
        public IActionResult RemoveFavorite(Guid vacancyGuid, Guid researcherGuid)
        {
            if (researcherGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(researcherGuid));
            if (vacancyGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(vacancyGuid));

            var favoritesVacancies = _mediator.Send(new SelectFavoriteVacancyGuidsByResearcherQuery
            {
                ResearcherGuid = researcherGuid
            }).ToList();
            if (!favoritesVacancies.Any())
                return RedirectToVacancyCardAction(vacancyGuid);
            if (favoritesVacancies.All(c => c != vacancyGuid))
                return RedirectToVacancyCardAction(vacancyGuid);

            var preModel = _mediator.Send(new SingleVacancyQuery { VacancyGuid = vacancyGuid });
            var model = Mapper.Map<VacancyDetailsViewModel>(preModel);
            return View(model);
        }

        [Authorize(Roles = ConstTerms.RequireRoleResearcher)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [BindResearcherIdFromClaims]
        [PageTitle("Избранная вакансия")]
        public IActionResult RemoveFavoritePost(Guid vacancyGuid, Guid researcherGuid)
        {
            if (researcherGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(researcherGuid));
            if (vacancyGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(vacancyGuid));

            var favoritesVacancies = _mediator.Send(new SelectFavoriteVacancyGuidsByResearcherQuery
            {
                ResearcherGuid = researcherGuid
            }).ToList();
            if (!favoritesVacancies.Any())
                return RedirectToVacancyCardAction(vacancyGuid);
            if (favoritesVacancies.All(c => c != vacancyGuid))
                return RedirectToVacancyCardAction(vacancyGuid);

            if (!ModelState.IsValid)
            {
                var preModel = _mediator.Send(new SingleVacancyQuery { VacancyGuid = vacancyGuid });
                var model = Mapper.Map<VacancyDetailsViewModel>(preModel);
                return View("removefavorite", model);
            }

            _mediator.Send(new RemoveVacancyFromFavoritesCommand { ResearcherGuid = researcherGuid, VacancyGuid = vacancyGuid });
            return RedirectToVacancyCardAction(vacancyGuid);
        }

        private IActionResult RedirectToVacancyCardAction(Guid vacancyGuid)
        {
            return RedirectToAction("card", "vacancies", new { id = vacancyGuid });
        }
    }
}