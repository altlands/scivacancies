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
        private readonly IMediator _mediator;
        public VacanciesFilterSource(IMediator mediator){_mediator = mediator;}

        private IEnumerable<SelectListItem> _regions;
        private IEnumerable<SelectListItem> _vacancyStates;
        private IEnumerable<SelectListItem> _periods;
        private IEnumerable<SelectListItem> _orderBys;
        private IEnumerable<SelectListItem> _positions;
        private List<FoivViewModel> _foivs;
        private List<ResearchDirectionViewModel> _researchDirections;


        public IEnumerable<SelectListItem> Periods
        {
            get
            {
                return _periods ?? (_periods = new List<SelectListItem>
                {
                    new SelectListItem {Value = "0", Text = "За всё время"}
                    ,
                    new SelectListItem {Value = "30", Text = "За месяц"}
                    ,
                    new SelectListItem {Value = "7", Text = "За неделю"}
                    ,
                    new SelectListItem {Value = "1", Text = "за день"}
                });
            }
        }

        public IEnumerable<SelectListItem> OrderBys
        {
            get
            {
                return _orderBys ?? (_orderBys = new List<SelectListItem>
                    {
                        new SelectListItem {Value =ConstTerms.OrderByRelevant, Text ="По-релевантности" }
                        ,new SelectListItem {Value = ConstTerms.OrderByDateDescending, Text = "Сначала последние"}
                        ,new SelectListItem {Value =ConstTerms.OrderByDateAscending , Text ="Сначала первые" }
                    });
            }
        }

        public IEnumerable<SelectListItem> Regions
        {
            get
            {
                return _regions ?? (_regions = _mediator.Send(new SelectAllRegionsQuery())
                    .Select(c => new SelectListItem {Value = c.id.ToString(), Text = c.title}));
            }
        }

        public List<FoivViewModel> Foivs
        {
            get
            {
                if (_foivs == null)
                {
                    var allFoivs = Mapper.Map<IEnumerable<FoivViewModel>>(_mediator.Send(new SelectAllFoivsQuery())).ToList();
                    _foivs = allFoivs.Where(c => c.ParentId == null).ToList(); //заполнили первый уровень
                    Foivs.ForEach(firstLevelItem =>
                    {
                        firstLevelItem.Childs = allFoivs.Where(secondLevelItem => secondLevelItem.ParentId == firstLevelItem.Id).ToList(); //заполнили второй уровень
                    });
                }
                return _foivs;
            } 
        }

        public List<ResearchDirectionViewModel> ResearchDirections
        {
            get
            {
                if (_researchDirections == null)
                {
                    var allResearchDirections = Mapper.Map<IEnumerable<ResearchDirectionViewModel>>(_mediator.Send(new SelectAllResearchDirectionsQuery())).ToList();
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
                return _researchDirections;
            }
        }

        public IEnumerable<SelectListItem> Positions
        {
            get
            {
                return _positions ?? (_positions = _mediator.Send(new SelectAllPositionTypesQuery())
                    .Select(c => new SelectListItem { Value = c.id.ToString(), Text = c.title }));
            }
        }


        public IEnumerable<SelectListItem> VacancyStates
        {
            get
            {
                if (_mediator != null && _vacancyStates == null)
                    _vacancyStates = new List<VacancyStatus>
                        {
                            VacancyStatus.Published
                            ,VacancyStatus.InCommittee
                            ,VacancyStatus.Closed
                        }.Select(c => new SelectListItem { Value = ((int)c).ToString(), Text = c.GetDescription() });
                return _vacancyStates;
            }
        }

    }
}