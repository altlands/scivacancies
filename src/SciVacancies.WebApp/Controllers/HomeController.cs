using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using SciVacancies.WebApp.Infrastructure;

namespace SciVacancies.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IFart Fart {get;set;}

        public HomeController(IFart fart){
            Fart = fart;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewBag.Message = "Your application description page. SSSSSSSSS";

            return View();
        }

        public IActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
