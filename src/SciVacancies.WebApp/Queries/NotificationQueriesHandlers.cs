using SciVacancies.ReadModel.Core;

using System;

using NPoco;
using MediatR;

namespace SciVacancies.WebApp.Queries
{
    public class SelectPagedNotificationsByResearcherQueryHandler : IRequestHandler<SelectPagedNotificationsByResearcherQuery, Page<Notification>>
    {
        private readonly IDatabase _db;

        public SelectPagedNotificationsByResearcherQueryHandler(IDatabase db)
        {
            _db = db;
        }

        public Page<Notification> Handle(SelectPagedNotificationsByResearcherQuery message)
        {
            if (message.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {message.ResearcherGuid}");

            Page<Notification> notifications = _db.Page<Notification>(message.PageIndex, message.PageSize, new Sql("SELECT n.* FROM \"Notifications\" n WHERE n.\"ResearcherGuid\"=" + message.ResearcherGuid + " ORDER BY n.\"Guid\" DESC"));

            return notifications;
        }
    }
    public class SelectPagedNotificationsByOrganizationQueryHandler : IRequestHandler<SelectPagedNotificationsByOrganizationQuery, Page<Notification>>
    {
        private readonly IDatabase _db;

        public SelectPagedNotificationsByOrganizationQueryHandler(IDatabase db)
        {
            _db = db;
        }

        public Page<Notification> Handle(SelectPagedNotificationsByOrganizationQuery message)
        {
            if (message.OrganizationGuid == Guid.Empty) throw new ArgumentNullException($"OrganizationGuid is empty: {message.OrganizationGuid}");

            Page<Notification> notifications = _db.Page<Notification>(message.PageIndex, message.PageSize, new Sql("SELECT n.* FROM \"Notifications\" n WHERE n.\"OrganizationGuid\"=" + message.OrganizationGuid + " ORDER BY n.\"Guid\" DESC"));

            return notifications;
        }
    }
}