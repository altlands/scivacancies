using System;
using System.Collections.Generic;
using SciVacancies.Domain.Enums;

namespace SciVacancies.Services.Elastic
{
    public class SearchQuery
    {
        public string Query { get; set; }

        public long? PageSize { get; set; }
        public long? CurrentPage { get; set; }

        public string OrderFieldByDirection { get; set; }

        public DateTime? PublishDateFrom { get; set; }

        public IEnumerable<int> FoivIds { get; set; }
        public IEnumerable<int> PositionTypeIds { get; set; }
        public IEnumerable<int> RegionIds { get; set; }
        public IEnumerable<int> ResearchDirectionIds { get; set; }

        public int? SalaryFrom { get; set; }
        public int? SalaryTo { get; set; }
        public IEnumerable<VacancyStatus> VacancyStatuses { get; set; }
    }
}
