using System;

namespace SciVacancies.Domain.Events
{
    public class VacancySagaEventBase : EventBase
    {
        public Guid SagaGuid { get; set; }
        public Guid VacancyGuid { get; set; }
        public Guid OrganizationGuid { get; set; }
    }

    public class VacancySagaCreated : VacancySagaEventBase
    {
        public DateTime InCommitteeStartDate { get; set; }
        public DateTime InCommitteeEndDate { get; set; }
    }
    public class VacancySagaSwitchedInCommittee : VacancySagaEventBase
    {
    }
    public class VacancySagaProlongedInCommittee : VacancySagaEventBase
    {
        public DateTime InCommitteeEndDate { get; set; }
    }
    public class VacancySagaFirstInCommitteeNotificationSent : VacancySagaEventBase
    {
    }
    public class VacancySagaSecondInCommitteeNotificationSent : VacancySagaEventBase
    {
    }
    public class VacancySagaWinnerSet : VacancySagaEventBase
    {
        public Guid WinnerReasearcherGuid { get; set; }
        public Guid WinnerVacancyApplicationGuid { get; set; }
    }
    public class VacancySagaPretenderSet : VacancySagaEventBase
    {
        public Guid PretenderReasearcherGuid { get; set; }
        public Guid PretenderVacancyApplicationGuid { get; set; }
    }
    public class VacancySagaSwitchedInOfferResponseAwaitingFromWinner : VacancySagaEventBase
    {
        public DateTime OfferResponseAwaitingFromWinnerEndDate { get; set; }
    }
    public class VacancySagaOfferResponseNotificationSentToWinner : VacancySagaEventBase
    {
        public Guid WinnerReasearcherGuid { get; set; }
        public Guid WinnerVacancyApplicationGuid { get; set; }
    }
    public class VacancySagaOfferAcceptedByWinner : VacancySagaEventBase
    {
    }
    public class VacancySagaOfferRejectedByWinner : VacancySagaEventBase
    {
    }
    public class VacancySagaSwitchedInOfferResponseAwaitingFromPretender : VacancySagaEventBase
    {
        public DateTime OfferResponseAwaitingFromPretenderEndDate { get; set; }
    }
    public class VacancySagaOfferResponseNotificationSentToPretender : VacancySagaEventBase
    {
        public Guid PretenderReasearcherGuid { get; set; }
        public Guid PretenderVacancyApplicationGuid { get; set; }
    }
    public class VacancySagaOfferAcceptedByPretender : VacancySagaEventBase
    {
    }
    public class VacancySagaOfferRejectedByPretender : VacancySagaEventBase
    {
    }
    public class VacancySagaClosed : VacancySagaEventBase
    {
    }
    public class VacancySagaCancelled : VacancySagaEventBase
    {
    }
}