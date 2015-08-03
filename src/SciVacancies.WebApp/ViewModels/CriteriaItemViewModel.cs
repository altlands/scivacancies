using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.WebApp.ViewModels
{

    public class CriteriaItemViewModel
    {
        /// <summary>
        /// Идентификатор критерия
        /// </summary>
        public int Id { get; set; }
        ///// <summary>
        ///// Идентификатор родительского критерия
        ///// </summary>
        //public int? ParentId { get; set; }

        /// <summary>
        /// шт
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Заголовок
        /// </summary>
        public string Title { get; set; }

    }

}
