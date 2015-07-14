using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;

using System;

using CommonDomain.Core;

namespace SciVacancies.Domain.Aggregates
{
    public class Vacancy : AggregateBase
    {
        private Guid OrganizationGuid { get; set; }

        private VacancyDataModel Data { get; set; }

        private VacancyStatus Status { get; set; }

        public Guid WinnerResearcherGuid { get; set; }
        public Guid WinnerVacancyApplicationGuid { get; set; }
        private bool? IsWinnerAccept { get; set; }

        public Guid PretenderResearcherGuid { get; set; }
        public Guid PretenderVacancyApplicationGuid { get; set; }
        private bool? IsPretenderAccept { get; set; }

        private string CancelReason { get; set; }

        public Vacancy()
        {

        }
        public Vacancy(Guid guid, Guid organizationGuid, VacancyDataModel data)
        {
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
            if (Status == VacancyStatus.InProcess)
            {
                RaiseEvent(new VacancyUpdated
                {
                    VacancyGuid = this.Id,
                    OrganizationGuid = this.OrganizationGuid,
                    Data = data
                });
            }
        }
        public void Remove()
        {
            if (Status == VacancyStatus.InProcess)
            {
                RaiseEvent(new VacancyRemoved
                {
                    VacancyGuid = this.Id,
                    OrganizationGuid = this.OrganizationGuid
                });
            }
        }
        public void Publish()
        {
            if (Status == VacancyStatus.InProcess)
            {
                RaiseEvent(new VacancyPublished
                {
                    VacancyGuid = this.Id,
                    OrganizationGuid = this.OrganizationGuid
                });
            }
        }
        public void VacancyToCommittee()
        {
            if (Status == VacancyStatus.Published)
            {
                RaiseEvent(new VacancyInCommittee
                {
                    VacancyGuid = this.Id,
                    OrganizationGuid = this.OrganizationGuid
                });
            }
        }
        public void SetWinner(Guid winnerGuid, Guid winnerVacancyApplicationGuid, string reason)
        {
            if (Status == VacancyStatus.InCommittee)
            {
                RaiseEvent(new VacancyWinnerSet
                {
                    VacancyGuid = this.Id,
                    OrganizationGuid = this.OrganizationGuid,
                    WinnerReasearcherGuid = winnerGuid,
                    WinnerVacancyApplicationGuid = winnerVacancyApplicationGuid,
                    Reason = reason
                });
            }
        }
        public void SetPretender(Guid pretenderGuid, Guid pretenderVacancyApplicationGuid, string reason)
        {
            if (Status == VacancyStatus.InCommittee)
            {
                RaiseEvent(new VacancyPretenderSet
                {
                    VacancyGuid = this.Id,
                    OrganizationGuid = this.OrganizationGuid,
                    PretenderReasearcherGuid = pretenderGuid,
                    PretenderVacancyApplicationGuid = pretenderVacancyApplicationGuid,
                    Reason = reason
                });
            }
        }
        public void WinnerAcceptOffer()
        {
            if (Status == VacancyStatus.OfferResponseAwaiting
                && WinnerResearcherGuid != Guid.Empty
                && WinnerVacancyApplicationGuid != Guid.Empty
                && PretenderResearcherGuid != Guid.Empty
                && PretenderVacancyApplicationGuid != Guid.Empty)
            {
                RaiseEvent(new VacancyOfferAcceptedByWinner
                {
                    VacancyGuid = this.Id,
                    OrganizationGuid = this.OrganizationGuid
                });
            }
        }
        public void WinnerRejectOffer()
        {
            if (Status == VacancyStatus.OfferResponseAwaiting
                && WinnerResearcherGuid != Guid.Empty
                && WinnerVacancyApplicationGuid != Guid.Empty
                && PretenderResearcherGuid != Guid.Empty
                && PretenderVacancyApplicationGuid != Guid.Empty)
            {
                RaiseEvent(new VacancyOfferRejectedByWinner
                {
                    VacancyGuid = this.Id,
                    OrganizationGuid = this.OrganizationGuid
                });
            }
        }
        public void PretenderAcceptOffer()
        {
            if (Status == VacancyStatus.OfferResponseAwaiting
                && WinnerResearcherGuid != Guid.Empty
                && WinnerVacancyApplicationGuid != Guid.Empty
                && PretenderResearcherGuid != Guid.Empty
                && PretenderVacancyApplicationGuid != Guid.Empty
                && IsWinnerAccept == false)
            {
                RaiseEvent(new VacancyOfferAcceptedByPretender
                {
                    VacancyGuid = this.Id,
                    OrganizationGuid = this.OrganizationGuid
                });
            }
        }
        public void PretenderRejectOffer()
        {
            if (Status == VacancyStatus.OfferResponseAwaiting
                && WinnerResearcherGuid != Guid.Empty
                && WinnerVacancyApplicationGuid != Guid.Empty
                && PretenderResearcherGuid != Guid.Empty
                && PretenderVacancyApplicationGuid != Guid.Empty
                && IsWinnerAccept == false)
            {
                RaiseEvent(new VacancyOfferRejectedByPretender
                {
                    VacancyGuid = this.Id,
                    OrganizationGuid = this.OrganizationGuid
                });
            }
        }
        public void Close()
        {
            if (Status == VacancyStatus.OfferAccepted
                && WinnerResearcherGuid != Guid.Empty
                && WinnerVacancyApplicationGuid != Guid.Empty
                && PretenderResearcherGuid != Guid.Empty
                && PretenderVacancyApplicationGuid != Guid.Empty
                && (IsWinnerAccept == true || IsPretenderAccept == true))
            {
                RaiseEvent(new VacancyClosed
                {
                    VacancyGuid = this.Id,
                    OrganizationGuid = this.OrganizationGuid,
                    WinnerResearcherGuid = this.WinnerResearcherGuid,
                    WinnerVacancyApplicationGuid = this.WinnerVacancyApplicationGuid,
                    IsWinnerAccept = this.IsWinnerAccept.Value,
                    PretenderResearcherGuid = this.PretenderResearcherGuid,
                    PretenderVacancyApplicationGuid = this.PretenderVacancyApplicationGuid,
                    IsPretenderAccept = this.IsPretenderAccept.Value
                });
            }
        }
        public void Cancel(string reason)
        {
            if (Status == VacancyStatus.Published || Status == VacancyStatus.InCommittee || Status == VacancyStatus.OfferRejected)
            {
                RaiseEvent(new VacancyCancelled
                {
                    VacancyGuid = this.Id,
                    OrganizationGuid = this.OrganizationGuid,
                    Reason = reason
                });
            }
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
        }
        public void Apply(VacancyPretenderSet @event)
        {
            this.PretenderResearcherGuid = @event.PretenderReasearcherGuid;
            this.PretenderVacancyApplicationGuid = @event.PretenderVacancyApplicationGuid;
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
