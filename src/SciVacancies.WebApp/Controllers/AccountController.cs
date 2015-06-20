using System;
using System.Security.Claims;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Infrastructure;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly SciVacUserManager _userManager;

        public AccountController(SciVacUserManager userManager)
        {
            _userManager = userManager;
        }


        [HttpPost]
        public ActionResult Login(AccountLoginViewModel model)
        {            
            var timeStamp = GetExpiresTime();                                                
            var user = _userManager.FindByName("SashaOzz");            
            var identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            var cp = new ClaimsPrincipal(identity);
            Context.Response.SignIn(DefaultAuthenticationTypes.ApplicationCookie,cp);            
            return RedirectToHome();
        }


        public ViewResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(AccountRegisterViewModel model)
        {            
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
            Context.Response.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToHome();
        }

        private RedirectToActionResult RedirectToHome()
        {
            return RedirectToAction("index", "home");
        }
    }
}
