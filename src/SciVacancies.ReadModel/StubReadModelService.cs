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

        public List<VacancyApplication> SelectApplicationsToVacancy(Guid vacancyGuid)
        {
            var result = new List<VacancyApplication>();
            return result;
        }

        public List<Activity> SelectActivities(Guid organizationGuid)
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

        public List<Foiv> SelectFoivs(Guid organizationGuid)
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
                    Title ="Министерство культуры Российской Федерации

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

        public List<Criteria> SelectCriterias(Guid organizationGuid)
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

        public List<OrgForm> SelectOrgForms(Guid organizationGuid)
        {
            var result = new List<OrgForm>() {

            };
            return result;
        }

        public List<Region> SelectRegions(Guid organizationGuid)
        {
            var result = new List<Region>();
            return result;
        }

        public List<ResearchDirection> SelectResearchDirections(Guid organizationGuid)
        {
            var result = new List<ResearchDirection>();
            return result;
        }
    }
}
