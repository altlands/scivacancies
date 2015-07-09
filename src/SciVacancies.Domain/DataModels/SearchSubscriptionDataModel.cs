using SciVacancies.Domain.Enums;

namespace SciVacancies.Domain.DataModels
{
    public class SearchSubscriptionDataModel
    {
        #region General

        /// <summary>
        /// Название подписки (задаётся исследователем)
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Поисковой запрос в json-е
        /// </summary>
        public string Query { get; set; }

        #endregion

        [System.Obsolete("Status will be moved from dataModel")]
        public SearchSubscriptionStatus Status { get; set; }
    }
}
