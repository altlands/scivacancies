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
    public class SwitchNotificationToReadCommandHandler : RequestHandler<SwitchNotificationToReadCommand>
    {
        private readonly IDatabase _db;

        public SwitchNotificationToReadCommandHandler(IDatabase db)
        {
            _db = db;
        }

        protected override void HandleCore(SwitchNotificationToReadCommand message)
        {
            if (message.NotificationGuid == Guid.Empty) throw new ArgumentNullException($"NotificationGuid is empty: {message.NotificationGuid}");

            Notification notification = _db.SingleOrDefaultById<Notification>(message.NotificationGuid);

            notification.Status = NotificationStatus.Read;

            _db.Update(notification);
        }
    }
    public class RemoveNotificationCommandHandler : RequestHandler<RemoveNotificationCommand>
    {
        private readonly IDatabase _db;

        public RemoveNotificationCommandHandler(IDatabase db)
        {
            _db = db;
        }

        protected override void HandleCore(RemoveNotificationCommand message)
        {
            if (message.NotificationGuid == Guid.Empty) throw new ArgumentNullException($"NotificationGuid is empty: {message.NotificationGuid}");

            _db.Delete<Notification>(message.NotificationGuid);
        }
    }
}
