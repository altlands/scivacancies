using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.ViewModels
{
    /// <summary>
    /// объект для отображения в списке
    /// </summary>
    public class VacanciesItemViewModel: ViewModelBase
    {
        /// <summary>
        /// Название вакансии
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Минимальная заработная плата
        /// </summary>
        public int MinSalary { get; set; }

        /// <summary>
        /// Максимальная заработная плата
        /// </summary>
        public int MaxSalary { get; set; }

        /// <summary>
        /// Дата размещения
        /// </summary>
        public DateTime PublishedDate { get; set; }

        public Guid RegionGuid { get; set; }
        /// <summary>
        /// Регион
        /// </summary>
        public RegionItemViewModel Region { get; set; }

        public Guid OrganizationGuid { get; set; }
        /// <summary>
        /// Организация
        /// </summary>
        public OrganizationItemViewModel Organization { get; set; }

        /// <summary>
        /// Научные направления
        /// </summary>
        public ICollection<ScienceDirectionItemViewModel> ScienceDirections { get; set; }

    }
}
