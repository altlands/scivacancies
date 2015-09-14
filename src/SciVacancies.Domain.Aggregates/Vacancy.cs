using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;

using System;
using System.Collections.Generic;
using CommonDomain.Core;
using SciVacancies.Domain.Core;

namespace SciVacancies.Domain.Aggregates
{
    public class Vacancy : AggregateBase
    {
        public Guid OrganizationGuid { get; private set; }

        private VacancyDataModel Data { get; set; }

        public VacancyStatus Status { get; private set; }

        public Guid WinnerResearcherGuid { get; private set; }
        public Guid WinnerVacancyApplicationGuid { get; private set; }
        public bool? IsWinnerAccept { get; private set; }
        /// <summary>
        /// Обоснование выбора этой заявки в качестве победителя
        /// </summary>
        public string WinnerReason { get; private set; }

        public Guid PretenderResearcherGuid { get; private set; }
        public Guid PretenderVacancyApplicationGuid { get; private set; }
        public bool? IsPretenderAccept { get; private set; }
        /// <summary>
        /// Обоснование выбора этой заявки в качестве претендента
        /// </summary>
        public string PretenderReason { get; private set; }

        public string CancelReason { get; private set; }

        public Vacancy()
        {

        }
        public Vacancy(Guid guid, Guid organizationGuid, VacancyDataModel data)
        {
            if (guid.Equals(Guid.Empty)) throw new ArgumentNullException("guid is empty");
            if (organizationGuid.Equals(Guid.Empty)) throw new ArgumentNullException("organizationGuid is empty");
            if (data == null) throw new ArgumentNullException("data is empty");

            RaiseEvent(new VacancyCreated
            {
                VacancyGuid = guid,
                OrganizationGuid = organizationGuid,
                Data = data
            });
        }

        #region Methods

