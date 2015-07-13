using SciVacancies.Domain.Enums;

using System;

using MediatR;
using NPoco;

namespace SciVacancies.WebApp.Commands
{
    public class SwitchResearcherNotificationToReadCommandHandler : RequestHandler<SwitchResearcherNotificationToReadCommand>
    {
        private readonly IDatabase _db;

        public SwitchResearcherNotificationToReadCommandHandler(IDatabase db)
        {
            _db = db;
        }

        protected override void HandleCore(SwitchResearcherNotificationToReadCommand msg)
        {
            if (msg.NotificationGuid == Guid.Empty) throw new ArgumentNullException($"NotificationGuid is empty: {msg.NotificationGuid}");

            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"UPDATE res_notifications SET status = @0, update_date = @1 WHERE guid = @2", NotificationStatus.Read, msg.TimeStamp, msg.NotificationGuid));
                transaction.Complete();
            }
        }
    }
    public class RemoveResearcherNotificationCommandHandler : RequestHandler<RemoveResearcherNotificationCommand>
    {
        private readonly IDatabase _db;

        public RemoveResearcherNotificationCommandHandler(IDatabase db)
        {
            _db = db;
        }

        protected override void HandleCore(RemoveResearcherNotificationCommand msg)
        {
            if (msg.NotificationGuid == Guid.Empty) throw new ArgumentNullException($"NotificationGuid is empty: {msg.NotificationGuid}");

            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"UPDATE res_notifications SET status = @0, update_date = @1 WHERE guid = @2", NotificationStatus.Removed, msg.TimeStamp, msg.NotificationGuid));
                transaction.Complete();
            }
        }
    }

    public class SwitchOrganizationNotificationToReadCommandHandler : RequestHandler<SwitchOrganizationNotificationToReadCommand>
    {
        private readonly IDatabase _db;

        public SwitchOrganizationNotificationToReadCommandHandler(IDatabase db)
        {
            _db = db;
        }

        protected override void HandleCore(SwitchOrganizationNotificationToReadCommand msg)
        {
            if (msg.NotificationGuid == Guid.Empty) throw new ArgumentNullException($"NotificationGuid is empty: {msg.NotificationGuid}");

            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"UPDATE org_notifications SET status = @0, update_date = @1 WHERE guid = @2", NotificationStatus.Read, msg.TimeStamp, msg.NotificationGuid));
                transaction.Complete();
            }
        }
    }
    public class RemoveOrganizationNotificationCommandHandler : RequestHandler<RemoveOrganizationNotificationCommand>
    {
        private readonly IDatabase _db;

        public RemoveOrganizationNotificationCommandHandler(IDatabase db)
        {
            _db = db;
        }

        protected override void HandleCore(RemoveOrganizationNotificationCommand msg)
        {
            if (msg.NotificationGuid == Guid.Empty) throw new ArgumentNullException($"NotificationGuid is empty: {msg.NotificationGuid}");

            using (var transaction = _db.GetTransaction())
            {
                _db.Execute(new Sql($"UPDATE res_notifications SET status = @0, update_date = @1 WHERE guid = @2", NotificationStatus.Removed, msg.TimeStamp, msg.NotificationGuid));
                transaction.Complete();
            }
        }
    }
}
