using Microsoft.AspNet.Mvc;

namespace SciVacancies.WebApp.ViewComponents
{
    public class Breadcrumbs: ViewComponent
    {
        public IViewComponentResult Invoke(string title)
        {
            //var title = ViewBag.NavigationTitle;
            return View("/Views/Partials/_Breadcrumbs", title);
        }
    }
}
