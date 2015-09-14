using SciVacancies.Domain.Enums;

using System;
using MediatR;

namespace SciVacancies.WebApp.Infrastructure.Saga
{
    public class VacancySaga : SagaBase,
    INotificationHandler<VacancySagaCreated>,
    INotificationHandler<VacancySagaSwitchedInCommittee>,
    INotificationHandler<VacancySagaSwitchedInOfferAwaiting>
    {
        public Guid VacancyGuid { get; private set; }
        public Guid OrganizationGuid { get; private set; }

        public DateTime PublishDate { get; private set; }
        public DateTime InCommitteeDate { get; private set; }
        public DateTime OfferResponseAwaitingDate { get; private set; }

        public DateTime CancelDate { get; private set; }
        public DateTime CloseDate { get; private set; }

        public VacancyStatus State { get; private set; }

        public VacancySaga() : base() { }
        public VacancySaga(Guid id) : base(id) { }

        public void Handle(VacancySagaCreated msg)
        {
            this.VacancyGuid = Guid.NewGuid();
            this.OrganizationGuid = msg.SagaGuid;

            this.PublishDate = msg.TimeStamp;

            this.State = VacancyStatus.Published;
        }
        public void Handle(VacancySagaSwitchedInCommittee msg)
        {
            this.State = VacancyStatus.InCommittee;
        }
        public void Handle(VacancySagaSwitchedInOfferAwaiting msg)
        {
            this.State = VacancyStatus.OfferResponseAwaiting;
        }
    }
}
