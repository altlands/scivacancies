using Microsoft.AspNet.Mvc;
using SciVacancies.Domain.Aggregates.Interfaces;
using SciVacancies.ReadModel;

namespace SciVacancies.WebApp.Controllers
{
    public class ResearcherController : Controller
    {
        private readonly IResearcherService _res;
        private readonly IReadModelService _rm;

        public ResearcherController(IResearcherService researcherService,IReadModelService readModelService)
        {
            _res = researcherService;
            _rm = readModelService;
        }
        // GET: /<controller>/
        public void Index()
        {
            //Guid researcherGuid = _res.CreateResearcher(new ResearcherDataModel());

            //Researcher researcher = _rm.SingleResearcher(Guid.NewGuid());

            //return View();
        }
    }
}
