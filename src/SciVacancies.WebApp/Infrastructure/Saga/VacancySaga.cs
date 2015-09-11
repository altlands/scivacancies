using SciVacancies.Domain.Enums;

using System;
using MediatR;
//using CommonDomain.Core;

namespace SciVacancies.WebApp.Infrastructure.Saga
{
    public class VacancySaga : SagaBase,
    INotificationHandler<VacancySagaCreated>,
    INotificationHandler<VacancySagaSwitchedInCommittee>
    {
        public Guid OrganizationGuid { get; set; }
        public Guid VacancyGuid { get; set; }

        public DateTime PublishStartDate { get; set; }
        public DateTime PublishEndDate { get; set; }
        public DateTime InCommitteeStartDate { get; set; }
        public DateTime InCommitteeEndDate { get; set; }
        public DateTime OfferResponseStartDate { get; set; }
        public DateTime OfferResponseEndDate { get; set; }
        public DateTime ClosedDate { get; set; }

        public VacancyStatus State { get; set; }

        public VacancySaga() : base() { }
        public VacancySaga(Guid id) : base(id) { }

        public void Handle(VacancySagaCreated msg)
        {
            //this.Id = msg.SagaGuid.ToString();
            this.OrganizationGuid = msg.SagaGuid;
            this.VacancyGuid = Guid.NewGuid();
            this.State = VacancyStatus.InCommittee;
            var test = 0;
        }
        public void Handle(VacancySagaSwitchedInCommittee msg)
        {
            var t = 0;
            this.State = VacancyStatus.OfferRejected;
        }
    }
}
