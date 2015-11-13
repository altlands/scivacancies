using SciVacancies.ReadModel.ElasticSearchModel.Model;
using SciVacancies.Domain.Enums;

using System;
using System.Collections.Generic;
using Microsoft.Framework.Logging;

using Nest;

namespace SciVacancies.Services.Elastic
{
    public class AnalythicService : IAnalythicService
    {
        readonly IElasticClient Elastic;
        readonly ILogger Logger;

        public AnalythicService(IElasticClient elastic, ILoggerFactory loggerFactory)
        {
            this.Elastic = elastic;
            this.Logger = loggerFactory.CreateLogger<AnalythicService>();
        }

        public IDictionary<string, IAggregation> VacancyPayments(VacancyPaymentsAnalythicQuery q)
        {
            Func<VacancyPaymentsAnalythicQuery, SearchDescriptor<Vacancy>> searchSelector = VacancyPaymentsSearchDescriptor;

            return Elastic.Search<Vacancy>(searchSelector(q)).Aggregations;
        }

        #region VacancyPaymentsQueryContainers

        SearchDescriptor<Vacancy> VacancyPaymentsSearchDescriptor(VacancyPaymentsAnalythicQuery q)
        {
            SearchDescriptor<Vacancy> sdescriptor = new SearchDescriptor<Vacancy>();

            sdescriptor.Take(0);
            sdescriptor.MinScore(0);

            Func<VacancyPaymentsAnalythicQuery, QueryContainer> querySelector = VacancyPaymentsQueryContainer;
            Func<AggregationDescriptor<Vacancy>, VacancyPaymentsAnalythicQuery, AggregationDescriptor<Vacancy>> aggregationSelector = VacancyPaymentsAggregationDescriptor;

            sdescriptor.Query(querySelector(q));

            sdescriptor.Aggregations(ag => aggregationSelector(ag, q));

            return sdescriptor;
        }
        AggregationDescriptor<Vacancy> VacancyPaymentsAggregationDescriptor(AggregationDescriptor<Vacancy> aggDescriptor, VacancyPaymentsAnalythicQuery q)
        {
            aggDescriptor.DateHistogram("histogram", dt => dt
                                 .Field(fd => fd.PublishDate)
                                 .Interval(q.Interval)
                                 .Aggregations(agg => agg
                                    .Average("salary_from", av => av
                                         .Field(f => f.SalaryFrom)
                                    )
                                    .Average("salary_to", av => av
                                         .Field(f => f.SalaryTo)
                                    )
                                 )
                            );

            return aggDescriptor;
        }
        QueryContainer VacancyPaymentsQueryContainer(VacancyPaymentsAnalythicQuery q)
        {
            Func<QueryDescriptor<Vacancy>, VacancyPaymentsAnalythicQuery, QueryContainer> querySelector = VacancyPaymentsFilteredQueryContainer;
            Func<FilterDescriptor<Vacancy>, VacancyPaymentsAnalythicQuery, FilterContainer> filterSelector = VacancyPaymentsFilteredFilterContainer;

            return Query<Vacancy>.Filtered(flt => flt
                                    .Query(flq => querySelector(flq, q))
                                    .Filter(flf => filterSelector(flf, q))
            );
        }
        QueryContainer VacancyPaymentsFilteredQueryContainer(QueryDescriptor<Vacancy> queryDescriptor, VacancyPaymentsAnalythicQuery q)
        {
            QueryContainer queryContainer;

            queryContainer = queryDescriptor.MatchAll();

            return queryContainer;
        }
        FilterContainer VacancyPaymentsFilteredFilterContainer(FilterDescriptor<Vacancy> filterDescriptor, VacancyPaymentsAnalythicQuery q)
        {
            FilterContainer filterContainer;

            List<FilterContainer> filters = new List<FilterContainer>();

            switch (q.Interval)
            {
                case DateInterval.Month:
                    filters.Add(new FilterDescriptor<Vacancy>().Bool(b => b
                                                                    .Must(m => m
                                                                        .Range(r => r
                                                                            .GreaterOrEquals(DateTime.UtcNow.AddMonths((-1) * q.BarsNumber))
                                                                            .OnField(f => f.PublishDate)
                                                                        )

                                                                    )
                                                                )
                                                        );
                    break;
                case DateInterval.Week:
                    filters.Add(new FilterDescriptor<Vacancy>().Bool(b => b
                                                                    .Must(m => m
                                                                        .Range(r => r
                                                                            .GreaterOrEquals(DateTime.UtcNow.AddDays((-1) * q.BarsNumber * 7))
                                                                            .OnField(f => f.PublishDate)
                                                                        )
                                                                    )
                                                                )
                                                        );
                    break;

                case DateInterval.Day:
                    filters.Add(new FilterDescriptor<Vacancy>().Bool(b => b
                                                                    .Must(m => m
                                                                        .Range(r => r
                                                                            .GreaterOrEquals(DateTime.UtcNow.AddDays((-1) * q.BarsNumber))
                                                                            .OnField(f => f.PublishDate)
                                                                        )
                                                                    )
                                                                )
                                                        );
                    break;
            }

            if (q.RegionId != null)
            {
                filters.Add(new FilterDescriptor<Vacancy>().Bool(b => b
                                                                .Must(m => m
                                                                    .Term(t => t.RegionId, q.RegionId)
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

            filterContainer = filterDescriptor.Bool(b => b.Must(filters.ToArray()));

            return filterContainer;
        }

        #endregion

        public IDictionary<string, IAggregation> VacancyPositions(VacancyPositionsAnalythicQuery q)
        {
            Func<VacancyPositionsAnalythicQuery, SearchDescriptor<Vacancy>> searchSelector = VacancyPositionsSearchDescriptor;

            return Elastic.Search<Vacancy>(searchSelector(q)).Aggregations;
        }

        #region VacancyPositionsQueryContainers

        public SearchDescriptor<Vacancy> VacancyPositionsSearchDescriptor(VacancyPositionsAnalythicQuery q)
        {
            SearchDescriptor<Vacancy> sdescriptor = new SearchDescriptor<Vacancy>();

            sdescriptor.Take(0);
            sdescriptor.MinScore(0);

            Func<VacancyPositionsAnalythicQuery, QueryContainer> querySelector = VacancyPositionsQueryContainer;

            Func<AggregationDescriptor<Vacancy>, VacancyPositionsAnalythicQuery, AggregationDescriptor<Vacancy>> aggregationSelector = VacancyPositionAggregationDescriptor;

            sdescriptor.Query(querySelector(q));

            sdescriptor.Aggregations(ag => aggregationSelector(ag, q));

            return sdescriptor;
        }
        public AggregationDescriptor<Vacancy> VacancyPositionAggregationDescriptor(AggregationDescriptor<Vacancy> aggDescriptor, VacancyPositionsAnalythicQuery q)
        {
            aggDescriptor.Terms("position_ids", tm => tm
                                    .Field(f => f.PositionTypeId)
                                    .Aggregations(agg => agg
                                            .DateHistogram("histogram", dt => dt
                                                 .Field(fd => fd.PublishDate)
                                                 .Interval(q.Interval)

                                            )
                                    )
                        );

            return aggDescriptor;
        }
        public QueryContainer VacancyPositionsQueryContainer(VacancyPositionsAnalythicQuery q)
        {
            Func<QueryDescriptor<Vacancy>, VacancyPositionsAnalythicQuery, QueryContainer> querySelector = VacancyPositionsFilteredQueryContainer;
            Func<FilterDescriptor<Vacancy>, VacancyPositionsAnalythicQuery, FilterContainer> filterSelector = VacancyPositionsFilteredFilterContainer;

            return Query<Vacancy>.Filtered(flt => flt
                                    .Query(flq => querySelector(flq, q))
                                    .Filter(flf => filterSelector(flf, q))
            );
        }
        public QueryContainer VacancyPositionsFilteredQueryContainer(QueryDescriptor<Vacancy> queryDescriptor, VacancyPositionsAnalythicQuery q)
        {
            QueryContainer queryContainer;

            queryContainer = queryDescriptor.MatchAll();

            return queryContainer;
        }
        public FilterContainer VacancyPositionsFilteredFilterContainer(FilterDescriptor<Vacancy> filterDescriptor, VacancyPositionsAnalythicQuery q)
        {
            FilterContainer filterContainer;

            List<FilterContainer> filters = new List<FilterContainer>();

            switch (q.Interval)
            {
                case DateInterval.Month:
                    filters.Add(new FilterDescriptor<Vacancy>().Bool(b => b
                                                                    .Must(m => m
                                                                        .Range(r => r
                                                                            .GreaterOrEquals(DateTime.UtcNow.AddMonths((-1) * q.BarsNumber))
                                                                            .OnField(f => f.PublishDate)
                                                                        )

                                                                    )
                                                                )
                                                        );
                    break;
                case DateInterval.Week:
                    filters.Add(new FilterDescriptor<Vacancy>().Bool(b => b
                                                                    .Must(m => m
                                                                        .Range(r => r
                                                                            .GreaterOrEquals(DateTime.UtcNow.AddDays((-1) * q.BarsNumber * 7))
                                                                            .OnField(f => f.PublishDate)
                                                                        )
                                                                    )
                                                                )
                                                        );
                    break;

                case DateInterval.Day:
                    filters.Add(new FilterDescriptor<Vacancy>().Bool(b => b
                                                                    .Must(m => m
                                                                        .Range(r => r
                                                                            .GreaterOrEquals(DateTime.UtcNow.AddDays((-1) * q.BarsNumber))
                                                                            .OnField(f => f.PublishDate)
                                                                        )
                                                                    )
                                                                )
                                                        );
                    break;
            }

            if (q.RegionId != null)
            {
                filters.Add(new FilterDescriptor<Vacancy>().Bool(b => b
                                                                .Must(m => m
                                                                    .Term(t => t.RegionId, q.RegionId)
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

            filterContainer = filterDescriptor.Bool(b => b.Must(filters.ToArray()));

            return filterContainer;
        }

        #endregion
    }
}
