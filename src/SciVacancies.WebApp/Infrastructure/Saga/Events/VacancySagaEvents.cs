using System;

namespace SciVacancies.WebApp.Infrastructure.Saga
{
    public class VacancySagaCreated : SagaEventBase
    {
        public Guid SagaGuid { get; set; }
        public Guid VacancyGuid { get; set; }
        public Guid OrganizationGuid { get; set; }

        public DateTime PublishEndDate { get; set; }
    }
    public class VacancySagaSwitchedInCommittee : SagaEventBase
    {
        public DateTime InCommitteeEndDate { get; set; }
    }
    public class VacancySagaSwitchedInOfferAwaiting : SagaEventBase
    {
        public DateTime OfferResponseAwaitingEndDate { get; set; }
    }
    public class VacancySagaOfferRejected : SagaEventBase
    {

    }
    public class VacancySagaClosed : SagaEventBase
    {

    }
    public class VacancySagaCancelled : SagaEventBase
    {

    }
}