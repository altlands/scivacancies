using Microsoft.AspNet.Mvc;
using SciVacancies.ReadModel;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.ViewModels;
using SciVacancies.WebApp.Queries;

using MediatR;

namespace SciVacancies.WebApp
{
    public class SearchController : Controller
    {
        private readonly IReadModelService _readModelService;
        private readonly IMediator _mediator;

        public SearchController(IReadModelService readModelService, IMediator mediator)
        {
            _readModelService = readModelService;
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
            model.ValidateValues();
            var results = new SearchResultsViewModel();
            //result data
            //ViewBag.PagedData = _readModelService.SelectVacancies(
            //    ConstTerms.OrderByDateAscending /*model.OrderBy*/
            //    , model.PageSize
            //    , model.PageNumber
            //    );
            ViewBag.PagedData = _mediator.Send(new SearchQuery
            {
                Query = model.Search,
                PageIndex = model.PageNumber,
                PageSize = model.PageSize,

                PositionsTypes = model.Positions,
                Regions = model.Regions,
                Foivs = model.Foivs,
                ResearchDirections = model.ResearchDirections
            });

            //dicitonaries
            ViewBag.FilterSource = new VacanciesFilterSource(_readModelService); //.SelectRegions(Guid.NewGuid());

            //search request term
            ViewBag.Search = model.Search;

            return View(model);
        }
    }
}
