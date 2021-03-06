﻿using SciVacancies.Domain.Enums;

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
        public string orderby { get; set; }

        public string foiv_ids { get; set; }
        public string positiontype_ids { get; set; }
        public string region_ids { get; set; }
        public string researchdirection_ids { get; set; }
        public int? salary_from { get; set; }
        public int? salary_to { get; set; }
        public string vacancy_statuses { get; set; }

        /// <summary>
        /// количество записей, найденных в при последней обработке
        /// </summary>
        public long currenttotal_count { get; set; }
        /// <summary>
        /// дата последней обработки
        /// </summary>
        public DateTime? currentcheck_date { get; set; }
        /// <summary>
        /// количество записей, найденных в при предпоследней обработке
        /// </summary>
        public long lasttotal_count { get; set; }
        /// <summary>
        /// дата предпоследней обработки (позволяет искать только "свежие" записи)
        /// </summary>
        public DateTime? lastcheck_date { get; set; }

        public SearchSubscriptionStatus status { get; set; }

        public Guid researcher_guid { get; set; }

        public DateTime creation_date { get; set; }
        public DateTime? update_date { get; set; }
    }
}
