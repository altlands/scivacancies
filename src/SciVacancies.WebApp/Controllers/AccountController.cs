using System;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using SciVacancies.WebApp.Commands;
using SciVacancies.WebApp.Infrastructure.Identity;
using SciVacancies.WebApp.ViewModels;
using Thinktecture.IdentityModel.Client;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace SciVacancies.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;
        private readonly SciVacUserManager _userManager;

        //TODO - move to config.json
        #region OAuth configuration
        Uri sciencemonAuthorizationEndpoint = new Uri("http://www.sciencemon.ru/oauth/v2/auth");
        Uri sciencemonTokenEndpoint = new Uri("http://www.sciencemon.ru/oauth/v2/token");
        Uri sciencemonUserEndpoint = new Uri("http://www.sciencemon.ru/api/user");
        string sciencemonClientId = "1_ikniwc909y8kog4k4sc00oogg0g8scc8o4k0wocgw4cg84k00";
        string sciencemonClientSecret = "67sq61c6xekgw0wgkosg04gwo488osk48ogks4og40cgws8ook";
        string sciencemonScope = null;

        Uri redirectUrl = new Uri("http://localhost:59075/account/callback");
        #endregion

        public AccountController(SciVacUserManager userManager, IMediator mediator)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [ResponseCache(NoStore = true)]
        [HttpPost]
        public ActionResult Login(AccountLoginViewModel model)
        {
            var user = model.IsResearcher ? _userManager.FindByName("researcher1") : _userManager.FindByName("organization1");
            var identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            var cp = new ClaimsPrincipal(identity);
            Context.Response.SignIn(DefaultAuthenticationTypes.ApplicationCookie, cp);

            //TODO - uncomment to pin it down to any job
            //if (model.IsResearcher)
            //{
            //    var state = Guid.NewGuid().ToString("N");
            //    var nonce = Guid.NewGuid().ToString("N");
            //    SetTempCookies(state, nonce);

            //    //TODO - допилить авторизацию с картой науки
            //}
            //else
            //{
            //    //записываем ключи в печеньки (против CSRF атак)
            //    var state = Guid.NewGuid().ToString("N");
            //    var nonce = Guid.NewGuid().ToString("N");
            //    SetTempCookies(state, nonce);

            //    //получаем урл на страницу OAuth авторизации на их стороне
            //    var client = new OAuth2Client(sciencemonAuthorizationEndpoint);
            //    var authorizationUrl = client.CreateCodeFlowUrl(sciencemonClientId, sciencemonScope, redirectUrl.AbsoluteUri, state, nonce);

            //    return Redirect(authorizationUrl);
            //}

            return RedirectToHome();
        }

        #region OAuth

        private void SetTempCookies(string state, string nonce)
        {
            Response.Cookies.Append("state", state, new Microsoft.AspNet.Http.CookieOptions { Expires = DateTime.Now.AddHours(1) });
            Response.Cookies.Append("nonce", nonce, new Microsoft.AspNet.Http.CookieOptions { Expires = DateTime.Now.AddHours(1) });
        }
        private Tuple<string, string> GetTempCookies()
        {
            var state = Request.Cookies["state"];
            var nonce = Request.Cookies["nonce"];

            return Tuple.Create(state, nonce);
        }

        public async Task<ActionResult> Callback()
        {
            var authorizationCode = Request.Query["code"];
            var state = Request.Query["state"];

            var tempCookies = GetTempCookies();

            if (String.IsNullOrEmpty(authorizationCode) && !String.IsNullOrEmpty(state) && state.Equals(tempCookies.Item1, StringComparison.Ordinal))
            {
                var tokenResponse = await GetTokens(authorizationCode);

                await ValidateResponseAndSignInAsync(tokenResponse, tempCookies.Item2);

                if (!String.IsNullOrEmpty(tokenResponse.IdentityToken))
                {

                }
                if (!String.IsNullOrEmpty(tokenResponse.AccessToken))
                {

                }
            }
            else
            {
                ViewBag.Error = "Не подфартило";
            }

            return RedirectToHome();
        }

        private async Task<TokenResponse> GetTokens(string code)
        {
            var client = new OAuth2Client(sciencemonTokenEndpoint, sciencemonClientId, sciencemonClientSecret);

            return await client.RequestAuthorizationCodeAsync(code, redirectUrl.AbsoluteUri);
        }

        private async Task ValidateResponseAndSignInAsync(TokenResponse response, string nonce)
        {
            if (!String.IsNullOrEmpty(response.IdentityToken))
            {
                //TODO - валидация токенов нужна?
                //var tokenClaims = 

                var claims = new List<Claim>();

                if (!String.IsNullOrEmpty(response.AccessToken))
                {
                    claims.AddRange(await GetUserInfoClaimsAsync(response.AccessToken));

                    claims.Add(new Claim("access_token", response.AccessToken));
                    //Expires In = неправильно считает
                    claims.Add(new Claim("expires_in", DateTime.Now.AddSeconds(response.ExpiresIn).ToString()));
                }

                if (!string.IsNullOrEmpty(response.RefreshToken))
                {
                    claims.Add(new Claim("refresh_token", response.RefreshToken));
                }

                //TODO - по какому полю правильней искать?
                var orgUser = _userManager.FindByEmail(claims.FirstOrDefault(f => f.Type.Equals("email")).Value);

                if (orgUser == null)
                {
                    AccountOrganizationRegisterViewModel orgModel = new AccountOrganizationRegisterViewModel();

                    //TODO - достаём из claims ИНН
                    //TODO - забираем из их API данные по инн

                    var command = new RegisterUserOrganizationCommand
                    {
                        Data = orgModel
                    };
                    var user = _mediator.Send(command);

                    var identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    var cp = new ClaimsPrincipal(identity);
                    //signing out...
                    Context.Response.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    //...before signing in
                    Context.Response.SignIn(DefaultAuthenticationTypes.ApplicationCookie, cp);
                }
                else
                {
                    var identity = _userManager.CreateIdentity(orgUser, DefaultAuthenticationTypes.ApplicationCookie);
                    var cp = new ClaimsPrincipal(identity);
                    Context.Response.SignIn(DefaultAuthenticationTypes.ApplicationCookie, cp);
                }
            }
        }

        private async Task<IEnumerable<Claim>> GetUserInfoClaimsAsync(string accessToken)
        {
            var userInfoClient = new UserInfoClient(sciencemonUserEndpoint, accessToken);
            var userInfo = await userInfoClient.GetAsync();

            var claims = new List<Claim>();
            userInfo.Claims.ToList().ForEach(f => claims.Add(new Claim(f.Item1, f.Item2)));

            return claims;
        }

        #endregion


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
            Context.Response.SignIn(DefaultAuthenticationTypes.ApplicationCookie, cp);

            return RedirectToHome();
        }

        public ViewResult ForgotPassword()
        {
            return View();
        }

        [ResponseCache(NoStore = true)]
        public ActionResult Logout()
        {
            Context.Response.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToHome();
        }

        [ResponseCache(NoStore = true)]
        private RedirectToActionResult RedirectToHome()
        {
            return RedirectToAction("index", "home");
        }
    }
}
