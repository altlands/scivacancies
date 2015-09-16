using System;

namespace SciVacancies.WebApp.Infrastructure.Saga
{
    public class VacancySagaCreated : SagaEventBase
    {
        public Guid SagaGuid { get; set; }
        public Guid VacancyGuid { get; set; }
        public Guid OrganizationGuid { get; set; }

        public DateTime InCommitteeStartDate { get; set; }
        public DateTime InCommitteeEndDate { get; set; }
    }
    public class VacancySagaSwitchedInCommittee : SagaEventBase
    {
        public Guid SagaGuid { get; set; }
    }
    public class VacancySagaProlongedInCommittee : SagaEventBase
    {
        public Guid SagaGuid { get; set; }

        public DateTime InCommitteeEndDate { get; set; }
    }
    public class VacancySagaFirstInCommitteeNotificationSent : SagaEventBase
    {
        public Guid SagaGuid { get; set; }
    }
    public class VacancySagaSecondInCommitteeNotificationSent : SagaEventBase
    {
        public Guid SagaGuid { get; set; }
    }
    public class VacancySagaSwitchedInOfferResponseAwaitingFromWinner : SagaEventBase
    {
        public Guid SagaGuid { get; set; }

        public DateTime OfferResponseAwaitingFromWinnerEndDate { get; set; }
    }
    public class VacancySagaOfferResponseNotificationSentToWinner : SagaEventBase
    {
        public Guid SagaGuid { get; set; }
    }
    public class VacancySagaOfferAcceptedByWinner : SagaEventBase
    {
        public Guid SagaGuid { get; set; }
    }
    public class VacancySagaOfferRejectedByWinner : SagaEventBase
    {
        public Guid SagaGuid { get; set; }
    }
    public class VacancySagaSwitchedInOfferResponseAwaitingFromPretender : SagaEventBase
    {
        public Guid SagaGuid { get; set; }

        public DateTime OfferResponseAwaitingFromPretenderEndDate { get; set; }
    }
    public class VacancySagaOfferResponseNotificationSentToPretender : SagaEventBase
    {
        public Guid SagaGuid { get; set; }
    }
    public class VacancySagaOfferAcceptedByPretender : SagaEventBase
    {
        public Guid SagaGuid { get; set; }
    }
    public class VacancySagaOfferRejectedByPretender : SagaEventBase
    {
        public Guid SagaGuid { get; set; }
    }

    public class VacancySagaClosed : SagaEventBase
    {
        public Guid SagaGuid { get; set; }
    }
    public class VacancySagaCancelled : SagaEventBase
    {
        public Guid SagaGuid { get; set; }
    }
}