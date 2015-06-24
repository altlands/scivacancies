using MediatR;
using Microsoft.AspNet.Mvc;
using SciVacancies.ReadModel;
using SciVacancies.ReadModel.Core;

using System;

using SciVacancies.WebApp.Commands;
using SciVacancies.Domain.DataModels;

using Nest;

namespace SciVacancies.WebApp.Controllers
{
    public class ResearcherController : Controller
    {
        private readonly IElasticClient _elastic;

        public ResearcherController(IElasticClient elastic)
        {
            _elastic = elastic;
        }
        // GET: /<controller>/
        public void Index()
        {
            var sd = _elastic.Search<Vacancy>(s=>s.Index("scivacancies"));
        }
    }
}
