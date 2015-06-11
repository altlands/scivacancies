using System;
using Microsoft.AspNet.Mvc;
using SciVacancies.WebApp.Engine.CustomAttribute;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Controllers
{
    public class ResearchersController: Controller
    {
        [SiblingPage]
        [PageTitle("Информация")]
        public ViewResult Account()
        {
            var model = new ResearcherDetailsViewModel();
            return View(model);
        }

        [PageTitle("Изменить данные")]
        public ViewResult Edit(Guid id)
        {
            var model = new ResearcherEditViewModel {Guid = id };
            return View(model);
        }
        [HttpPut]
        [HttpPost]
        public RedirectToActionResult Edit(ResearcherEditViewModel model)
        {
            return RedirectToAction("account");
        }

        [SiblingPage]
        [PageTitle("Мои заявки")]
        public ViewResult Applications()
        {
            var model = new ResearcherDetailsViewModel();
            return View(model);
        }

        [SiblingPage]
        [PageTitle("Избранные вакансии")]
        public ActionResult Favorities()
        {
            var model = new ResearcherDetailsViewModel();
            return View(model);
        }

        [SiblingPage]
        [PageTitle("Подписки")]
        public ViewResult Subscriptions()
        {
            var model = new ResearcherDetailsViewModel();
            return View(model);
        }

        [PageTitle("Уведомления")]
        public ViewResult Notifications()
        {
            var model = new ResearcherDetailsViewModel();
            return View(model);
        }

    }
}
