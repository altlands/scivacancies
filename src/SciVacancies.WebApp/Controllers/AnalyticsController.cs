using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SciVacancies.WebApp.Controllers
{
    public class AnalyticsController : Controller
    {
        /// <summary>
        /// Число вакансий (среднее предложение в месяц)
        /// </summary>
        /// <returns></returns>
        public JsonResult VacancyAmountInPeriod()
        {
            List<int> model = new List<int>();

            return Json(model);
        }


    }
}
