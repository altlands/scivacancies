using Microsoft.AspNet.Mvc;

namespace SciVacancies.WebApp.Controllers
{
    public class CallbackController: Controller
    {
        [PageTitle("Вы успешно авторизованы")]
        public IActionResult Index()
        {
            return View();
        }

    }
}
