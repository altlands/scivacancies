using Microsoft.AspNet.Mvc;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.ViewComponents
{
    public class VacancyInfo: ViewComponent
    {
        public IViewComponentResult Invoke(VacancyDetailsViewModel model)
        {
            return View("/Views/Partials/_VacancyInfo", model);
        }
    }
}
