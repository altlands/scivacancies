using System;

namespace SciVacancies.Domain.Events
{
    public class VacancyEventBase : EventBase
    {
        public VacancyEventBase() : base() { }

        public Guid VacancyGuid { get; set; }
        public Guid OrganizationGuid { get; set; }
    }

    public class VacancyCreated : VacancyEventBase
    {

    }

    public class VacancyRemoved : VacancyEventBase
    {

    }
    public class VacancyUpdated : VacancyEventBase
    {

    }

    public class VacancyAddedToFavorites : VacancyEventBase
    {
        public VacancyAddedToFavorites() : base() { }

        public Guid ResearcherGuid { get; set; }
    }
    public class VacancyRemovedFromFavorites : VacancyEventBase
    {
        public VacancyRemovedFromFavorites() : base() { }

        public Guid ResearcherGuid { get; set; }
    }
}
