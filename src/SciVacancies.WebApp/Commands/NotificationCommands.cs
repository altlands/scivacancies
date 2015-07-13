using System;

using MediatR;

namespace SciVacancies.WebApp.Commands
{
    public class SwitchResearchNotificationToReadCommand : CommandBase, IRequest
    {
        public Guid NotificationGuid { get; set; }
    }
    public class RemoveResearchNotificationCommand : CommandBase, IRequest
    {
        public Guid NotificationGuid { get; set; }
    }

    public class SwitchOrganizationNotificationToReadCommand : CommandBase, IRequest
    {
        public Guid NotificationGuid { get; set; }
    }
    public class RemoveOrganizationNotificationCommand : CommandBase, IRequest
    {
        public Guid NotificationGuid { get; set; }
    }
}
