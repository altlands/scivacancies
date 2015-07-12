using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;

using System;

using CommonDomain.Core;

namespace SciVacancies.Domain.Aggregates
{
    public class Organization : AggregateBase
    {
        private OrganizationDataModel Data { get; set; }

        private OrganizationStatus Status { get; set; }

        public Organization()
        {

        }
        public Organization(Guid guid, OrganizationDataModel data)
        {
            RaiseEvent(new OrganizationCreated()
            {
                OrganizationGuid = guid,
                Data = data
            });
        }

        #region Methods

        public void Update(OrganizationDataModel data)
        {
            if (Status == OrganizationStatus.Active)
            {
                RaiseEvent(new OrganizationUpdated()
                {
                    OrganizationGuid = this.Id,
                    Data = data
                });
            }
        }
        public void Remove()
        {
            if (Status != OrganizationStatus.Removed)
            {
                RaiseEvent(new OrganizationRemoved()
                {
                    OrganizationGuid = this.Id
                });
            }
        }

        #endregion

        #region Apply-Handlers

        public void Apply(OrganizationCreated @event)
        {
            Id = @event.OrganizationGuid;
            Data = @event.Data;
        }
        public void Apply(OrganizationUpdated @event)
        {
            this.Data = @event.Data;

        }
        public void Apply(OrganizationRemoved @event)
        {
            this.Status = OrganizationStatus.Removed;
        }

        #endregion
    }
}
