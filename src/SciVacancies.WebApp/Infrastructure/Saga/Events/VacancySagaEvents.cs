using System;

namespace SciVacancies.WebApp.Infrastructure.Saga
{
    public class VacancySagaCreated : SagaEventBase
    {
        public Guid SagaGuid { get; set; }
        public Guid VacancyGuid { get; set; }
        public Guid OrganizationGuid { get; set; }
    }
    public class VacancySagaSwitchedInCommittee : SagaEventBase
    {
    }
    public class VacancySagaSwitchedInOfferAwaiting : SagaEventBase
    {
    }
}