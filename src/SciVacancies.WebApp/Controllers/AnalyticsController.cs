using SciVacancies.WebApp.Queries;

using Microsoft.AspNet.Mvc;
using Microsoft.Framework.OptionsModel;

using MediatR;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SciVacancies.WebApp.Controllers
{
    public class AnalyticsController : Controller
    {
        private readonly IOptions<AnalythicSettings> _analythicSettings;
        private readonly IMediator _mediator;

        public AnalyticsController(IOptions<AnalythicSettings> analythicSettings, IMediator mediator)
        {
            _analythicSettings = analythicSettings;
            _mediator = mediator;
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
    }
}
