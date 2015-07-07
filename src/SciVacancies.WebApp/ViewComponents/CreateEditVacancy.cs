using Microsoft.AspNet.Mvc;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.ViewComponents
{
    public class CreateEditVacancy : ViewComponent
    {
        public IViewComponentResult Invoke(PositionCreateViewModel model)
        {
            return View("/Views/Vacancies/_CreateEdit", model);
        }
    }
}
