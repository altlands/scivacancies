using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NPoco;

namespace SciVacancies.ReadModel
{
    public class StubReadModelService : IReadModelService
    {
        public Researcher SingleResearcher(Guid researcherGuid)
        {
            var result = new Researcher();

            return result;
        }
        public List<Researcher> SelectResearchers()
        {
            var result = new List<Researcher>();

            return result;
        }

        public VacancyApplication SingleVacancyApplication(Guid vacancyApplicationGuid)
        {
            var result = new VacancyApplication();

            return result;
        }
        public List<VacancyApplication> SelectVacancyApplicationsByResearcher(Guid researcherGuid)
        {
            var result = new List<VacancyApplication>();

            return result;
        }
        public List<VacancyApplication> SelectVacancyApplicationsByVacancy(Guid vacancyGuid)
        {
            var result = new List<VacancyApplication>();

            return result;
        }

        public SearchSubscription SingleSearchSubscription(Guid searchSubscriptionGuid)
        {
            var result = new SearchSubscription();

            return result;
        }
        public List<SearchSubscription> SelectSearchSubscriptions()
        {
            var result = new List<SearchSubscription>();

            return result;
        }
        public List<SearchSubscription> SelectSearchSubscriptions(Guid researcherGuid)
        {
            var result = new List<SearchSubscription>();

            return result;
        }

        public Attachment SingleAttachment(Guid attachmentGuid)
        {
            var result = new Attachment();

            return result;
        }
        public List<Attachment> SelectAttachments(Guid vacancyApplicationGuid)
        {
            var result = new List<Attachment>();

            return result;
        }

        public Vacancy SingleVacancy(Guid vacancyGuid)
        {
            var result = new Vacancy();

            return result;
        }
        public List<Vacancy> SelectVacancies(Guid organizationGuid)
        {
            var result = new List<Vacancy>();

            return result;
        }
        public List<Vacancy> SelectVacancies(string title, int count)
        {
            var result = new List<Vacancy>();

            return result;
        }
        public Page<Vacancy> SelectVacancies(string orderBy, long pageSize, long pageIndex, string nameFilterValue = null, string addressFilterValue = null)
        {
            if (pageSize < 1)
                throw new Exception($"PageSize too small: {pageSize}");
            if (pageIndex < 1)
                throw new Exception($"PageIndex too small: {pageIndex}");

            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "Guid_descending";

            var data = new List<Vacancy>
            {
                new Vacancy
                {
                    Guid = Guid.NewGuid(),
                    Name = "первая вакансия",
                },
                new Vacancy
                {
                    Guid = Guid.NewGuid(),
                    Name = "Вторая вакансия",
                },
                new Vacancy
                {
                    Guid = Guid.NewGuid(),
                    Name = "Третья вакансия",
                },
                new Vacancy
                {
                    Guid = Guid.NewGuid(),
                    Name = "Четвёртая вакансия",
                }
            };

            var result = new Page<Vacancy>
            {
                Items = data.Take((int)pageSize).ToList(),
                CurrentPage = pageIndex,
                ItemsPerPage = pageSize,
                TotalItems = data.Count,
                TotalPages = data.Count / pageSize + ((data.Count % pageSize) > 0 ? 1 : 0)
            };

            return result;
        }

        public List<Vacancy> SelectClosedVacancies(Guid organizationGuid)
        {
            var result = new List<Vacancy>();

            return result;
        }
        public Page<Vacancy> SelectClosedVacancies(string orderBy, long pageSize, long pageIndex, string nameFilterValue = null, string addressFilterValue = null)
        {
            if (pageSize < 1)
                throw new Exception($"PageSize too small: {pageSize}");
            if (pageIndex < 1)
                throw new Exception($"PageIndex too small: {pageIndex}");

            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "Guid_descending";

            var data = new List<Vacancy>
            {
                new Vacancy
                {
                    Guid = Guid.NewGuid(),
                    Name = "первая вакансия",
                },
                new Vacancy
                {
                    Guid = Guid.NewGuid(),
                    Name = "Вторая вакансия",
                },
                new Vacancy
                {
                    Guid = Guid.NewGuid(),
                    Name = "Третья вакансия",
                },
                new Vacancy
                {
                    Guid = Guid.NewGuid(),
                    Name = "Четвёртая вакансия",
                }
            };

            var result = new Page<Vacancy>
            {
                Items = data.Take((int)pageSize).ToList(),
                CurrentPage = pageIndex,
                ItemsPerPage = pageSize,
                TotalItems = data.Count,
                TotalPages = data.Count / pageSize + ((data.Count % pageSize) > 0 ? 1 : 0)
            };

            return result;
        }


