using System;

namespace SciVacancies.Domain.Events
{
    public class VacancyEventBase : EventBase
    {
        public Guid VacancyId { get; set; }
        public Guid OrganizationId { get; set; }
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
}
