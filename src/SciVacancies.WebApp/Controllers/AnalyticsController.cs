﻿using System;
using System.Collections.Generic;
using System.Web.Caching;
using SciVacancies.WebApp.Queries;

using Microsoft.AspNet.Mvc;
using Microsoft.Framework.OptionsModel;
using Microsoft.Framework.Caching.Memory;

using MediatR;
using SciVacancies.Domain.Enums;
using SciVacancies.Services.Elastic;
using SciVacancies.WebApp.ViewModels;
using SciVacancies.Services.Elastic;
using SearchQuery = SciVacancies.WebApp.Queries.SearchQuery;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SciVacancies.WebApp.Controllers
{
    public class AnalyticsController : Controller
    {
        private readonly IOptions<AnalythicSettings> _analythicSettings;
        private readonly IMediator _mediator;
        private readonly IAnalythicService _analythicService;
        private readonly IMemoryCache _cache;
        private readonly IOptions<CacheSettings> _cacheSettings;
        private MemoryCacheEntryOptions _cacheOptions
        {
            get
            {
                return new MemoryCacheEntryOptions().SetAbsoluteExpiration(DateTimeOffset.Now.AddSeconds(_cacheSettings.Value.MainPageExpiration));
            }
        }

        public AnalyticsController(IOptions<AnalythicSettings> analythicSettings, IMediator mediator, IAnalythicService analythicService, IMemoryCache cache, IOptions<CacheSettings> cacheSettings)
        {
            _analythicSettings = analythicSettings;
            _mediator = mediator;
            _analythicService = analythicService;
            _cache = cache;
            _cacheSettings = cacheSettings;
        }

        /// <summary>
        /// Число вакансий (среднее предложение в месяц)
        /// </summary>
        /// <returns></returns>
        public JsonResult VacancyPositions(int? regionId, Nest.DateInterval interval)
        {
            var result = _mediator.Send(new AverageVacancyAndPositionAnalythicQuery
            {
                RegionId = regionId,
                Interval = interval,
                BarsNumber = _analythicSettings.Value.BarsNumber
            });

            return Json(result);
        }

        public JsonResult VacancyPayments(int? regionId, Nest.DateInterval interval)
        {
            var result = _mediator.Send(new AveragePaymentAndVacancyCountByRegionAnalythicQuery
            {
                RegionId = regionId,
                Interval = interval,
                BarsNumber = _analythicSettings.Value.BarsNumber
            });

            return Json(result);
        }
        public IActionResult CountByResearchDirection(int id)
        {
            ResearchDirectionAggregationViewModel model;
            if (!_cache.TryGetValue($"vacancies_by_research_directions{id}", out model))
            {
                var query = new VacancyPaymentsByResearchDirectionAnalythicQuery
                {
                    ResearchDirectionId = id
                };
                var result = _analythicService.VacancyPayments(query);

                var min = (result["salary_from"] as Nest.ValueMetric).Value ?? 0D;
                var max = (result["salary_to"] as Nest.ValueMetric).Value ?? 0D;

                if (max < min) { min = max = 0; }

                var items = _mediator.Send(new SearchQuery
                {
                    Query = string.Empty,
                    PageSize = 1,
                    CurrentPage = 1,
                    OrderFieldByDirection = ConstTerms.OrderByFieldDate,
                    ResearchDirectionIds = new List<int> { id },
                    VacancyStatuses = new List<VacancyStatus> { VacancyStatus.Published }
                });

                model = new ResearchDirectionAggregationViewModel
                {
                    AverageSalary = items.TotalItems > 0 ? Decimal.Round(Convert.ToDecimal((max - min) / items.TotalItems), 0) : 0,
                    Count = items.TotalItems,
                    Id = id
                };
                _cache.Set($"vacancies_by_research_directions{id}", model, _cacheOptions);
            }

            return Json(model);

        }
    }
}
