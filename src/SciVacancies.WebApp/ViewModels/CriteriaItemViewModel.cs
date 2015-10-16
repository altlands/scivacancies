using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SciVacancies.WebApp.ViewModels
{

    public class CriteriaItemViewModel
    {
        /// <summary>
        /// Идентификатор критерия
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Идентификатор родительского критерия
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// шт
        /// </summary>
        [Range(0,int.MaxValue, ErrorMessage = "Критерий не может иметь отрицательное значение")]
        public long Count { get; set; }

        /// <summary>
        /// Заголовок
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// внутренние критерии
        /// </summary>
        public List<CriteriaItemViewModel> Items { get; set; } = new List<CriteriaItemViewModel>();

        public string Code { get; set; }

    }

}
