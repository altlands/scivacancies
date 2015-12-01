using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MediatR;
using Microsoft.AspNet.Mvc.Rendering;
using SciVacancies.Domain.Enums;
using SciVacancies.WebApp.Queries;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Models
{
    public class VacanciesFilterSource
    {
        private readonly IMediator _mediator;
        public VacanciesFilterSource(IMediator mediator) { _mediator = mediator; }

        private List<ResearchDirectionViewModel> _researchDirections;

        public IEnumerable<SelectListItem> Periods => new List<SelectListItem>
        {
            new SelectListItem {Value = "0", Text = "За всё время"}
            ,
            new SelectListItem {Value = "30", Text = "За месяц"}
            ,
            new SelectListItem {Value = "7", Text = "За неделю"}
            ,
            new SelectListItem {Value = "1", Text = "за день"}
        };

        public IEnumerable<SelectListItem> OrderBys => new List<SelectListItem>
        {
            new SelectListItem {Value =ConstTerms.SearchFilterOrderByRelevant, Text ="По-релевантности" }
            ,new SelectListItem {Value = ConstTerms.SearchFilterOrderByDateDescending, Text = "Сначала последние"}
            ,new SelectListItem {Value =ConstTerms.SearchFilterOrderByDateAscending, Text ="Сначала первые" }
        };

        public IEnumerable<SelectListItem> Regions
        {
            get
            {
                return _mediator.Send(new SelectAllRegionsQuery())
                    ?.Select(c => new SelectListItem { Value = c.id.ToString(), Text = c.title });
            }
        }

        public IEnumerable<SelectListItem> Foivs
        {
            get
            {
                return _mediator.Send(new SelectAllFoivsQuery())?
                    .Where(c => c.parent_id == null)
                    .Where(c=>!c.title.ToLower().Contains("не") && !c.title.ToLower().Contains("указан"))
                    .Select(c => new SelectListItem { Value = c.id.ToString(), Text = c.title }).OrderBy(c=>c.Text);
            }
        }

        public List<ResearchDirectionViewModel> ResearchDirections
        {
            get
            {
                if (_researchDirections == null)
                {
                    var data = _mediator.Send(new SelectAllResearchDirectionsQuery());
                    if (data == null) return null;
                    var source =Mapper.Map<IEnumerable<ResearchDirectionViewModel>>(data);
                    if (source != null)
                    {
                        var allResearchDirections = source.ToList();
                        _researchDirections = allResearchDirections.Where(c => c.ParentId == 0).ToList(); //заполнили первый уровень
                        ResearchDirections.ForEach(firstLevelItem =>
                        {
                            firstLevelItem.Childs = allResearchDirections.Where(secondLevelItem => secondLevelItem.ParentId == firstLevelItem.Id).ToList(); //заполнили второй уровень
                            firstLevelItem.Childs.ForEach(secondLevelItem =>
                                {
                                    secondLevelItem.Childs = allResearchDirections.Where(f => f.ParentId == secondLevelItem.Id).ToList(); //заполнили третий уровень

                                    secondLevelItem.Childs.ForEach(thirdLevelItem =>
                                        {
                                            thirdLevelItem.Childs = allResearchDirections.Where(f => f.ParentId == thirdLevelItem.Id).ToList(); //заполнили четвертый уровень
                                        });

                                });
                        });
                    }
                }
                return _researchDirections;
            }
        }

        public IEnumerable<SelectListItem> PositionTypes => _mediator.Send(new SelectAllPositionTypesQuery())
            ?.Select(c => new SelectListItem { Value = c.id.ToString(), Text = c.title });


        public IEnumerable<SelectListItem> VacancyStates => new List<VacancyStatus>
        {
            VacancyStatus.Published,
            VacancyStatus.Closed,
            VacancyStatus.Cancelled
        }.Select(c => new SelectListItem { Value = ((int)c).ToString(), Text = c.GetDescription() });
    }
}