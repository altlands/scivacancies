using MediatR;
using Microsoft.AspNet.Mvc;
using SciVacancies.Domain.Aggregates.Interfaces;
using SciVacancies.ReadModel;

using System;

using SciVacancies.WebApp.Commands;
using SciVacancies.Domain.DataModels;

using MediatR;

namespace SciVacancies.WebApp.Controllers
{
    public class ResearcherController : Controller
    {
        private readonly IResearcherService _res;
        private readonly IReadModelService _rm;
        private readonly IMediator _mediator;
        private readonly IElasticService _elastic;

        public ResearcherController(IResearcherService researcherService, IReadModelService readModelService, IMediator mediator,IElasticService elastic)
        {
            _res = researcherService;
            _rm = readModelService;
            _mediator = mediator;
            _elastic = elastic;
        }
        // GET: /<controller>/
        public void Index()
        {
            _elastic.CreateIndex();
            //var organizaiontGuid = _mediator.Send(new CreateOrganizationCommand()
            //{
            //    Data = new OrganizationDataModel()
            //    {
            //        Name = "Зонтик"
            //    }
            //});
            //_mediator.Send(new RemoveOrganizationCommand()
            //{
            //    OrganizationGuid = organizaiontGuid
            //});
            //Guid researcherGuid = _res.CreateResearcher(new ResearcherDataModel());

            //Researcher researcher = _rm.SingleResearcher(Guid.NewGuid());

            //return View();
        }
    }
}
