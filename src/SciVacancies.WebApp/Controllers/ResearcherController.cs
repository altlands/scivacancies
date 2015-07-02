using MediatR;
using Microsoft.AspNet.Mvc;
using Nest;
using SciVacancies.ReadModel.Core;
using SciVacancies.WebApp.Queries;

using Newtonsoft.Json.Linq;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Thinktecture.IdentityModel.Client;

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
        public ActionResult Index(string id)
        {

            return RedirectToAction("index", "home");
        }
    }
}
