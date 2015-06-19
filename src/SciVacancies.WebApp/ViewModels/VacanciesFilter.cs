using System.Collections.Generic;
using SciVacancies.WebApp.Engine;

namespace SciVacancies.WebApp.ViewModels
{

    public class VacanciesFilter
    {
        public IEnumerable<string> Regions { get; set; } = new List<string>();
        public IEnumerable<string> Foivs { get; set; } = new List<string>();
        public IEnumerable<string> ResearchDirections { get; set; } = new List<string>();
        public IEnumerable<string> Positions { get; set; } = new List<string>();
        public IEnumerable<string> Organizations { get; set; } = new List<string>();
        public IEnumerable<string> Salaries { get; set; } = new List<string>();
        public IEnumerable<string> VacancyStates { get; set; } = new List<string>();

        public int Period { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }

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

        public void ValidateValues()
        {
            if (PageSize < 1) PageSize = 10;
            if (PageNumber < 1) PageNumber = 1;
        }
    }
}
