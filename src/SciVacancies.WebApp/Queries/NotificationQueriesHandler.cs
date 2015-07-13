using SciVacancies.ReadModel.Core;

using System;

using NPoco;
using MediatR;

namespace SciVacancies.WebApp.Queries
{
    public class NotificationQueriesHandler :
        IRequestHandler<SingleResearcherNotificationQuery, ResearcherNotification>,
        IRequestHandler<SelectPagedResearcherNotificationsQuery, Page<ResearcherNotification>>,
        IRequestHandler<SingleOrganizationNotificationQuery, OrganizationNotification>,
        IRequestHandler<SelectPagedOrganizationNotificationsQuery, Page<OrganizationNotification>>
    {
        private readonly IDatabase _db;

        public NotificationQueriesHandler(IDatabase db)
        {
            _db = db;
        }
        public ResearcherNotification Handle(SingleResearcherNotificationQuery msg)
        {
            if (msg.Guid == Guid.Empty) throw new ArgumentNullException($"Guid is empty: {msg.Guid}");

            ResearcherNotification notification = _db.SingleById<ResearcherNotification>(msg.Guid);

            return notification;
        }
        public Page<ResearcherNotification> Handle(SelectPagedResearcherNotificationsQuery msg)
        {
            if (msg.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {msg.ResearcherGuid}");

            Page<ResearcherNotification> notifications = _db.Page<ResearcherNotification>(msg.PageIndex, msg.PageSize, new Sql($"SELECT n.* FROM res_notifications n WHERE n.researcher_guid = @0 ORDER BY n.guid DESC", msg.ResearcherGuid));

            return notifications;
        }

        public OrganizationNotification Handle(SingleOrganizationNotificationQuery msg)
        {
            if (msg.Guid == Guid.Empty) throw new ArgumentNullException($"Guid is empty: {msg.Guid}");

            OrganizationNotification notification = _db.SingleById<OrganizationNotification>(msg.Guid);

            return notification;
        }
        public Page<OrganizationNotification> Handle(SelectPagedOrganizationNotificationsQuery msg)
        {
            if (msg.OrganizationGuid == Guid.Empty) throw new ArgumentNullException($"OrganizationGuid is empty: {msg.OrganizationGuid}");

            Page<OrganizationNotification> notifications = _db.Page<OrganizationNotification>(msg.PageIndex, msg.PageSize, new Sql($"SELECT n.* FROM org_notifications n WHERE n.organization_guid = @0 ORDER BY n.guid DESC", msg.OrganizationGuid));

            return notifications;
        }
    }
}