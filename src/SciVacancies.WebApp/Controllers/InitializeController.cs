using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using Microsoft.AspNet.Mvc;
using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Enums;
using SciVacancies.WebApp.Commands;
using SciVacancies.WebApp.Infrastructure.Identity;
using SciVacancies.WebApp.Queries;
using SciVacancies.WebApp.ViewModels;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using SciVacancies.WebApp.Models.DataModels;

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

        public IActionResult Index()
        {
            Guid researcherGuid1;
            var rnd = new Random();

            /*инициализируем пользователей*/
            var user = _userManager.FindByName("researcher1");
            if (user == null)
            {

                _mediator.Send(new RemoveSearchIndexCommand());
                _mediator.Send(new CreateSearchIndexCommand());


                var createUserResearcherCommand1 = new RegisterUserResearcherCommand
                {
                    Data = new ResearcherRegisterDataModel
                    {
                        Email = $"scivac@yandex.ru",
                        Phone = "8-333-22-22",
                        UserName = "researcher1",
                        FirstName = "Генрих",
                        SecondName = "Дубощит",
                        Patronymic = "Иванович",
                        FirstNameEng = "Genrih",
                        SecondNameEng = "Pupkin",
                        PatronymicEng = "Ivanovich",
                        BirthYear = DateTime.Now.AddYears(-50).Year,
                        Interests = JsonConvert.SerializeObject(new List<InterestEditViewModel>
                        {
                            new InterestEditViewModel {IntName = "Научный интерес 1", IntNameEn = "Research Interest 1"},
                            new InterestEditViewModel {IntName = "Научный интерес 2", IntNameEn = "Research Interest 2"},
                            new InterestEditViewModel {IntName = "Научный интерес 3", IntNameEn = "Research Interest 3"},
                            new InterestEditViewModel {IntName = "Научный интерес 4", IntNameEn = "Research Interest 4"}
                        })
                    }
                };
                var user1 = _mediator.Send(createUserResearcherCommand1);
                researcherGuid1 =
                    Guid.Parse(user1.Claims.Single(s => s.ClaimType.Equals(ConstTerms.ClaimTypeResearcherId)).ClaimValue);
                if (!_userManager.IsInRole(user1.Id, ConstTerms.RequireRoleResearcher))
                    _userManager.AddToRole(user1.Id, ConstTerms.RequireRoleResearcher);


                _mediator.Send(new CreateSearchSubscriptionCommand
                {
                    ResearcherGuid = researcherGuid1,
                    Data =
                        new SearchSubscriptionDataModel
                        {
                            Title = "Разведение лазерных акул",
                            Query = "",
                            OrderBy = "relevant"
                        }
                });

                var createUserResearcherCommand2 = new RegisterUserResearcherCommand
                {
                    Data = new ResearcherRegisterDataModel
                    {
                        Email = $"scivac@mail.ru",
                        Phone = "8-333-22-22",
                        UserName = "researcher2",
                        FirstName = "Анфиса",
                        SecondName = "Иванова",
                        Patronymic = "Павловна",
                        FirstNameEng = "Anfisa",
                        SecondNameEng = "Ivanova",
                        PatronymicEng = "Pavlovna",
                        BirthYear = DateTime.Now.AddYears(-29).Year
                    }
                };
                var user2 = _mediator.Send(createUserResearcherCommand2);
                if (!_userManager.IsInRole(user2.Id, ConstTerms.RequireRoleResearcher))
                    _userManager.AddToRole(user2.Id, ConstTerms.RequireRoleResearcher);

                var createUserResearcherCommand3 = new RegisterUserResearcherCommand
                {
                    Data = new ResearcherRegisterDataModel
                    {
                        Email = $"researcher{rnd.Next(999)}@mailer.org",
                        Phone = "8-954-12-54",
                        UserName = "researcher3",
                        FirstName = "Ангелина",
                        SecondName = "Юрьевна",
                        Patronymic = "Орехова",
                        FirstNameEng = "Angelina",
                        SecondNameEng = "Yrjevna",
                        PatronymicEng = "Orehova",
                        BirthYear = DateTime.Now.AddYears(-50).Year
                    }
                };
                var user3 = _mediator.Send(createUserResearcherCommand3);
                if (!_userManager.IsInRole(user3.Id, ConstTerms.RequireRoleResearcher))
                    _userManager.AddToRole(user3.Id, ConstTerms.RequireRoleResearcher);
            }

            /*инициализируем организации*/
            if (_mediator.Send(new CountOrganizationsQuery()) == 0)
            {
                var organization0_Data = new AccountOrganizationRegisterViewModel
                {
                    Email = "technobrowser@gmail.com",
                    UserName = "organization2",
                    Name = "Акционерное общество 'Океанприбор'-2",
                    ShortName = "Company 'Okeanpribor-2'",
                    Address = "Москва, Ленинградский проспект",
                    INN = "2332232332",
                    OGRN = "23111113",
                    OrgFormId = 2,
                    FoivId = 42,
                    ActivityId = 1,
                    HeadFirstName = "Саруман",
                    HeadLastName = "Саур",
                    HeadPatronymic = "Сауронович"
                };
                
                var organization0 = _mediator.Send(new RegisterUserOrganizationCommand { Data = organization0_Data });
                var organization0_Guid = Guid.Parse( organization0.Claims.Single(s => s.ClaimType.Equals(ConstTerms.ClaimTypeOrganizationId)).ClaimValue);


                //var organization1_Data = new AccountOrganizationRegisterViewModel
                //{
                //    Email = $"organization{rnd.Next(2000, 3000)}@mailer.org",
                //    UserName = "organization1",
                //    Name = "Научно Исследотельский Институт Горных массивов",
                //    ShortName = "НИИ Горных массивов",
                //    Address = "Ул. Василяб д.100",
                //    INN = "23093209230923",
                //    OGRN = "2309230923",
                //    OrgFormId = 1,
                //    FoivId = 42,
                //    ActivityId = 1,
                //    HeadFirstName = "Овидий",
                //    HeadLastName = "Грек",
                //    HeadPatronymic = "Иванович"
                //};

                //var organization1 = _mediator.Send(new RegisterUserOrganizationCommand { Data = organization1_Data });
                //var organization1_Guid =Guid.Parse(organization1.Claims.Single(s => s.ClaimType.Equals(ConstTerms.ClaimTypeOrganizationId)).ClaimValue);


                /*инициализируем вакансии*/
                if (_mediator.Send(new CountVacanciesQuery()) == 0)
                {
                    var positions = _mediator.Send(new SelectAllPositionTypesQuery()).ToList();
                    var researchDiretions = _mediator.Send(new SelectAllResearchDirectionsQuery()).ToList();

                    var vacancyGuid1 = _mediator.Send(new CreateVacancyCommand
                    {
                        OrganizationGuid = organization0_Guid,
                        Data = new VacancyDataModel
                        {
                            Name = "Разводчик акул",
                            FullName = "Младший сотрудник по разведению лазерных акул",
                            Tasks = "чистить плавники у акул; кормить;",
                            ContactName = "Доктор Зло",
                            ContactEmail = "technobrowser@gmail.com",
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
                            ResearchDirectionId = 3026,
                            OrganizationFoivId = organization0_Data.FoivId
                        }
                    });
                    _mediator.Send(new PublishVacancyCommand
                    {
                        VacancyGuid = vacancyGuid1,
                        //InCommitteeStartDate = DateTime.UtcNow.AddDays(60),
                        InCommitteeStartDate = DateTime.UtcNow.AddMinutes(7),
                        //InCommitteeEndDate = DateTime.UtcNow.AddDays(65)
                        InCommitteeEndDate = DateTime.UtcNow.AddMinutes(11)
                    });
                    /*
                    var vacancyGuid2 = _mediator.Send(new CreateVacancyCommand
                    {
                        OrganizationGuid = organization0_Guid,
                        Data = new VacancyDataModel
                        {
                            PositionTypeId = positions.First(c => c.title.Contains("Младший научный сотрудник")).id,
                            Name = "Младший научный сотрудник",
                            FullName = "Младший научный сотрудник по разработке лазерных систем",
                            ResearchDirection = "Прикладная математика",
                            ResearchDirectionId = 2999,
                            Tasks = "Исследование химического состава материалов для минилазеров",
                            SalaryFrom = 45000,
                            SalaryTo = 55000,
                            ContractType = ContractType.Permanent,
                            //ContractTime = null,
                            SocialPackage = true,
                            //Rent = false,
                            TransportCompensation = true,
                            //OfficeAccomodation = false,
                            ContactName = "Сидоров Петр Иванович",
                            ContactEmail = "info@vniilaz.ru",
                            ContactPhone = "89665467233",
                            EmploymentType = EmploymentType.Full,
                            OperatingScheduleType = OperatingScheduleType.FullTime,
                            RegionId = 27,
                            OrganizationFoivId = organization0_Data.FoivId
                        }
                    });
                    _mediator.Send(new PublishVacancyCommand
                    {
                        VacancyGuid = vacancyGuid2,
                        //InCommitteeStartDate = DateTime.UtcNow.AddDays(60),
                        InCommitteeStartDate = DateTime.UtcNow.AddMinutes(20),
                        //InCommitteeEndDate = DateTime.UtcNow.AddDays(65)
                        InCommitteeEndDate = DateTime.UtcNow.AddMinutes(26)
                    });
                    */
                    //var vacancyGuid3 = _mediator.Send(new CreateVacancyCommand
                    //{
                    //    OrganizationGuid = organization0_Guid,
                    //    Data = new VacancyDataModel
                    //    {
                    //        PositionTypeId = positions.First(c => c.title.Contains("Ведущий научный сотрудник")).id,
                    //        Name = "Ведущий научный сотрудник",
                    //        FullName = "Ведущий научный сотрудник по калибровке фокусного зеркала",
                    //        ResearchDirection = "Прикладная математика",
                    //        ResearchDirectionId = 2999,
                    //        Tasks = "калибровка зеркала",
                    //        //SalaryFrom = 0,
                    //        //SalaryTo = 0,
                    //        ContractType = ContractType.FixedTerm,
                    //        ContractTime = 0.9m,
                    //        SocialPackage = true,
                    //        Rent = true,
                    //        //TransportCompensation = true,
                    //        //OfficeAccomodation = true,
                    //        ContactName = "",
                    //        ContactEmail = "sauron@thering.com",
                    //        ContactPhone = "900923-322",
                    //        EmploymentType = EmploymentType.Full,
                    //        OperatingScheduleType = OperatingScheduleType.FullTime,
                    //        RegionId = 27,
                    //        OrganizationFoivId = organization0_Data.FoivId
                    //    }
                    //});
                    //_mediator.Send(new PublishVacancyCommand
                    //{
                    //    VacancyGuid = vacancyGuid3,
                    //    InCommitteeStartDate = DateTime.Now.AddDays(60),
                    //    InCommitteeEndDate = DateTime.Now.AddDays(65)
                    //});

                    //var vacancyGuid4 = _mediator.Send(new CreateVacancyCommand
                    //{
                    //    OrganizationGuid = organization0_Guid,
                    //    Data = new VacancyDataModel
                    //    {
                    //        PositionTypeId =
                    //            positions.First(
                    //                c =>
                    //                    c.title.Contains("начальник") && c.title.Contains("научно-") &&
                    //                    c.title.Contains("лаборатории")).id,
                    //        Name = "Начальник научно-исследовательской лаборатории",
                    //        FullName = "Начальник научно-исследовательской лаборатории лазерных систем",
                    //        ResearchDirection = researchDiretions.First(c => c.title.Contains("системы")).title,
                    //        ResearchDirectionId = researchDiretions.First(c => c.title.Contains("системы")).id,
                    //        Tasks = "Исследование в области химических структур для лазерных систем",
                    //        //SalaryFrom = 0,
                    //        //SalaryTo = 0,
                    //        ContractType = ContractType.Permanent,
                    //        //ContractTime = 0.9m,
                    //        SocialPackage = true,
                    //        Rent = true,
                    //        //TransportCompensation = true,
                    //        //OfficeAccomodation = true,
                    //        ContactName = "Сидоров Петр Иванович",
                    //        ContactEmail = "info@vniilaz.ru",
                    //        ContactPhone = "89665467233",
                    //        EmploymentType = EmploymentType.Full,
                    //        OperatingScheduleType = OperatingScheduleType.FullTime,
                    //        RegionId = 27,
                    //        OrganizationFoivId = organization0_Data.FoivId
                    //    }
                    //});
                    //_mediator.Send(new PublishVacancyCommand
                    //{
                    //    VacancyGuid = vacancyGuid4,
                    //    InCommitteeStartDate = DateTime.Now.AddDays(60),
                    //    InCommitteeEndDate = DateTime.Now.AddDays(65)
                    //});

                    //var vacancyGuid5 = _mediator.Send(new CreateVacancyCommand
                    //{
                    //    OrganizationGuid = organization0_Guid,
                    //    Data = new VacancyDataModel
                    //    {
                    //        PositionTypeId = positions.First(c => c.title.Contains("Младший научный сотрудник")).id,
                    //        Name = "Младший научный сотрудник",
                    //        FullName = "Младший научный сотрудник по разработке лазерных систем",
                    //        ResearchDirection = "Прикладная математика",
                    //        ResearchDirectionId = 2999,
                    //        Tasks = "Исследование химического состава материалов для минилазеров",
                    //        //SalaryFrom = 0,
                    //        //SalaryTo = 0,
                    //        ContractType = ContractType.Permanent,
                    //        //ContractTime = null,
                    //        //SocialPackage = true,
                    //        //Rent = true,
                    //        TransportCompensation = true,
                    //        //OfficeAccomodation = false,
                    //        ContactName = "Сидоров Петр Иванович",
                    //        ContactEmail = "info@vniilaz.ru",
                    //        ContactPhone = "89665467233",
                    //        EmploymentType = EmploymentType.Full,
                    //        OperatingScheduleType = OperatingScheduleType.FullTime,
                    //        RegionId = 27,
                    //        OrganizationFoivId = organization0_Data.FoivId
                    //    }
                    //});
                    //_mediator.Send(new PublishVacancyCommand
                    //{
                    //    VacancyGuid = vacancyGuid5,
                    //    InCommitteeStartDate = DateTime.Now.AddDays(60),
                    //    InCommitteeEndDate = DateTime.Now.AddDays(65)
                    //});



                    //var vacancyGuid6 = _mediator.Send(new CreateVacancyCommand
                    //{
                    //    OrganizationGuid = organization0_Guid,
                    //    Data = new VacancyDataModel
                    //    {
                    //        PositionTypeId = positions.First(c => c.title.Contains("инженер-исследователь")).id,
                    //        Name = "Инженер-исследователь",
                    //        FullName = "Инженер-исследователь лазерных систем (Младший научный сотрудник)",
                    //        ResearchDirection = researchDiretions.First(c => c.title.Contains("системы")).title,
                    //        ResearchDirectionId = researchDiretions.First(c => c.title.Contains("системы")).id,
                    //        Tasks = "Исследование лазерных систем",
                    //        //SalaryFrom = 0,
                    //        //SalaryTo = 0,
                    //        ContractType = ContractType.Permanent,
                    //        //ContractTime = null,
                    //        //SocialPackage = true,
                    //        //Rent = true,
                    //        TransportCompensation = true,
                    //        //OfficeAccomodation = false,
                    //        ContactName = "Ким Валерий Миронович",
                    //        ContactEmail = "flashbrowser@gmail.com",
                    //        ContactPhone = "666-999",
                    //        EmploymentType = EmploymentType.Full,
                    //        OperatingScheduleType = OperatingScheduleType.FullTime,
                    //        RegionId = 27,
                    //        OrganizationFoivId = organization0_Data.FoivId
                    //    }
                    //});
                    //_mediator.Send(new PublishVacancyCommand
                    //{
                    //    VacancyGuid = vacancyGuid6,
                    //    InCommitteeStartDate = DateTime.Now.AddDays(60),
                    //    InCommitteeEndDate = DateTime.Now.AddDays(65)
                    //});



                    //var vacancyGuid7 = _mediator.Send(new CreateVacancyCommand
                    //{
                    //    OrganizationGuid = organization0_Guid,
                    //    Data = new VacancyDataModel
                    //    {
                    //        PositionTypeId = positions.First(c => c.title.Contains("Ведущий научный сотрудник")).id,
                    //        Name = "Ведущий научный сотрудник",
                    //        FullName = "Ведущий научный сотрудник по калибровке",
                    //        ResearchDirection = researchDiretions.First(c => c.title.Contains("системы")).title,
                    //        ResearchDirectionId = researchDiretions.First(c => c.title.Contains("системы")).id,
                    //        Tasks = "чистить плавники у акул; кормить;",
                    //        SalaryFrom = 100000,
                    //        SalaryTo = 120000,
                    //        ContractType = ContractType.FixedTerm,
                    //        ContractTime = 0.6m,
                    //        SocialPackage = true,
                    //        Rent = true,
                    //        //TransportCompensation = true,
                    //        //OfficeAccomodation = false,
                    //        ContactName = "Саурон Сауронович",
                    //        ContactEmail = "info@vniilaz.ru",
                    //        ContactPhone = "900923-322",
                    //        EmploymentType = EmploymentType.Full,
                    //        OperatingScheduleType = OperatingScheduleType.FullTime,
                    //        RegionId = 27,
                    //        OrganizationFoivId = organization0_Data.FoivId
                    //    }
                    //});
                    //_mediator.Send(new PublishVacancyCommand
                    //{
                    //    VacancyGuid = vacancyGuid7,
                    //    InCommitteeStartDate = DateTime.Now.AddDays(60),
                    //    InCommitteeEndDate = DateTime.Now.AddDays(65)
                    //});


                    //var vacancyGuid8 = _mediator.Send(new CreateVacancyCommand
                    //{
                    //    OrganizationGuid = organization0_Guid,
                    //    Data = new VacancyDataModel
                    //    {
                    //        PositionTypeId = positions.First(c => c.title.Contains("инженер-исследователь")).id,
                    //        Name = "Инженер-исследователь лазерных систем (Младший научный сотрудник)",
                    //        FullName = "Инженер-исследователь лазерных систем (Младший научный сотрудник)",
                    //        ResearchDirection = researchDiretions.First(c => c.title.Contains("системы")).title,
                    //        ResearchDirectionId = researchDiretions.First(c => c.title.Contains("системы")).id,
                    //        Tasks = "Исследование лазерных систем",
                    //        SalaryFrom = 60000,
                    //        SalaryTo = 85000,
                    //        ContractType = ContractType.Permanent,
                    //        //ContractTime = 0.6m,
                    //        //SocialPackage = true,
                    //        //Rent = true,
                    //        TransportCompensation = true,
                    //        //OfficeAccomodation = false,
                    //        ContactName = "Ким Валерий Миронович",
                    //        ContactEmail = "flashbrowser@gmail.com",
                    //        ContactPhone = "666-999",
                    //        EmploymentType = EmploymentType.Full,
                    //        OperatingScheduleType = OperatingScheduleType.FullTime,
                    //        RegionId = 27,
                    //        OrganizationFoivId = organization0_Data.FoivId
                    //    }
                    //});
                    //_mediator.Send(new PublishVacancyCommand
                    //{
                    //    VacancyGuid = vacancyGuid8,
                    //    InCommitteeStartDate = DateTime.Now.AddDays(60),
                    //    InCommitteeEndDate = DateTime.Now.AddDays(65)
                    //});


                    //var vacancyGuid9 = _mediator.Send(new CreateVacancyCommand
                    //{
                    //    OrganizationGuid = organization0_Guid,
                    //    Data = new VacancyDataModel
                    //    {
                    //        PositionTypeId =
                    //            positions.First(c => c.title.Contains("Заведующий") && c.title.Contains("отдела")).id,
                    //        Name = "Заведующий отдела информационных технологий",
                    //        FullName = "Заведующий отдела информационных технологий",
                    //        ResearchDirection = researchDiretions.First(c => c.title.Contains("системы")).title,
                    //        ResearchDirectionId = researchDiretions.First(c => c.title.Contains("системы")).id,
                    //        Tasks = "чистить плавники у акул; кормить;",
                    //        SalaryFrom = 80000,
                    //        SalaryTo = 100000,
                    //        ContractType = ContractType.Permanent,
                    //        //ContractTime = 0.6m,
                    //        SocialPackage = true,
                    //        Rent = true,
                    //        //TransportCompensation = true,
                    //        //OfficeAccomodation = false,
                    //        ContactName = "Алексей",
                    //        ContactEmail = "n567@mail.com",
                    //        ContactPhone = "89980012233",
                    //        EmploymentType = EmploymentType.Full,
                    //        OperatingScheduleType = OperatingScheduleType.FullTime,
                    //        RegionId = 27,
                    //        OrganizationFoivId = organization0_Data.FoivId
                    //    }
                    //});
                    //_mediator.Send(new PublishVacancyCommand
                    //{
                    //    VacancyGuid = vacancyGuid9,
                    //    InCommitteeStartDate = DateTime.Now.AddDays(60),
                    //    InCommitteeEndDate = DateTime.Now.AddDays(65)
                    //});




                    //var vacancyGuid10 = _mediator.Send(new CreateVacancyCommand
                    //{
                    //    OrganizationGuid = organization0_Guid,
                    //    Data = new VacancyDataModel
                    //    {
                    //        PositionTypeId = positions.First(c => c.title.Contains("Ведущий научный сотрудник")).id,
                    //        Name = "Ведущий научный сотрудник",
                    //        FullName = "Ведущий научный сотрудник информационных технологий",
                    //        ResearchDirection = researchDiretions.First(c => c.title.Contains("системы")).title,
                    //        ResearchDirectionId = researchDiretions.First(c => c.title.Contains("системы")).id,
                    //        Tasks = "чистить плавники у акул; кормить;",
                    //        SalaryFrom = 80000,
                    //        SalaryTo = 100001,
                    //        ContractType = ContractType.Permanent,
                    //        //ContractTime = 0.6m,
                    //        SocialPackage = true,
                    //        Rent = true,
                    //        //TransportCompensation = true,
                    //        //OfficeAccomodation = false,
                    //        ContactName = "Алексей",
                    //        ContactEmail = "n567@mail.com",
                    //        ContactPhone = "89980012234",
                    //        EmploymentType = EmploymentType.Full,
                    //        OperatingScheduleType = OperatingScheduleType.FullTime,
                    //        RegionId = 27,
                    //        OrganizationFoivId = organization0_Data.FoivId
                    //    }
                    //});
                    //_mediator.Send(new PublishVacancyCommand
                    //{
                    //    VacancyGuid = vacancyGuid10,
                    //    InCommitteeStartDate = DateTime.Now.AddDays(60),
                    //    InCommitteeEndDate = DateTime.Now.AddDays(65)
                    //});


                    return Content("инициализация успешно завершена");
                }
            }

            //return Content("инициализация уже проходила");
            return RedirectToAction("index", "home");

        }
    }
}
