using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;

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
                transaction.Complete();
            }
        }
        public void Handle(OrganizationUpdated msg)
        {
            Organization organization = _db.SingleById<Organization>(msg.OrganizationGuid);


            using (var transaction = _db.GetTransaction())
            {

                _db.Update(organization);
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