using System;
using Microsoft.AspNet.Mvc;

namespace SciVacancies.WebApp.Controllers
{
    public class PositionsController: Controller
    {
        public PositionsController()
        {

        }

        public IActionResult Details(Guid id) => View();
        public IActionResult Edit(Guid id) => View();
        public IActionResult Delete(Guid id) => View();
        public IActionResult Pyblish(Guid id) => View();
    }
}
