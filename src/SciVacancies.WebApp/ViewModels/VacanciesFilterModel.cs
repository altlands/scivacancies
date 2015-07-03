using System.Collections.Generic;
using SciVacancies.ReadModel.ElasticSearchModel.Model;
using SciVacancies.ReadModel.Pager;
using SciVacancies.WebApp.Engine;

namespace SciVacancies.WebApp.ViewModels
{

    public class VacanciesFilterModel
    {
        public IEnumerable<string> Regions { get; set; }
        public IEnumerable<string> Foivs { get; set; }
        public IEnumerable<string> ResearchDirections { get; set; }
        //TODO - переименовать в PositionTypes
        public IEnumerable<string> Positions { get; set; }
        public IEnumerable<string> Organizations { get; set; }
        public IEnumerable<string> VacancyStates { get; set; }

        public int Period { get; set; }
        public int PageSize { get; set; } = 10;
        public int CurrentPage { get; set; } = 1;

        public int SalaryMin { get; set; }
        public int SalaryMax { get; set; }

        private string _orderBy;
        private string _search;

        public string OrderBy
        {
            get
            {
                _orderBy = string.IsNullOrWhiteSpace(_orderBy) ? ConstTerms.OrderByDateDescending : _orderBy.ToLower();
                return _orderBy;
            }
            set { _orderBy = value; }
        }

        public string Search
        {
            get { return _search; }
            set
            {
                _search = string.IsNullOrWhiteSpace(value) ? null : value;
            }
        }

        public PagedList<Vacancy> Items { get; set; }
    }
}
