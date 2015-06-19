using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc.Rendering;
using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel;
using SciVacancies.WebApp.Engine;

namespace SciVacancies.WebApp.ViewModels
{
    public class VacanciesFilterSource
    {
        public VacanciesFilterSource(IReadModelService readModelService)
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
                new SelectListItem {Value = ConstTerms.OrderByDateDescending, Text = "Сначала последние"}
                ,new SelectListItem {Value =ConstTerms.OrderByDateAscending , Text ="Сначала первые" }
            };

            Salaries = new List<SelectListItem>
            {
                new SelectListItem {Value = "1", Text = "До 30 000 руб."}
                ,new SelectListItem {Value ="2" , Text ="30 000 – 60 000 руб." }
                ,new SelectListItem {Value ="3" , Text ="От 60 000 руб. и выше" }
            };

            VacancyStates = new List<VacancyStatus>
            {
                VacancyStatus.Published
                ,VacancyStatus.AppliesAcceptance
                ,VacancyStatus.Closed
            }.Select(c => new SelectListItem { Value = ((int)c).ToString(), Text = c.GetDescription() });

            if (readModelService != null)
            {
                Regions =
                    readModelService.SelectRegions()
                        .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Title });

                Foivs =
                    readModelService.SelectFoivs()
                        .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Title });

                ResearchDirections =
                    readModelService.SelectResearchDirections()
                        .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Title });

                //Positions =
                //    _readModelService.SelectPositionTypes()
                //        .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Title }); ;

                Organizations =
                    readModelService.SelectOrganizations("name_descending", 50, 1).Items
                        .Select(c => new SelectListItem { Value = c.Guid.ToString(), Text = c.Name });
            }
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