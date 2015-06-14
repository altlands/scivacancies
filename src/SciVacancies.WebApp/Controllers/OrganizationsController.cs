using Microsoft.AspNet.Mvc;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Engine.CustomAttribute;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Controllers
{
    public class OrganizationsController: Controller
    {
        [SiblingPage]
        [PageTitle("Информация")]
        public ViewResult Account()
        {
            var model = new OrganizationDetailsViewModel();
            return View(model);
        }

        [SiblingPage]
        [PageTitle("Вакансии")]
        public ViewResult Vacancies()
        {
            var model = new OrganizationDetailsViewModel();
            return View(model);
        }

        [SiblingPage]
        [PageTitle("Уведомления")]
        public ViewResult Notifications()
        {
            var model = new OrganizationDetailsViewModel();
            return View(model);
        }
    }
}
