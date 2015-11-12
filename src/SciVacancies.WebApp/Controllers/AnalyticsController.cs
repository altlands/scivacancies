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
            var result = Mediator.Send(new AverageVacancyAndPositionAnalythicQuery { Interval = interval });

            var model = new List<object>
            {
                new {
                    type = "stackedColumn",
                    toolTipContent = "{label}<br/><span style='\"'color: {color};'\"'><strong>{name}</strong></span>: {y} вакансий",
                    name = "Руководители групп структурных подразделений",
                    showInLegend = "true",
                    dataPoints = new List<object>{
                        new { y= 38600, label= "Июль" },
                        new { y= 11338, label= "Август" },
                        new { y= 49088, label= "Сентябрь" },
                        new { y= 62200, label= "Октябрь" },
                        new { y= 90085, label= "Ноябрь" }
                    }
                },
                new {
                    type = "stackedColumn",
                    toolTipContent = "{label}<br/><span style='\"'color: {color};'\"'><strong>{name}</strong></span>: {y} вакансий",
                    name = "Руководители структурных подразделений",
                    showInLegend = "true",
                    dataPoints = new List<object>{
                        new { y= 39900, label= "Июль" },
                        new { y= 135305, label= "Август" },
                        new { y= 107922, label= "Сентябрь" },
                        new { y= 52300, label= "Октябрь" },
                        new { y= 3360, label= "Ноябрь" }
                    }
                }
            };
            return Json(model);
        }

        public JsonResult VacancyPayments(int? regionId, Nest.DateInterval interval)
        {
            var result = Mediator.Send(new AveragePaymentAndVacancyCountByRegionAnalythicQuery { RegionId = regionId, Interval = interval });

            var model = new List<object>
            {
                 new {
                     type = "line",
                     name = "Средняя зп",
                     showInLegend = "true",
                     dataPoints = new List<object>{
                         new   { x= 6, y= 0, label= "Июнь" },
                         new   { x= 7, y= 8.2, label= "Июль" },
                         new   { x= 8, y= 41.7, label= "Август" },
                         new   { x= 9, y= 16.7, label= "Сентябрь" },
                         new   { x= 10, y= 31.3, label= "Октябрь" }
                     }
                 },
                new {
                    type = "line",
                    axisYType = "secondary",
                    name = "Вакансий",
                    showInLegend = "true",
                    dataPoints = new List<object> {
                        new {x = 6, y = 0, label = "Июнь"},
                        new {x = 7, y = 90, label = "Июль"},
                        new {x = 8, y = 1590, label = "Август"},
                        new {x = 9, y = 1740, label = "Сентябрь"},
                        new {x = 10, y = 3740, label = "Октябрь"}
                    }
                }
            };
            return Json(model);
        }
    }
}
