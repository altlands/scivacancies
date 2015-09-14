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

        public OrganizationStatus Status { get; private set; }

        public Organization()
        {

        }
        public Organization(Guid guid, OrganizationDataModel data)
        {
            if (guid.Equals(Guid.Empty)) throw new ArgumentNullException("guid is empty");
            if (data == null) throw new ArgumentNullException("data is empty");

            RaiseEvent(new OrganizationCreated()
            {
                OrganizationGuid = guid,
                Data = data
            });
        }

        #region Methods

        public void Update(OrganizationDataModel data)
        {
            if (data == null) throw new ArgumentNullException("data is empty");
            if (Status != OrganizationStatus.Active) throw new InvalidOperationException("organization state is invalid");

            RaiseEvent(new OrganizationUpdated()
            {
                OrganizationGuid = this.Id,
                Data = data
            });
        }
        public void Remove()
        {
            if (Status == OrganizationStatus.Removed) throw new InvalidOperationException("organization has been already removed");

            RaiseEvent(new OrganizationRemoved()
            {
                OrganizationGuid = this.Id
            });
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
