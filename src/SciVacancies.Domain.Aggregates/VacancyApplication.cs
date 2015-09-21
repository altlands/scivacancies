using CommonDomain.Core;
using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;
using System;

namespace SciVacancies.Domain.Aggregates
{
    public class VacancyApplication : AggregateBase
    {
        public Guid ResearcherGuid { get; private set; }
        public Guid VacancyGuid { get; private set; }

        //todo: get - нигде не используется
        private VacancyApplicationDataModel Data { get; set; }

        public VacancyApplicationStatus Status { get; private set; }

        /// <summary>
        /// Обоснование выбора этой заявки в качестве победителя или претендента
        /// </summary>
        public string Reason { get; private set; }

        public VacancyApplication()
        {

        }
        public VacancyApplication(Guid guid, Guid researcherGuid, Guid vacancyGuid, VacancyApplicationDataModel data)
        {
            if (guid.Equals(Guid.Empty)) throw new ArgumentNullException(nameof(guid));
            if (researcherGuid.Equals(Guid.Empty)) throw new ArgumentNullException(nameof(researcherGuid));
            if (vacancyGuid.Equals(Guid.Empty)) throw new ArgumentNullException(nameof(vacancyGuid));
            if (data == null) throw new ArgumentNullException(nameof(data));

            RaiseEvent(new VacancyApplicationCreated()
            {
                VacancyApplicationGuid = guid,
                ResearcherGuid = researcherGuid,
                VacancyGuid = vacancyGuid,
                Data = data
            });
        }

        #region Methods

        public void Update(VacancyApplicationDataModel data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (Status != VacancyApplicationStatus.InProcess) throw new InvalidOperationException("vacancyApplication state is invalid");

            RaiseEvent(new VacancyApplicationUpdated()
            {
                VacancyApplicationGuid = Id,
                ResearcherGuid = ResearcherGuid,
                VacancyGuid = VacancyGuid,
                Data = data
            });
        }
        public void Remove()
        {
            if (Status != VacancyApplicationStatus.InProcess) throw new InvalidOperationException("vacancyApplication state is invalid");

            RaiseEvent(new VacancyApplicationRemoved()
            {
                VacancyApplicationGuid = Id,
                VacancyGuid = VacancyGuid,
                ResearcherGuid = ResearcherGuid
            });
        }

        public void ApplyToVacancy()
        {
            if (Status != VacancyApplicationStatus.InProcess) throw new InvalidOperationException("vacancyApplication state is invalid");

            RaiseEvent(new VacancyApplicationApplied()
            {
                VacancyApplicationGuid = Id,
                VacancyGuid = VacancyGuid,
                ResearcherGuid = ResearcherGuid
            });
        }
        public void Cancel()
        {
            if (!(Status == VacancyApplicationStatus.InProcess || Status == VacancyApplicationStatus.Applied)) throw new InvalidOperationException("vacancyApplication state is invalid");

            RaiseEvent(new VacancyApplicationCancelled()
            {
                VacancyApplicationGuid = Id,
                VacancyGuid = VacancyGuid,
                ResearcherGuid = ResearcherGuid
            });
        }
        public void MakeVacancyApplicationWinner(string reason)
        {
            if (string.IsNullOrEmpty(reason)) throw new ArgumentNullException(nameof(reason));
            if (Status != VacancyApplicationStatus.Applied) throw new InvalidOperationException("vacancyApplication state is invalid");

            RaiseEvent(new VacancyApplicationWon
            {
                VacancyApplicationGuid = Id,
                VacancyGuid = VacancyGuid,
                ResearcherGuid = ResearcherGuid,
                Reason = reason
            });
        }
        public void MakeVacancyApplicationPretender(string reason)
        {
            if (string.IsNullOrEmpty(reason)) throw new ArgumentNullException(nameof(reason));
            if (Status != VacancyApplicationStatus.Applied) throw new InvalidOperationException("vacancyApplication state is invalid");

            RaiseEvent(new VacancyApplicationPretended
            {
                VacancyApplicationGuid = Id,
                VacancyGuid = VacancyGuid,
                ResearcherGuid = ResearcherGuid,
                Reason = reason
            });
        }
        public void MakeVacancyApplicationLooser()
        {
            if (Status != VacancyApplicationStatus.Applied) throw new InvalidOperationException("vacancyApplication state is invalid");

            RaiseEvent(new VacancyApplicationLost
            {
                VacancyApplicationGuid = Id,
                VacancyGuid = VacancyGuid,
                ResearcherGuid = ResearcherGuid
            });
        }

        #endregion

        #region Apply-Handlers

        public void Apply(VacancyApplicationCreated @event)
        {
            Id = @event.VacancyApplicationGuid;
            ResearcherGuid = @event.ResearcherGuid;
            VacancyGuid = @event.VacancyGuid;
            Data = @event.Data;
        }
        public void Apply(VacancyApplicationUpdated @event)
        {
            Data = @event.Data;
        }
        public void Apply(VacancyApplicationRemoved @event)
        {
            Status = VacancyApplicationStatus.Removed;
        }

        public void Apply(VacancyApplicationApplied @event)
        {
            Status = VacancyApplicationStatus.Applied;
        }
        public void Apply(VacancyApplicationCancelled @event)
        {
            Status = VacancyApplicationStatus.Cancelled;
        }
        public void Apply(VacancyApplicationWon @event)
        {
            Status = VacancyApplicationStatus.Won;
            Reason = @event.Reason;
        }
        public void Apply(VacancyApplicationPretended @event)
        {
            Status = VacancyApplicationStatus.Pretended;
            Reason = @event.Reason;
        }
        public void Apply(VacancyApplicationLost @event)
        {
            Status = VacancyApplicationStatus.Lost;
        }

        #endregion
    }
}
