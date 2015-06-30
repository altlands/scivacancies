using SciVacancies.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("Notifications")]
    [PrimaryKey("Guid", AutoIncrement = false)]
    public class Notification:BaseEntity
    {
        public Guid ResearcherGuid { get; set; }
        public Guid OrganizationGuid { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public string Title { get; set; }

        public NotificationStatus Status { get; set; }
    }
}
