using System.Collections.Generic;
using Microsoft.AspNet.Mvc.Rendering;
using SciVacancies.ReadModel;
using SciVacancies.WebApp.Engine;

namespace SciVacancies.WebApp.ViewModels
{
    public class VacanciesFilterSource
    {
        private IReadModelService _readModelService;

        public VacanciesFilterSource(IReadModelService readModelService)
        {
            _readModelService = readModelService;

            Periods = new List<SelectListItem>
            {
                new SelectListItem {Value = "0", Text = "За всё время"}
                ,new SelectListItem {Value ="30" , Text ="За месяц" }
                ,new SelectListItem {Value = "7", Text ="За неделю" }
                ,new SelectListItem {Value = "1", Text = "за день"}
            };

            OrderBys = new List<SelectListItem>
            {
                new SelectListItem {Value = ConstTerms.OrderByDateDescending, Text = "Сначала последние"}
                ,new SelectListItem {Value =ConstTerms.OrderByDateAscending , Text ="Сначала первые" }
            };

            Regions = new List<SelectListItem>
            {
                new SelectListItem {Value = "1", Text = "Москва и область"}
                ,new SelectListItem {Value ="2" , Text ="Санкт-Петербург" }
                ,new SelectListItem {Value ="3" , Text ="Новосибирск" }
                ,new SelectListItem {Value ="4" , Text ="Самара" }
                ,new SelectListItem {Value ="5" , Text ="Владивосток" }
            };

            Foivs = new List<SelectListItem>
            {
                new SelectListItem {Value = "1", Text = "Минздрав РФ"}
                ,new SelectListItem {Value ="2" , Text ="Минобонауки РФ" }
                ,new SelectListItem {Value ="3" , Text ="РАМН" }
                ,new SelectListItem {Value ="4" , Text ="РАН" }
                ,new SelectListItem {Value ="5" , Text ="РосГидроМет" }
            };

            ResearchDirections = new List<SelectListItem>
            {
                new SelectListItem {Value = "1", Text = "Экономика"}
                ,new SelectListItem {Value ="2" , Text ="Юриспруденция " }
                ,new SelectListItem {Value ="3" , Text ="Инструменты и приборы" }
                ,new SelectListItem {Value ="4" , Text ="Материаловедение – междисциплинарное " }
                ,new SelectListItem {Value ="5" , Text ="Науки об окружающей среде" }
            };

            Positions = new List<SelectListItem>
            {
                new SelectListItem {Value = "1", Text = "Руководитель научного подразделения"}
                ,new SelectListItem {Value ="2" , Text ="Ведущий научный сотрудник" }
                ,new SelectListItem {Value ="3" , Text ="Старший научный сотрудник" }
                ,new SelectListItem {Value ="4" , Text ="Младший научный сотрудник" }
                ,new SelectListItem {Value ="5" , Text ="Инженер-исследователь" }
            };

            Organizations = new List<SelectListItem>
            {
                new SelectListItem {Value = "1", Text = "Новосибирский государственный технический университет"}
                ,new SelectListItem {Value ="2" , Text ="Ульяновский государственный технический университет" }
                ,new SelectListItem {Value ="3" , Text ="ОАО \"Российские железные дороги\"" }
                ,new SelectListItem {Value ="4" , Text ="Дагестанский государственный технический университет" }
                ,new SelectListItem {Value ="5" , Text ="Волгоградский государственный технический университет" }
            };

            Salaries = new List<SelectListItem>
            {
                new SelectListItem {Value = "1", Text = "До 30 000 руб."}
                ,new SelectListItem {Value ="2" , Text ="30 000 – 60 000 руб." }
                ,new SelectListItem {Value ="3" , Text ="От 60 000 руб. и выше" }
            };

            VacancyStates = new List<SelectListItem>
            {
                new SelectListItem {Value = "1", Text = "Объявлен"}
                ,new SelectListItem {Value ="2" , Text ="Приём заявок" }
                ,new SelectListItem {Value ="3" , Text ="Завершён" }
            };
        }

        public IEnumerable<SelectListItem> Periods;
        public IEnumerable<SelectListItem> OrderBys;
        public IEnumerable<SelectListItem> Regions;
        public IEnumerable<SelectListItem> Foivs;
        public IEnumerable<SelectListItem> ResearchDirections;
        public IEnumerable<SelectListItem> Positions;
        public IEnumerable<SelectListItem> Organizations;
        public IEnumerable<SelectListItem> Salaries;
        public IEnumerable<SelectListItem> VacancyStates;

        public List<int> PageSize;

    }
}