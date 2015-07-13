using SciVacancies.ReadModel.Core;

using System;

using NPoco;
using MediatR;

namespace SciVacancies.WebApp.Queries
{
    public class NotificationQueriesHandler :
        IRequestHandler<SelectPagedResearcherNotificationsQuery, Page<ResearcherNotification>>,
        IRequestHandler<SelectPagedOrganizationNotificationsQuery, Page<OrganizationNotification>>
    {
        private readonly IDatabase _db;

        public NotificationQueriesHandler(IDatabase db)
        {
            _db = db;
        }

        public Page<ResearcherNotification> Handle(SelectPagedResearcherNotificationsQuery message)
        {
            if (message.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {message.ResearcherGuid}");

            Page<ResearcherNotification> notifications = _db.Page<ResearcherNotification>(message.PageIndex, message.PageSize, new Sql($"SELECT n.* FROM res_notifications n WHERE n.researcher_guid = @0 ORDER BY n.guid DESC", message.ResearcherGuid));

            return notifications;
        }

        public Page<OrganizationNotification> Handle(SelectPagedOrganizationNotificationsQuery message)
        {
            if (message.OrganizationGuid == Guid.Empty) throw new ArgumentNullException($"OrganizationGuid is empty: {message.OrganizationGuid}");

            Page<OrganizationNotification> notifications = _db.Page<OrganizationNotification>(message.PageIndex, message.PageSize, new Sql($"SELECT n.* FROM org_notifications n WHERE n.organization_guid = @0 ORDER BY n.guid DESC", message.OrganizationGuid));

            return notifications;
        }
    }
}