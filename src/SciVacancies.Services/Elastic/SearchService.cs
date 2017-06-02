﻿using SciVacancies.ReadModel.ElasticSearchModel.Model;
using SciVacancies.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Nest;

namespace SciVacancies.Services.Elastic
{
    public class SearchService : ISearchService
    {
        private readonly IElasticClient _elastic;
        private readonly ILogger _logger;
        private readonly double _minscore;

        public SearchService(double minScore, IElasticClient elastic, ILoggerFactory loggerFactory)
        {
            _minscore = minScore;
            _elastic = elastic;
            _logger = loggerFactory.CreateLogger<SearchService>();
        }

        public ISearchResponse<Vacancy> VacancySearch(SearchQuery sq)
        {
            Func<SearchQuery, SearchDescriptor<Vacancy>> searchSelector = VacancySearchDescriptor;

            return _elastic.Search<Vacancy>(searchSelector(sq));
        }

        #region VacancyQueryContainers

        public SearchDescriptor<Vacancy> VacancySearchDescriptor(SearchQuery sq)
        {
            SearchDescriptor<Vacancy> sdescriptor = new SearchDescriptor<Vacancy>();

            if (sq.PageSize.HasValue && sq.CurrentPage.HasValue &&
                sq.PageSize.Value != 0 && sq.CurrentPage.Value != 0)
            {
                sdescriptor.Skip((int)((sq.CurrentPage - 1) * sq.PageSize));
                sdescriptor.Take((int)sq.PageSize);
            }

            _logger.LogInformation($" Parsed minscore from config : {_minscore}");
            sdescriptor.MinScore(_minscore);

            switch (sq.OrderFieldByDirection)
            {
                case ConstTerms.SearchFilterOrderByDateDescending:
                    sdescriptor.Sort(sort => sort.OnField(of => of.PublishDate).Descending());
                    break;
                case ConstTerms.SearchFilterOrderByDateAscending:
                    sdescriptor.Sort(sort => sort.OnField(of => of.PublishDate).Ascending());
                    break;
            }
            Func<SearchQuery, QueryContainer> querySelector = VacancyQueryContainer;

            sdescriptor.Query(querySelector(sq));

            return sdescriptor;
        }
        public QueryContainer VacancyQueryContainer(SearchQuery sq)
        {
            Func<QueryDescriptor<Vacancy>, SearchQuery, QueryContainer> querySelector = VacancyFilteredQueryContainer;
            Func<FilterDescriptor<Vacancy>, SearchQuery, FilterContainer> filterSelector = VacancyFilteredFilterContainer;

            return Query<Vacancy>.Filtered(flt => flt
                                    .Query(flq => querySelector(flq, sq))
                                    .Filter(flf => filterSelector(flf, sq))
                                );
        }
        public QueryContainer VacancyFilteredQueryContainer(QueryDescriptor<Vacancy> queryDescriptor, SearchQuery sq)
        {
            QueryContainer queryContainer;

            if (String.IsNullOrEmpty(sq.Query))
            {
                queryContainer = queryDescriptor.MatchAll();
            }
            else
            {
                queryContainer = queryDescriptor.MultiMatch(m => m
                                                 .Query(sq.Query)
                                                 .OnFieldsWithBoost(fb => fb
                                                     //.Add(f => f.FullName, 10.0)
                                                     .Add(f => f.Name, 10.0)
                                                     .Add(f => f.Tasks, 7.0)
                                                     .Add(f => f.Criterias.FirstOrDefault().CriteriaTitle, 7.0)
                                                     .Add(f => f.ResearchTheme, 2.0)
                                                     .Add(f => f.CityName, 2.0)
                                                     .Add(f => f.Details, 2.0)
                                                     .Add(f => f.Bonuses, 2.0)
                                                 )
                                            );
            }

            return queryContainer;
        }
        public FilterContainer VacancyFilteredFilterContainer(FilterDescriptor<Vacancy> filterDescriptor, SearchQuery sq)
        {
            var filters = new List<FilterContainer>();

            if (sq.FoivIds != null && sq.FoivIds.Any())
            {
                filters.Add(new FilterDescriptor<Vacancy>().Bool(b => b
                                                                .Must(m => m
                                                                    .Terms(t => t.OrganizationFoivId, sq.FoivIds)
                                                                )
                                                            )
                                                        );
            }
            if (sq.PositionTypeIds != null && sq.PositionTypeIds.Any())
            {
                filters.Add(new FilterDescriptor<Vacancy>().Bool(b => b
                                                                .Must(m => m
                                                                    .Terms(t => t.PositionTypeId, sq.PositionTypeIds)
                                                                )
                                                            )
                                                        );
            }
            if (sq.RegionIds != null && sq.RegionIds.Any())
            {
                filters.Add(new FilterDescriptor<Vacancy>().Bool(b => b
                                                                .Must(m => m
                                                                    .Terms(t => t.RegionId, sq.RegionIds)
                                                                )
                                                            )
                                                        );
            }
            if (sq.ResearchDirectionIds != null && sq.ResearchDirectionIds.Any())
            {
                filters.Add(new FilterDescriptor<Vacancy>().Bool(b => b
                                                                .Must(m => m
                                                                    .Terms(t => t.ResearchDirectionId, sq.ResearchDirectionIds)
                                                                )
                                                            )
                                                        );
            }
            if (sq.SalaryFrom.HasValue && sq.SalaryFrom > 0)
            {
                //filters.Add(new FilterDescriptor<Vacancy>().Bool(b => b
                //                                                .Must(m => m
                //                                                    .Range(r => r
                //                                                        .GreaterOrEquals(sq.SalaryFrom)
                //                                                        .OnField(of => of.SalaryFrom)
                //                                                    )
                //                                                )
                //                                            )
                //                                        );

                filters.Add(new FilterDescriptor<Vacancy>().Bool(b => b
                                                                .Must(m => m
                                                                        .Missing(ms => ms.SalaryFrom)
                                                                        || m.Range(r => r
                                                                             .GreaterOrEquals(sq.SalaryFrom)
                                                                             .OnField(of => of.SalaryFrom)
                                                                            )
                                                                        || m.Bool(bb => bb
                                                                                    .Must(mm => mm
                                                                                        .Range(r => r
                                                                                            .LowerOrEquals(sq.SalaryFrom)
                                                                                            .OnField(of => of.SalaryFrom)
                                                                                        )
                                                                                        && mm.Range(r => r
                                                                                             .GreaterOrEquals(sq.SalaryFrom)
                                                                                             .OnField(of => of.SalaryTo)
                                                                                        )
                                                                                    )
                                                                            )
                                                                )
                                                            )
                                                        );
            }
            if (sq.SalaryTo.HasValue && sq.SalaryTo > 0)
            {
                filters.Add(new FilterDescriptor<Vacancy>().Bool(b => b
                                                                .Must(m => m
                                                                        .Missing(ms => ms.SalaryTo)
                                                                        || m.Range(r => r
                                                                             .LowerOrEquals(sq.SalaryTo)
                                                                             .OnField(of => of.SalaryTo)
                                                                            )
                                                                        || m.Bool(bb => bb
                                                                                    .Must(mm => mm
                                                                                        .Range(r => r
                                                                                            .GreaterOrEquals(sq.SalaryTo)
                                                                                            .OnField(of => of.SalaryTo)
                                                                                        )
                                                                                        && mm.Range(r => r
                                                                                             .LowerOrEquals(sq.SalaryTo)
                                                                                             .OnField(of => of.SalaryFrom)
                                                                                        )
                                                                                    )
                                                                            )
                                                                )
                                                            )
                                                        );

                //filters.Add(new FilterDescriptor<Vacancy>().Bool(b => b
                //                                                .Must(m => m
                //                                                    .Range(r => r
                //                                                        .LowerOrEquals(sq.SalaryTo)
                //                                                        .OnField(of => of.SalaryTo)
                //                                                    )
                //                                                )
                //                                            )
                //                                        );
            }
            if (sq.VacancyStatuses != null && sq.VacancyStatuses.Any())
            {
                filters.Add(new FilterDescriptor<Vacancy>().Bool(b => b
                                                                .Must(m => m
                                                                    .Terms(t => t.Status, sq.VacancyStatuses)
                                                                )
                                                            )
                                                        );
            }
            if (sq.PublishDateFrom.HasValue)
            {
                filters.Add(new FilterDescriptor<Vacancy>().Bool(b => b
                                                                .Must(m => m
                                                                    .Range(r => r
                                                                        .GreaterOrEquals(sq.PublishDateFrom)
                                                                        .OnField(of => of.PublishDate)
                                                                    )
                                                                )
                                                            )
                                                        );
            }
            filters.Add(new FilterDescriptor<Vacancy>().Bool(b => b
                                                            .MustNot(mn => mn
                                                                .Terms(t => t.Status, new List<VacancyStatus> {
                                                                    VacancyStatus.InProcess
                                                                })
                                                            )
                                                        )
                                                    );

            return filterDescriptor.Bool(b => b.Must(filters.ToArray()));
        }

        #endregion
    }
}