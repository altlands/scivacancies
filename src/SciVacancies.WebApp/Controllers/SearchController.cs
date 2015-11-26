using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using MediatR;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel.ElasticSearchModel.Model;
using SciVacancies.WebApp.Commands;
using SciVacancies.WebApp.Models;
using SciVacancies.WebApp.Queries;
using SciVacancies.WebApp.ViewModels;

using Microsoft.Framework.Logging;

namespace SciVacancies.WebApp.Controllers
{

    [ResponseCache(NoStore = true)]
    public class SearchController : Controller
    {
        private readonly IMediator _mediator;
        private ILogger Logger;

        public SearchController(IMediator mediator, ILoggerFactory loggerFactory)
        {
            _mediator = mediator;
            this.Logger = loggerFactory.CreateLogger<SearchController>();
        }

        /// <summary>
        /// фильтрация, страницы, сортировка 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        [PageTitle("Результаты поиска")]
        [BindResearcherIdFromClaims]
        public ActionResult Index(VacanciesFilterModel model, Guid researcherGuid)
        {
            if (TempData["VacanciesFilterModel"] != null)
                model = JsonConvert.DeserializeObject<VacanciesFilterModel>(TempData["VacanciesFilterModel"].ToString());

            //если нужно добавить новую подписку
            if (researcherGuid != Guid.Empty && model.NewSubscriptionAdd)
            {
                var newSubscriptionGuid = _mediator.Send(new CreateSearchSubscriptionCommand
                {
                    ResearcherGuid = researcherGuid,
                    Data = new SearchSubscriptionDataModel
                    {
                        OrderBy = model.OrderBy,
                        Query = model.Search ?? string.Empty,
                        Title = model.NewSubscriptionTitle,
                        FoivIds = model.Foivs,
                        PositionTypeIds = model.PositionTypes,
                        RegionIds = model.Regions,
                        ResearchDirectionIds = model.ResearchDirections,
                        VacancyStatuses = model.VacancyStates?.Select(c => (VacancyStatus)c).ToList(),
                        SalaryFrom = model.SalaryMin,
                        SalaryTo = model.SalaryMax
                    }
                });

                if (!model.NewSubscriptionNotifyByEmail)
                    _mediator.Send(new CancelSearchSubscriptionCommand { ResearcherGuid = researcherGuid, SearchSubscriptionGuid = newSubscriptionGuid });

                model.SubscriptionInfo = new SubscriptionInfoViewModel
                {
                    NewGuid = newSubscriptionGuid,
                    Title = model.NewSubscriptionTitle,
                    NewJustAdded = true
                };
                //перезугружаем страницу с инофрмацией о добавленной подписке
                model.NewSubscriptionAdd = false;
                model.NewSubscriptionTitle = string.Empty;
                model.NewSubscriptionNotifyByEmail = false;

                //var modelBase = Mapper.Map<VacanciesFilterModelBase>(model);
                TempData["VacanciesFilterModel"] = JsonConvert.SerializeObject(model);

                return RedirectToAction("index");
            }

            //dictionaries
            var filterSource = new VacanciesFilterSource(_mediator);
            ViewBag.FilterSource = filterSource;

            //задаем значения по-умолчанию
            if (model.VacancyStates == null)
                model.VacancyStates = filterSource.VacancyStates.Select(c => int.Parse(c.Value));

            //получить список дочерних ФОИВов
            IEnumerable<int> subFoivs = null;
            if (model.Foivs != null && model.Foivs.Any())
            {
                subFoivs =
                    _mediator.Send(new SelectAllFoivsQuery())
                        .Where(c => c.parent_id.HasValue && model.Foivs.Contains(c.parent_id.Value))
                        .Select(c => c.id);
                subFoivs = subFoivs.Union(model.Foivs);
            }

            model.Items = _mediator.Send(new SearchQuery
            {
                Query = model.Search,

                PageSize = model.PageSize,
                CurrentPage = model.CurrentPage,
                OrderFieldByDirection = model.OrderBy,
                PositionTypeIds = model.PositionTypes,
                //PublishDateFrom = DateTime.Now.AddDays(-10),

                FoivIds = subFoivs,
                RegionIds = model.Regions,
                ResearchDirectionIds = model.ResearchDirections,

                SalaryFrom = model.SalaryMin,
                SalaryTo = model.SalaryMax,

                VacancyStatuses = model.VacancyStates?.Select(c => (VacancyStatus)c)

            }).MapToPagedList<Vacancy, VacancyElasticResult>();

            if (model.Items.Items != null && model.Items.Items.Any())
            {
                {
                    //заполняем Организации в Вакансиях
                    var organizaitonsGuid = model.Items.Items.Select(c => c.OrganizationGuid).ToList();
                    var organizationsSource =
                        _mediator.Send(new SelectOrganizationsByGuidsQuery { OrganizationGuids = organizaitonsGuid });
                    if (organizationsSource != null)
                    {
                        model.Items.Items.Where(c => organizationsSource.Select(d => d.guid).Contains(c.OrganizationGuid))
                        .ToList()
                        .ForEach(c => c.OrganizationName = organizationsSource.First(d => d.guid == c.OrganizationGuid).name);
                    }
                }

                {   //заполняем 
                    var regionIds = model.Items.Items.Select(c => c.RegionId).ToList();
                    var regions = _mediator.Send(new SelectRegionsByIdsQuery { RegionIds = regionIds }).ToList();
                    model.Items.Items.Where(c => regions.Select(d => d.id).Contains(c.RegionId))
                        .ToList()
                        .ForEach(c => c.Region = regions.First(d => d.id == c.RegionId).title);
                }
            }

            return View(model);
        }



