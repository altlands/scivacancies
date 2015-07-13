using System;
using System.Linq;
using MediatR;
using Microsoft.AspNet.Mvc;
using SciVacancies.Domain.DataModels;
using SciVacancies.WebApp.Commands;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Queries;
using SciVacancies.WebApp.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SciVacancies.WebApp.Controllers
{
    public class InitializeController : Controller
    {
        private readonly IMediator _mediator;

        public InitializeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Index()
        {
            var rnd = new Random();

            _mediator.Send(new RemoveSearchIndexCommand());
            _mediator.Send(new CreateSearchIndexCommand());

            var createUserResearcherCommand = new RegisterUserResearcherCommand
            {
                Data = new AccountResearcherRegisterViewModel
                {
                    Email = $"researcher{rnd.Next(999)}@mailer.org",
                    Phone = "8-333-22-22",
                    UserName = "researcher1",
                    FirstName = "Генрих",
                    SecondName = "Дубощит",
                    Patronymic = "Иванович",
                    FirstNameEng = "Genrih",
                    SecondNameEng = "Pupkin",
                    PatronymicEng = "Ivanovich",
                    Education = "Получено высшее образование с 2000 по 2005гг.",
                    BirthYear = DateTime.Now.AddYears(-50).Year
                }
            };
            var user = _mediator.Send(createUserResearcherCommand);
            var resGuid = Guid.Parse(user.Claims.Single(s => s.ClaimType.Equals(ConstTerms.ClaimTypeResearcherId)).ClaimValue);



            var createUserOrganizationCommand = new RegisterUserOrganizationCommand
            {
                Data = new AccountOrganizationRegisterViewModel
                {
                    Email = $"organization{rnd.Next(999)}@mailer.org",
                    UserName = "organization1",
                    Name = "Научно Исследотельский Институт Горных массивов",
                    ShortName = "НИИ Горных массивов",
                    OrgFormId = 1,
                    FoivId = 42,
                    ActivityId = 1,
                    HeadFirstName = "Овидий",
                    HeadLastName = "Грек",
                    HeadPatronymic = "Иванович"
                }
            };
            var organization = _mediator.Send(createUserOrganizationCommand);
            var organizationGuid0 = Guid.Parse(organization.Claims.Single(s => s.ClaimType.Equals(ConstTerms.ClaimTypeOrganizationId)).ClaimValue);




            var createUserOrganizationCommand1 = new RegisterUserOrganizationCommand
            {
                Data = new AccountOrganizationRegisterViewModel
                {
                    Email = $"organization{rnd.Next(2000,3000)}@mailer.org",
                    UserName = "organization2",
                    Name = "НИИ добра",
                    ShortName = "Good Science",
                    OrgFormId = 2,
                    FoivId = 42,
                    ActivityId = 1,
                    HeadFirstName = "Саруман",
                    HeadLastName = "Саур",
                    HeadPatronymic = "Сауронович"
                }
            };
            var organization1 = _mediator.Send(createUserOrganizationCommand1);
            var organizationGuid1 = Guid.Parse(organization1.Claims.Single(s => s.ClaimType.Equals(ConstTerms.ClaimTypeOrganizationId)).ClaimValue);

            _mediator.Send(new CreateSearchSubscriptionCommand
            {
                ResearcherGuid = resGuid,
                Data = new SearchSubscriptionDataModel { Title = "Разведение лазерных акул" }
            });


            var positions = _mediator.Send(new SelectAllPositionTypesQuery()).ToList();

            var vacancyGuid1 = _mediator.Send(new CreateVacancyCommand
            {
                OrganizationGuid = organizationGuid0,
                Data = new VacancyDataModel
                {
                    Name = "Разводчик акул",
                    FullName = "Младший сотрудник по разведению лазерных акул",
                    PositionTypeId = positions.Skip( rnd.Next(positions.Count()-1) ).First().id,
                    ResearchDirection = "Аналитическая химия",
                    ResearchDirectionId = 3026
                }
            });
            _mediator.Send(new CreateVacancyCommand
            {
                OrganizationGuid = organizationGuid0,
                Data = new VacancyDataModel
                {
                    Name = "Настройщик лазеров",
                    FullName = "Младший сотрудник по настройке лазеров",
                    PositionTypeId = positions.Skip(rnd.Next(positions.Count() - 1)).First().id,
                    ResearchDirection = "Прикладная математика",
                    ResearchDirectionId = 2999
                }
            });
            _mediator.Send(new PublishVacancyCommand { VacancyGuid = vacancyGuid1 });

            var vacancyGuid3 = _mediator.Send(new CreateVacancyCommand
            {
                OrganizationGuid = organizationGuid1,
                Data = new VacancyDataModel
                {
                    Name = "Ремонтник всевидящего ока",
                    FullName = "Младший сотрудник по калибровке фокусного зеркала",
                    PositionTypeId = positions.Skip(rnd.Next(positions.Count() - 1)).First().id,
                    ResearchDirection = "Аналитическая химия",
                    ResearchDirectionId = 3026
                }
            });
            _mediator.Send(new PublishVacancyCommand { VacancyGuid = vacancyGuid3 });
        }
    }
}
