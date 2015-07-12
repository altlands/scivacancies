using SciVacancies.Domain.Enums;

using System;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("res_searchsubscriptions")]
    [PrimaryKey("guid", AutoIncrement = false)]
    public class SearchSubscription : BaseEntity
    {
        public string title { get; set; }
        public string query { get; set; }

        public SearchSubscriptionStatus status { get; set; }

        public Guid researcher_guid { get; set; }

        public DateTime creation_date { get; set; }
        public DateTime? update_date { get; set; }
    }
}
