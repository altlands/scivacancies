using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel.ElasticSearchModel.Model;
using SciVacancies.WebApp.Engine;

using System;
using System.Collections.Generic;
using System.Linq;

using Nest;
using MediatR;
using NPoco;

namespace SciVacancies.WebApp.Queries
{
    public class SearchQueriesHandler : IRequestHandler<SearchQuery, Page<Vacancy>>
    {
        private readonly IElasticClient _elastic;

        public SearchQueriesHandler(IElasticClient elastic)
        {
            _elastic = elastic;
        }

        public Page<Vacancy> Handle(SearchQuery msg)
        {
            Func<SearchQuery, SearchDescriptor<Vacancy>> searchSelector = VacancySearchDescriptor;

            var results = _elastic.Search<Vacancy>(searchSelector(msg));

            var pageVacancies = new Page<Vacancy>
            {
                CurrentPage = msg.CurrentPage.Value,
                ItemsPerPage = msg.PageSize.Value,
                TotalItems = results.Total,
                TotalPages = msg.PageSize.HasValue ? results.Total / msg.PageSize.Value : 0,
                Items = results.Documents.ToList()
            };

            return pageVacancies;
        }

        #region QueryContainers

        public SearchDescriptor<Vacancy> VacancySearchDescriptor(SearchQuery sq)
        {
            SearchDescriptor<Vacancy> sdescriptor = new SearchDescriptor<Vacancy>();

            sdescriptor.Index("scivacancies");
            if (sq.PageSize.Value != 0 && sq.CurrentPage.Value != 0)
            {
                sdescriptor.Skip((int)((sq.CurrentPage - 1) * sq.PageSize));
                sdescriptor.Take((int)sq.PageSize);
            }
            switch (sq.OrderBy)
            {
                case ConstTerms.OrderByDateDescending:
                    sdescriptor.Sort(sort => sort.OnField(of => of.PublishDate).Descending());
                    break;
                case ConstTerms.OrderByDateAscending:
                    sdescriptor.Sort(sort => sort.OnField(of => of.PublishDate).Ascending());
                    break;
            }
            Func<SearchQuery, QueryContainer> querySelector = VacancyQueryContainer;

            sdescriptor.Query(querySelector(sq));

            return sdescriptor;
        }
        public QueryContainer VacancyQueryContainer(SearchQuery sq)
        {

            QueryContainer query = Query<Vacancy>.Filtered(fltd => fltd
                                                        .Query(q => q
                                                            .FuzzyLikeThis(flt => flt
                                                                .LikeText(sq.Query)
                                                            )
                                                        )
                                                        .Filter(f => f
                                                            .Terms<int>(ft => ft.OrganizationFoivId, sq.FoivIds)
                                                            && f.Terms<int>(ft => ft.PositionTypeId, sq.PositionTypeIds)
                                                            && f.Terms<int>(ft => ft.RegionId, sq.RegionIds)
                                                            && f.Terms<int>(ft => ft.ResearchDirectionId, sq.ResearchDirectionIds)
                                                            && f.Range(fr => fr
                                                                    .GreaterOrEquals((long?)sq.SalaryFrom)
                                                                    .OnField(of => of.SalaryFrom)
                                                            )
                                                            && f.Range(fr => fr
                                                                    .LowerOrEquals((long?)sq.SalaryTo)
                                                                    .OnField(of => of.SalaryTo)
                                                            )
                                                            && f.Terms<VacancyStatus>(ft => ft.Status, sq.VacancyStatuses)
                                                            && f.Range(fr => fr
                                                                    .GreaterOrEquals(sq.PublishDateFrom)
                                                                    .OnField(of => of.PublishDate)
                                                            )
                                                            && f.Bool(b => b
                                                                    .MustNot(mn => mn
                                                                        .Terms<VacancyStatus>(mnt => mnt.Status, new List<VacancyStatus> { VacancyStatus.InProcess })
                                                                    )
                                                            )
                                                        )
                                                );

            return query;
        }

        #endregion
    }
}