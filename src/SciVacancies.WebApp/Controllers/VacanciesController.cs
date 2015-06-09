using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace SciVacancies.WebApp.Controllers
{
    public class VacanciesController : Controller
    {
        public ViewResult Search() => View();
    }
}
