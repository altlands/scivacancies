using System;
using System.Linq;
using MediatR;
using Microsoft.AspNet.Mvc;
using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Enums;
using SciVacancies.WebApp.Commands;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Infrastructure.Identity;
using SciVacancies.WebApp.Queries;
using SciVacancies.WebApp.ViewModels;
using Microsoft.AspNet.Identity;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SciVacancies.WebApp.Controllers
{
        [ResponseCache(NoStore = true)]
    public class InitializeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly SciVacUserManager _userManager;

        public InitializeController(SciVacUserManager userManager, IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        public void Index()
        {
            var user = _userManager.FindByName("researcher1");
            if (user != null)
                return;

            //TODO: ntemnikov -> инициализировать БД бОльшим колчеством записей на основе предоставленных данных

            var rnd = new Random();

            _mediator.Send(new RemoveSearchIndexCommand());
            _mediator.Send(new CreateSearchIndexCommand());

            var createUserResearcherCommand1 = new RegisterUserResearcherCommand
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
            var user1 = _mediator.Send(createUserResearcherCommand1);
            var researcherGuid1 = Guid.Parse(user1.Claims.Single(s => s.ClaimType.Equals(ConstTerms.ClaimTypeResearcherId)).ClaimValue);

            var createUserResearcherCommand2 = new RegisterUserResearcherCommand
            {
                Data = new AccountResearcherRegisterViewModel
                {
                    Email = $"researcher{rnd.Next(999)}@mailer.org",
                    Phone = "8-333-22-22",
                    UserName = "researcher2",
                    FirstName = "Анфиса",
                    SecondName = "Иванова",
                    Patronymic = "Павловна",
                    FirstNameEng = "Anfisa",
                    SecondNameEng = "Ivanova",
                    PatronymicEng = "Pavlovna",
                    Education = "Получено высшее образование с 2004 по 2009гг.",
                    BirthYear = DateTime.Now.AddYears(-29).Year
                }
            };
            var user2 = _mediator.Send(createUserResearcherCommand2);
            var researcherGuid2 = Guid.Parse(user2.Claims.Single(s => s.ClaimType.Equals(ConstTerms.ClaimTypeResearcherId)).ClaimValue);



            var createUserOrganizationCommand0 = new RegisterUserOrganizationCommand
            {
                Data = new AccountOrganizationRegisterViewModel
                {
                    Email = $"organization{rnd.Next(999)}@mailer.org",
                    UserName = "organization1",
                    Name = "Научно Исследотельский Институт Горных массивов",
                    ShortName = "НИИ Горных массивов",
                    Address = "Ул. Василяб д.100",
                    INN = "23093209230923",
                    OGRN = "2309230923",
                    OrgFormId = 1,
                    FoivId = 42,
                    ActivityId = 1,
                    HeadFirstName = "Овидий",
                    HeadLastName = "Грек",
                    HeadPatronymic = "Иванович"
                }
            };
            var organization0 = _mediator.Send(createUserOrganizationCommand0);
            var organizationGuid0 = Guid.Parse(organization0.Claims.Single(s => s.ClaimType.Equals(ConstTerms.ClaimTypeOrganizationId)).ClaimValue);




            var createUserOrganizationCommand1 = new RegisterUserOrganizationCommand
            {
                Data = new AccountOrganizationRegisterViewModel
                {
                    Email = $"organization{rnd.Next(2000, 3000)}@mailer.org",
                    UserName = "organization2",
                    Name = "НИИ добра",
                    ShortName = "Good Science",
                    Address = "Луна, море спокойствия",
                    INN = "2332232332",
                    OGRN = "23111113",
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
                ResearcherGuid = researcherGuid1,
                Data = new SearchSubscriptionDataModel { Title = "Разведение лазерных акул", Query = "",OrderBy= "relevant" }
            });


            var positions = _mediator.Send(new SelectAllPositionTypesQuery()).ToList();

            var vacancyGuid1 = _mediator.Send(new CreateVacancyCommand
            {
                OrganizationGuid = organizationGuid0,
                Data = new VacancyDataModel
                {
                    Name = "Разводчик акул",
                    FullName = "Младший сотрудник по разведению лазерных акул",
                    Tasks = "чистить плавники у акул; кормить;",
                    ContactName = "Доктор Зло",
                    ContactEmail = "zlo@gmail.com",
                    ContactPhone = "666-999",
                    ContractType = ContractType.Permanent,
                    EmploymentType = EmploymentType.Probation,
                    OperatingScheduleType = OperatingScheduleType.Rotation,
                    SocialPackage = false,
                    Rent = false,
                    OfficeAccomodation = true,
                    TransportCompensation = true,
                    PositionTypeId = positions.Skip(rnd.Next(positions.Count() - 1)).First().id,
                    RegionId = 25,
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
                    Tasks = "калибровка зеркал лазеров; быть живой мешенью",
                    ContactName = "Доктор Зло",
                    ContactEmail = "zlo@gmail.com",
                    ContactPhone = "666-999",
                    ContractType = ContractType.Permanent,
                    EmploymentType = EmploymentType.Probation,
                    OperatingScheduleType = OperatingScheduleType.Rotation,
                    SocialPackage = false,
                    Rent = false,
                    OfficeAccomodation = true,
                    TransportCompensation = true,
                    PositionTypeId = positions.Skip(rnd.Next(positions.Count() - 1)).First().id,
                    RegionId = 26,
                    ResearchDirection = "Прикладная математика",
                    ResearchDirectionId = 2999
                }
            });
            _mediator.Send(new PublishVacancyCommand { VacancyGuid = vacancyGuid1 });

            var vacancyGuid3 = _mediator.Send(new CreateVacancyCommand
            {
                OrganizationGuid = organizationGuid0,
                Data = new VacancyDataModel
                {
                    Name = "Ремонтник всевидящего ока",
                    FullName = "Младший сотрудник по калибровке фокусного зеркала",
                    Tasks = "калибровка зеркала Саурона;",
                    ContactName = "Саурон Сауронович",
                    ContactEmail = "sauron@thering.com",
                    ContactPhone = "900923-322",
                    ContractType = ContractType.FixedTerm,
                    EmploymentType = EmploymentType.Full,
                    OperatingScheduleType = OperatingScheduleType.FullTime,
                    SocialPackage = true,
                    Rent = true,
                    OfficeAccomodation = true,
                    TransportCompensation = false,
                    PositionTypeId = positions.Skip(rnd.Next(positions.Count() - 1)).First().id,
                    RegionId = 29,
                    ResearchDirection = "Аналитическая химия",
                    ResearchDirectionId = 3026
                }
            });
            _mediator.Send(new PublishVacancyCommand { VacancyGuid = vacancyGuid3 });
        }
    }
}
