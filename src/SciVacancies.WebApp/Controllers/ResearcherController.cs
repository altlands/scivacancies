using MediatR;
using Microsoft.AspNet.Mvc;
using Nest;
using SciVacancies.ReadModel.Core;
using SciVacancies.WebApp.Queries;

namespace SciVacancies.WebApp.Controllers
{
    public class ResearcherController : Controller
    {
        private readonly IElasticClient _elastic;
        private readonly IMediator _mediator;

        public ResearcherController(IElasticClient elastic, IMediator mediator)
        {
            _elastic = elastic;
            _mediator = mediator;
        }
        // GET: /<controller>/
        public void Index()
        {
            var pagedOrganizations = _mediator.Send(new SelectPagedOrganizationsQuery { PageIndex = 1, PageSize = 10 });
            var sd = _elastic.Search<Vacancy>(s => s.Index("scivacancies"));
        }
    }
}
