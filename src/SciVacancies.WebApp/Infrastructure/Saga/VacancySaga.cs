using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;

using System;
using MediatR;

namespace SciVacancies.WebApp.Infrastructure.Saga
{
    public class VacancySaga : SagaBase,

    //TODO переделать. Должны быть CommandHandlers чтобы во время Transition команда шла дальше (за одну транзакцию менялась сага и вакансия)
    INotificationHandler<VacancySagaCreated>,
    INotificationHandler<VacancySagaSwitchedInCommittee>,
    INotificationHandler<VacancySagaProlongedInCommittee>,
    INotificationHandler<VacancySagaFirstInCommitteeNotificationSent>,
    INotificationHandler<VacancySagaSecondInCommitteeNotificationSent>,

    INotificationHandler<VacancySagaWinnerSet>,
    INotificationHandler<VacancySagaPretenderSet>,

    INotificationHandler<VacancySagaSwitchedInOfferResponseAwaitingFromWinner>,
    INotificationHandler<VacancySagaOfferResponseNotificationSentToWinner>,
    INotificationHandler<VacancySagaOfferAcceptedByWinner>,
    INotificationHandler<VacancySagaOfferRejectedByWinner>,

    INotificationHandler<VacancySagaSwitchedInOfferResponseAwaitingFromPretender>,
    INotificationHandler<VacancySagaOfferResponseNotificationSentToPretender>,
    INotificationHandler<VacancySagaOfferAcceptedByPretender>,
    INotificationHandler<VacancySagaOfferRejectedByPretender>,

    INotificationHandler<VacancySagaCancelled>,
    INotificationHandler<VacancySagaClosed>
    {
        public Guid VacancyGuid { get; private set; }
        public Guid OrganizationGuid { get; private set; }

        public DateTime PublishStartDate { get; private set; }
        public DateTime PublishEndDate { get; private set; }

        public DateTime InCommitteeStartDate { get; private set; }
        public DateTime InCommitteeEndDate { get; private set; }
        public bool FirstInCommitteeNotificationSent { get; private set; }
        public bool SecondInCommitteeNotificationSent { get; private set; }

        public Guid WinnerResearcherGuid { get; private set; }
        public Guid WinnerVacancyApplicationGuid { get; private set; }

        public DateTime OfferResponseAwaitingFromWinnerStartDate { get; private set; }
        public DateTime OfferResponseAwaitingFromWinnerEndDate { get; private set; }
        public bool OfferResponseNotificationSentToWinner { get; private set; }

        public Guid PretenderResearcherGuid { get; private set; }
        public Guid PretenderVacancyApplicationGuid { get; private set; }

        public DateTime OfferResponseAwaitingFromPretenderStartDate { get; private set; }
        public DateTime OfferResponseAwaitingFromPretenderEndDate { get; private set; }
        public bool OfferResponseNotificationSentToPretender { get; private set; }

        public DateTime CancelDate { get; private set; }
        public DateTime CloseDate { get; private set; }

        public VacancyStatus State { get; private set; }

        public VacancySaga()
        { }
        public VacancySaga(Guid id) : base(id) { }

        public void Handle(VacancySagaCreated msg)
        {
            VacancyGuid = msg.VacancyGuid;
            OrganizationGuid = msg.OrganizationGuid;

            PublishStartDate = msg.TimeStamp;
            PublishEndDate = msg.InCommitteeStartDate;

            InCommitteeStartDate = msg.InCommitteeStartDate;
            InCommitteeEndDate = msg.InCommitteeEndDate;

            State = VacancyStatus.Published;
        }
        public void Handle(VacancySagaSwitchedInCommittee msg)
        {
            State = VacancyStatus.InCommittee;
        }
        public void Handle(VacancySagaProlongedInCommittee msg)
        {
            InCommitteeEndDate = msg.InCommitteeEndDate;
        }
        public void Handle(VacancySagaWinnerSet msg)
        {
            WinnerResearcherGuid = msg.WinnerReasearcherGuid;
            WinnerVacancyApplicationGuid = msg.WinnerVacancyApplicationGuid;
        }
        public void Handle(VacancySagaPretenderSet msg)
        {
            PretenderResearcherGuid = msg.PretenderReasearcherGuid;
            PretenderVacancyApplicationGuid = msg.PretenderVacancyApplicationGuid;
        }
        public void Handle(VacancySagaFirstInCommitteeNotificationSent msg)
        {
            FirstInCommitteeNotificationSent = true;
        }
        public void Handle(VacancySagaSecondInCommitteeNotificationSent msg)
        {
            SecondInCommitteeNotificationSent = true;
        }
        public void Handle(VacancySagaSwitchedInOfferResponseAwaitingFromWinner msg)
        {
            OfferResponseAwaitingFromWinnerStartDate = msg.TimeStamp;
            OfferResponseAwaitingFromWinnerEndDate = msg.OfferResponseAwaitingFromWinnerEndDate; //this.OfferResponseAwaitingFromWinnerStartDate.AddDays(30);
            State = VacancyStatus.OfferResponseAwaitingFromWinner;
        }
        public void Handle(VacancySagaOfferResponseNotificationSentToWinner msg)
        {
            OfferResponseNotificationSentToWinner = true;
        }
        public void Handle(VacancySagaOfferAcceptedByWinner msg)
        {
            State = VacancyStatus.OfferAcceptedByWinner;
        }
        public void Handle(VacancySagaOfferRejectedByWinner msg)
        {
            State = VacancyStatus.OfferRejectedByWinner;
        }
        public void Handle(VacancySagaSwitchedInOfferResponseAwaitingFromPretender msg)
        {
            OfferResponseAwaitingFromPretenderStartDate = msg.TimeStamp;
            OfferResponseAwaitingFromPretenderEndDate = msg.OfferResponseAwaitingFromPretenderEndDate; //this.OfferResponseAwaitingFromPretenderStartDate.AddDays(30);
            State = VacancyStatus.OfferResponseAwaitingFromPretender;
        }
        public void Handle(VacancySagaOfferResponseNotificationSentToPretender msg)
        {
            OfferResponseNotificationSentToPretender = true;
        }
        public void Handle(VacancySagaOfferAcceptedByPretender msg)
        {
            State = VacancyStatus.OfferAcceptedByPretender;
        }
        public void Handle(VacancySagaOfferRejectedByPretender msg)
        {
            State = VacancyStatus.OfferRejectedByPretender;
        }
        public void Handle(VacancySagaCancelled msg)
        {
            CancelDate = msg.TimeStamp;
            State = VacancyStatus.Cancelled;
        }
        public void Handle(VacancySagaClosed msg)
        {
            CloseDate = msg.TimeStamp;
            State = VacancyStatus.Closed;
        }
    }
}
