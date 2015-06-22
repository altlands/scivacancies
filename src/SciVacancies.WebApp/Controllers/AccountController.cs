using System;
using System.Linq;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using SciVacancies.ReadModel;
using SciVacancies.WebApp.Commands;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Infrastructure;
using SciVacancies.WebApp.Infrastructure.Identity;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IReadModelService _readModelService;
        private readonly IMediator _mediator;
        private readonly SciVacUserManager _userManager;

        public AccountController(SciVacUserManager userManager, IReadModelService readModelService, IMediator mediator)
        {
            _readModelService = readModelService;
            _mediator = mediator;
            _userManager = userManager;
        }

        private void DeleteUserCookies()
        {
            Context.Response.Cookies.Append(ConstTerms.CookiesKeyForUserRole, string.Empty, new CookieOptions { Expires = DateTime.Now.AddDays(-1) });
            Context.Response.Cookies.Append(ConstTerms.CookiesKeyForUserName, string.Empty, new CookieOptions { Expires = DateTime.Now.AddDays(-1) });
            Context.Response.Cookies.Append(ConstTerms.CookiesKeyForResearcherGuid, string.Empty, new CookieOptions { Expires = DateTime.Now.AddDays(-1) });
            Context.Response.Cookies.Append(ConstTerms.CookiesKeyForOrganizationGuid, string.Empty, new CookieOptions { Expires = DateTime.Now.AddDays(-1) });

        }

        [HttpPost]
        public ActionResult Login(AccountLoginViewModel model)
        {
            DeleteUserCookies();
            var timeStamp = GetExpiresTime();
            Context.Response.Cookies.Append(ConstTerms.CookiesKeyForUserName, model.Login, new CookieOptions { Expires = timeStamp });
            Context.Response.Cookies.Append(ConstTerms.CookiesKeyForUserRole, model.IsResearcher.ToString(), new CookieOptions { Expires = timeStamp });

            //попытка получить существующий Guid из БД
            var userGuid = (model.IsResearcher
                ? _readModelService.SelectResearchers().Any() ? _readModelService.SelectResearchers().First().Guid : Guid.NewGuid()
                : _readModelService.SelectOrganizations().Any() ? _readModelService.SelectOrganizations().First().Guid : Guid.NewGuid()
                ).ToString();
            var userGuidKey = model.IsResearcher ? ConstTerms.CookiesKeyForResearcherGuid : ConstTerms.CookiesKeyForOrganizationGuid;
            Context.Response.Cookies.Append(userGuidKey, userGuid, new CookieOptions { Expires = timeStamp });

            //var user = _userManager.FindByName("SashaOzz");            
            //var identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            //var cp = new ClaimsPrincipal(identity);
            //Context.Response.SignIn(DefaultAuthenticationTypes.ApplicationCookie,cp);            
            return RedirectToHome();
        }


        public ViewResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(AccountRegisterViewModel model)
        {
            var command = new RegisterUserCommand() {Data = model};
            var user = _mediator.Send(command);
                       
            var identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            var cp = new ClaimsPrincipal(identity);
            Context.Response.SignIn(DefaultAuthenticationTypes.ApplicationCookie,cp);
            
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
            //Context.Response.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToHome();
        }

        private RedirectToActionResult RedirectToHome()
        {
            return RedirectToAction("index", "home");
        }
    }
}
