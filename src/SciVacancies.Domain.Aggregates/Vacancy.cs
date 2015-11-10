using CommonDomain.Core;
using SciVacancies.Domain.Core;
using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;
using System;
using System.Collections.Generic;

namespace SciVacancies.Domain.Aggregates
{
    public class Vacancy : AggregateBase
    {
        public Guid OrganizationGuid { get; private set; }

        private VacancyDataModel Data { get; set; }

        public VacancyStatus Status { get; private set; }

        /// <summary>
        /// Дато окончания приёма заявок и начала работы комиссии
        /// </summary>
        public DateTime InCommitteeStartDate { get; private set; }
        /// <summary>
        /// Дата окончания работы комиссии (далее даётся Х дней на оглашение результатов, статус при этом остаётся "на комиссии".
        /// Дата может сдвигаться соответсующей командой
        /// </summary>
        public DateTime InCommitteeEndDate { get; private set; }

        /// <summary>
        /// Причина, почему продлили комиссию
        /// </summary>
        public string ProlongingInCommitteeReason { get; private set; }
        /// <summary>
        /// Заключение комиссии (помимо этого ещё файл "протокол комиссии" в attachments)
        /// </summary>
        public string CommitteeResolution { get; private set; }

        public Guid WinnerResearcherGuid { get; private set; }
        public Guid WinnerVacancyApplicationGuid { get; private set; }
        /// <summary>
        /// Если null, значит победитель ещё не принял решение или ему не отсылалось уведомление
        /// </summary>
        public bool? IsWinnerAccept { get; private set; }
        /// <summary>
        /// Обоснование выбора этой заявки в качестве победителя
        /// </summary>


        public Guid PretenderResearcherGuid { get; private set; }
        public Guid PretenderVacancyApplicationGuid { get; private set; }
        /// <summary>
        /// Если null, значит претендент ещё не принял решение или ему не отсылалось уведомление
        /// </summary>
        public bool? IsPretenderAccept { get; private set; }

        public string CancelReason { get; private set; }

