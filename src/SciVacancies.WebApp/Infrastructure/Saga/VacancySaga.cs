﻿using SciVacancies.Domain.Enums;

using System;
using MediatR;

namespace SciVacancies.WebApp.Infrastructure.Saga
{
    public class VacancySaga : SagaBase,
    INotificationHandler<VacancySagaCreated>,
    INotificationHandler<VacancySagaSwitchedInCommittee>,
    INotificationHandler<VacancySagaSwitchedInOfferAwaiting>,
    INotificationHandler<VacancySagaOfferRejected>
    {
        public Guid VacancyGuid { get; private set; }
        public Guid OrganizationGuid { get; private set; }

        public DateTime PublishStartDate { get; private set; }
        public DateTime PublishEndDate { get; private set; }

        public DateTime InCommitteeStartDate { get; private set; }
        public DateTime InCommitteeEndDate { get; private set; }
        public bool FirstInCommitteNotificationSent { get; private set; }

        public DateTime OfferResponseAwaitingStartDate { get; private set; }
        public DateTime OfferResponseAwaitingEndDate { get; private set; }
        public bool OfferResponseNotificationSent { get; private set; }

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
            this.PublishEndDate = msg.PublishEndDate;

            this.State = VacancyStatus.Published;
        }
        public void Handle(VacancySagaSwitchedInCommittee msg)
        {
            this.InCommitteeStartDate = msg.TimeStamp;
            this.InCommitteeEndDate = msg.InCommitteeEndDate;

            this.State = VacancyStatus.InCommittee;
        }
        public void Handle(VacancySagaSwitchedInOfferAwaiting msg)
        {
            this.OfferResponseAwaitingStartDate = msg.TimeStamp;
            this.OfferResponseAwaitingEndDate = msg.OfferResponseAwaitingEndDate;

            this.State = VacancyStatus.OfferResponseAwaiting;
        }
        public void Handle(VacancySagaOfferRejected msg)
        {

        }
    }
}
