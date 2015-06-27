using System;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using SciVacancies.WebApp.Commands;
using SciVacancies.WebApp.Infrastructure.Identity;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;
        private readonly SciVacUserManager _userManager;

        public AccountController(SciVacUserManager userManager, IMediator mediator)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [HttpPost]
        public ActionResult Login(AccountLoginViewModel model)
        {
            var user = model.IsResearcher ? _userManager.FindByName("researcher1@mailer.org") : _userManager.FindByName("organization1@mailer.org");
            var identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            var cp = new ClaimsPrincipal(identity);
            Context.Response.SignIn(DefaultAuthenticationTypes.ApplicationCookie, cp);
            return RedirectToHome();
        }

        [PageTitle("Регистрация")]
        public ViewResult Register() => View(new AccountResearcherRegisterViewModel());
        [PageTitle("Регистрация")]
        public ViewResult RegisterFilled() => View("register", new AccountResearcherRegisterViewModel
        {
            SecondName = "Фамилько",
            FirstName = "Имён",
            Patronymic = "Отчествович",
            UserName = "researcher@mail.scivacancies.org",
            Phone = "964abcdefg",
            BirthYear = DateTime.Now.AddYears(-40).Year,
            Email = "researcher@mail.scivacancies.org"
        });

        [HttpPost]
        public IActionResult Register(AccountResearcherRegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (!model.Agreement)
            {
                ModelState.AddModelError("Agreement", "Нет согласия на обработку персональных данных");
                return View(model);
            }

            var command = new RegisterUserResearcherCommand
            {
                Data = model
            };
            var user = _mediator.Send(command);
                       
            var identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            var cp = new ClaimsPrincipal(identity);

            //signing out...
            Context.Response.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            //...before signing in
            Context.Response.SignIn(DefaultAuthenticationTypes.ApplicationCookie,cp);
            
            return RedirectToHome();
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