        public Vacancy()
        {

        }
        public Vacancy(Guid guid, Guid organizationGuid, VacancyDataModel data)
        {
            if (guid.Equals(Guid.Empty)) throw new ArgumentNullException(nameof(guid));
            if (organizationGuid.Equals(Guid.Empty)) throw new ArgumentNullException(nameof(organizationGuid));
            if (data == null) throw new ArgumentNullException(nameof(data));

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
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (Status != VacancyStatus.InProcess) throw new InvalidOperationException("vacancy state is invalid");

            RaiseEvent(new VacancyUpdated
            {
                VacancyGuid = Id,
                OrganizationGuid = OrganizationGuid,
                Data = data
            });
        }
        public void Remove()
        {
            //if (Status != VacancyStatus.InProcess) throw new InvalidOperationException("vacancy state is invalid");

            RaiseEvent(new VacancyRemoved
            {
                VacancyGuid = Id,
                OrganizationGuid = OrganizationGuid
            });
        }
        public void Publish(DateTime inCommitteeStartDate, DateTime inCommitteeEndDate)
        {
            if (inCommitteeStartDate > inCommitteeEndDate) throw new ArgumentException("inCommitteeStartDate is bigger than inCommitteeEndDate");
            if (Status != VacancyStatus.InProcess) throw new InvalidOperationException("vacancy state is invalid");

            RaiseEvent(new VacancyPublished
            {
                VacancyGuid = Id,
                OrganizationGuid = OrganizationGuid,
                InCommitteeStartDate = inCommitteeStartDate,
                InCommitteeEndDate = inCommitteeEndDate
            });
        }
        public void VacancyToCommittee()
        {
            if (Status != VacancyStatus.Published) throw new InvalidOperationException("vacancy state is invalid");

            RaiseEvent(new VacancyInCommittee
            {
                VacancyGuid = Id,
                OrganizationGuid = OrganizationGuid
            });
        }
        public void ProlongInCommittee(string reason, DateTime inCommitteeEndDate)
        {
            if (string.IsNullOrEmpty(reason)) throw new ArgumentNullException(nameof(reason));
            if (InCommitteeEndDate >= inCommitteeEndDate) throw new ArgumentException("invalid inCommitteeEndDate");
            if (Status != VacancyStatus.InCommittee) throw new InvalidOperationException("vacancy state is invalid");

            RaiseEvent(new VacancyProlongedInCommittee
            {
                VacancyGuid = Id,
                OrganizationGuid = OrganizationGuid,
                Reason = reason,
                InCommitteeEndDate = inCommitteeEndDate
            });
        }
        public void SetCommitteeResolution(string resolution, List<VacancyAttachment> attachments)
        {
            if (string.IsNullOrEmpty(resolution)) throw new ArgumentNullException(nameof(resolution));


            //todo: Vacancy -> SetCommitteeResolution -> нужно ли требовать 
            //по приказу не требуется, на данный момент, обязательно добавлять файл.
            //if (attachments == null || attachments.Count == 0) throw new ArgumentNullException("attachments is null or empty");

            if ( (attachments == null || attachments.Count == 0)
                && string.IsNullOrWhiteSpace(resolution))
                throw new ArgumentNullException($"{nameof(attachments)} or {nameof(resolution)} should be added");

            if (Status != VacancyStatus.InCommittee) throw new InvalidOperationException("vacancy state is invalid");

            RaiseEvent(new VacancyCommitteeResolutionSet
            {
                VacancyGuid = Id,
                OrganizationGuid = OrganizationGuid,
                Resolution = resolution,
                Attachments = attachments
            });
        }
        public void SetWinner(Guid winnerGuid, Guid winnerVacancyApplicationGuid)
        {
            if (winnerGuid.Equals(Guid.Empty)) throw new ArgumentNullException(nameof(winnerGuid));
            if (winnerVacancyApplicationGuid.Equals(Guid.Empty)) throw new ArgumentNullException(nameof(winnerVacancyApplicationGuid));
            if (Status != VacancyStatus.InCommittee) throw new InvalidOperationException("vacancy state is invalid");

            RaiseEvent(new VacancyWinnerSet
            {
                VacancyGuid = Id,
                OrganizationGuid = OrganizationGuid,
                WinnerReasearcherGuid = winnerGuid,
                WinnerVacancyApplicationGuid = winnerVacancyApplicationGuid
            });
        }
        public void SetPretender(Guid pretenderGuid, Guid pretenderVacancyApplicationGuid)
        {
            if (pretenderGuid.Equals(Guid.Empty)) throw new ArgumentNullException(nameof(pretenderGuid));
            if (pretenderVacancyApplicationGuid.Equals(Guid.Empty)) throw new ArgumentNullException(nameof(pretenderVacancyApplicationGuid));
            if (Status != VacancyStatus.InCommittee) throw new InvalidOperationException("vacancy state is invalid");

            RaiseEvent(new VacancyPretenderSet
            {
                VacancyGuid = Id,
                OrganizationGuid = OrganizationGuid,
                PretenderReasearcherGuid = pretenderGuid,
                PretenderVacancyApplicationGuid = pretenderVacancyApplicationGuid
            });
        }
        public void VacancyToResponseAwaitingFromWinner()
        {
            if (Status != VacancyStatus.InCommittee) throw new InvalidOperationException("vacancy state is invalid");
            if (WinnerResearcherGuid == Guid.Empty || WinnerVacancyApplicationGuid == Guid.Empty) throw new ArgumentNullException("WinnerGuid or WinnerVacancyApplicationGuid is empty");

            RaiseEvent(new VacancyInOfferResponseAwaitingFromWinner
            {
                VacancyGuid = Id,
                OrganizationGuid = OrganizationGuid
            });
        }
        public void WinnerAcceptOffer()
        {
            if (WinnerResearcherGuid.Equals(Guid.Empty)) throw new ArgumentNullException($"{nameof(WinnerResearcherGuid)} is empty");
            if (WinnerVacancyApplicationGuid.Equals(Guid.Empty)) throw new ArgumentNullException($"{nameof(WinnerVacancyApplicationGuid)} is empty");
            if (Status != VacancyStatus.OfferResponseAwaitingFromWinner) throw new InvalidOperationException("vacancy state is invalid");

            RaiseEvent(new VacancyOfferAcceptedByWinner
            {
                VacancyGuid = Id,
                OrganizationGuid = OrganizationGuid
            });
        }
        public void WinnerRejectOffer()
        {
            if (WinnerResearcherGuid.Equals(Guid.Empty)) throw new ArgumentNullException($"{nameof(WinnerResearcherGuid)} is empty");
            if (WinnerVacancyApplicationGuid.Equals(Guid.Empty)) throw new ArgumentNullException($"{nameof(WinnerVacancyApplicationGuid)} is empty");
            if (Status != VacancyStatus.OfferResponseAwaitingFromWinner) throw new InvalidOperationException("vacancy state is invalid");

            RaiseEvent(new VacancyOfferRejectedByWinner
            {
                VacancyGuid = Id,
                OrganizationGuid = OrganizationGuid
            });
        }
        public void VacancyToResponseAwaitingFromPretender()
        {
            if (!(Status==VacancyStatus.OfferAcceptedByWinner|| Status == VacancyStatus.OfferRejectedByWinner)) throw new InvalidOperationException("vacancy state is invalid");
            if (PretenderResearcherGuid == Guid.Empty || PretenderVacancyApplicationGuid == Guid.Empty) throw new ArgumentNullException("WinnerGuid or WinnerVacancyApplicationGuid is empty");

            RaiseEvent(new VacancyInOfferResponseAwaitingFromPretender
            {
                VacancyGuid = Id,
                OrganizationGuid = OrganizationGuid
            });
        }
        public void PretenderAcceptOffer()
        {
            if (PretenderResearcherGuid.Equals(Guid.Empty)) throw new ArgumentNullException("PretenderResearcherGuid is empty");
            if (PretenderVacancyApplicationGuid.Equals(Guid.Empty)) throw new ArgumentNullException("PretenderVacancyApplicationGuid is empty");
            if (!IsWinnerAccept.HasValue) throw new InvalidOperationException("IsWinnerAccept is invalid");
            if (Status != VacancyStatus.OfferResponseAwaitingFromPretender) throw new InvalidOperationException("vacancy state is invalid");

            RaiseEvent(new VacancyOfferAcceptedByPretender
            {
                VacancyGuid = Id,
                OrganizationGuid = OrganizationGuid
            });
        }
        public void PretenderRejectOffer()
        {
            if (PretenderResearcherGuid.Equals(Guid.Empty)) throw new ArgumentNullException("PretenderResearcherGuid is empty");
            if (PretenderVacancyApplicationGuid.Equals(Guid.Empty)) throw new ArgumentNullException("PretenderVacancyApplicationGuid is empty");
            if (!IsWinnerAccept.HasValue) throw new InvalidOperationException("IsWinnerAccept is invalid");
            if (Status != VacancyStatus.OfferResponseAwaitingFromPretender) throw new InvalidOperationException("vacancy state is invalid");

            RaiseEvent(new VacancyOfferRejectedByPretender
            {
                VacancyGuid = Id,
                OrganizationGuid = OrganizationGuid
            });
        }
        public void Close()
        {
            if (!(Status == VacancyStatus.OfferAcceptedByWinner || Status == VacancyStatus.OfferAcceptedByPretender)) throw new InvalidOperationException("vacancy state is invalid");

            RaiseEvent(new VacancyClosed
            {
                VacancyGuid = Id,
                OrganizationGuid = OrganizationGuid,
                WinnerResearcherGuid = WinnerResearcherGuid,
                WinnerVacancyApplicationGuid = WinnerVacancyApplicationGuid,
                IsWinnerAccept = IsWinnerAccept.Value,
                PretenderResearcherGuid = PretenderResearcherGuid,
                PretenderVacancyApplicationGuid = PretenderVacancyApplicationGuid,
                IsPretenderAccept = IsPretenderAccept
            });
        }
        public void Cancel(string reason)
        {
            if (string.IsNullOrEmpty(reason)) throw new ArgumentNullException(nameof(reason));
            if (!(Status == VacancyStatus.Published
                || Status == VacancyStatus.InCommittee
                || Status == VacancyStatus.OfferAcceptedByWinner
                || Status == VacancyStatus.OfferRejectedByWinner
                || Status == VacancyStatus.OfferAcceptedByPretender
                || Status == VacancyStatus.OfferRejectedByPretender))
                throw new InvalidOperationException("vacancy state is invalid");

            RaiseEvent(new VacancyCancelled
            {
                VacancyGuid = Id,
                OrganizationGuid = OrganizationGuid,
                Reason = reason
            });
        }

