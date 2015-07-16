using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel.ElasticSearchModel.Model;
using SciVacancies.WebApp.Commands;
using SciVacancies.WebApp.Queries;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Controllers
{

    public class SearchController : Controller
    {
        private readonly IMediator _mediator;

        public SearchController(IMediator mediator)
        {
            _mediator = mediator;
        }


        /// <summary>
        /// фильтрация, страницы, сортировка 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
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
                        VacancyStatuses = model.VacancyStates?.Select(c => (VacancyStatus)c),
                        SalaryFrom = model.SalaryMin,
                        SalaryTo = model.SalaryMax
                    }
                });

                if (model.NewSubscriptionNotifyByEmail)
                    _mediator.Send(new ActivateSearchSubscriptionCommand { ResearcherGuid = researcherGuid, SearchSubscriptionGuid = newSubscriptionGuid });

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

            //dicitonaries
            var filterSource = new VacanciesFilterSource(_mediator);
            ViewBag.FilterSource = filterSource;

            //задаем значения по-умолчанию
            if (model.VacancyStates == null)
                model.VacancyStates = filterSource.VacancyStates.Select(c => int.Parse(c.Value));

            model.Items = _mediator.Send(new SearchQuery
            {
                Query = model.Search,

                PageSize = model.PageSize,
                CurrentPage = model.CurrentPage,
                OrderBy = model.OrderBy,
                PositionTypeIds = model.PositionTypes,
                //PublishDateFrom = DateTime.Now.AddDays(-10),

                FoivIds = model.Foivs,
                RegionIds = model.Regions,
                ResearchDirectionIds = model.ResearchDirections,

                SalaryFrom = model.SalaryMin,
                SalaryTo = model.SalaryMax,

                VacancyStatuses = model.VacancyStates.Select(c => (VacancyStatus)c)

            }).MapToPagedList<Vacancy, VacancyElasticResult>();

            if (model.Items.Items != null && model.Items.Items.Any())
            {
                var organizaitonsGuid = model.Items.Items.Select(c => c.OrganizationGuid).ToList();
                var organizations = _mediator.Send(new SelectOrganizationsByGuidsQuery { OrganizationGuids = organizaitonsGuid }).ToList();
                model.Items.Items.Where(c => organizations.Select(d => d.guid).Contains(c.OrganizationGuid)).ToList().ForEach(c => c.OrganizationName = organizations.First(d => d.guid == c.OrganizationGuid).name);
            }

            return View(model);
        }
    }
}
