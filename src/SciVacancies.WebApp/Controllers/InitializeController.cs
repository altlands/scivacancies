using System;
using MediatR;
using Microsoft.AspNet.Mvc;
using SciVacancies.Domain.DataModels;
using SciVacancies.ReadModel;
using SciVacancies.WebApp.Commands;
using SciVacancies.WebApp.ViewModels;

using System.Linq;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SciVacancies.WebApp.Controllers
{
    public class InitializeController : Controller
    {
        private readonly IReadModelService _rm;
        private readonly IMediator _mediator;

        public InitializeController(IReadModelService rm, IMediator mediator)
        {
            _rm = rm;
            _mediator = mediator;
        }
        // GET: /<controller>/
        public void Index()
        {


            var command = new RegisterUserResearcherCommand
            {
                Data = new AccountRegisterViewModel
                {
                    Email = "researcher1@mailer.org",
                    UserName = "researcher1",
                    FirstName = "Генрих",
                    SecondName = "Дубощит",
                    Patronymic = "Иванович",
                    FirstNameEng = "Genrih",
                    SecondNameEng = "Pupkin",
                    PatronymicEng = "Ivanovich",
                    BirthYear = DateTime.Now.AddYears(-50).Year
                }
            };
            var user = _mediator.Send(command);

            Guid resGuid = Guid.Parse(user.Claims.Single(s => s.ClaimType.Equals("researcher_id")).ClaimValue);

            Guid subGuid = _mediator.Send(new CreateSearchSubscriptionCommand
            {
                ResearcherGuid = resGuid,
                Data = new SearchSubscriptionDataModel { Title = "Разведение лазерных акул" }
            });

            Guid orgGuid = _mediator.Send(new CreateOrganizationCommand
            {
                Data = new OrganizationDataModel
                {
                    Name = "Научно Исследотельский Институт Горных массивов",
                    ShortName = "НИИ Горных массивов",
                    OrgFormId = 1,
                    FoivId = 42,
                    ActivityId = 1,
                    HeadFirstName = "Овидий",
                    HeadLastName = "Грек",
                    HeadPatronymic = "Иванович"
                }
            });
            Guid posGuid1 = _mediator.Send(new CreatePositionCommand
            {
                OrganizationGuid = orgGuid,
                Data = new PositionDataModel
                {
                    Name = "Разводчик акул",
                    FullName = "Младший сотрудник по разведению лазерных акул",
                    PositionTypeGuid = Guid.Parse("b7280ace-d237-c007-42fe-ec4aed8f52d4"),
                    ResearchDirection = "Аналитическая химия",
                    ResearchDirectionId = 3026
                }
            });
            Guid posGuid2 = _mediator.Send(new CreatePositionCommand
            {
                OrganizationGuid = orgGuid,
                Data = new PositionDataModel
                {
                    Name = "Настройщик лазеров",
                    FullName = "Младший сотрудник по настройке лазеров",
                    PositionTypeGuid = Guid.Parse("b7280ace-d237-c007-42fe-ec4aed8f52d4"),
                    ResearchDirection = "Прикладная математика",
                    ResearchDirectionId = 2999
                }
            });
            Guid vacGuid1 = _mediator.Send(new PublishVacancyCommand
            {
                OrganizationGuid = orgGuid,
                PositionGuid = posGuid1,
                Data = new VacancyDataModel
                {
                    Name = "Разводчик акул",
                    FullName = "Младший сотрудник по разведению лазерных акул",
                    ResearchDirection = "Аналитическая химия"
                }
            });

            Guid orgGuid1 = _mediator.Send(new CreateOrganizationCommand
            {
                Data = new OrganizationDataModel
                {
                    Name = "НИИ добра",
                    ShortName = "Good Science",
                    OrgFormId = 2,
                    FoivId = 42,
                    ActivityId = 1,
                    HeadFirstName = "Саруман",
                    HeadLastName = "Саур",
                    HeadPatronymic = "Сауронович"
                }
            });
            Guid posGuid3 = _mediator.Send(new CreatePositionCommand
            {
                OrganizationGuid = orgGuid,
                Data = new PositionDataModel
                {
                    Name = "Ремонтник всевидящего ока",
                    FullName = "Младший сотрудник по калибровке фокусного зеркала",
                    PositionTypeGuid = Guid.Parse("b7280ace-d237-c007-42fe-ec4aed8f52d4"),
                    ResearchDirection = "Аналитическая химия",
                    ResearchDirectionId = 3026
                }
            });

            Guid vacGuid3 = _mediator.Send(new PublishVacancyCommand
            {
                OrganizationGuid = orgGuid,
                PositionGuid = posGuid1,
                Data = new VacancyDataModel
                {
                    Name = "Разводчик акул",
                    FullName = "Младший сотрудник по разведению лазерных акул",
                    ResearchDirection = "Аналитическая химия"
                }
            });
        }
    }
}
