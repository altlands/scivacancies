using SciVacancies.ReadModel.Core;

using System;

using MediatR;
using NPoco;

namespace SciVacancies.WebApp.Queries
{
    public class SingleResearcherNotificationQuery:IRequest<ResearcherNotification>
    {
        public Guid Guid { get; set; }
    }
    public class SelectPagedResearcherNotificationsQuery : IRequest<Page<ResearcherNotification>>
    {
        public Guid ResearcherGuid { get; set; }

        public long PageSize { get; set; }
        public long PageIndex { get; set; }
        public string OrderBy { get; set; }
        public string OrderDirection { get; set; }
    }

    public class SingleOrganizationNotificationQuery : IRequest<OrganizationNotification>
    {
        public Guid Guid { get; set; }
    }
    public class SelectPagedOrganizationNotificationsQuery : IRequest<Page<OrganizationNotification>>
    {
        public Guid OrganizationGuid { get; set; }

        public long PageSize { get; set; }
        public long PageIndex { get; set; }
        public string OrderBy { get; set; }
        public string OrderDirection { get; set; }
    }
}
