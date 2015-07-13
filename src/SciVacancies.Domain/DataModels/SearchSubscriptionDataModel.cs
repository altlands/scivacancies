using SciVacancies.Domain.Enums;

using System.Collections.Generic;

namespace SciVacancies.Domain.DataModels
{
    public class SearchSubscriptionDataModel
    {
        /// <summary>
        /// Название подписки (задаётся исследователем)
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Поисковой запрос
        /// </summary>
        public string Query { get; set; }

        public IEnumerable<int> FoivIds { get; set; }
        public IEnumerable<int> PositionTypeIds { get; set; }
        public IEnumerable<int> RegionIds { get; set; }
        public IEnumerable<int> ResearchDirectionIds { get; set; }

        public int SalaryFrom { get; set; }
        public int SalaryTo { get; set; }
        public IEnumerable<VacancyStatus> VacancyStatuses { get; set; }
    }
}
