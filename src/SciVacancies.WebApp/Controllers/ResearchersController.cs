using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.IO;
using System.Linq;
using AutoMapper;
using MediatR;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Runtime;
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
    [Authorize(Roles = ConstTerms.RequireRoleResearcher)]
    public class ResearchersController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IApplicationEnvironment _hostingEnvironment;
        public ResearchersController(IMediator mediator, IApplicationEnvironment hostingEnvironment)
        {
            _mediator = mediator;
            _hostingEnvironment = hostingEnvironment;
        }

        [ResponseCache(NoStore = true)]
        [SiblingPage]
        [PageTitle("Информация")]
        [BindResearcherIdFromClaims]
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
            return View(model);
        }
        [HttpPut]
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



            //TODO: сохранение фото в БД (сделать)
            //фотографии в byte
            byte[] byteData;
            using (var memoryStream = new MemoryStream())
            {
                foreach (var file in model.Files)
                {

                    //сценарий-А: сохранить фото на диск
                    //var fileName = ContentDispositionHeaderValue
                    //  .Parse(file.ContentDisposition)
                    //  .FileName
                    //  .Trim('"');
                    //var filePath = _hostingEnvironment.ApplicationBasePath + "\\uploads\\" + DateTime.Now.ToString("yyyyddMHHmmss") + fileName;
                    //file.SaveAs(filePath);

                    //сценарий-Б: сохранить фото в БД
                    var openReadStream = file.OpenReadStream();
                    openReadStream.CopyTo(memoryStream);
                    byteData = memoryStream.ToArray();
                    memoryStream.SetLength(0);
                }
            }



            var preModel = _mediator.Send(new SingleResearcherQuery { ResearcherGuid = authorizedUserGuid });
            if (preModel == null)
                return HttpNotFound(); //throw new ObjectNotFoundException();

            var data = Mapper.Map<ResearcherDataModel>(model);
            _mediator.Send(new UpdateResearcherCommand { Data = data, ResearcherGuid = authorizedUserGuid });

            return RedirectToAction("account");
        }

        [SiblingPage]
        [PageTitle("Мои заявки")]
        [BindResearcherIdFromClaims]
        public ViewResult Applications(Guid researcherGuid, int pageSize = 10, int currentPage = 1)
        {
            if (researcherGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(researcherGuid));

            var source =
                _mediator.Send(new SelectPagedVacancyApplicationsByResearcherQuery
                {
                    PageSize = pageSize,
                    PageIndex = currentPage,
                    OrderBy = ConstTerms.OrderByCreationDateDescending,
                    ResearcherGuid = researcherGuid
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
                    OrderBy = ConstTerms.OrderByDateDescending
                });
                model.Applications.Items.ForEach(
                    c =>
                        c.Vacancy = Mapper.Map<VacancyDetailsViewModel>(innerObjects.Items.Single(d => d.guid == c.VacancyGuid)));
            }

            return View(model);
        }

        [SiblingPage]
        [PageTitle("Избранные вакансии")]
        [BindResearcherIdFromClaims]
        public ActionResult Favorities(Guid researcherGuid, int pageSize = 10, int currentPage = 1)
        {
            if (researcherGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(researcherGuid));

            var model = new VacanciesFavoritiesInResearcherIndexViewModel
            {
                Vacancies = _mediator.Send(new SelectPagedFavoriteVacanciesByResearcherQuery { PageSize = pageSize, PageIndex = currentPage, OrderBy = ConstTerms.OrderByCreationDateDescending, ResearcherGuid = researcherGuid }).MapToPagedList()
            };
            return View(model);
        }

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


        [SiblingPage]
        [PageTitle("Уведомления")]
        [BindResearcherIdFromClaims]
        public ViewResult Notifications(Guid researcherGuid, int pageSize = 10, int currentPage = 1)
        {
            var model = new NotificationsInResearcherIndexViewModel
            {
                PagedItems = _mediator.Send(new SelectPagedResearcherNotificationsQuery
                {
                    ResearcherGuid = researcherGuid,
                    PageSize = pageSize,
                    PageIndex = currentPage
                }).MapToPagedList()
            };
            return View(model);
        }

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
                //TODO: оптимизировать запрос и его обработку
                //если есть GUID Исследователя
                var favoritesVacancies = _mediator.Send(new SelectPagedFavoriteVacanciesByResearcherQuery { PageSize = 1000, PageIndex = 1, ResearcherGuid = researcherGuid, OrderBy = ConstTerms.OrderByDateAscending });
                //если текущей вакансии нет в списке избранных
                if (favoritesVacancies == null
                    || favoritesVacancies.TotalItems == 0
                    || !favoritesVacancies.Items.Select(c => c.guid).ToList().Contains(vacancyGuid))
                    _mediator.Send(new AddVacancyToFavoritesCommand { ResearcherGuid = researcherGuid, VacancyGuid = vacancyGuid });
            }

            return Redirect(Context.Request.Headers["referer"]);
        }

        [BindResearcherIdFromClaims]
        [PageTitle("Избранная вакансия")]
        public IActionResult RemoveFavorite(Guid vacancyGuid, Guid researcherGuid)
        {
            if (researcherGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(researcherGuid));
            if (vacancyGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(vacancyGuid));

            //TODO: оптимизировать запрос и его обработку
            var favoritesVacancies = _mediator.Send(new SelectPagedFavoriteVacanciesByResearcherQuery { PageSize = 1000, PageIndex = 1, ResearcherGuid = researcherGuid, OrderBy = ConstTerms.OrderByDateAscending });
            if (favoritesVacancies == null)
                return View("Error", "У вас нет избранных вакансий");
            if (favoritesVacancies.Items.All(c => c.guid != vacancyGuid))
                return View("Error", $"У вас нет избранной вакансии с идентификатором {vacancyGuid}");

            var preModel = _mediator.Send(new SingleVacancyQuery { VacancyGuid = vacancyGuid });
            var model = Mapper.Map<VacancyDetailsViewModel>(preModel);
            return View(model);
        }

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

            //TODO: оптимизировать запрос и его обработку
            var favoritesVacancies = _mediator.Send(new SelectPagedFavoriteVacanciesByResearcherQuery { PageSize = 100, PageIndex = 1, ResearcherGuid = researcherGuid, OrderBy = ConstTerms.OrderByDateAscending });
            if (favoritesVacancies == null)
                return View("Error", "У вас нет избранных вакансий");
            if (favoritesVacancies.Items.All(c => c.guid != vacancyGuid))
                return View("Error", $"У вас нет избранной вакансии с идентификатором {vacancyGuid}");

            if (!ModelState.IsValid)
            {
                var preModel = _mediator.Send(new SingleVacancyQuery { VacancyGuid = vacancyGuid });
                var model = Mapper.Map<VacancyDetailsViewModel>(preModel);
                return View("removefavorite", model);
            }

            _mediator.Send(new RemoveVacancyFromFavoritesCommand { ResearcherGuid = researcherGuid, VacancyGuid = vacancyGuid });
            return RedirectToAction("card", "vacancies", new { id = vacancyGuid });
        }


    }
}