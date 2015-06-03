using SciVacancies.Domain.Core;
using SciVacancies.Domain.Events;

using System;
using System.Collections.Generic;

using CommonDomain.Core;

namespace SciVacancies.Domain.Aggregates
{
    public class Researcher : AggregateBase
    {
        private bool Removed { get; set; }
        private List<VacancyApplication> VacancyApplications { get; set; }

        public Researcher()
        {

        }
        public Researcher(Guid guid)
        {
            RaiseEvent(new ResearcherCreated()
            {
                ResearcherGuid = guid
            });
        }
        public void RemoveResearcher()
        {
            if (!Removed)
            {
                RaiseEvent(new ResearcherRemoved()
                {
                    ResearcherGuid = this.Id
                });
            }
        }

        public Guid CreateVacancyApplicationTemplate(Guid vacancyGuid)
        {
            Guid vacancyApplicationGuid = Guid.NewGuid();
            RaiseEvent(new VacancyApplicationCreated()
            {
                VacancyApplicationGuid = vacancyApplicationGuid,
                VacancyGuid = vacancyGuid,
                ResearcherGuid = this.Id
            });

            return vacancyApplicationGuid;
        }

        public void ApplyToVacancy(Guid vacancyApplicationGuid)
        {
            VacancyApplication vacancyApplication = this.VacancyApplications.Find(f => f.VacancyApplicationGuid == vacancyApplicationGuid);
            if (vacancyApplication != null && vacancyApplication.Status == VacancyApplicationStatus.InProcess)
            {
                RaiseEvent(new VacancyApplicationApplied()
                {
                    VacancyApplicationGuid = vacancyApplicationGuid,
                    VacancyGuid = vacancyApplication.VacancyGuid,
                    ResearcherGuid = this.Id
                });
            }
        }

        #region Apply-Handlers
        public void Apply(ResearcherCreated @event)
        {
            this.Id = @event.ResearcherGuid;
        }
        public void Apply(ResearcherRemoved @event)
        {
            this.Removed = true;
        }

        public void Apply(VacancyApplicationCreated @event)
        {
            this.VacancyApplications.Add(new VacancyApplication()
            {
                VacancyApplicationGuid = @event.VacancyApplicationGuid,
                VacancyGuid = @event.VacancyGuid,
                Status = VacancyApplicationStatus.InProcess
            });
        }
        public void Apply(VacancyApplicationApplied @event)
        {
            this.VacancyApplications.Find(f => f.VacancyApplicationGuid == @event.VacancyApplicationGuid).Status = VacancyApplicationStatus.Applied;
        }
        #endregion
    }
}
