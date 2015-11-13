using SciVacancies.WebApp.Queries;

using Microsoft.AspNet.Mvc;
using Microsoft.Framework.OptionsModel;

using MediatR;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SciVacancies.WebApp.Controllers
{
    public class AnalyticsController : Controller
    {
        private readonly IOptions<AnalythicSettings> AnalythicSettings;
        private readonly IMediator Mediator;

        public AnalyticsController(IOptions<AnalythicSettings> analythicSettings, IMediator mediator)
        {
            this.AnalythicSettings = analythicSettings;
            this.Mediator = mediator;
        }

        /// <summary>
        /// Число вакансий (среднее предложение в месяц)
        /// </summary>
        /// <returns></returns>
        public JsonResult VacancyPositions(int? regionId, Nest.DateInterval interval)
        {
            var result = Mediator.Send(new AverageVacancyAndPositionAnalythicQuery
            {
                RegionId = regionId,
                Interval = interval,
                BarsNumber = AnalythicSettings.Value.BarsNumber
            });

            return Json(result);
        }

        public JsonResult VacancyPayments(int? regionId, Nest.DateInterval interval)
        {
            var result = Mediator.Send(new AveragePaymentAndVacancyCountByRegionAnalythicQuery
            {
                RegionId = regionId,
                Interval = interval,
                BarsNumber = AnalythicSettings.Value.BarsNumber
            });

            return Json(result);
        }
    }
}
