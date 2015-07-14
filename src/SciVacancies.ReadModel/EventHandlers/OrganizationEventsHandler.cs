using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;

using System;

using MediatR;
using NPoco;
using AutoMapper;

namespace SciVacancies.ReadModel.EventHandlers
{
    public class OrganizationEventsHandler :
        INotificationHandler<OrganizationCreated>,
        INotificationHandler<OrganizationUpdated>,
        INotificationHandler<OrganizationRemoved>
    {
        private readonly IDatabase _db;

        public OrganizationEventsHandler(IDatabase db)
        {
            _db = db;
        }

        public void Handle(OrganizationCreated msg)
        {
            Organization organization = Mapper.Map<Organization>(msg);

            using (var transaction = _db.GetTransaction())
            {
                _db.Insert(organization);
                foreach (ResearchDirection rd in organization.researchdirections)
                {
                    _db.Execute(new Sql($"INSERT INTO org_researchdirections (guid, researchdirection_id, organization_guid) VALUES (@0, @1, @2)", Guid.NewGuid(), rd.id, msg.OrganizationGuid));
                }
                transaction.Complete();
            }
        }
        public void Handle(OrganizationUpdated msg)
        {
            Organization organization = _db.SingleById<Organization>(msg.OrganizationGuid);

            Organization updatedOrganization = Mapper.Map<Organization>(msg);
            updatedOrganization.creation_date = organization.creation_date;

            using (var transaction = _db.GetTransaction())
            {
                _db.Update(updatedOrganization);
                //TODO - без удаления надо
                _db.Execute(new Sql($"DELETE FROM org_researchdirections WHERE organization_guid = @0", msg.OrganizationGuid));
                foreach (ResearchDirection rd in updatedOrganization.researchdirections)
                {
                    _db.Execute(new Sql($"INSERT INTO org_researchdirections (guid, researchdirection_id, organization_guid) VALUES (@0, @1, @2)", Guid.NewGuid(), rd.id, msg.OrganizationGuid));
                }
                transaction.Complete();
            }
        }
        public void Handle(OrganizationRemoved msg)
        {
            using (var transaction = _db.GetTransaction())
            {
                _db.Update(new Sql($"UPDATE org_organizations SET status = @0, update_date = @1 WHERE guid = @2", OrganizationStatus.Removed, msg.TimeStamp, msg.OrganizationGuid));
                transaction.Complete();
            }
        }
    }
}