using System.Collections.Generic;
using MediatR;
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
        public IMediator CurrentMediator { get; set; }
        public int RegionId { get; set; }
    }
}