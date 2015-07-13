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
            model.Items= _mediator.Send(new SearchQuery
            {
                Query = model.Search,
                PageSize = model.PageSize,
                CurrentPage = model.CurrentPage,
                
                PositionsTypeIds = model.Positions,
                RegionIds = model.Regions,
                FoivIds = model.Foivs,
                ResearchDirectionIds = model.ResearchDirections
            }).MapToPagedList();

            //dicitonaries
            ViewBag.FilterSource = new VacanciesFilterSource(_mediator);

            return View(model);
        }
    }
}
