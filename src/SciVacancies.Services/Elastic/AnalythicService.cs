using SciVacancies.ReadModel.ElasticSearchModel.Model;
using SciVacancies.Domain.Enums;

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

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

            IDictionary<string, IAggregation> aggregations;
            try
            {
                aggregations = Elastic.Search<Vacancy>(searchSelector(q)).Aggregations;
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message, e);
                throw;
            }

            return aggregations;
        }
        public IDictionary<string, IAggregation> VacancyPayments(VacancyPaymentsByResearchDirectionAnalythicQuery q)
        {
            Func<VacancyPaymentsByResearchDirectionAnalythicQuery, SearchDescriptor<Vacancy>> searchSelector = VacancyPaymentsSearchDescriptor;

            IDictionary<string, IAggregation> aggregations;
            try
            {
                aggregations = Elastic.Search<Vacancy>(searchSelector(q)).Aggregations;
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message, e);
                throw;
            }

            return aggregations;
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

        SearchDescriptor<Vacancy> VacancyPaymentsSearchDescriptor(VacancyPaymentsByResearchDirectionAnalythicQuery q)
        {
            SearchDescriptor<Vacancy> sdescriptor = new SearchDescriptor<Vacancy>();

            sdescriptor.Take(0);
            sdescriptor.MinScore(0);

            Func<VacancyPaymentsByResearchDirectionAnalythicQuery, QueryContainer> querySelector = VacancyPaymentsQueryContainer;
            Func<AggregationDescriptor<Vacancy>, VacancyPaymentsByResearchDirectionAnalythicQuery, AggregationDescriptor<Vacancy>> aggregationSelector = VacancyPaymentsAggregationDescriptor;

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
        AggregationDescriptor<Vacancy> VacancyPaymentsAggregationDescriptor(AggregationDescriptor<Vacancy> aggDescriptor, VacancyPaymentsByResearchDirectionAnalythicQuery q)
        {
            aggDescriptor
                .Average("salary_from", av => av.Field(f => f.SalaryFrom))
                .Average("salary_to", av => av.Field(f => f.SalaryTo));
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
        QueryContainer VacancyPaymentsQueryContainer(VacancyPaymentsByResearchDirectionAnalythicQuery q)
        {
            Func<FilterDescriptor<Vacancy>, VacancyPaymentsByResearchDirectionAnalythicQuery, FilterContainer> filterSelector = VacancyPaymentsByResearchDirectionsFilteredFilterContainer;

            return Query<Vacancy>.Filtered(flt => flt
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

        FilterContainer VacancyPaymentsByResearchDirectionsFilteredFilterContainer(FilterDescriptor<Vacancy> filterDescriptor, VacancyPaymentsByResearchDirectionAnalythicQuery q)
        {
            FilterContainer filterContainer;

            List<FilterContainer> filters = new List<FilterContainer>();
            

            //filters.Add(new FilterDescriptor<Vacancy>().Bool(b => b
            //                                                .Must(m => m
            //                                                    .Term(t => t.ResearchDirectionId, q.ResearchDirectionId)
            //                                                )
            //                                            )
            //                                        );

            filters.Add(new FilterDescriptor<Vacancy>().Bool(b => b
                                                            .Must(mn => mn
                                                                .Terms(t => t.Status, new List<VacancyStatus> {
                                                                    VacancyStatus.Published
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

            IDictionary<string, IAggregation> aggregations;
            try
            {
                aggregations = Elastic.Search<Vacancy>(searchSelector(q)).Aggregations;
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message, e);
                throw;
            }

            return aggregations;
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
            aggDescriptor.DateHistogram("histogram", dt => dt
                     .Field(fd => fd.PublishDate)
                     .Interval(q.Interval)
                     .Aggregations(agg => agg
                        .Terms("position_ids", tm => tm
                                 .Field(f => f.PositionTypeId)
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
                                                                            .GreaterOrEquals(DateTime.UtcNow.AddDays((-7) * q.BarsNumber))
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
