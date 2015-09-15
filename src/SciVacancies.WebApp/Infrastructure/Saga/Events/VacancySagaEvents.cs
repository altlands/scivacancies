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
    public class VacancySagaFirstInCommitteeNotificationSent : SagaEventBase
    {

    }
    public class VacancySagaSecondInCommitteeNotificationSent : SagaEventBase
    {

    }
    public class VacancySagaSwitchedInOfferResponseAwaitingFromWinner : SagaEventBase
    {
        public DateTime OfferResponseAwaitingFromWinnerEndDate { get; set; }
    }
    public class VacancySagaOfferResponseNotificationSentToWinner : SagaEventBase
    {

    }
    public class VacancySagaOfferRejectByWinner : SagaEventBase
    {

    }
    public class VacancySagaSwitchedInOfferResponseAwaitingFromPretender : SagaEventBase
    {
        public DateTime OfferResponseAwaitingFromPretenderEndDate { get; set; }
    }
    public class VacancySagaOfferResponseNotificationSentToPretender : SagaEventBase
    {

    }
    public class VacancySagaOfferRejectByPretender : SagaEventBase
    {

    }

    public class VacancySagaClosed : SagaEventBase
    {

    }
    public class VacancySagaCancelled : SagaEventBase
    {

    }
}