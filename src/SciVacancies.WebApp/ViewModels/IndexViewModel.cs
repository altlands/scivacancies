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
        public PagedList<Organization> OrganizationsList{ get; set; }
        public PagedList<Vacancy> VacanciesList { get; set; }
        public List<ResearchDirectionViewModel> ResearchDirections { get; set; }
    }
}