        public List<Vacancy> SelectFavoriteVacancies(Guid researcherGuid)
        {
            var result = new List<Vacancy>();

            return result;
        }

        public Notification SingleNotification(Guid notificationGuid)
        {
            var result = new Notification();

            return result;
        }
        public List<Notification> SelectNotificationsByResearcher(Guid researcherGuid)
        {
            var result = new List<Notification>();

            return result;
        }
        public List<Notification> SelectNotificationsByOrganization(Guid organizationGuid)
        {
            var result = new List<Notification>();

            return result;
        }
        public int CountNotificationsByResearcher(Guid researcherGuid)
        {
            return 10;
        }
        public int CountNotificationsByOrganization(Guid organizationGuid)
        {
            return 121;
        }

        public Organization SingleOrganization(Guid orgnizationGuid)
        {
            var result = new Organization();

            return result;
        }
        public List<Organization> SelectOrganizations()
        {
            var result = new List<Organization>();

            return result;
        }

        public Page<Organization> SelectOrganizations(string orderBy, long pageSize, long pageIndex, string nameFilterValue,
            string addressFilterValue)
        {
            if (pageSize < 1)
                throw new Exception($"PageSize too small: {pageSize}");
            if (pageIndex < 1)
                throw new Exception($"PageIndex too small: {pageIndex}");

            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "Guid_descending";

            var data = new List<Organization>
            {
                new Organization
                {
                    Guid = Guid.NewGuid(),
                    Name = "Московский государственный университет им. М.В. Ломоносова",
                    NameEng = "Moscow State University",
                    ShortName = "МГУ им.М.В.Ломоносова",
                    ShortNameEng = "MSU"
                },
                new Organization
                {
                    Guid = Guid.NewGuid(),
                    Name = "Московский государственный университет им. М.В. Ломоносова",
                    NameEng = "Moscow State University",
                    ShortName = "МГУ им.М.В.Ломоносова",
                    ShortNameEng = "MSU"
                },
                new Organization
                {
                    Guid = Guid.NewGuid(),
                    Name = "Московский государственный университет им. М.В. Ломоносова",
                    NameEng = "Moscow State University",
                    ShortName = "МГУ им.М.В.Ломоносова",
                    ShortNameEng = "MSU"
                },
                new Organization
                {
                    Guid = Guid.NewGuid(),
                    Name = "Московский государственный университет им. М.В. Ломоносова",
                    NameEng = "Moscow State University",
                    ShortName = "МГУ им.М.В.Ломоносова",
                    ShortNameEng = "MSU"
                }
            };

            var result = new Page<Organization>
            {
                Items = data.Take((int)pageSize).ToList(),
                CurrentPage = pageIndex,
                ItemsPerPage = pageSize,
                TotalItems = data.Count,
                TotalPages = data.Count / pageSize + ((data.Count % pageSize) > 0 ? 1 : 0)
            };

            return result;
        }
        public List<Organization> SelectOrganizations(string title, int count)
        {
            var result = SelectOrganizations("desc", 2, 4, "", "").Items;

            return result;
        }

        public Position SinglePosition(Guid positionGuid)
        {
            var result = new Position();

            return result;
        }
        public List<Position> SelectPositions(Guid organizationGuid)
        {
            var result = new List<Position>();

            return result;
        }
        [Obsolete("Метод будет удалён, использовать SelectVacancyApplicationsByVacancy(Guid vacancyGuid)")]
        public List<VacancyApplication> SelectApplicationsToVacancy(Guid vacancyGuid)
        {
            var result = new List<VacancyApplication>();

            return result;
        }

        public List<PositionType> SelectPositionTypes()
        {
            var result = new List<PositionType>
            {
                new PositionType()
                {
                    Guid=Guid.NewGuid(),
                    Id=1,
                    Title="Младший научный сотрудник"
                },
                new PositionType()
                {
                    Guid=Guid.NewGuid(),
                    Id=2,
                    Title="Научный сотрудник"
                },
                new PositionType()
                {
                    Guid=Guid.NewGuid(),
                    Id=3,
                    Title="Старший научный сотрудник"
                },
                new PositionType()
                {
                    Guid=Guid.NewGuid(),
                    Id=4,
                    Title="Професор зло"
                },
                new PositionType()
                {
                    Guid=Guid.NewGuid(),
                    Id=5,
                    Title="Заведующий лабораторей ужаса"
                }
            };

            return result;
        }
        public List<PositionType> SelectPositionTypes(string query)
        {
            var result = SelectPositionTypes();

            return result;
        }

