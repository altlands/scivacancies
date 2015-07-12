using SciVacancies.Domain.Enums;

using System;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("res_notifications")]
    [PrimaryKey("guid", AutoIncrement = false)]
    public class ResearcherNotification : BaseEntity
    {
        public string title { get; set; }

        public Guid vacancy_guid { get; set; }
        public Guid researcher_guid { get; set; }

        public NotificationStatus status { get; set; }

        public DateTime creation_date { get; set; }
        public DateTime? update_date { get; set; }
    }
}
