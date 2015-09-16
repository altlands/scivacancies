using SciVacancies.Domain.Enums;

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

        public DateTime OfferResponseAwaitingFromWinnerStartDate { get; private set; }
        public DateTime OfferResponseAwaitingFromWinnerEndDate { get; private set; }
        public bool OfferResponseNotificationSentToWinner { get; private set; }

        public DateTime OfferResponseAwaitingFromPretenderStartDate { get; private set; }
        public DateTime OfferResponseAwaitingFromPretenderEndDate { get; private set; }
        public bool OfferResponseNotificationSentToPretender { get; private set; }

        public DateTime CancelDate { get; private set; }
        public DateTime CloseDate { get; private set; }

        public VacancyStatus State { get; private set; }

        public VacancySaga() : base() { }
        public VacancySaga(Guid id) : base(id) { }

        public void Handle(VacancySagaCreated msg)
        {
            this.VacancyGuid = Guid.NewGuid();
            this.OrganizationGuid = msg.SagaGuid;

            this.PublishStartDate = msg.TimeStamp;
            this.PublishEndDate = msg.InCommitteeStartDate;

            this.InCommitteeStartDate = msg.InCommitteeStartDate;
            this.InCommitteeEndDate = msg.InCommitteeEndDate;

            this.State = VacancyStatus.Published;
        }
        public void Handle(VacancySagaSwitchedInCommittee msg)
        {
            this.State = VacancyStatus.InCommittee;
        }
        public void Handle(VacancySagaProlongedInCommittee msg)
        {
            this.InCommitteeEndDate = msg.InCommitteeEndDate;
        }
        public void Handle(VacancySagaFirstInCommitteeNotificationSent msg)
        {
            this.FirstInCommitteeNotificationSent = true;
        }
        public void Handle(VacancySagaSecondInCommitteeNotificationSent msg)
        {
            this.SecondInCommitteeNotificationSent = true;
        }
        public void Handle(VacancySagaSwitchedInOfferResponseAwaitingFromWinner msg)
        {
            this.OfferResponseAwaitingFromWinnerStartDate = msg.TimeStamp;
            this.OfferResponseAwaitingFromWinnerEndDate = this.OfferResponseAwaitingFromWinnerStartDate.AddDays(30);
            this.State = VacancyStatus.OfferResponseAwaitingFromWinner;
        }
        public void Handle(VacancySagaOfferResponseNotificationSentToWinner msg)
        {
            this.OfferResponseNotificationSentToWinner = true;
        }
        public void Handle(VacancySagaOfferAcceptedByWinner msg)
        {
            this.State = VacancyStatus.OfferAcceptedByWinner;
        }
        public void Handle(VacancySagaOfferRejectedByWinner msg)
        {
            this.State = VacancyStatus.OfferRejectedByWinner;
        }
        public void Handle(VacancySagaSwitchedInOfferResponseAwaitingFromPretender msg)
        {
            this.OfferResponseAwaitingFromPretenderStartDate = msg.TimeStamp;
            this.OfferResponseAwaitingFromPretenderEndDate = this.OfferResponseAwaitingFromPretenderStartDate.AddDays(30);
            this.State = VacancyStatus.OfferResponseAwaitingFromPretender;
        }
        public void Handle(VacancySagaOfferResponseNotificationSentToPretender msg)
        {
            this.OfferResponseNotificationSentToPretender = true;
        }
        public void Handle(VacancySagaOfferAcceptedByPretender msg)
        {
            this.State = VacancyStatus.OfferAcceptedByPretender;
        }
        public void Handle(VacancySagaOfferRejectedByPretender msg)
        {
            this.State = VacancyStatus.OfferRejectedByPretender;
        }
        public void Handle(VacancySagaCancelled msg)
        {
            this.CancelDate = msg.TimeStamp;
            this.State = VacancyStatus.Cancelled;
        }
        public void Handle(VacancySagaClosed msg)
        {
            this.CloseDate = msg.TimeStamp;
            this.State = VacancyStatus.Closed;
        }
    }
}
