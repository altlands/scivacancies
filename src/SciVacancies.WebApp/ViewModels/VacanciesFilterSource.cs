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

                Foivs = mediator.Send(new SelectAllFoivsQuery())
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Title });

                var allFoivs = Mapper.Map<IEnumerable<FoivViewModel>>(mediator.Send(new SelectAllFoivsQuery())).ToList();
                HFoivs = allFoivs.Where(c => c.ParentId == null).ToList(); //заоплнили первый уровень
                HFoivs.ForEach(firstLevelItem =>
                {
                    firstLevelItem.Items = allFoivs.Where(secondLevelItem => secondLevelItem.ParentId == firstLevelItem.Id).ToList().Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Title }); //заполнили второй уровень
                });

                var allResearchDirections = Mapper.Map<IEnumerable<ResearchDirectionViewModel>>(mediator.Send(new SelectAllResearchDirectionsQuery())).ToList();
                ResearchDirections = allResearchDirections.Where(c => c.ParentId == 0).ToList(); //заоплнили первый уровень
                ResearchDirections.ForEach(firstLevelItem =>
                {
                    firstLevelItem.Childs = allResearchDirections.Where(secondLevelItem => secondLevelItem.ParentId == firstLevelItem.Id).ToList(); //заполнили второй уровень
                    firstLevelItem.Childs.ForEach(secondLevelItem =>
                    {
                        secondLevelItem.Items = allResearchDirections.Where(f => f.ParentId == secondLevelItem.Id).Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Title }); //заполнили третий уровень
                    });
                });


                
                Positions =
                    mediator.Send(new SelectAllPositionTypesQuery())
                        .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Title }); ;

            }
        }


        public IEnumerable<SelectListItem> Periods;
        public IEnumerable<SelectListItem> OrderBys;
        public IEnumerable<SelectListItem> Regions;

        public IEnumerable<SelectListItem> Foivs;
        public List<FoivViewModel> HFoivs;
        public List<ResearchDirectionViewModel> ResearchDirections;

        public IEnumerable<SelectListItem> Positions;
        public IEnumerable<SelectListItem> VacancyStates;
        public List<int> PageSize;
    }
}