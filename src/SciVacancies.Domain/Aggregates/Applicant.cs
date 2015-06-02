using SciVacancies.Domain.Core;
using SciVacancies.Domain.Events;

using System;
using System.Collections.Generic;

using CommonDomain.Core;

namespace SciVacancies.Domain.Aggregates
{
    public class Applicant : AggregateBase
    {
        private bool Deleted { get; set; }
        private List<Request> Requests { get; set; }

        public Applicant()
        {

        }
        public Applicant(Guid Id)
        {
            RaiseEvent(new ApplicantCreated()
            {
                ApplicantGuid = Guid.NewGuid()
            });
        }

        public void Remove()
        {
            if (!Deleted)
            {
                RaiseEvent(new ApplicantRemoved()
                {
                    ApplicantGuid = this.Id
                });
            }
        }
        public void CreateRequest(Guid competitionGuid)
        {
            RaiseEvent(new RequestCreated()
            {
                RequestGuid = Guid.NewGuid(),
                CompetitionGuid = competitionGuid
            });
        }
        public void SendRequest()
        {
            RaiseEvent(new RequestSent()
            {

            });
        }
        #region Apply-Handlers
        public void Apply(ApplicantCreated @event)
        {
            this.Id = @event.ApplicantGuid;
        }
        public void Apply(ApplicantRemoved @event)
        {
            this.Deleted = true;
        }
        public void Apply(RequestCreated @event)
        {
            this.Requests.Add(new Request()
            {
                RequestGuid = @event.RequestGuid,
                CompetitionGuid = @event.CompetitionGuid,
                Status=RequestStatus.InProcess
            });
        }

        public void Apply(RequestSent @event)
        {
     
        }
        #endregion
    }
}
