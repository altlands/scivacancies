using MediatR;
using Microsoft.AspNet.Mvc;
using SciVacancies.Domain.Aggregates.Interfaces;
using SciVacancies.ReadModel;

namespace SciVacancies.WebApp.Controllers
{
    public class ResearcherController : Controller
    {
        private readonly IResearcherService _res;
        private readonly IReadModelService _rm;
        private readonly IMediator _med;

        public ResearcherController(IResearcherService researcherService,IReadModelService readModelService, IMediator mediator)
        {
            _res = researcherService;
            _rm = readModelService;
            _med = mediator;
        }
        // GET: /<controller>/
        public void Index(/*ViewModel*/)
        {

            //var model = _med.Send(new CreateResearcherCommand(ViewModel));
            //return View(model);
            //Guid researcherGuid = _res.CreateResearcher(new ResearcherDataModel());

            //Researcher researcher = _rm.SingleResearcher(Guid.NewGuid());

            //return View();
        }
    }
}
