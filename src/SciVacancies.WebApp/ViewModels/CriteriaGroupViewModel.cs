using System.Collections.Generic;

namespace SciVacancies.WebApp.ViewModels
{
    public class CriteriaGroupViewModel
    {

        /// <summary>
        /// Заголовок
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Критерии группы
        /// </summary>
        public List<CriteriaItemViewModel> Items { get; set; } = new List<CriteriaItemViewModel>();
    }
}
