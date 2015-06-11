using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace SciVacancies.WebApp.Controllers
{
    public class VacanciesController : Controller
    {

        public ViewResult Search(
            List<int> salaries,
            List<int> contestStates,
            
            int period = 0 /* 0=all | 1=1 day |  7=7 days | 30=30 days */,
            string orderBy = "date" /* count */, 
            bool orderDescending = true,
            int pageSize = 10,
            int page = 1 
            )
        {

            return View();
        }


        public ViewResult Details(string id) => View();
    }
}
