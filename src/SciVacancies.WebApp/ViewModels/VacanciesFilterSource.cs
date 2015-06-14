using System.Collections.Generic;
using Microsoft.AspNet.Mvc.Rendering;

namespace SciVacancies.WebApp.ViewModels
{
    public class VacanciesFilterSource
    {
        public VacanciesFilterSource()
        {
            Periods = new List<SelectListItem>
            {
                new SelectListItem {Value = "0", Text = "За всё время"}
                ,new SelectListItem {Value ="30" , Text ="За месяц" }
                ,new SelectListItem {Value = "7", Text ="За неделю" }
                ,new SelectListItem {Value = "1", Text = "за день"}
            };

            OrderBys = new List<SelectListItem>
            {
                new SelectListItem {Value = "date_descending", Text = "Сначала последние"}
                ,new SelectListItem {Value ="date_ascending" , Text ="Сначала первые" }
            };

            Regions = new List<SelectListItem>
            {
                new SelectListItem {Value = "1", Text = "Москва и область"}
                ,new SelectListItem {Value ="2" , Text ="Санкт-Петербург" }
                ,new SelectListItem {Value ="3" , Text ="Новосибирск" }
                ,new SelectListItem {Value ="4" , Text ="Самара" }
                ,new SelectListItem {Value ="5" , Text ="Владивосток" }
            };
        }

        public IEnumerable<SelectListItem> Periods;
        public IEnumerable<SelectListItem> OrderBys;
        public IEnumerable<SelectListItem> Regions;

        public Dictionary<string, string> Salaries;
        public Dictionary<string, string> ContestStates;
        public List<int> PageSize;
    }
}