using System;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Controllers
{
    public class AccountController : Controller
    {
        public const string UserNameKey = "UserTempName";
        public const string IsResearcherKey = "IsResearcher";

        [HttpPost]
        public ActionResult Login(AccountLoginViewModel model)
        {
            Context.Session.SetString(UserNameKey, model.Login);
            Context.Session.SetString(IsResearcherKey, model.IsResearcher.ToString());
            return RedirectToHome();
        }


        public ViewResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(AccountRegisterViewModel model)
        {
            Context.Session.SetString(UserNameKey, model.UserName);
            return RedirectToHome();
        }


        public ViewResult ForgotPassword()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Context.Session.SetString(UserNameKey, string.Empty);
            return RedirectToHome();
        }

        private RedirectToActionResult RedirectToHome()
        {
            return RedirectToAction("index", "home");
        }
    }
}
