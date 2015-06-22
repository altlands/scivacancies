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
