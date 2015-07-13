using System.Collections.Generic;
using SciVacancies.ReadModel.Core;
using SciVacancies.ReadModel.Pager;

namespace SciVacancies.WebApp.ViewModels
{
    /// <summary>
    /// Данные для отображения на главной странице сайта
    /// </summary>
    public class IndexViewModel
    {
        public PagedList<OrganizationDetailsViewModel> OrganizationsList{ get; set; }
        public PagedList<VacancyDetailsViewModel> VacanciesList { get; set; }
        public List<ResearchDirectionViewModel> ResearchDirections { get; set; }
    }
}