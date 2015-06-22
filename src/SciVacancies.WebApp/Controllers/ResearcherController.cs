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

        public ResearcherController(IResearcherService researcherService, IReadModelService readModelService, IMediator mediator)
        {
            _res = researcherService;
            _rm = readModelService;
            _mediator = mediator;
        }
        // GET: /<controller>/
        public void Index()
        {

            var organizaiontGuid = _mediator.Send(new CreateOrganizationCommand()
            {
                Data = new OrganizationDataModel()
                {
                    Name = "Зонтик"
                }
            });
            _mediator.Send(new RemoveOrganizationCommand()
            {
                OrganizationGuid = organizaiontGuid
            });
            //Guid researcherGuid = _res.CreateResearcher(new ResearcherDataModel());

            //Researcher researcher = _rm.SingleResearcher(Guid.NewGuid());

            //return View();
        }
    }
}
