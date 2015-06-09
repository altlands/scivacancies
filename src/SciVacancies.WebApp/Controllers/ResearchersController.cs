using Microsoft.AspNet.Mvc;
using SciVacancies.WebApp.Engine.CustomAttribute;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Controllers
{
    public class ResearchersController: Controller
    {
        [ActionName("details")]
        [NavigationTitle("Информация")]
        public ViewResult Account()
        {
            var model = new ResearcherDetailsViewModel();
            return View("Account", model);
        }

        public ViewResult Edit()
        {
            var model = new ResearcherEditViewModel();
            return View(model);
        }
        [HttpPut]
        public RedirectToActionResult Edit(ResearcherEditViewModel model)
        {
            return RedirectToAction("account");
        }

        [NavigationTitle("Мои заявки")]
        public ViewResult Applications()
        {
            var model = new ResearcherDetailsViewModel();
            return View(model);
        }

        [NavigationTitle("Избранные вакансии")]
        public ActionResult Favorities()
        {
            var model = new ResearcherDetailsViewModel();
            return View(model);
        }

        [NavigationTitle("Подписки")]
        public ViewResult Subscriptions()
        {
            var model = new ResearcherDetailsViewModel();
            return View(model);
        }

        [NavigationTitle("Уведомления")]
        public ViewResult Notifications()
        {
            var model = new ResearcherDetailsViewModel();
            return View(model);
        }

    }
}
