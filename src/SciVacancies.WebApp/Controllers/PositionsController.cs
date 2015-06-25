using Microsoft.AspNet.Mvc;

namespace SciVacancies.WebApp.Controllers
{
    public class PositionsController: Controller
    {
        public IActionResult Details() => View();
        public IActionResult Edit() => View();
        public IActionResult Delete() => View();
        public IActionResult Pyblish() => View();
    }
}
