using System;

namespace SciVacancies.Domain.Events
{
    public class VacancySagaCreated : EventBase
    {
        public Guid SagaGuid { get; set; }
    }
    public class VacancySagaSwitchedInCommittee : EventBase
    {
        public Guid SagaGuid { get; set; }
    }
    public class VacancySagaSwitchedInOfferResponseAwaiting:EventBase
    {
        public Guid SagaGuid { get; set; }
    }
    //TODO
    public class VacancySagaTimeout : EventBase
    {
        public Guid SagaGuid { get; set; }
    }
}
