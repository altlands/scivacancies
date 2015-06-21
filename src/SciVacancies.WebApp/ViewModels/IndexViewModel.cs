using NPoco;
using SciVacancies.ReadModel.Core;

namespace SciVacancies.WebApp.ViewModels
{
    /// <summary>
    /// Данные для отображения на главной странице сайта
    /// </summary>
    public class IndexViewModel
    {
        public Page<Organization> OrganizationsList{ get; set; }
        public Page<Vacancy> VacanciesList { get; set; }
    }
}