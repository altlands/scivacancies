using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Aggregates.Interfaces;
using SciVacancies.ReadModel;
using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SciVacancies.WebApp.Controllers
{
    public class InitializeController : Controller
    {
        private readonly IResearcherService _res;
        private readonly IOrganizationService _org;
        private readonly IReadModelService _rm;

        public InitializeController(IResearcherService res,IOrganizationService org,IReadModelService rm)
        {
            _res = res;
            _org = org;
            _rm = rm;
        }
        // GET: /<controller>/
        public void Index()
        {
            Guid resGuid = _res.CreateResearcher(new ResearcherDataModel()
            {
                Login = "Cartman",
                FirstName = "Генрих",
                SecondName = "Пупкин",
                Patronymic = "Дубощит",
                FirstNameEng = "Genrih",
                SecondNameEng = "Pupkin",
                PatronymicEng = "Duboit",
                BirthDate = DateTime.Now
            });

            Guid subGuid = _res.CreateSearchSubscription(resGuid, new SearchSubscriptionDataModel()
            {
                Title = "Разведение лазерных акул"
            });

            Guid orgGuid = _org.CreateOrganization(new OrganizationDataModel()
            {
                Name="Корпорация Umbrella",
                ShortName="Ubmrella",
                OrgFormId=1,
                FoivId=42,
                ActivityId=1,
                HeadFirstName="Овидий",
                HeadLastName="Грек",
                HeadPatronymic="Иванович"
            });

            Guid posGuid1 = _org.CreatePosition(orgGuid, new PositionDataModel()
            {
                Name="Разводчик акул",
                FullName="Младший сотрудник по разведению лазерных акул",
                PositionTypeGuid=Guid.Parse("b7280ace-d237-c007-42fe-ec4aed8f52d4"),
                ResearchDirection= "Аналитическая химия",
                ResearchDirectionId= 3026
            });
            Guid posGuid2 = _org.CreatePosition(orgGuid, new PositionDataModel()
            {
                Name = "Настройщик лазеров",
                FullName = "Младший сотрудник по настройке лазеров",
                PositionTypeGuid = Guid.Parse("b7280ace-d237-c007-42fe-ec4aed8f52d4"),
                ResearchDirection = "Прикладная математика",
                ResearchDirectionId = 2999
            });
            Guid vacGuid1 = _org.PublishVacancy(orgGuid, posGuid1, new VacancyDataModel()
            {
                Name = "Разводчик акул",
                FullName = "Младший сотрудник по разведению лазерных акул",
                ResearchDirection = "Аналитическая химия",
            });


            Guid orgGuid1 = _org.CreateOrganization(new OrganizationDataModel()
            {
                Name = "НИИ добра",
                ShortName = "Good Science",
                OrgFormId = 2,
                FoivId = 42,
                ActivityId = 1,
                HeadFirstName = "Саруман",
                HeadLastName = "Саур",
                HeadPatronymic = "Сауронович"
            });

            Guid posGuid3 = _org.CreatePosition(orgGuid, new PositionDataModel()
            {
                Name = "Ремонтни всевидящего ока",
                FullName = "Младший сотрудник по калибровке фокусного зеркала",
                PositionTypeGuid = Guid.Parse("b7280ace-d237-c007-42fe-ec4aed8f52d4"),
                ResearchDirection = "Аналитическая химия",
                ResearchDirectionId = 3026
            });
            Guid vacGuid2 = _org.PublishVacancy(orgGuid1, posGuid2, new VacancyDataModel()
            {
                Name = "Ремонтни всевидящего ока",
                FullName = "Младший сотрудник по калибровке фокусного зеркала",
                ResearchDirection = "Аналитическая химия",
            });
        }
    }
}
