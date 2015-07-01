using System;

namespace SciVacancies.Domain.DataModels
{
    public class SearchSubscriptionDataModel
    {
        /// <summary>
        /// Название подписки (задаётся исследователем)
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Поисковой запрос в json-е
        /// </summary>
        public string Query { get; set; }
    }
}
