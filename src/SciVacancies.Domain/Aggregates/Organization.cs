using SciVacancies.Domain.Core;
using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;

using System;
using System.Collections.Generic;

using CommonDomain.Core;

namespace SciVacancies.Domain.Aggregates
{
    public class Organization : AggregateBase
    {

        private bool Removed { get; set; }
        private List<Vacancy> Vacancies { get; set; }
        public OrganizationDataModel Data { get; set; }



        public Organization()
        {

        }
        public Organization(Guid guid, OrganizationDataModel data)
        {
            //this.Vacancies = new List<Vacancy>();

            RaiseEvent(new OrganizationCreated()
            {
                OrganizationGuid = guid,
                Data=data
            });
        }

        
        public void Remove()
        {
            if (!this.Removed)
            {
                RaiseEvent(new OrganizationRemoved()
                {
                    OrganizationGuid = this.Id
                });
            }
        }

        public void Update(OrganizationDataModel data)
        {
            RaiseEvent(new OrganizationUpdated()
            {
                OrganizationGuid = this.Id,
                Data = data
            });
        }

        public void CreateVacancy()
        {


        }
        #region Apply-Handlers
        public void Apply(OrganizationCreated @event)
        {
            Id = @event.OrganizationGuid;
            Data = @event.Data;
        }
        public void Apply(OrganizationRemoved @event)
        {
            this.Removed = true;
        }
        public void Apply(OrganizationUpdated @event)
        {
            Data = @event.Data;

        }

        public void Apply(VacancyCreated @event)
        {
            //this.Vacancies.Add(new Vacancy());
        }
        #endregion
    }
}
