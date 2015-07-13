using SciVacancies.Domain.Enums;

using System;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("org_notifications")]
    [PrimaryKey("guid", AutoIncrement = false)]
    public class OrganizationNotification : BaseEntity
    {
        public string title { get; set; }

        public Guid vacancyapplication_guid { get; set; }
        public Guid organization_guid { get; set; }

        public NotificationStatus status { get; set; }

        public DateTime creation_date { get; set; }
        public DateTime? update_date { get; set; }
    }
}