        public void Update(VacancyDataModel data)
        {
            if (data == null) throw new ArgumentNullException("data is empty");
            if (Status != VacancyStatus.InProcess) throw new InvalidOperationException("vacancy state is invalid");

            RaiseEvent(new VacancyUpdated
            {
                VacancyGuid = this.Id,
                OrganizationGuid = this.OrganizationGuid,
                Data = data
            });
        }
        public void Remove()
        {
            if (Status != VacancyStatus.InProcess) throw new InvalidOperationException("vacancy state is invalid");

            RaiseEvent(new VacancyRemoved
            {
                VacancyGuid = this.Id,
                OrganizationGuid = this.OrganizationGuid
            });
        }
        public void Publish()
        {
            if (Status != VacancyStatus.InProcess) throw new InvalidOperationException("vacancy state is invalid");

            RaiseEvent(new VacancyPublished
            {
                VacancyGuid = this.Id,
                OrganizationGuid = this.OrganizationGuid
            });
        }
        public void VacancyToCommittee()
        {
            if (Status != VacancyStatus.Published) throw new InvalidOperationException("vacancy state is invalid");

            RaiseEvent(new VacancyInCommittee
            {
                VacancyGuid = this.Id,
                OrganizationGuid = this.OrganizationGuid
            });
        }
        public void SetWinner(Guid winnerGuid, Guid winnerVacancyApplicationGuid, string reason, List<VacancyAttachment> attachments)
        {
            if (winnerGuid.Equals(Guid.Empty)) throw new ArgumentNullException("winnerGuid is empty");
            if (winnerVacancyApplicationGuid.Equals(Guid.Empty)) throw new ArgumentNullException("winnerVacancyApplicationGuid is empty");
            if (String.IsNullOrEmpty(reason)) throw new ArgumentNullException("reason is empty");
            if (attachments == null || attachments.Count == 0) throw new ArgumentNullException("attachments is null or empty");
            if (Status != VacancyStatus.InCommittee) throw new InvalidOperationException("vacancy state is invalid");

            RaiseEvent(new VacancyWinnerSet
            {
                VacancyGuid = this.Id,
                OrganizationGuid = this.OrganizationGuid,
                WinnerReasearcherGuid = winnerGuid,
                WinnerVacancyApplicationGuid = winnerVacancyApplicationGuid,
                Reason = reason,
                Attachments = attachments
            });
        }
        public void SetPretender(Guid pretenderGuid, Guid pretenderVacancyApplicationGuid, string reason)
        {
            if (pretenderGuid.Equals(Guid.Empty)) throw new ArgumentNullException("pretenderGuid is empty");
            if (pretenderVacancyApplicationGuid.Equals(Guid.Empty)) throw new ArgumentNullException("pretenderVacancyApplicationGuid is empty");
            if (String.IsNullOrEmpty(reason)) throw new ArgumentNullException("reason is empty");
            if (Status != VacancyStatus.InCommittee) throw new InvalidOperationException("vacancy state is invalid");

            RaiseEvent(new VacancyPretenderSet
            {
                VacancyGuid = this.Id,
                OrganizationGuid = this.OrganizationGuid,
                PretenderReasearcherGuid = pretenderGuid,
                PretenderVacancyApplicationGuid = pretenderVacancyApplicationGuid,
                Reason = reason
            });
        }
        public void VacancyToResponseAwaitingFromWinner()
        {
            if (Status != VacancyStatus.InCommittee) throw new InvalidOperationException("vacancy state is invalid");
            if (WinnerResearcherGuid == Guid.Empty || WinnerVacancyApplicationGuid == Guid.Empty) throw new ArgumentNullException("WinnerGuid or WinnerVacancyApplicationGuid is empty");

            RaiseEvent(new VacancyInOfferResponseAwaitingFromWinner
            {
                VacancyGuid = this.Id,
                OrganizationGuid = this.OrganizationGuid
            });
        }
        public void WinnerAcceptOffer()
        {
            if (this.WinnerResearcherGuid.Equals(Guid.Empty)) throw new ArgumentNullException("WinnerResearcherGuid is empty");
            if (this.WinnerVacancyApplicationGuid.Equals(Guid.Empty)) throw new ArgumentNullException("WinnerVacancyApplicationGuid is empty");
            if (Status != VacancyStatus.OfferResponseAwaitingFromWinner) throw new InvalidOperationException("vacancy state is invalid");

            RaiseEvent(new VacancyOfferAcceptedByWinner
            {
                VacancyGuid = this.Id,
                OrganizationGuid = this.OrganizationGuid
            });
        }
        public void WinnerRejectOffer()
        {
            if (this.WinnerResearcherGuid.Equals(Guid.Empty)) throw new ArgumentNullException("WinnerResearcherGuid is empty");
            if (this.WinnerVacancyApplicationGuid.Equals(Guid.Empty)) throw new ArgumentNullException("WinnerVacancyApplicationGuid is empty");
            if (Status != VacancyStatus.OfferResponseAwaitingFromWinner) throw new InvalidOperationException("vacancy state is invalid");

            RaiseEvent(new VacancyOfferRejectedByWinner
            {
                VacancyGuid = this.Id,
                OrganizationGuid = this.OrganizationGuid
            });
        }
        public void VacancyToResponseAwaitingFromPretender()
        {
            if (Status != VacancyStatus.OfferRejectedByWinner) throw new InvalidOperationException("vacancy state is invalid");
            if (PretenderResearcherGuid == Guid.Empty || PretenderVacancyApplicationGuid == Guid.Empty) throw new ArgumentNullException("WinnerGuid or WinnerVacancyApplicationGuid is empty");

            RaiseEvent(new VacancyInOfferResponseAwaitingFromPretender
            {
                VacancyGuid = this.Id,
                OrganizationGuid = this.OrganizationGuid
            });
        }
        public void PretenderAcceptOffer()
        {
            if (this.PretenderResearcherGuid.Equals(Guid.Empty)) throw new ArgumentNullException("PretenderResearcherGuid is empty");
            if (this.PretenderVacancyApplicationGuid.Equals(Guid.Empty)) throw new ArgumentNullException("PretenderVacancyApplicationGuid is empty");
            if (!(this.IsWinnerAccept.HasValue && !this.IsWinnerAccept.Value)) throw new InvalidOperationException("IsWinnerAccept is invalid");
            if (Status != VacancyStatus.OfferResponseAwaitingFromPretender) throw new InvalidOperationException("vacancy state is invalid");

            RaiseEvent(new VacancyOfferAcceptedByPretender
            {
                VacancyGuid = this.Id,
                OrganizationGuid = this.OrganizationGuid
            });
        }
        public void PretenderRejectOffer()
        {
            if (this.PretenderResearcherGuid.Equals(Guid.Empty)) throw new ArgumentNullException("PretenderResearcherGuid is empty");
            if (this.PretenderVacancyApplicationGuid.Equals(Guid.Empty)) throw new ArgumentNullException("PretenderVacancyApplicationGuid is empty");
            if (!(this.IsWinnerAccept.HasValue && !this.IsWinnerAccept.Value)) throw new InvalidOperationException("IsWinnerAccept is invalid");
            if (Status != VacancyStatus.OfferResponseAwaitingFromPretender) throw new InvalidOperationException("vacancy state is invalid");

            RaiseEvent(new VacancyOfferRejectedByPretender
            {
                VacancyGuid = this.Id,
                OrganizationGuid = this.OrganizationGuid
            });
        }
        public void Close()
        {
            //if (!(Status == VacancyStatus.OfferAcceptedByWinner||Status==VacancyStatus.of)) throw new InvalidOperationException("vacancy state is invalid");

            RaiseEvent(new VacancyClosed
            {
                VacancyGuid = this.Id,
                OrganizationGuid = this.OrganizationGuid,
                WinnerResearcherGuid = this.WinnerResearcherGuid,
                WinnerVacancyApplicationGuid = this.WinnerVacancyApplicationGuid,
                IsWinnerAccept = this.IsWinnerAccept.Value,
                PretenderResearcherGuid = this.PretenderResearcherGuid,
                PretenderVacancyApplicationGuid = this.PretenderVacancyApplicationGuid,
                IsPretenderAccept = this.IsPretenderAccept
            });
        }
        public void Cancel(string reason)
        {
            if (!(Status == VacancyStatus.Published
                || Status == VacancyStatus.InCommittee
                || Status == VacancyStatus.OfferAccepted
                || Status == VacancyStatus.OfferRejected))
                throw new InvalidOperationException("vacancy state is invalid");

            RaiseEvent(new VacancyCancelled
            {
                VacancyGuid = this.Id,
                OrganizationGuid = this.OrganizationGuid,
                Reason = reason
            });
        }

