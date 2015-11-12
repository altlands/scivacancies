using SciVacancies.ReadModel.Core;
using SciVacancies.Services.Elastic;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Framework.OptionsModel;
using Microsoft.Framework.Caching.Memory;

using MediatR;

namespace SciVacancies.WebApp.Queries
{
    public class AnalythicQueriesHandler :
        IRequestHandler<AverageVacancyAndPositionAnalythicQuery, IDictionary<string, Nest.IAggregation>>,
        IRequestHandler<AveragePaymentAndVacancyCountByRegionAnalythicQuery, IDictionary<string, Nest.IAggregation>>
    {
        private readonly IMemoryCache Cache;
        private readonly IAnalythicService AnalythicService;
        private readonly IOptions<CacheSettings> CacheSettings;
        private MemoryCacheEntryOptions CacheOptions
        {
            get
            {
                return new MemoryCacheEntryOptions().SetAbsoluteExpiration(DateTimeOffset.Now.AddSeconds(CacheSettings.Value.MainPageExpiration));
            }
        }

        public AnalythicQueriesHandler(IAnalythicService analythicService, IMemoryCache cache, IOptions<CacheSettings> cacheSettings)
        {
            this.AnalythicService = analythicService;
            this.Cache = cache;
            this.CacheSettings = cacheSettings;
        }

        public IDictionary<string, Nest.IAggregation> Handle(AverageVacancyAndPositionAnalythicQuery message)
        {
            IDictionary<string, Nest.IAggregation> aggregations;
            if (!Cache.TryGetValue<IDictionary<string, Nest.IAggregation>>("vacancy_position_analythic", out aggregations))
            {
                aggregations = AnalythicService.VacancyPositions(message);
                Cache.Set<IDictionary<string, Nest.IAggregation>>("vacancy_position_analythic", aggregations);
            }

            return aggregations;
        }

        public IDictionary<string, Nest.IAggregation> Handle(AveragePaymentAndVacancyCountByRegionAnalythicQuery message)
        {
            IDictionary<string, Nest.IAggregation> aggregations;
            if (!Cache.TryGetValue<IDictionary<string, Nest.IAggregation>>("vacancy_payment_analythic", out aggregations))
            {
                aggregations = AnalythicService.VacancyPayments(message);
                Cache.Set<IDictionary<string, Nest.IAggregation>>("vacancy_payment_analythic", aggregations);
            }

            return aggregations;
        }
    }
}
