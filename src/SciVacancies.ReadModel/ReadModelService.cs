using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.ReadModel
{
    public class ReadModelService:IReadModelService
    {
        private readonly IDatabase _db;
        public ReadModelService(IDatabase db)
        {
            _db = db;
        }

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
    }
}
