using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace SciVacancies.WebApp.Controllers
{
    /// <summary>
    /// страница с конкурсами
    /// </summary>
    public class ContestsController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }
    }
}
