using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;

using MediatR;
using NPoco;

namespace SciVacancies.WebApp.Queries
{
    public class SingleNotificationQuery : IRequest<Notification>
    {
        public Guid NotificationGuid { get; set; }
    }
    public class SelectPagedNotificationsByResearcherQuery : IRequest<Page<Notification>>
    {
        public Guid ResearcherGuid { get; set; }
        public string OrderBy { get; set; }
        public long PageSize { get; set; }
        public long PageIndex { get; set; }
    }
    public class SelectPagedNotificationsByOrganizationQuery : IRequest<Page<Notification>>
    {
        public Guid OrganizationGuid { get; set; }
        public string OrderBy { get; set; }
        public long PageSize { get; set; }
        public long PageIndex { get; set; }
    }
}
