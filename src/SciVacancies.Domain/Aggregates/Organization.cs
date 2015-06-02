using SciVacancies.Domain.Events;

using System;
using System.Collections.Generic;

using CommonDomain.Core;

namespace SciVacancies.Domain.Aggregates
{
    public class Organization : AggregateBase
    {

        private bool Deleted { get; set; }
        //private List<Guid> VacanciesId { get; set; }
        public List<Vacancy> Vacancies { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }


        public Organization()
        {

        }
        public Organization(Guid id, string name, string shortName)
        {
            this.Vacancies = new List<Vacancy>();

            RaiseEvent(new OrganizationCreated()
            {
                Id = Guid.NewGuid(),
                TimeStamp = DateTime.UtcNow,
                OrganizationId = id,
                Name = name,
                ShortName = shortName
            });
        }
        public void Delete()
        {
            if (!this.Deleted)
            {
                RaiseEvent(new OrganizationRemoved()
                {
                    Id = Guid.NewGuid(),
                    TimeStamp = DateTime.UtcNow,
                    OrganizationId = this.Id
                });
            }
        }
        public void CreateVacancy()
        {
            this.Vacancies.Add(new Vacancy(Guid.NewGuid(), this.Id));
        }
        #region Apply-Handlers
        public void Apply(OrganizationCreated @event)
        {
            Id = @event.OrganizationId;
            Name = @event.Name;
            ShortName = @event.ShortName;
        }
        public void Apply(OrganizationRemoved @event)
        {
            this.Deleted = true;
        }
        public void Apply(OrganizationUpdated @event)
        {

        }
        #endregion
    }
}
