using System.Collections.Generic;

namespace SciVacancies.WebApp.ViewModels
{

    public class VacanciesFilter
    {
        public VacanciesFilter()
        {
            Regions = new List<string>();
        }
        public IEnumerable<string> Salaries { get; set; }
        public IEnumerable<string> ContestStates { get; set; }
        public IEnumerable<string> Regions { get; set; }

        public int Period { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }

        private string _orderBy;
        private string _search;

        public string OrderBy
        {
            get
            {
                _orderBy = string.IsNullOrWhiteSpace(_orderBy) ? "date_descending" : _orderBy.ToLower();
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
