using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel.Core;

using System;
using System.Linq;
using NPoco;
using MediatR;

namespace SciVacancies.WebApp.Queries
{
    public class NotificationQueriesHandler :
        IRequestHandler<SingleResearcherNotificationQuery, ResearcherNotification>,
        IRequestHandler<SelectPagedResearcherNotificationsQuery, Page<ResearcherNotification>>,
        IRequestHandler<CountResearcherNotificationsUnreadQuery, int>,
        IRequestHandler<SingleOrganizationNotificationQuery, OrganizationNotification>,
        IRequestHandler<SelectPagedOrganizationNotificationsQuery, Page<OrganizationNotification>>,
        IRequestHandler<CountOrganizationNotificationsUnreadQuery, int>
    {
        private readonly IDatabase _db;

        public NotificationQueriesHandler(IDatabase db)
        {
            _db = db;
        }
        public ResearcherNotification Handle(SingleResearcherNotificationQuery msg)
        {
            if (msg.Guid == Guid.Empty) throw new ArgumentNullException($"Guid is empty: {msg.Guid}");

            var notification = _db.SingleById<ResearcherNotification>(msg.Guid);

            return notification;
        }
        public Page<ResearcherNotification> Handle(SelectPagedResearcherNotificationsQuery msg)
        {
            if (msg.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {msg.ResearcherGuid}");
            if (string.IsNullOrWhiteSpace(msg.OrderDirection))
                msg.OrderDirection = "DESC";
            if (string.IsNullOrWhiteSpace(msg.OrderBy))
                msg.OrderBy = nameof(OrganizationNotification.creation_date);

            var notifications = _db.Page<ResearcherNotification>(msg.PageIndex, msg.PageSize, new Sql($"SELECT n.* FROM res_notifications n WHERE n.researcher_guid = @0 AND n.status != @1 ORDER BY n.{msg.OrderBy} {msg.OrderDirection.ToUpper()}", msg.ResearcherGuid, NotificationStatus.Removed));

            return notifications;
        }
        public int Handle(CountResearcherNotificationsUnreadQuery msg)
        {
            if (msg.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {msg.ResearcherGuid}");
            var notifications = _db.Fetch<ResearcherNotification>(new Sql($"SELECT n.guid , n.status FROM res_notifications n WHERE n.researcher_guid = @0 AND n.status != @1", msg.ResearcherGuid, NotificationStatus.Removed));
            return notifications.Where(c => c.status != NotificationStatus.Read && c.status != NotificationStatus.Removed).ToList().Count;
        }

        public OrganizationNotification Handle(SingleOrganizationNotificationQuery msg)
        {
            if (msg.Guid == Guid.Empty) throw new ArgumentNullException($"Guid is empty: {msg.Guid}");

            var notification = _db.SingleById<OrganizationNotification>(msg.Guid);

            return notification;
        }
        public Page<OrganizationNotification> Handle(SelectPagedOrganizationNotificationsQuery msg)
        {
            if (msg.OrganizationGuid == Guid.Empty) throw new ArgumentNullException($"OrganizationGuid is empty: {msg.OrganizationGuid}");
            if (string.IsNullOrWhiteSpace(msg.OrderDirection))
                msg.OrderDirection = "DESC";
            if (string.IsNullOrWhiteSpace(msg.OrderBy))
                msg.OrderBy = nameof(OrganizationNotification.creation_date);

            var notifications = _db.Page<OrganizationNotification>(msg.PageIndex, msg.PageSize, new Sql($"SELECT n.* FROM org_notifications n WHERE n.organization_guid = @0 AND n.status != @1  ORDER BY n.{msg.OrderBy} {msg.OrderDirection.ToUpper()}", msg.OrganizationGuid, NotificationStatus.Removed));

            return notifications;
        }
        public int Handle(CountOrganizationNotificationsUnreadQuery msg)
        {
            if (msg.OrganizationGuid == Guid.Empty) throw new ArgumentNullException($"OrganizationGuid is empty: {msg.OrganizationGuid}");
            var notifications = _db.Fetch<ResearcherNotification>(new Sql($"SELECT n.guid , n.status FROM org_notifications n WHERE n.organization_guid = @0 AND n.status != @1", msg.OrganizationGuid, NotificationStatus.Removed));
            return notifications.Where(c => c.status != NotificationStatus.Read && c.status != NotificationStatus.Removed).ToList().Count;
        }
    }
}