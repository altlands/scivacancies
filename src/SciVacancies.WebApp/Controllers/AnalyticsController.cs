using SciVacancies.WebApp.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.OptionsModel;
using Microsoft.Framework.Caching.Memory;

using MediatR;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SciVacancies.WebApp.Controllers
{
    public class AnalyticsController : Controller
    {

        private readonly IMediator Mediator;
        private readonly IMemoryCache Cache;
        private readonly IOptions<CacheSettings> CacheSettings;
        private MemoryCacheEntryOptions CacheOptions
        {
            get
            {
                return new MemoryCacheEntryOptions().SetAbsoluteExpiration(DateTimeOffset.Now.AddSeconds(CacheSettings.Value.MainPageExpiration));
            }
        }
        public AnalyticsController(IMediator mediator, IMemoryCache cache, IOptions<CacheSettings> cacheSettings)
        {
            this.Mediator = mediator;
            this.Cache = cache;
            this.CacheSettings = cacheSettings;
        }

        /// <summary>
        /// Число вакансий (среднее предложение в месяц)
        /// </summary>
        /// <returns></returns>
        public JsonResult VacancyPositions(int? regionId, Nest.DateInterval interval)
        {
            List<int> model = new List<int>();
            var result = Mediator.Send(new AverageVacancyAndPositionAnalythicQuery { Interval = interval });

            return Json(model);
        }

        public JsonResult VacancyPayments(int? regionId, Nest.DateInterval interval)
        {
            List<int> model = new List<int>();
            var result = Mediator.Send(new AveragePaymentAndVacancyCountByRegionAnalythicQuery { RegionId = regionId, Interval = interval });

            return Json(model);
        }
    }
}
