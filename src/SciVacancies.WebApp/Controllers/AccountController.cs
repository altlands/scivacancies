using System;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Controllers
{
    public class AccountController : Controller
    {

        private void DeleteUserCookies()
        {
            Context.Response.Cookies.Append(ConstTerms.CookieKeyForUserRole, string.Empty, new CookieOptions { Expires = DateTime.Now.AddDays(-1) });
            Context.Response.Cookies.Append(ConstTerms.CookieKeyForUserName, string.Empty, new CookieOptions { Expires = DateTime.Now.AddDays(-1) });
            Context.Response.Cookies.Append(ConstTerms.CookieKeyForResearcherGuid, string.Empty, new CookieOptions { Expires = DateTime.Now.AddDays(-1) });
            Context.Response.Cookies.Append(ConstTerms.CookieKeyForOrganizationGuid, string.Empty, new CookieOptions { Expires = DateTime.Now.AddDays(-1) });

        }

        [HttpPost]
        public ActionResult Login(AccountLoginViewModel model)
        {
            DeleteUserCookies();
            var timeStamp = GetExpiresTime();
            Context.Response.Cookies.Append(ConstTerms.CookieKeyForUserName, model.Login, new CookieOptions { Expires = timeStamp });
            Context.Response.Cookies.Append(ConstTerms.CookieKeyForUserRole, model.IsResearcher.ToString(), new CookieOptions { Expires = timeStamp });
            if (model.IsResearcher)
                Context.Response.Cookies.Append(ConstTerms.CookieKeyForResearcherGuid, Guid.NewGuid().ToString(), new CookieOptions { Expires = timeStamp });
            else
                Context.Response.Cookies.Append(ConstTerms.CookieKeyForOrganizationGuid, Guid.NewGuid().ToString(), new CookieOptions { Expires = timeStamp });
            return RedirectToHome();
        }


        public ViewResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(AccountRegisterViewModel model)
        {
            DeleteUserCookies();
            var timeStamp = GetExpiresTime();
            Context.Response.Cookies.Append(ConstTerms.CookieKeyForUserName, model.UserName, new CookieOptions { Expires = timeStamp });
            //Context.Response.Cookies.Append(ConstTerms.CookieKeyForUserRole, model.IsResearcher.ToString(), new CookieOptions { Expires = timeStamp });
            return RedirectToHome();
        }

        private static DateTime GetExpiresTime()
        {
            return DateTime.Now.AddMinutes(10);
        }


        public ViewResult ForgotPassword()
        {
            return View();
        }

        public ActionResult Logout()
        {
            DeleteUserCookies();
            return RedirectToHome();
        }

        private RedirectToActionResult RedirectToHome()
        {
            return RedirectToAction("index", "home");
        }
    }
}
