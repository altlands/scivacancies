using Microsoft.AspNet.Mvc;

namespace SciVacancies.WebApp.Controllers
{
    public class StatusCodesController : Controller
    {
        public IActionResult StatusCode404()
        {
            return View("Error", "Объект не найден");
        }

        public IActionResult StatusCode401()
        {
            return View("Error", "Вы не авторизованы");
        }

        public IActionResult StatusCode500()
        {
            return View("Error", "Что-то пошло не так");
        }

        //... more actions here to handle other status codes
    }
}
