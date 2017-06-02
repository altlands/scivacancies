using SciVacancies.ReadModel.Core;
using SciVacancies.Services.Elastic;
using SciVacancies.WebApp.Models.Analythic;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

using MediatR;
using Nest;
using NPoco;
using System.Globalization;
using SciVacancies.WebApp.Engine;

namespace SciVacancies.WebApp.Queries
{
    public class AnalythicQueriesHandler :
        IRequestHandler<AverageVacancyAndPositionAnalythicQuery, List<PositionsHistogram>>,
        IRequestHandler<AveragePaymentAndVacancyCountByRegionAnalythicQuery, List<PaymentsHistogram>>
    {
        private readonly IMediator _mediator;
        private readonly IMemoryCache Cache;
        private readonly ILogger Logger;
        private readonly IOptions<AnalythicSettings> AnalythicSettings;
        private readonly IAnalythicService AnalythicService;
        private readonly IOptions<CacheSettings> CacheSettings;
        private MemoryCacheEntryOptions CacheOptions
        {
            get
            {
                return new MemoryCacheEntryOptions().SetAbsoluteExpiration(DateTimeOffset.Now.AddSeconds(CacheSettings.Value.MainPageExpiration));
            }
        }

        public AnalythicQueriesHandler(IMediator mediator, ILoggerFactory loggerFactory, IOptions<AnalythicSettings> analythicSettings, IAnalythicService analythicService, IMemoryCache cache, IOptions<CacheSettings> cacheSettings)
        {
            this._mediator = mediator;
            this.Logger = loggerFactory.CreateLogger<AnalythicQueriesHandler>();
            //Logger.LogInformation("Test FOR ANALITYC QUERY HANDLER");
            this.AnalythicSettings = analythicSettings;
            this.AnalythicService = analythicService;
            this.Cache = cache;
            this.CacheSettings = cacheSettings;
        }

        public List<PositionsHistogram> Handle(AverageVacancyAndPositionAnalythicQuery message)
        {
            List<PositionsHistogram> histograms;
            string positionsCacheKey = "a_positions_" + message.Interval.ToString() + "_region_" + message.RegionId.ToString();
            if (!Cache.TryGetValue<List<PositionsHistogram>>(positionsCacheKey, out histograms))
            {
                //IEnumerable<PositionType> positionTypes;
                //try
                //{
                //    positionTypes = _mediator.Send(new SelectAllPositionTypesQuery());
                //}
                //catch (Exception e)
                //{
                //    Logger.LogError(e.Message, e);
                //    throw;
                //}

                IDictionary<string, IAggregation> aggregations;
                try
                {
                    aggregations = AnalythicService.VacancyPositions(message);
                }
                catch (Exception e)
                {
                    Logger.LogError(e.Message, e);
                    throw;
                }

                histograms = new List<PositionsHistogram>();
                Dictionary<string, PositionsHistogram> positionsDictionary = new Dictionary<string, PositionsHistogram>();

                if (aggregations == null)
                    throw new Exception($"{nameof(aggregations)} is null");

                if (!aggregations.ContainsKey("histogram"))
                    throw new Exception("aggregations[\"histogram\"] is null");

                Bucket dateBucket = aggregations["histogram"] as Bucket;

                int k = AnalythicSettings.Value.BarsNumber - 1;
                if ((bool) dateBucket?.Items?.Any())
                {
                    for (int i = dateBucket.Items.Count() - 1; i >= 0 && k >= 0; i--, k--)
                    {
                        HistogramItem currentItem = dateBucket.Items.ElementAt(i) as HistogramItem;

                        #region добавляем новую гистограмму, если есть новый ключ-id позиции
                        Bucket positionIds = currentItem.Aggregations["position_ids"] as Bucket;
                        List<KeyItem> keyItems = new List<KeyItem>();
                        foreach (KeyItem keyItem in positionIds.Items)
                        {
                            keyItems.Add(keyItem);
                        }

                        if (positionIds?.Items?.Count() > 0)
                        {
                            foreach (KeyItem keyItem in positionIds.Items)
                            {
                                if (!positionsDictionary.ContainsKey(keyItem.Key))
                                {
                                    positionsDictionary.Add(keyItem.Key, new PositionsHistogram
                                    {
                                        type = "stackedColumn",
                                        name = keyItem.Key,
                                        //name = positionTypes.FirstOrDefault(f => f.id == Int32.Parse(keyItem.Key)).title,
                                        dataPoints = new PositionDataPoint[AnalythicSettings.Value.BarsNumber]
                                    });
                                }
                            }
                        }
                        #endregion

                        if (k < AnalythicSettings.Value.BarsNumber - 1)
                        {
                            //Пустые столбцы
                            HistogramItem lastItem = dateBucket.Items.ElementAt(i + 1) as HistogramItem;

                            TimeSpan dateTimeDifference = lastItem.Date.Subtract(currentItem.Date);
                            int dm = 1;
                            switch (message.Interval)
                            {
                                case DateInterval.Month:
                                    int differenceInMonths = (lastItem.Date.Month + lastItem.Date.Year * 12) - (currentItem.Date.Month + currentItem.Date.Year * 12);
                                    while (differenceInMonths > 1 && k >= 0)
                                    {
                                        foreach (KeyItem keyItem in positionIds.Items)
                                        {
                                            AddPositionDataPoint(positionsDictionary[keyItem.Key], k, 0, lastItem.Date.AddMonths(-dm), message.Interval);
                                        }
                                        //те ключи, которых нет в текущем временном интервале
                                        foreach (string key in positionsDictionary.Keys.Where(w => !keyItems.Select(s => s.Key).Contains(w)).ToList())
                                        {
                                            AddPositionDataPoint(positionsDictionary[key], k, 0, lastItem.Date.AddMonths(-dm), message.Interval);
                                        }

                                        k--;
                                        dm++;
                                        differenceInMonths--;
                                    }
                                    break;
                                case DateInterval.Week:
                                    while (dateTimeDifference.TotalDays > 7 && k >= 0)
                                    {
                                        foreach (KeyItem keyItem in positionIds.Items)
                                        {
                                            AddPositionDataPoint(positionsDictionary[keyItem.Key], k, 0, lastItem.Date.AddDays((-7) * dm), message.Interval);
                                        }
                                        //те ключи, которых нет в текущем временном интервале
                                        foreach (string key in positionsDictionary.Keys.Where(w => !keyItems.Select(s => s.Key).Contains(w)).ToList())
                                        {
                                            AddPositionDataPoint(positionsDictionary[key], k, 0, lastItem.Date.AddDays((-7) * dm), message.Interval);
                                        }

                                        k--;
                                        dm++;
                                        dateTimeDifference = dateTimeDifference.Subtract(new TimeSpan(7, 0, 0, 0));
                                    }
                                    break;
                                case DateInterval.Day:
                                    while (dateTimeDifference.TotalDays > 1 && k >= 0)
                                    {
                                        foreach (KeyItem keyItem in positionIds.Items)
                                        {
                                            AddPositionDataPoint(positionsDictionary[keyItem.Key], k, 0, lastItem.Date.AddDays((-1) * dm), message.Interval);
                                        }
                                        //те ключи, которых нет в текущем временном интервале
                                        foreach (string key in positionsDictionary.Keys.Where(w => !keyItems.Select(s => s.Key).Contains(w)).ToList())
                                        {
                                            AddPositionDataPoint(positionsDictionary[key], k, 0, lastItem.Date.AddDays((-1) * dm), message.Interval);
                                        }

                                        k--;
                                        dm++;
                                        dateTimeDifference = dateTimeDifference.Subtract(new TimeSpan(1, 0, 0, 0));
                                    }
                                    break;
                            }
                            //текущий столбец
                            if (k >= 0)
                            {
                                foreach (KeyItem keyItem in positionIds.Items)
                                {
                                    AddPositionDataPoint(positionsDictionary[keyItem.Key], k, keyItem.DocCount, currentItem.Date, message.Interval);
                                }
                                //те ключи, которых нет в текущем временном интервале
                                foreach (string key in positionsDictionary.Keys.Where(w => !keyItems.Select(s => s.Key).Contains(w)).ToList())
                                {
                                    AddPositionDataPoint(positionsDictionary[key], k, 0, currentItem.Date, message.Interval);
                                }
                            }
                        }
                        else
                        {
                            //Смотрим, чтобы последний столбец в гистограмме был за текущий период
                            DateTime currentDate = DateTime.Now;
                            TimeSpan dateTimeDifference = currentDate.Subtract(currentItem.Date);
                            int dm = 0;
                            switch (message.Interval)
                            {
                                case DateInterval.Month:
                                    int differenceInMonths = (currentDate.Month + currentDate.Year * 12) - (currentItem.Date.Month + currentItem.Date.Year * 12);
                                    while (differenceInMonths > 1 && k >= 0)
                                    {
                                        foreach (KeyItem keyItem in positionIds.Items)
                                        {
                                            AddPositionDataPoint(positionsDictionary[keyItem.Key], k, 0, currentDate.AddMonths(-dm), message.Interval);
                                        }
                                        //те ключи, которых нет в текущем временном интервале
                                        foreach (string key in positionsDictionary.Keys.Where(w => !keyItems.Select(s => s.Key).Contains(w)).ToList())
                                        {
                                            AddPositionDataPoint(positionsDictionary[key], k, 0, currentDate.AddMonths(-dm), message.Interval);
                                        }

                                        k--;
                                        dm++;
                                        differenceInMonths--;
                                    }
                                    break;
                                case DateInterval.Week:
                                    while (dateTimeDifference.TotalDays > 7 && k >= 0)
                                    {
                                        foreach (KeyItem keyItem in positionIds.Items)
                                        {
                                            AddPositionDataPoint(positionsDictionary[keyItem.Key], k, 0, currentDate.AddDays((-7) * dm), message.Interval);
                                        }
                                        //те ключи, которых нет в текущем временном интервале
                                        foreach (string key in positionsDictionary.Keys.Where(w => !keyItems.Select(s => s.Key).Contains(w)).ToList())
                                        {
                                            AddPositionDataPoint(positionsDictionary[key], k, 0, currentDate.AddDays((-7) * dm), message.Interval);
                                        }

                                        k--;
                                        dm++;
                                        dateTimeDifference = dateTimeDifference.Subtract(new TimeSpan(7, 0, 0, 0));
                                    }
                                    break;
                                case DateInterval.Day:
                                    while (dateTimeDifference.TotalDays > 1 && k >= 0)
                                    {
                                        foreach (KeyItem keyItem in positionIds.Items)
                                        {
                                            AddPositionDataPoint(positionsDictionary[keyItem.Key], k, 0, currentDate.AddDays((-1) * dm), message.Interval);
                                        }
                                        //те ключи, которых нет в текущем временном интервале
                                        foreach (string key in positionsDictionary.Keys.Where(w => !keyItems.Select(s => s.Key).Contains(w)).ToList())
                                        {
                                            AddPositionDataPoint(positionsDictionary[key], k, 0, currentDate.AddDays((-1) * dm), message.Interval);
                                        }

                                        k--;
                                        dm++;
                                        dateTimeDifference = dateTimeDifference.Subtract(new TimeSpan(1, 0, 0, 0));
                                    }
                                    break;
                            }
                            if (k >= 0)
                            {
                                //последний столбец (обрабатываем первым)
                                foreach (KeyItem keyItem in keyItems)
                                {
                                    AddPositionDataPoint(positionsDictionary[keyItem.Key], k, keyItem.DocCount, currentItem.Date, message.Interval);
                                }
                                //те ключи, которых нет в текущем временном интервале
                                foreach (string key in positionsDictionary.Keys.Where(w => !keyItems.Select(s => s.Key).Contains(w)).ToList())
                                {
                                    AddPositionDataPoint(positionsDictionary[key], k, 0, currentItem.Date, message.Interval);
                                }
                            }
                        }
                    }
                    if (k >= 0)
                    {
                        HistogramItem lastItem = dateBucket.Items.ElementAt(0) as HistogramItem;
                        int dm = 1;
                        for (int i = k; i >= 0; i--)
                        {
                            switch (message.Interval)
                            {
                                case DateInterval.Month:
                                    foreach (string key in positionsDictionary.Keys)
                                    {
                                        AddPositionDataPoint(positionsDictionary[key], i, 0, lastItem.Date.AddMonths(-dm), message.Interval);
                                    }

                                    dm++;
                                    break;
                                case DateInterval.Week:
                                    foreach (string key in positionsDictionary.Keys)
                                    {
                                        AddPositionDataPoint(positionsDictionary[key], i, 0, lastItem.Date.AddDays((-7) * dm), message.Interval);
                                    }

                                    dm++;

                                    break;
                                case DateInterval.Day:
                                    foreach (string key in positionsDictionary.Keys)
                                    {
                                        AddPositionDataPoint(positionsDictionary[key], i, 0, lastItem.Date.AddDays((-1) * dm), message.Interval);
                                    }

                                    dm++;
                                    break;
                            }
                        }
                    }
                    //relaxation
                    List<string> keys = positionsDictionary.Keys.ToList();
                    foreach (string key in keys)
                    {
                        k = AnalythicSettings.Value.BarsNumber;
                        for (int i = 0; i < k; i++)
                        {
                            if (positionsDictionary[key].dataPoints[i] == null)
                            {
                                foreach (string key2 in keys)
                                {
                                    if (positionsDictionary[key2].dataPoints[i] != null)
                                    {
                                        positionsDictionary[key].dataPoints[i] = new PositionDataPoint
                                        {
                                            x = positionsDictionary[key2].dataPoints[i].x,
                                            y = 0,
                                            label = positionsDictionary[key2].dataPoints[i].label
                                        };
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    //Если по региону пусто
                    //Смотрим, чтобы последний столбец в гистограмме был за текущий период
                    DateTime currentDate = DateTime.Now;
                    int dm = 0;

                    positionsDictionary.Add("empty", new PositionsHistogram
                    {
                        type = "stackedColumn",
                        name = "Все должности",
                        dataPoints = new PositionDataPoint[AnalythicSettings.Value.BarsNumber]
                    });
                    switch (message.Interval)
                    {
                        case DateInterval.Month:
                            while (k >= 0)
                            {
                                AddPositionDataPoint(positionsDictionary["empty"], k, 0, currentDate.AddMonths(-dm), message.Interval);

                                k--;
                                dm++;
                            }
                            break;
                        case DateInterval.Week:
                            while (k >= 0)
                            {
                                AddPositionDataPoint(positionsDictionary["empty"], k, 0, currentDate.AddDays((-7) * dm), message.Interval);

                                k--;
                                dm++;
                            }
                            break;
                        case DateInterval.Day:
                            while (k >= 0)
                            {
                                AddPositionDataPoint(positionsDictionary["empty"], k, 0, currentDate.AddDays((-1) * dm), message.Interval);

                                k--;
                                dm++;
                            }
                            break;
                    }
                }
                PositionsHistogram managementHistogram = new PositionsHistogram
                {
                    type = "stackedColumn",
                    name = "Руководящие должности",
                    dataPoints = new PositionDataPoint[AnalythicSettings.Value.BarsNumber]
                };
                for (int n = 0; n < AnalythicSettings.Value.BarsNumber; n++)
                {
                    managementHistogram.dataPoints[n] = new PositionDataPoint();
                }
                PositionsHistogram employeeHistogram = new PositionsHistogram
                {
                    type = "stackedColumn",
                    name = "Сотрудники",
                    dataPoints = new PositionDataPoint[AnalythicSettings.Value.BarsNumber]
                };
                for (int n = 0; n < AnalythicSettings.Value.BarsNumber; n++)
                {
                    employeeHistogram.dataPoints[n] = new PositionDataPoint();
                }
                foreach (PositionsHistogram histogram in positionsDictionary.Values)
                {
                    string id = histogram.name;
                    if (id == "1" || id == "2" || id == "3" || id == "4" || id == "5" || id == "6" || id == "7")
                    {
                        for (int n = 0; n < AnalythicSettings.Value.BarsNumber; n++)
                        {
                            managementHistogram.dataPoints[n].x = histogram.dataPoints[n].x;
                            managementHistogram.dataPoints[n].y += histogram.dataPoints[n].y;
                            managementHistogram.dataPoints[n].label = histogram.dataPoints[n].label;
                        }
                    }
                    if (id == "8" || id == "9" || id == "10" || id == "11" || id == "12")
                    {
                        for (int n = 0; n < AnalythicSettings.Value.BarsNumber; n++)
                        {
                            employeeHistogram.dataPoints[n].x = histogram.dataPoints[n].x;
                            employeeHistogram.dataPoints[n].y += histogram.dataPoints[n].y;
                            employeeHistogram.dataPoints[n].label = histogram.dataPoints[n].label;
                        }
                    }
                    if (histogram.name == "Все должности")
                    {
                        for (int n = 0; n < AnalythicSettings.Value.BarsNumber; n++)
                        {
                            managementHistogram.dataPoints[n].x = histogram.dataPoints[n].x;
                            managementHistogram.dataPoints[n].y += histogram.dataPoints[n].y;
                            managementHistogram.dataPoints[n].label = histogram.dataPoints[n].label;
                        }
                        for (int n = 0; n < AnalythicSettings.Value.BarsNumber; n++)
                        {
                            employeeHistogram.dataPoints[n].x = histogram.dataPoints[n].x;
                            employeeHistogram.dataPoints[n].y += histogram.dataPoints[n].y;
                            employeeHistogram.dataPoints[n].label = histogram.dataPoints[n].label;
                        }
                    }

                    //histograms.Add(histogram);
                }
                histograms.Add(managementHistogram);
                histograms.Add(employeeHistogram);

                Cache.Set<List<PositionsHistogram>>(positionsCacheKey, histograms, CacheOptions);
            }

            return histograms;
        }


        public List<PaymentsHistogram> Handle(AveragePaymentAndVacancyCountByRegionAnalythicQuery message)
        {
            List<PaymentsHistogram> histograms;
            string paymentsCacheKey = "a_payments_" + message.Interval.ToString() + "_region_" + message.RegionId.ToString();
            if (!Cache.TryGetValue<List<PaymentsHistogram>>(paymentsCacheKey, out histograms))
            {
                IDictionary<string, Nest.IAggregation> aggregations;
                try
                {
                    aggregations = AnalythicService.VacancyPayments(message);
                }
                catch (Exception e)
                {
                    Logger.LogError(e.Message, e);
                    throw;
                }

                histograms = new List<PaymentsHistogram>();

                if (aggregations == null)
                    throw new Exception($"{nameof(aggregations)} is null");

                if (!aggregations.ContainsKey("histogram"))
                    throw new Exception("aggregations[\"histogram\"] is null");

                Bucket dateBucket = aggregations["histogram"] as Bucket;

                PaymentsHistogram averageHistogram = new PaymentsHistogram
                {
                    legendMarkerColor = "#041833",
                    legendMarkerBorderThickness = "3",
                    legendMarkerBorderColor = "#041833",
                    type = "line",
                    axisYType = "primary",
                    legendText = "Средняя зп",
                    name = "Средняя зп",
                    showInLegend = false,
                    dataPoints = new PaymentDataPoint[AnalythicSettings.Value.BarsNumber]
                };
                PaymentsHistogram countHistogram = new PaymentsHistogram
                {
                    legendMarkerColor = "#3a5e90",
                    legendMarkerBorderThickness = "3",
                    legendMarkerBorderColor = "#3a5e90",
                    type = "line",
                    axisYType = "secondary",
                    legendText = "Вакансии",
                    name = "Вакансий",
                    showInLegend = false,
                    dataPoints = new PaymentDataPoint[AnalythicSettings.Value.BarsNumber]
                };
                int k = AnalythicSettings.Value.BarsNumber - 1;
                if ((bool) dateBucket?.Items?.Any())
                {
                    for (int i = dateBucket.Items.Count() - 1; i >= 0 && k >= 0; i--, k--)
                    {
                        HistogramItem currentItem = dateBucket.Items.ElementAt(i) as HistogramItem;

                        if (k < AnalythicSettings.Value.BarsNumber - 1)
                        {
                            //Пустые столбцы
                            HistogramItem lastItem = dateBucket.Items.ElementAt(i + 1) as HistogramItem;

                            TimeSpan dateTimeDifference = lastItem.Date.Subtract(currentItem.Date);
                            int dm = 1;
                            switch (message.Interval)
                            {
                                case DateInterval.Month:
                                    int differenceInMonths = (lastItem.Date.Month + lastItem.Date.Year * 12) - (currentItem.Date.Month + currentItem.Date.Year * 12);
                                    while (differenceInMonths > 1 && k >= 0)
                                    {
                                        AddPaymentDataPoint(averageHistogram, k, 0.0, lastItem.Date.AddMonths(-dm), message.Interval);
                                        AddPaymentDataPoint(countHistogram, k, 0.0, lastItem.Date.AddMonths(-dm), message.Interval);

                                        k--;
                                        dm++;
                                        differenceInMonths--;
                                    }
                                    break;
                                case DateInterval.Week:
                                    while (dateTimeDifference.TotalDays > 7 && k >= 0)
                                    {
                                        AddPaymentDataPoint(averageHistogram, k, 0.0, lastItem.Date.AddDays((-7) * dm), message.Interval);
                                        AddPaymentDataPoint(countHistogram, k, 0.0, lastItem.Date.AddDays((-7) * dm), message.Interval);

                                        k--;
                                        dm++;
                                        dateTimeDifference = dateTimeDifference.Subtract(new TimeSpan(7, 0, 0, 0));
                                    }
                                    break;
                                case DateInterval.Day:
                                    while (dateTimeDifference.TotalDays > 1 && k >= 0)
                                    {
                                        AddPaymentDataPoint(averageHistogram, k, 0.0, lastItem.Date.AddDays((-1) * dm), message.Interval);
                                        AddPaymentDataPoint(countHistogram, k, 0.0, lastItem.Date.AddDays((-1) * dm), message.Interval);

                                        k--;
                                        dm++;
                                        dateTimeDifference = dateTimeDifference.Subtract(new TimeSpan(1, 0, 0, 0));
                                    }
                                    break;
                            }
                            //текущий столбец
                            if (k >= 0)
                            {
                                ValueMetric salaryFrom = currentItem.Aggregations["salary_from"] as ValueMetric;
                                ValueMetric salaryTo = currentItem.Aggregations["salary_to"] as ValueMetric;

                                AddPaymentDataPoint(averageHistogram, k, (double)(((((salaryFrom.Value ?? 0) + (salaryTo.Value ?? 0)) / 2) / 1000D)), currentItem.Date, message.Interval);
                                AddPaymentDataPoint(countHistogram, k, currentItem.DocCount, currentItem.Date, message.Interval);
                            }
                        }
                        else
                        {
                            //Смотрим, чтобы последний столбец в гистограмме был за текущий период
                            DateTime currentDate = DateTime.Now;
                            TimeSpan dateTimeDifference = currentDate.Subtract(currentItem.Date);
                            int dm = 0;
                            switch (message.Interval)
                            {
                                case DateInterval.Month:
                                    int differenceInMonths = (currentDate.Month + currentDate.Year * 12) - (currentItem.Date.Month + currentItem.Date.Year * 12);
                                    while (differenceInMonths > 1 && k >= 0)
                                    {
                                        AddPaymentDataPoint(averageHistogram, k, 0.0, currentDate.AddMonths(-dm), message.Interval);
                                        AddPaymentDataPoint(countHistogram, k, 0.0, currentDate.AddMonths(-dm), message.Interval);

                                        k--;
                                        dm++;
                                        differenceInMonths--;
                                    }
                                    break;
                                case DateInterval.Week:
                                    while (dateTimeDifference.TotalDays > 7 && k >= 0)
                                    {
                                        AddPaymentDataPoint(averageHistogram, k, 0.0, currentDate.AddDays((-7) * dm), message.Interval);
                                        AddPaymentDataPoint(countHistogram, k, 0.0, currentDate.AddDays((-7) * dm), message.Interval);

                                        k--;
                                        dm++;
                                        dateTimeDifference = dateTimeDifference.Subtract(new TimeSpan(7, 0, 0, 0));
                                    }
                                    break;
                                case DateInterval.Day:
                                    while (dateTimeDifference.TotalDays > 1 && k >= 0)
                                    {
                                        AddPaymentDataPoint(averageHistogram, k, 0.0, currentDate.AddDays((-1) * dm), message.Interval);
                                        AddPaymentDataPoint(countHistogram, k, 0.0, currentDate.AddDays((-1) * dm), message.Interval);

                                        k--;
                                        dm++;
                                        dateTimeDifference = dateTimeDifference.Subtract(new TimeSpan(1, 0, 0, 0));
                                    }
                                    break;
                            }
                            //последний столбец (обрабатываем первым)
                            if (k >= 0)
                            {
                                ValueMetric salaryFrom = currentItem.Aggregations["salary_from"] as ValueMetric;
                                ValueMetric salaryTo = currentItem.Aggregations["salary_to"] as ValueMetric;

                                AddPaymentDataPoint(averageHistogram, k, (double)(((((salaryFrom.Value ?? 0) + (salaryTo.Value ?? 0)) / 2) / 1000D)), currentItem.Date, message.Interval);
                                AddPaymentDataPoint(countHistogram, k, currentItem.DocCount, currentItem.Date, message.Interval);
                            }
                        }
                    }
                    if (k >= 0)
                    {
                        HistogramItem lastItem = dateBucket.Items.ElementAt(0) as HistogramItem;
                        int dm = 1;
                        for (int i = k; i >= 0; i--)
                        {
                            switch (message.Interval)
                            {
                                case DateInterval.Month:

                                    AddPaymentDataPoint(averageHistogram, i, 0.0, lastItem.Date.AddMonths(-dm), message.Interval);
                                    AddPaymentDataPoint(countHistogram, i, 0.0, lastItem.Date.AddMonths(-dm), message.Interval);

                                    dm++;
                                    break;
                                case DateInterval.Week:
                                    AddPaymentDataPoint(averageHistogram, i, 0.0, lastItem.Date.AddDays((-7) * dm), message.Interval);
                                    AddPaymentDataPoint(countHistogram, i, 0.0, lastItem.Date.AddDays((-7) * dm), message.Interval);

                                    dm++;

                                    break;
                                case DateInterval.Day:
                                    AddPaymentDataPoint(averageHistogram, i, 0.0, lastItem.Date.AddDays((-1) * dm), message.Interval);
                                    AddPaymentDataPoint(countHistogram, i, 0.0, lastItem.Date.AddDays((-1) * dm), message.Interval);

                                    dm++;
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    //Если по данному региону нет ничего
                    //Смотрим, чтобы последний столбец в гистограмме был за текущий период
                    DateTime currentDate = DateTime.Now;
                    int dm = 0;
                    switch (message.Interval)
                    {
                        case DateInterval.Month:
                            while (k >= 0)
                            {
                                AddPaymentDataPoint(averageHistogram, k, 0.0, currentDate.AddMonths(-dm), message.Interval);
                                AddPaymentDataPoint(countHistogram, k, 0.0, currentDate.AddMonths(-dm), message.Interval);

                                k--;
                                dm++;
                            }
                            break;
                        case DateInterval.Week:
                            while (k >= 0)
                            {
                                AddPaymentDataPoint(averageHistogram, k, 0.0, currentDate.AddDays((-7) * dm), message.Interval);
                                AddPaymentDataPoint(countHistogram, k, 0.0, currentDate.AddDays((-7) * dm), message.Interval);

                                k--;
                                dm++;
                            }
                            break;
                        case DateInterval.Day:
                            while (k >= 0)
                            {
                                AddPaymentDataPoint(averageHistogram, k, 0.0, currentDate.AddDays((-1) * dm), message.Interval);
                                AddPaymentDataPoint(countHistogram, k, 0.0, currentDate.AddDays((-1) * dm), message.Interval);

                                k--;
                                dm++;
                            }
                            break;
                    }
                }

                histograms.Add(averageHistogram);
                histograms.Add(countHistogram);

                Cache.Set<List<PaymentsHistogram>>(paymentsCacheKey, histograms, CacheOptions);
            }

            return histograms;
        }

        private void AddPositionDataPoint(PositionsHistogram histogram, int index, long y, DateTime date, DateInterval interval)
        {
            string label;
            switch (interval)
            {
                case DateInterval.Month:
                    label = date.ToString("MMM", new CultureInfo("ru-RU"));
                    break;
                case DateInterval.Week:
                    label = date.ToString("dd MMM", new CultureInfo("ru-RU")) + " - " + date.AddDays(6).ToString("dd MMM", new CultureInfo("ru-RU"));
                    break;
                case DateInterval.Day:
                    label = date.ToString("ddd", new CultureInfo("ru-RU"));
                    break;
                default:
                    label = "";
                    break;
            }
            try
            {
                histogram.dataPoints[index] = new PositionDataPoint
                {
                    x = index,
                    y = y,
                    label = label
                };
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message, e);
                throw;
            }
        }
        private void AddPaymentDataPoint(PaymentsHistogram histogram, int index, double y, DateTime date, DateInterval interval)
        {
            string label;
            switch (interval)
            {
                case DateInterval.Month:
                    label = date.ToString("MMM", new CultureInfo("ru-RU"));
                    break;
                case DateInterval.Week:
                    label = date.ToString("dd MMM", new CultureInfo("ru-RU")) + " - " + date.AddDays(6).ToString("dd MMM", new CultureInfo("ru-RU"));
                    break;
                case DateInterval.Day:
                    label = date.ToString("ddd", new CultureInfo("ru-RU"));
                    break;
                default:
                    label = "";
                    break;
            }
            try
            {
                histogram.dataPoints[index] = new PaymentDataPoint
                {
                    x = index,
                    y = Math.Round(y, 2),
                    label = label
                };
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message, e);
                throw;
            }
        }
    }
}

