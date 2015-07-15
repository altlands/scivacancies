using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using Microsoft.AspNet.Mvc;
using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel.ElasticSearchModel.Model;
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
        public ViewResult Index(VacanciesFilterModel model)
        {
            model.Items= _mediator.Send(new SearchQuery
            {
                Query = model.Search,

                PageSize = model.PageSize,
                CurrentPage = model.CurrentPage,
                OrderBy = model.OrderBy,

                //PublishDateFrom = DateTime.Now.AddDays(-10),

                FoivIds = model.Foivs,
                PositionTypeIds = model.Positions,
                RegionIds = model.Regions,
                ResearchDirectionIds = model.ResearchDirections,

                SalaryFrom = model.SalaryMin,
                SalaryTo = model.SalaryMax,

                VacancyStatuses =(IEnumerable<VacancyStatus>)model.VacancyStates

            }).MapToPagedList<Vacancy, VacancyElasticResult>();

            if (model.Items.Items != null && model.Items.Items.Any())
            {
                var organizaitonsGuid = model.Items.Items.Select(c => c.OrganizationGuid).ToList();
                var organizations = _mediator.Send(new SelectOrganizationsByGuidsQuery {OrganizationGuids = organizaitonsGuid }).ToList();
                model.Items.Items.Where(c=> organizations.Select(d => d.guid).Contains(c.OrganizationGuid)).ToList().ForEach(c=> c.OrganizationName = organizations.First(d=>d.guid == c.OrganizationGuid).name);
            }

            //dicitonaries
            ViewBag.FilterSource = new VacanciesFilterSource(_mediator);

            return View(model);
        }
    }
}
