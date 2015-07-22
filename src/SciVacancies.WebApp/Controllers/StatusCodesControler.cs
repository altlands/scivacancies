using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace SciVacancies.WebApp.Controllers
{
    public class StatusCodesController : Controller
    {
        public IActionResult StatusCode404()
        {
            return Content("объект не найден");
            //return View(viewName: "NotFound"); // you have a view called NotFound.cshtml
        }

        public IActionResult StatusCode401()
        {
            return Content("вы не авторизованы");
            //return View(viewName: "NotFound"); // you have a view called NotFound.cshtml
        }

        //... more actions here to handle other status codes
    }
}
