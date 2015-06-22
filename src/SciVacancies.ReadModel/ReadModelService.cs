using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.ReadModel
{
    public class ReadModelService : IReadModelService
    {
        private readonly IDatabase _db;
        public ReadModelService(IDatabase db)
        {
            _db = db;
        }

        public Researcher SingleResearcher(Guid researcherGuid)
        {
            if (researcherGuid == Guid.Empty)
            {
                throw new Exception($"researcherGuid is empty: {researcherGuid}");
            }

            Researcher researcher = _db.SingleOrDefaultById<Researcher>(researcherGuid);

            return researcher;
        }
        public List<Researcher> SelectResearchers()
        {
            List<Researcher> researchers = _db.Fetch<Researcher>();

            return researchers;
        }

        public VacancyApplication SingleVacancyApplication(Guid vacancyApplicationGuid)
        {
            if (vacancyApplicationGuid == Guid.Empty)
            {
                throw new Exception($"vacancyApplicationGuid is empty: {vacancyApplicationGuid}");
            }

            VacancyApplication vacancyApplication = _db.SingleOrDefaultById<VacancyApplication>(vacancyApplicationGuid);

            return vacancyApplication;
        }
        public List<VacancyApplication> SelectVacancyApplicationsByResearcher(Guid researcherGuid)
        {
            if(researcherGuid ==Guid.Empty)
            {
                throw new Exception($"researcherGuid is empty: {researcherGuid}");
            }

            List<VacancyApplication> vacancyApplications = _db.FetchBy<VacancyApplication>(f => f.Where(w => w.ResearcherGuid == researcherGuid));

            return vacancyApplications;
        }
        public List<VacancyApplication> SelectVacancyApplicationsByVacancy(Guid vacancyGuid)
        {
            if (vacancyGuid == Guid.Empty)
            {
                throw new Exception($"vacancyGuid is empty: {vacancyGuid}");
            }

            List<VacancyApplication> vacancyApplication = _db.FetchBy<VacancyApplication>(f => f.Where(w => w.VacancyGuid == vacancyGuid));

            return vacancyApplication;
        }

        public SearchSubscription SingleSearchSubscription(Guid searchSubscriptionGuid)
        {
            if (searchSubscriptionGuid == Guid.Empty)
            {
                throw new Exception($"searchSubscriptionGuid is empty: {searchSubscriptionGuid}");
            }

            SearchSubscription searchSubscription = _db.SingleById<SearchSubscription>(searchSubscriptionGuid);

            return searchSubscription;
        }
        public List<SearchSubscription> SelectSearchSubscriptions()
        {
            List<SearchSubscription> searchSubscriptions = _db.Fetch<SearchSubscription>();

            return searchSubscriptions;
        }
        public List<SearchSubscription> SelectSearchSubscriptions(Guid researcherGuid)
        {
            List<SearchSubscription> searchSubscriptions = _db.FetchBy<SearchSubscription>(f => f.Where(w => w.ResearcherGuid == researcherGuid));

            return searchSubscriptions;
        }

        public Attachment SingleAttachment(Guid attachmentGuid)
        {
            Attachment attachment = _db.SingleById<Attachment>(attachmentGuid);

            return attachment;
        }
        public List<Attachment> SelectAttachments(Guid vacancyApplicationGuid)
        {
            List<Attachment> attachments = _db.FetchBy<Attachment>(f => f.Where(w => w.VacancyApplicationGuid == vacancyApplicationGuid));

            return attachments;
        }

        public Vacancy SingleVacancy(Guid vacancyGuid)
        {
            Vacancy vacancy = _db.SingleById<Vacancy>(vacancyGuid);

            return vacancy;
        }
        public List<Vacancy> SelectVacancies(Guid organizationGuid)
        {
            List<Vacancy> vacancies = _db.FetchBy<Vacancy>(f => f.Where(w => w.OrganizationGuid == organizationGuid));

            return vacancies;
        }
        public List<Vacancy> SelectVacancies(string title, int count)
        {

            List<Vacancy> vacancies;
            if (count != 0)
            {
                vacancies = _db.FetchBy<Vacancy>(f => f.Where(w => w.Name.Contains(title))).Take(count).ToList();
            }
            else
            {
                vacancies = _db.FetchBy<Vacancy>(f => f.Where(w => w.Name.Contains(title)));
            }

            return vacancies;
        }
        public Page<Vacancy> SelectVacancies(string orderBy, long pageSize, long pageIndex, string nameFilterValue = null, string addressFilterValue = null)
        {
            //TODO - Complete this method by filter and proper ordering
            if (pageSize < 1) throw new Exception($"PageSize too small: {pageSize}");
            if (pageIndex < 1) throw new Exception($"PageIndex too small: {pageIndex}");

            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "Guid_descending";

            Page<Vacancy> vacancies = _db.Page<Vacancy>(pageIndex, pageSize, new Sql("SELECT o.* FROM \"Vacancies\" o ORDER BY o.\"Guid\" DESC"));

            return vacancies;
        }

        public List<Vacancy> SelectClosedVacancies(Guid organizationGuid)
        {
            List<Vacancy> vacancies = _db.FetchBy<Vacancy>(f => f.Where(w => w.OrganizationGuid == organizationGuid && w.Status == VacancyStatus.Closed));

            return vacancies;
        }
        public Page<Vacancy> SelectClosedVacancies(string orderBy, long pageSize, long pageIndex, string nameFilterValue = null, string addressFilterValue = null)
        {
            if (pageSize < 1)
                throw new Exception($"PageSize too small: {pageSize}");
            if (pageIndex < 1)
                throw new Exception($"PageIndex too small: {pageIndex}");

            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "Guid_descending";

            Page<Vacancy> vacancies = _db.Page<Vacancy>(pageIndex, pageSize, new Sql("SELECT o.* FROM \"Vacancies\" o ORDER BY o.\"Guid\" DESC"));

            return vacancies;
        }

        public List<Vacancy> SelectFavoriteVacancies(Guid researcherGuid)
        {
            List<Guid> guids = _db.FetchBy<FavoriteVacancy>(f => f.Where(w => w.ResearcherGuid == researcherGuid)).Select(s => s.VacancyGuid).ToList();
            if (guids.Any())
            {
                List<Vacancy> vacancies = _db.FetchBy<Vacancy>(f => f.Where(w => guids.Contains(w.Guid)));

                return vacancies;
            }
            else
            {
                return null;
            }
        }

        public Notification SingleNotification(Guid notificationGuid)
        {
            Notification notification = _db.SingleById<Notification>(notificationGuid);

            return notification;
        }
        public List<Notification> SelectNotificationsByResearcher(Guid researcherGuid)
        {
            List<Notification> notifications = _db.FetchBy<Notification>(f => f.Where(w => w.ResearcherGuid == researcherGuid));

            return notifications;
        }
        public List<Notification> SelectNotificationsByOrganization(Guid organizationGuid)
        {
            List<Notification> notifications = _db.FetchBy<Notification>(f => f.Where(w => w.OrganizationGuid == organizationGuid));

            return notifications;
        }
        public int CountNotificationsByResearcher(Guid researcherGuid)
        {
            int counter = _db.FetchBy<Notification>(f => f.Where(w => w.ResearcherGuid == researcherGuid)).Count();

            return counter;
        }
        public int CountNotificationsByOrganization(Guid organizationGuid)
        {
            int counter = _db.FetchBy<Notification>(f => f.Where(w => w.OrganizationGuid == organizationGuid)).Count();

            return counter;
        }

        public Organization SingleOrganization(Guid organizationGuid)
        {
            Organization organization = _db.SingleById<Organization>(organizationGuid);

            return organization;
        }
        public List<Organization> SelectOrganizations()
        {
            List<Organization> organizations = _db.Fetch<Organization>();

            return organizations;
        }
        public Page<Organization> SelectOrganizations(string orderBy, long pageSize, long pageIndex, string nameFilterValue, string addressFilterValue)
        {
            //TODO - Complete this method by filter and proper ordering
            if (pageSize < 1) throw new Exception($"PageSize too small: {pageSize}");
            if (pageIndex < 1) throw new Exception($"PageIndex too small: {pageIndex}");

            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "Guid_descending";

            Page<Organization> organizations = _db.Page<Organization>(pageIndex, pageSize, new Sql("SELECT o.* FROM \"Organizations\" o ORDER BY o.\"Guid\" DESC"));

            return organizations;
        }
        public List<Organization> SelectOrganizations(string title, int count)
        {
            List<Organization> organizations;
            if (count != 0)
            {
                organizations = _db.FetchBy<Organization>(f => f.Where(w => w.Name.Contains(title))).Take(count).ToList();
            }
            else
            {
                organizations = _db.FetchBy<Organization>(f => f.Where(w => w.Name.Contains(title)));
            }

            return organizations;
        }

        public Position SinglePosition(Guid positionGuid)
        {
            Position position = _db.SingleById<Position>(positionGuid);

            return position;
        }
        public List<Position> SelectPositions(Guid organizationGuid)
        {
            List<Position> positions = _db.FetchBy<Position>(f => f.Where(w => w.OrganizationGuid == organizationGuid));

            return positions;
        }

        public List<PositionType> SelectPositionTypes()
        {
            List<PositionType> positionTypes = _db.Fetch<PositionType>();

            return positionTypes;
        }
        public List<PositionType> SelectPositionTypes(string query)
        {
            List<PositionType> positionTypes = _db.FetchBy<PositionType>(f => f.Where(w => w.Title.Contains(query)));

            return positionTypes;
        }

        public List<Activity> SelectActivities()
        {
            List<Activity> activities = _db.Fetch<Activity>();

            return activities;
        }
        public List<Activity> SelectActivities(string query)
        {
            List<Activity> activities = _db.FetchBy<Activity>(f => f.Where(w => w.Title.Contains(query)));

            return activities;
        }
        public List<Foiv> SelectFoivs()
        {
            List<Foiv> foivs = _db.Fetch<Foiv>();

            return foivs;
        }
        public List<Foiv> SelectFoivs(string query)
        {
            List<Foiv> foivs = _db.FetchBy<Foiv>(f => f.Where(w => w.Title.Contains(query)));

            return foivs;
        }
        public List<Foiv> SelectFoivs(int parentId)
        {
            List<Foiv> foivs = _db.FetchBy<Foiv>(f => f.Where(w => w.ParentId == parentId));

            return foivs;
        }
        public List<Criteria> SelectCriterias()
        {
            List<Criteria> criterias = _db.Fetch<Criteria>();

            return criterias;
        }
        public List<Criteria> SelectCriterias(string query)
        {
            List<Criteria> criterias = _db.FetchBy<Criteria>(f => f.Where(w => w.Title.Contains(query)));

            return criterias;
        }
        public List<Criteria> SelectCriterias(int parentId)
        {
            List<Criteria> criterias = _db.FetchBy<Criteria>(f => f.Where(w => w.ParentId == parentId));

            return criterias;
        }
        public List<OrgForm> SelectOrgForms()
        {
            List<OrgForm> orgForms = _db.Fetch<OrgForm>();

            return orgForms;
        }
        public List<OrgForm> SelectOrgForms(string query)
        {
            List<OrgForm> orgForms = _db.FetchBy<OrgForm>(f => f.Where(w => w.Title.Contains(query)));

            return orgForms;
        }
        public List<Region> SelectRegions()
        {
            List<Region> regions = _db.Fetch<Region>();

            return regions;
        }
        public List<Region> SelectRegions(string query)
        {
            List<Region> regions = _db.FetchBy<Region>(f => f.Where(w => w.Title.Contains(query)));

            return regions;
        }
        public List<ResearchDirection> SelectResearchDirections()
        {
            List<ResearchDirection> researchDirections = _db.Fetch<ResearchDirection>();

            return researchDirections;
        }
        public List<ResearchDirection> SelectResearchDirections(string query)
        {
            List<ResearchDirection> researchDirections = _db.FetchBy<ResearchDirection>(f => f.Where(w => w.Title.Contains(query)));

            return researchDirections;
        }
        public List<ResearchDirection> SelectResearchDirections(int parentId)
        {
            List<ResearchDirection> researchDirections = _db.FetchBy<ResearchDirection>(f => f.Where(w => w.ParentId == parentId));

            return researchDirections;
        }
    }
}