        public List<Activity> SelectActivities()
        {
            var result = new List<Activity>() {
                new Activity()
                {
                    Guid=Guid.NewGuid(),
                    Id=1,
                    Title="Государственный сектор"
                },
                new Activity()
                {
                    Guid=Guid.NewGuid(),
                    Id=16,
                    Title="Федеральные университеты"
                }
            };

            return result;
        }
        public List<Activity> SelectActivities(string query)
        {
            var result = SelectActivities();

            return result;
        }

        public List<Foiv> SelectFoivs()
        {
            var result = new List<Foiv>()
            {
                 new Foiv()
                {
                    Guid=Guid.NewGuid(),
                    Id =7,
                    ParentId=null,
                    Title ="Министерство обороны Российской Федерации"
                }
                 ,
                new Foiv()
                {
                    Guid=Guid.NewGuid(),
                    Id =11,
                    ParentId=7,
                    Title ="Федеральное агентство специального строительства"
                }
                ,
                 new Foiv()
                {
                    Guid=Guid.NewGuid(),
                    Id =24,
                    ParentId=null,
                    Title ="Министерство культуры Российской Федерации"

                }
                ,
                new Foiv()
                {
                    Guid=Guid.NewGuid(),
                    Id =26,
                    ParentId=24,
                    Title ="Федеральное агентство по туризму"
                }
            };

            return result;
        }
        public List<Foiv> SelectFoivs(string query)
        {
            var result = SelectFoivs();

            return result;
        }
        public List<Foiv> SelectFoivs(int parentId)
        {
            var result = SelectFoivs();

            return result;
        }

        public List<Criteria> SelectCriterias()
        {
            var result = new List<Criteria>() {
                new Criteria()
                {
                    Guid=Guid.NewGuid(),
                    Title="Web of science"
                }
                ,
                new Criteria()
                {
                    Guid=Guid.NewGuid(),
                    Title="Scopus"
                }
                ,
                new Criteria()
                {
                    Guid=Guid.NewGuid(),
                    Title="Российский индекс научного цитирования"
                }
            };

            return result;
        }
        public List<Criteria> SelectCriterias(string query)
        {
            var result = SelectCriterias();

            return result;
        }
        public List<Criteria> SelectCriterias(int parentId)
        {
            var result = SelectCriterias();

            return result;
        }

        public List<OrgForm> SelectOrgForms()
        {
            var result = new List<OrgForm>() {
                new OrgForm()
                {
                    Guid=Guid.NewGuid(),
                    Id =12,
                    Title="Автономная некоммерческая организация"
                }
                ,
                new OrgForm()
                {
                    Guid=Guid.NewGuid(),
                    Id =12,
                    Title="Автономная некоммерческая организация"
                }
            };

            return result;
        }
        public List<OrgForm> SelectOrgForms(string query)
        {
            var result = SelectOrgForms();

            return result;
        }

        public List<Region> SelectRegions()
        {
            var result = new List<Region>() {

                new Region()
                {
                    Guid=Guid.NewGuid(),
                    Id=77,
                    Title="Москва",
                    Code=77,
                    FedDistrictId=1,
                    OsmId=102269,
                    Okato="45000000",
                    Slug="moscow"
                }
                ,
                new Region()
                {
                    Guid=Guid.NewGuid(),
                    Id=78,
                    Title="Санкт-Петербург",
                    Code=78,
                    FedDistrictId=2,
                    OsmId=337422,
                    Okato="40000000",
                    Slug="s-petersburg"
                }

            };

            return result;
        }
        public List<Region> SelectRegions(string query)
        {
            var result = SelectRegions();

            return result;
        }

        public List<ResearchDirection> SelectResearchDirections()
        {
            var result = new List<ResearchDirection>()
            {
                new ResearchDirection()
                {
                    Guid=Guid.NewGuid(),
                    Id=2850,
                    Title="Социальные науки",
                    ParentId=2780,
                    Lft=160,
                    Rgt=269,
                    Root=2780,
                    Lvl=1
                }
                ,
                new ResearchDirection()
                {
                    Guid=Guid.NewGuid(),
                    Id=2862,
                    Title="Прочие социальные науки",
                    ParentId=2850,
                    Lft=183,
                    Rgt=192,
                    Root=2780,
                    Lvl=2
                }
                ,
                new ResearchDirection()
                {
                    Guid=Guid.NewGuid(),
                    Id=2865,
                    Title="Культурология",
                    ParentId=2862,
                    Lft=188,
                    Rgt=189,
                    Root=2780,
                    Lvl=3
                }
            };

            return result;
        }
        public List<ResearchDirection> SelectResearchDirections(string query)
        {
            var result = SelectResearchDirections();

            return result;
        }
        public List<ResearchDirection> SelectResearchDirections(int parentId)
        {
            var result = SelectResearchDirections();

            return result;
        }
    }
}
