using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MediatR;
using Microsoft.AspNet.Mvc.Rendering;
using SciVacancies.Domain.Enums;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Queries;

namespace SciVacancies.WebApp.ViewModels
{
    public class VacanciesFilterSource
    {
        public VacanciesFilterSource(IMediator mediator)
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

            VacancyStates = new List<VacancyStatus>
            {
                VacancyStatus.Published
                ,VacancyStatus.InCommittee
                ,VacancyStatus.Closed
            }.Select(c => new SelectListItem { Value = ((int)c).ToString(), Text = c.GetDescription() });

            if (mediator != null)
            {
                Regions =
                    mediator.Send(new SelectAllRegionsQuery())
                        .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Title });

                var allFoivs = Mapper.Map<IEnumerable<FoivViewModel>>(mediator.Send(new SelectAllFoivsQuery())).ToList();
                Foivs = allFoivs.Where(c => c.ParentId == 0).ToList(); //заоплнили первый уровень
                Foivs.ForEach(firstLevelItem =>
                {
                    firstLevelItem.Childs = allFoivs.Where(secondLevelItem => secondLevelItem.ParentId == firstLevelItem.Id).ToList(); //заполнили второй уровень
                    firstLevelItem.Childs.ForEach(thirdLevelItem =>
                    {
                        thirdLevelItem.Items= allFoivs.Where(f => f.ParentId == thirdLevelItem.Id).Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Title }); //заполнили третий уровень
                    }); 
                });

                ResearchDirections =
                    mediator.Send(new SelectAllResearchDirectionsQuery())
                        .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Title });

                Positions =
                    mediator.Send(new SelectAllPositionTypesQuery())
                        .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Title }); ;

            }
        }


        public IEnumerable<SelectListItem> Periods;
        public IEnumerable<SelectListItem> OrderBys;
        public IEnumerable<SelectListItem> Regions;
        public List<FoivViewModel> Foivs;

        public IEnumerable<SelectListItem> ResearchDirections;
        public IEnumerable<SelectListItem> Positions;
        public IEnumerable<SelectListItem> VacancyStates;
        public List<int> PageSize;
    }
}