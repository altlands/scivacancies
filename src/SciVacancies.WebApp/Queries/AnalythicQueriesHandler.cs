using SciVacancies.ReadModel.Core;
using SciVacancies.Services.Elastic;
using SciVacancies.WebApp.Models.Analythic;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Framework.OptionsModel;
using Microsoft.Framework.Caching.Memory;

using MediatR;
using Nest;
using NPoco;
using System.Globalization;

namespace SciVacancies.WebApp.Queries
{
    public class AnalythicQueriesHandler :
        IRequestHandler<AverageVacancyAndPositionAnalythicQuery, List<PositionsHistogram>>,
        IRequestHandler<AveragePaymentAndVacancyCountByRegionAnalythicQuery, List<PaymentsHistogram>>
    {
        private readonly IDatabase _db;
        private readonly IMemoryCache Cache;
        private readonly IAnalythicService AnalythicService;
        private readonly IOptions<CacheSettings> CacheSettings;
        private MemoryCacheEntryOptions CacheOptions
        {
            get
            {
                return new MemoryCacheEntryOptions().SetAbsoluteExpiration(DateTimeOffset.Now.AddSeconds(CacheSettings.Value.DictionaryExpiration));
            }
        }

        public AnalythicQueriesHandler(IDatabase db, IAnalythicService analythicService, IMemoryCache cache, IOptions<CacheSettings> cacheSettings)
        {
            this._db = db;
            this.AnalythicService = analythicService;
            this.Cache = cache;
            this.CacheSettings = cacheSettings;
        }

        public List<PositionsHistogram> Handle(AverageVacancyAndPositionAnalythicQuery message)
        {
            IEnumerable<PositionType> positionTypes;
            if (!Cache.TryGetValue<IEnumerable<PositionType>>("d_positiontypes", out positionTypes))
            {
                positionTypes = _db.Fetch<PositionType>();
                Cache.Set<IEnumerable<PositionType>>("d_positiontypes", positionTypes, CacheOptions);
            }

            IDictionary<string, Nest.IAggregation> aggregations;
            aggregations = AnalythicService.VacancyPositions(message);

            List<PositionsHistogram> histograms = new List<PositionsHistogram>();

            Bucket positionsBucket = aggregations["position_ids"] as Bucket;

            foreach (KeyItem keyItem in positionsBucket.Items)
            {
                PositionsHistogram histogram = new PositionsHistogram
                {
                    type = "stackedColumn",
                    toolTipContent = "{label}<br/><span style='\"'color: {color};'\"'><strong>{name}</strong></span>: {y} вакансий",
                    name = positionTypes.FirstOrDefault(f => f.id == Int32.Parse(keyItem.Key)).title,
                    showInLegend = true
                };

                Bucket dateBucket = keyItem.Aggregations["histogram"] as Bucket;
                foreach (HistogramItem histogramItem in dateBucket.Items)
                {
                    switch (message.Interval)
                    {
                        case DateInterval.Month:
                            histogram.dataPoints.Add(new PositionDataPoint
                            {
                                x = histogramItem.Date.Ticks,
                                y = histogramItem.DocCount,
                                label = histogramItem.Date.ToString("MMMM", new CultureInfo("ru-RU"))
                            });
                            break;
                        case DateInterval.Week:
                            histogram.dataPoints.Add(new PositionDataPoint
                            {
                                x = histogramItem.Date.Ticks,
                                y = histogramItem.DocCount,
                                label = histogramItem.Date.ToString("dd MMMM", new CultureInfo("ru-RU")) + " - " + histogramItem.Date.AddDays(6).ToString("dd MMMM", new CultureInfo("ru-RU"))
                            });
                            break;
                        case DateInterval.Day:
                            histogram.dataPoints.Add(new PositionDataPoint
                            {
                                x = histogramItem.Date.Ticks,
                                y = histogramItem.DocCount,
                                label = histogramItem.Date.ToString("dddd", new CultureInfo("ru-RU"))
                            });
                            break;
                    }
                }

                histograms.Add(histogram);
            }

            return histograms;
        }

        public List<PaymentsHistogram> Handle(AveragePaymentAndVacancyCountByRegionAnalythicQuery message)
        {
            IDictionary<string, Nest.IAggregation> aggregations;
            aggregations = AnalythicService.VacancyPayments(message);

            List<PaymentsHistogram> histograms = new List<PaymentsHistogram>();

            Bucket dateBucket = aggregations["histogram"] as Bucket;

            PaymentsHistogram averageHistogram = new PaymentsHistogram
            {
                type = "line",
                axisYType = "primary",
                name = "Средняя зп",
                showInLegend = true
            };
            PaymentsHistogram countHistogram = new PaymentsHistogram
            {
                type = "line",
                axisYType = "secondary",
                name = "Вакансий",
                showInLegend = true
            };

            foreach (HistogramItem histogramItem in dateBucket.Items)
            {
                ValueMetric salaryFrom = histogramItem.Aggregations["salary_from"] as ValueMetric;
                ValueMetric salaryTo = histogramItem.Aggregations["salary_to"] as ValueMetric;

                switch (message.Interval)
                {
                    case DateInterval.Month:
                        averageHistogram.dataPoints.Add(new PaymentDataPoint
                        {
                            x = histogramItem.Date.Ticks,
                            y = (double)((salaryFrom.Value + salaryTo.Value) / 2),
                            label = histogramItem.Date.ToString("MMMM", new CultureInfo("ru-RU"))
                        });

                        countHistogram.dataPoints.Add(new PaymentDataPoint
                        {
                            x = histogramItem.Date.Ticks,
                            y = histogramItem.DocCount,
                            label = histogramItem.Date.ToString("MMMM", new CultureInfo("ru-RU"))
                        });
                        break;
                    case DateInterval.Week:
                        averageHistogram.dataPoints.Add(new PaymentDataPoint
                        {
                            x = histogramItem.Date.Ticks,
                            y = (double)((salaryFrom.Value + salaryTo.Value) / 2),
                            label = histogramItem.Date.ToString("dd MMMM", new CultureInfo("ru-RU")) + " - " + histogramItem.Date.AddDays(6).ToString("dd MMMM", new CultureInfo("ru-RU"))
                        });

                        countHistogram.dataPoints.Add(new PaymentDataPoint
                        {
                            x = histogramItem.Date.Ticks,
                            y = histogramItem.DocCount,
                            label = histogramItem.Date.ToString("dd MMMM", new CultureInfo("ru-RU")) + " - " + histogramItem.Date.AddDays(6).ToString("dd MMMM", new CultureInfo("ru-RU"))
                        });
                        break;
                    case DateInterval.Day:
                        averageHistogram.dataPoints.Add(new PaymentDataPoint
                        {
                            x = histogramItem.Date.Ticks,
                            y = (double)((salaryFrom.Value + salaryTo.Value) / 2),
                            label = histogramItem.Date.ToString("dddd", new CultureInfo("ru-RU"))
                        });

                        countHistogram.dataPoints.Add(new PaymentDataPoint
                        {
                            x = histogramItem.Date.Ticks,
                            y = histogramItem.DocCount,
                            label = histogramItem.Date.ToString("dddd", new CultureInfo("ru-RU"))
                        });
                        break;
                }
            }

            histograms.Add(averageHistogram);
            histograms.Add(countHistogram);

            return histograms;
        }
    }
}
