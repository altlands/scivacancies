using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Aggregates.Interfaces;
using SciVacancies.ReadModel;
using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

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
        public IActionResult Index()
        {
            Guid researcherGuid = _res.CreateResearcher(new ResearcherDataModel());

            Researcher researcher = _rm.SingleResearcher(Guid.NewGuid());

            return View();
        }
    }
}
