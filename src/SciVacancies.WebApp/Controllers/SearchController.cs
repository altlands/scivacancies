using MediatR;
using Microsoft.AspNet.Mvc;
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
            model.ValidateValues();

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
            ViewBag.FilterSource = new VacanciesFilterSource(_mediator);

            return View(model);
        }
    }
}
