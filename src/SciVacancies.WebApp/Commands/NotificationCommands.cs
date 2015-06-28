using System;
using MediatR;

namespace SciVacancies.WebApp.Commands
{
    public class SwitchNotificationToReadCommand : CommandBase, IRequest
    {
        public Guid NotificationGuid { get; set; }
    }
    public class RemoveNotificationCommand : CommandBase, IRequest
    {
        public Guid NotificationGuid { get; set; }
    }
}
