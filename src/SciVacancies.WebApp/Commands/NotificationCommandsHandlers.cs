using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MediatR;
using NPoco;

namespace SciVacancies.WebApp.Commands
{
    public class SwitchResearchNotificationToReadCommandHandler : RequestHandler<SwitchResearchNotificationToReadCommand>
    {
        private readonly IDatabase _db;

        public SwitchResearchNotificationToReadCommandHandler(IDatabase db)
        {
            _db = db;
        }

        protected override void HandleCore(SwitchResearchNotificationToReadCommand msg)
        {
            if (msg.NotificationGuid == Guid.Empty) throw new ArgumentNullException($"NotificationGuid is empty: {msg.NotificationGuid}");

            using (var transaction = _db.GetTransaction())
            {
                _db.Update(new Sql($"UPDATE res_notifications SET status = @0, update_date = @1 WHERE guid = @2", NotificationStatus.Read, msg.TimeStamp, msg.NotificationGuid));
                transaction.Complete();
            }
        }
    }
    public class RemoveResearcherNotificationCommandHandler : RequestHandler<RemoveResearchNotificationCommand>
    {
        private readonly IDatabase _db;

        public RemoveResearcherNotificationCommandHandler(IDatabase db)
        {
            _db = db;
        }

        protected override void HandleCore(RemoveResearchNotificationCommand msg)
        {
            if (msg.NotificationGuid == Guid.Empty) throw new ArgumentNullException($"NotificationGuid is empty: {msg.NotificationGuid}");

            using (var transaction = _db.GetTransaction())
            {
                _db.Update(new Sql($"UPDATE res_notifications SET status = @0, update_date = @1 WHERE guid = @2", NotificationStatus.Removed, msg.TimeStamp, msg.NotificationGuid));
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
                _db.Update(new Sql($"UPDATE org_notifications SET status = @0, update_date = @1 WHERE guid = @2", NotificationStatus.Read, msg.TimeStamp, msg.NotificationGuid));
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
                _db.Update(new Sql($"UPDATE res_notifications SET status = @0, update_date = @1 WHERE guid = @2", NotificationStatus.Removed, msg.TimeStamp, msg.NotificationGuid));
                transaction.Complete();
            }
        }
    }
}
