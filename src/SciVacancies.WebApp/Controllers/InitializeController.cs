using System;
using System.Linq;
using MediatR;
using Microsoft.AspNet.Mvc;
using SciVacancies.Domain.DataModels;
using SciVacancies.WebApp.Commands;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SciVacancies.WebApp.Controllers
{
    public class InitializeController : Controller
    {
<<<<<<< HEAD
        private readonly IMediator _mediator;

        public InitializeController(IMediator mediator)
        {
=======
        private readonly IReadModelService _rm;
        private readonly IMediator _mediator;

        public InitializeController(IReadModelService rm, IMediator mediator)
        {
            _rm = rm;
>>>>>>> master
            _mediator = mediator;
        }
        
        public void Index()
        {
            var createUserResearcherCommand = new RegisterUserResearcherCommand
            {
                Data = new AccountResearcherRegisterViewModel
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
            var user = _mediator.Send(createUserResearcherCommand);

<<<<<<< HEAD
            var resGuid = Guid.Parse(user.Claims.Single(s => s.ClaimType.Equals(ConstTerms.ClaimTypeResearcherId)).ClaimValue);

            var subGuid = _mediator.Send(new CreateSearchSubscriptionCommand
=======
            Guid resGuid = Guid.Parse(user.Claims.Single(s => s.ClaimType.Equals("researcher_id")).ClaimValue);

            Guid subGuid = _mediator.Send(new CreateSearchSubscriptionCommand
>>>>>>> master
            {
                ResearcherGuid = resGuid,
                Data = new SearchSubscriptionDataModel { Title = "Разведение лазерных акул" }
            });

<<<<<<< HEAD
            var createUserOrganizationCommand = new RegisterUserOrganizationCommand
            {
                Data = new AccountOrganizationRegisterViewModel
                {
                    Email = "organization1@mailer.org",
                    UserName = "organization1",
=======
            Guid orgGuid = _mediator.Send(new CreateOrganizationCommand
            {
                Data = new OrganizationDataModel
                {
>>>>>>> master
                    Name = "Научно Исследотельский Институт Горных массивов",
                    ShortName = "НИИ Горных массивов",
                    OrgFormId = 1,
                    FoivId = 42,
                    ActivityId = 1,
                    HeadFirstName = "Овидий",
                    HeadLastName = "Грек",
                    HeadPatronymic = "Иванович"
                }
<<<<<<< HEAD
            };
            var organization = _mediator.Send(createUserOrganizationCommand);
            var orgGuid = Guid.Parse(organization.Claims.Single(s => s.ClaimType.Equals(ConstTerms.ClaimTypeOrganizationId)).ClaimValue);

=======
            });
>>>>>>> master
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
<<<<<<< HEAD
            var vacGuid1 = _mediator.Send(new PublishVacancyCommand
=======
            Guid vacGuid1 = _mediator.Send(new PublishVacancyCommand
>>>>>>> master
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

<<<<<<< HEAD
            var createUserOrganizationCommand1 = new RegisterUserOrganizationCommand
            {
                Data = new AccountOrganizationRegisterViewModel
                {
                    Email = "organization2@mailer.org",
                    UserName = "organization2",
=======
            Guid orgGuid1 = _mediator.Send(new CreateOrganizationCommand
            {
                Data = new OrganizationDataModel
                {
>>>>>>> master
                    Name = "НИИ добра",
                    ShortName = "Good Science",
                    OrgFormId = 2,
                    FoivId = 42,
                    ActivityId = 1,
                    HeadFirstName = "Саруман",
                    HeadLastName = "Саур",
                    HeadPatronymic = "Сауронович"
                }
<<<<<<< HEAD
            };
            var organization1 = _mediator.Send(createUserOrganizationCommand1);
            var orgGuid1 = Guid.Parse(organization1.Claims.Single(s => s.ClaimType.Equals(ConstTerms.ClaimTypeOrganizationId)).ClaimValue);

            var posGuid3 = _mediator.Send(new CreatePositionCommand
            {
                OrganizationGuid = orgGuid1,
=======
            });
            Guid posGuid3 = _mediator.Send(new CreatePositionCommand
            {
                OrganizationGuid = orgGuid,
>>>>>>> master
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
<<<<<<< HEAD
                OrganizationGuid = orgGuid1,
                PositionGuid = posGuid3,
=======
                OrganizationGuid = orgGuid,
                PositionGuid = posGuid1,
>>>>>>> master
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
