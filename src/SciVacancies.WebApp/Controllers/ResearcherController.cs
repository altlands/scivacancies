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

        public ResearcherController(IResearcherService researcherService, IReadModelService readModelService, IMediator mediator, IElasticService elastic)
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
            Guid organizationGuid = _mediator.Send(new CreateOrganizationCommand()
            {
                Data = new OrganizationDataModel()
                {
                    Name = "Рога и Копыта"
                }
            });
            Guid positionGuid = _mediator.Send(new CreatePositionCommand()
            {
                OrganizationGuid = organizationGuid,
                Data = new PositionDataModel()
                {
                    Name = "Бендер",
                    FullName = "Аферист"
                }
            });
            Guid vacancyGuid = _mediator.Send(new PublishVacancyCommand()
            {
                OrganizationGuid = organizationGuid,
                PositionGuid = positionGuid,
                Data = new VacancyDataModel()
                {
                    Name = "Бендер",
                    FullName = "Аферист"
                }
            });
            _mediator.Send(new SwitchVacancyToAcceptApplicationsCommand()
            {
                OrganizationGuid = organizationGuid,
                VacancyGuid = vacancyGuid
            });
            //Guid orgGuid = _org.CreateOrganization(new OrganizationDataModel()
            //{
            //    Name = "Корпорация Umbrella",
            //    ShortName = "Ubmrella",
            //    OrgFormId = 1,
            //    FoivId = 42,
            //    ActivityId = 1,
            //    HeadFirstName = "Овидий",
            //    HeadLastName = "Грек",
            //    HeadPatronymic = "Иванович"
            //});

            //Guid posGuid1 = _org.CreatePosition(orgGuid, new PositionDataModel()
            //{
            //    Name = "Разводчик акул",
            //    FullName = "Младший сотрудник по разведению лазерных акул",
            //    PositionTypeGuid = Guid.Parse("b7280ace-d237-c007-42fe-ec4aed8f52d4"),
            //    ResearchDirection = "Аналитическая химия",
            //    ResearchDirectionId = 3026
            //});

            //Guid vacGuid1 = _org.PublishVacancy(orgGuid, posGuid1, new VacancyDataModel()
            //{
            //    Name = "Разводчик акул",
            //    FullName = "Младший сотрудник по разведению лазерных акул",
            //    ResearchDirection = "Аналитическая химия",
            //});
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
