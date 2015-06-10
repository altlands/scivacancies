using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace SciVacancies.WebApp.ViewComponents
{
    public class Login: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("/Views/Partials/_Login");
        }
    }
}