        #endregion

        #region Apply-Handlers

        public void Apply(VacancyCreated @event)
        {
            this.Id = @event.VacancyGuid;
            this.OrganizationGuid = @event.OrganizationGuid;
            this.Data = @event.Data;
        }
        public void Apply(VacancyUpdated @event)
        {
            this.Data = @event.Data;
        }
        public void Apply(VacancyRemoved @event)
        {
            this.Status = VacancyStatus.Removed;
        }
        public void Apply(VacancyPublished @event)
        {
            this.Status = VacancyStatus.Published;
        }
        public void Apply(VacancyInCommittee @event)
        {
            this.Status = VacancyStatus.InCommittee;
        }
        public void Apply(VacancyWinnerSet @event)
        {
            this.WinnerResearcherGuid = @event.WinnerReasearcherGuid;
            this.WinnerVacancyApplicationGuid = @event.WinnerVacancyApplicationGuid;

            this.Data.Attachments.AddRange(@event.Attachments);
        }
        public void Apply(VacancyPretenderSet @event)
        {
            this.PretenderResearcherGuid = @event.PretenderReasearcherGuid;
            this.PretenderVacancyApplicationGuid = @event.PretenderVacancyApplicationGuid;
        }
        public void Apply(VacancyInAwaitingOfferResponse @event)
        {
            this.Status = VacancyStatus.OfferResponseAwaiting;
        }
        public void Apply(VacancyOfferAcceptedByWinner @event)
        {
            this.IsWinnerAccept = true;
            this.Status = VacancyStatus.OfferAccepted;
        }
        public void Apply(VacancyOfferRejectedByWinner @event)
        {
            this.IsWinnerAccept = false;
            this.Status = VacancyStatus.OfferRejected;
        }
        public void Apply(VacancyOfferAcceptedByPretender @event)
        {
            this.IsPretenderAccept = true;
            this.Status = VacancyStatus.OfferAccepted;
        }
        public void Apply(VacancyOfferRejectedByPretender @event)
        {
            this.IsPretenderAccept = false;
            this.Status = VacancyStatus.OfferRejected;
        }
        public void Apply(VacancyClosed @event)
        {
            this.Status = VacancyStatus.Closed;
        }
        public void Apply(VacancyCancelled @event)
        {
            this.CancelReason = @event.Reason;
            this.Status = VacancyStatus.Cancelled;
        }

        #endregion
    }
}
