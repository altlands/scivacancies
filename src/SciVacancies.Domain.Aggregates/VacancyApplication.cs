using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;

using System;

using CommonDomain.Core;

namespace SciVacancies.Domain.Aggregates
{
    public class VacancyApplication : AggregateBase
    {
        public Guid ResearcherGuid { get; private set; }
        public Guid VacancyGuid { get; private set; }

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
            if (guid.Equals(Guid.Empty)) throw new ArgumentNullException("guid is empty");
            if (researcherGuid.Equals(Guid.Empty)) throw new ArgumentNullException("researcherGuid is empty");
            if (vacancyGuid.Equals(Guid.Empty)) throw new ArgumentNullException("vacancyGuid is empty");
            if (data == null) throw new ArgumentNullException("data is empty");

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
            if (data == null) throw new ArgumentNullException("data is empty");
            if (Status != VacancyApplicationStatus.InProcess) throw new InvalidOperationException("vacancyApplication state is invalid");

            RaiseEvent(new VacancyApplicationUpdated()
            {
                VacancyApplicationGuid = this.Id,
                ResearcherGuid = this.ResearcherGuid,
                VacancyGuid = this.VacancyGuid,
                Data = data
            });
        }
        public void Remove()
        {
            if (Status != VacancyApplicationStatus.InProcess) throw new InvalidOperationException("vacancyApplication state is invalid");

            RaiseEvent(new VacancyApplicationRemoved()
            {
                VacancyApplicationGuid = this.Id,
                VacancyGuid = this.VacancyGuid,
                ResearcherGuid = this.ResearcherGuid
            });
        }

        public void ApplyToVacancy()
        {
            if (Status != VacancyApplicationStatus.InProcess) throw new InvalidOperationException("vacancyApplication state is invalid");

            RaiseEvent(new VacancyApplicationApplied()
            {
                VacancyApplicationGuid = this.Id,
                VacancyGuid = this.VacancyGuid,
                ResearcherGuid = this.ResearcherGuid
            });
        }
        public void Cancel()
        {
            if (!(Status == VacancyApplicationStatus.InProcess || Status == VacancyApplicationStatus.Applied)) throw new InvalidOperationException("vacancyApplication state is invalid");

            RaiseEvent(new VacancyApplicationCancelled()
            {
                VacancyApplicationGuid = this.Id,
                VacancyGuid = this.VacancyGuid,
                ResearcherGuid = this.ResearcherGuid
            });
        }
        public void MakeVacancyApplicationWinner(string reason)
        {
            if (String.IsNullOrEmpty(reason)) throw new ArgumentNullException("reason is null or empty");
            if (Status != VacancyApplicationStatus.Applied) throw new InvalidOperationException("vacancyApplication state is invalid");

            RaiseEvent(new VacancyApplicationWon
            {
                VacancyApplicationGuid = this.Id,
                VacancyGuid = this.VacancyGuid,
                ResearcherGuid = this.ResearcherGuid,
                Reason = reason
            });
        }
        public void MakeVacancyApplicationPretender(string reason)
        {
            if (String.IsNullOrEmpty(reason)) throw new ArgumentNullException("reason is null or empty");
            if (Status != VacancyApplicationStatus.Applied) throw new InvalidOperationException("vacancyApplication state is invalid");

            RaiseEvent(new VacancyApplicationPretended
            {
                VacancyApplicationGuid = this.Id,
                VacancyGuid = this.VacancyGuid,
                ResearcherGuid = this.ResearcherGuid,
                Reason = reason
            });
        }
        public void MakeVacancyApplicationLooser()
        {
            if (Status != VacancyApplicationStatus.Applied) throw new InvalidOperationException("vacancyApplication state is invalid");

            RaiseEvent(new VacancyApplicationLost
            {
                VacancyApplicationGuid = this.Id,
                VacancyGuid = this.VacancyGuid,
                ResearcherGuid = this.ResearcherGuid
            });
        }

        #endregion

        #region Apply-Handlers

        public void Apply(VacancyApplicationCreated @event)
        {
            this.Id = @event.VacancyApplicationGuid;
            this.ResearcherGuid = @event.ResearcherGuid;
            this.VacancyGuid = @event.VacancyGuid;
            this.Data = @event.Data;
        }
        public void Apply(VacancyApplicationUpdated @event)
        {
            this.Data = @event.Data;
        }
        public void Apply(VacancyApplicationRemoved @event)
        {
            this.Status = VacancyApplicationStatus.Removed;
        }

        public void Apply(VacancyApplicationApplied @event)
        {
            this.Status = VacancyApplicationStatus.Applied;
        }
        public void Apply(VacancyApplicationCancelled @event)
        {
            this.Status = VacancyApplicationStatus.Cancelled;
        }
        public void Apply(VacancyApplicationWon @event)
        {
            this.Status = VacancyApplicationStatus.Won;
            this.Reason = @event.Reason;
        }
        public void Apply(VacancyApplicationPretended @event)
        {
            this.Status = VacancyApplicationStatus.Pretended;
            this.Reason = @event.Reason;
        }
        public void Apply(VacancyApplicationLost @event)
        {
            this.Status = VacancyApplicationStatus.Lost;
        }

        #endregion
    }
}