        #endregion

        #region Apply-Handlers

        public void Apply(VacancyCreated @event)
        {
            Id = @event.VacancyGuid;
            OrganizationGuid = @event.OrganizationGuid;
            Data = @event.Data;
        }
        public void Apply(VacancyUpdated @event)
        {
            Data = @event.Data;
        }
        public void Apply(VacancyRemoved @event)
        {
            Status = VacancyStatus.Removed;
        }
        public void Apply(VacancyPublished @event)
        {
            InCommitteeStartDate = @event.InCommitteeStartDate;
            InCommitteeEndDate = @event.InCommitteeEndDate;
            Status = VacancyStatus.Published;
        }
        public void Apply(VacancyInCommittee @event)
        {
            Status = VacancyStatus.InCommittee;
        }
        public void Apply(VacancyProlongedInCommittee @event)
        {
            ProlongingInCommitteeReason = @event.Reason;
            InCommitteeEndDate = @event.InCommitteeEndDate;
        }
        public void Apply(VacancyCommitteeResolutionSet @event)
        {
            CommitteeResolution = @event.Resolution;
            Data.Attachments.AddRange(@event.Attachments);
        }
        public void Apply(VacancyWinnerSet @event)
        {
            WinnerResearcherGuid = @event.WinnerReasearcherGuid;
            WinnerVacancyApplicationGuid = @event.WinnerVacancyApplicationGuid;
        }
        public void Apply(VacancyPretenderSet @event)
        {
            PretenderResearcherGuid = @event.PretenderReasearcherGuid;
            PretenderVacancyApplicationGuid = @event.PretenderVacancyApplicationGuid;
        }
        public void Apply(VacancyInOfferResponseAwaitingFromWinner @event)
        {
            Status = VacancyStatus.OfferResponseAwaitingFromWinner;
        }
        public void Apply(VacancyOfferAcceptedByWinner @event)
        {
            IsWinnerAccept = true;
            Status = VacancyStatus.OfferAcceptedByWinner;
        }
        public void Apply(VacancyOfferRejectedByWinner @event)
        {
            IsWinnerAccept = false;
            Status = VacancyStatus.OfferRejectedByWinner;
        }
        public void Apply(VacancyInOfferResponseAwaitingFromPretender @event)
        {
            Status = VacancyStatus.OfferResponseAwaitingFromPretender;
        }
        public void Apply(VacancyOfferAcceptedByPretender @event)
        {
            IsPretenderAccept = true;
            Status = VacancyStatus.OfferAcceptedByPretender;
        }
        public void Apply(VacancyOfferRejectedByPretender @event)
        {
            IsPretenderAccept = false;
            Status = VacancyStatus.OfferRejectedByPretender;
        }
        public void Apply(VacancyClosed @event)
        {
            Status = VacancyStatus.Closed;
        }
        public void Apply(VacancyCancelled @event)
        {
            CancelReason = @event.Reason;
            Status = VacancyStatus.Cancelled;
        }

        #endregion
    }
}
