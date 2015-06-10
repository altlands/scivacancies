using Microsoft.AspNet.Mvc;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.ViewComponents
{
    public class ApplicationCreate: ViewComponent
    {
        public IViewComponentResult Invoke(ApplicationCreateViewModel model)
        {
            return View("/Views/Partials/_ApplicationCreateForm", model);
        }
    }
}