        /// <summary>
        /// фильтрация, страницы, сортировка 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        [PageTitle("Результаты поиска")]
        [BindResearcherIdFromClaims]
        [Authorize(Roles = ConstTerms.RequireRoleResearcher)]
        public ActionResult AddSubscription(VacanciesFilterModel model, Guid researcherGuid)
        {
            if (TempData["VacanciesFilterModel"] != null)
                model = JsonConvert.DeserializeObject<VacanciesFilterModel>(TempData["VacanciesFilterModel"].ToString());


                var newSubscriptionGuid = _mediator.Send(new CreateSearchSubscriptionCommand
                {
                    ResearcherGuid = researcherGuid,
                    Data = new SearchSubscriptionDataModel
                    {
                        OrderBy = model.OrderBy,
                        Query = model.Search ?? string.Empty,
                        Title = model.NewSubscriptionTitle,
                        FoivIds = model.Foivs,
                        PositionTypeIds = model.PositionTypes,
                        RegionIds = model.Regions,
                        ResearchDirectionIds = model.ResearchDirections,
                        VacancyStatuses = model.VacancyStates?.Select(c => (VacancyStatus)c).ToList(),
                        SalaryFrom = model.SalaryMin,
                        SalaryTo = model.SalaryMax
                    }
                });

                if (!model.NewSubscriptionNotifyByEmail)
                    _mediator.Send(new CancelSearchSubscriptionCommand { ResearcherGuid = researcherGuid, SearchSubscriptionGuid = newSubscriptionGuid });

                model.SubscriptionInfo = new SubscriptionInfoViewModel
                {
                    NewGuid = newSubscriptionGuid,
                    Title = model.NewSubscriptionTitle,
                    NewJustAdded = true
                };
                //перезугружаем страницу с инофрмацией о добавленной подписке
                model.NewSubscriptionTitle = string.Empty;
                model.NewSubscriptionNotifyByEmail = false;

                //var modelBase = Mapper.Map<VacanciesFilterModelBase>(model);
                TempData["VacanciesFilterModel"] = JsonConvert.SerializeObject(model);

                return RedirectToAction("index");

        }
    }
}
