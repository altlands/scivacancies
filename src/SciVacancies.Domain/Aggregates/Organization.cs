using SciVacancies.Domain.Core;
using SciVacancies.Domain.Events;

using System;
using System.Collections.Generic;

using CommonDomain.Core;

namespace SciVacancies.Domain.Aggregates
{
    public class Organization : AggregateBase
    {

        private bool Deleted { get; set; }
        private List<Vacancy> Vacancies { get; set; }

        public string Name { get; set; }
        public string ShortName { get; set; }


        public Organization()
        {

        }
        public Organization(Guid guid, string name, string shortName)
        {
            //this.Vacancies = new List<Vacancy>();

            RaiseEvent(new OrganizationCreated()
            {
                OrganizationGuid = guid,
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
                    OrganizationGuid = this.Id
                });
            }
        }

        public void CreateVacancy()
        {


        }
        #region Apply-Handlers
        public void Apply(OrganizationCreated @event)
        {
            Id = @event.OrganizationGuid;
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

        public void Apply(VacancyCreated @event)
        {
            //this.Vacancies.Add(new Vacancy());
        }
        #endregion
    }
}
