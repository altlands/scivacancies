﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using SciVacancies.WebApp.Commands;
using SciVacancies.WebApp.Infrastructure.Identity;
using SciVacancies.WebApp.ViewModels;
using Thinktecture.IdentityModel.Client;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using SciVacancies.WebApp.Models.OAuth;
using Microsoft.Framework.OptionsModel;
using AutoMapper;
using Microsoft.AspNet.Authorization;
using SciVacancies.SmtpNotificationsHandlers.SmtpNotificators;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Infrastructure;
using SciVacancies.WebApp.Infrastructure.WebAuthorize;
using SciVacancies.WebApp.Models.DataModels;
using SciVacancies.WebApp.Queries;

namespace SciVacancies.WebApp.Controllers
{
    [ResponseCache(NoStore = true)]
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;
        private readonly SciVacUserManager _userManager;
        private readonly IOptions<OAuthSettings> _oauthSettings;
        private readonly IAuthorizeService _authorizeService;
        private readonly IRecoveryPasswordService _recoveryPasswordService;

        public AccountController(SciVacUserManager userManager, IMediator mediator, IOptions<OAuthSettings> oAuthSettings, IAuthorizeService authorizeService, IRecoveryPasswordService recoveryPasswordService)
        {
            _mediator = mediator;
            _userManager = userManager;
            _oauthSettings = oAuthSettings;
            _authorizeService = authorizeService;
            _recoveryPasswordService = recoveryPasswordService;
        }

        public IActionResult LoginUser(string id)
        {
            SciVacUser user = null;
            switch (id)
            {

                case "user1":
                    user = _userManager.FindByName("researcher1");
                    break;
                case "user2":
                    user = _userManager.FindByName("researcher2");
                    break;
                default: //user3
                    user = _userManager.FindByName("researcher3");
                    break;
            }

            var cp = _authorizeService.LogOutAndLogInUser(Response, _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie));
            return RedirectToAccount(cp);
        }
        public IActionResult LoginOrganization()
        {
            var user = _userManager.FindByName("organization1");

            var cp = _authorizeService.LogOutAndLogInUser(Response, _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie));
            return RedirectToAccount(cp);
        }

        [PageTitle("Вход")]
        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> Login(AccountLoginViewModel model)
        {
            switch (model.User)
            {
                case AuthorizeUserTypes.Researcher:
                    switch (model.Resource)
                    {
                        case AuthorizeResourceTypes.OwnAuthorization:
                            //ищем учётку по логину и паролю
                            var user = await _userManager.FindAsync(model.Login, model.Password);

                            if (user == null)
                            {
                                //проверим, что учётка не найдена, т.к. пароль был введен неправильно
                                var userWithoutPassword = await _userManager.FindByNameAsync(model.Login);
                                if (userWithoutPassword != null)
                                {
                                    //TODO: отреагировать на превышение количества попыток входа
                                    //if (userWithoutPassword.AccessFailedCount) { ...; }

                                    await _userManager.AccessFailedAsync(userWithoutPassword.Id);
                                    return View("Error", model: "Вы ввели неверный пароль");
                                }

                                return View("Error", model: "Пользователь не найден");
                            }
                            var cp = _authorizeService.LogOutAndLogInUser(Response, _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie));
                            return RedirectToAccount(cp);
                        case AuthorizeResourceTypes.ScienceMap:
                            SetAuthorizationCookies(AuthorizeUserTypes.Researcher, AuthorizeResourceTypes.ScienceMap);
                            return Redirect(GetOAuthAuthorizationUrl(_oauthSettings.Options.Mapofscience));
                    }
                    break;
                case AuthorizeUserTypes.Organization:
                    switch (model.Resource)
                    {
                        case AuthorizeResourceTypes.Sciencemon:
                            SetAuthorizationCookies(AuthorizeUserTypes.Organization, AuthorizeResourceTypes.Sciencemon);

                            return Redirect(GetOAuthAuthorizationUrl(_oauthSettings.Options.Sciencemon));
                    }
                    break;
            }

            return null;
        }

        private void SetAuthorizationCookies(AuthorizeUserTypes accountType, AuthorizeResourceTypes authorizationType)
        {
            Response.Cookies.Append("account_type", accountType.ToString(), new Microsoft.AspNet.Http.CookieOptions { Expires = DateTime.Now.AddMinutes(10) });
            Response.Cookies.Append("authorization_type", authorizationType.ToString(), new Microsoft.AspNet.Http.CookieOptions { Expires = DateTime.Now.AddMinutes(10) });
        }
        private Tuple<AuthorizeUserTypes, AuthorizeResourceTypes> GetAuthorizationCookies()
        {
            var authorizeUserTypes = (AuthorizeUserTypes)Enum.Parse(typeof(AuthorizeUserTypes), Request.Cookies["account_type"]);
            var authorizeResourceTypes = (AuthorizeResourceTypes)Enum.Parse(typeof(AuthorizeResourceTypes), Request.Cookies["authorization_type"]);

            return new Tuple<AuthorizeUserTypes, AuthorizeResourceTypes>(authorizeUserTypes, authorizeResourceTypes);
        }

        #region OAuth

        private string SetStateCookie()
        {
            var state = Guid.NewGuid().ToString("N");
            Response.Cookies.Append("state", state, new Microsoft.AspNet.Http.CookieOptions { Expires = DateTime.Now.AddMinutes(10) });

            return state;
        }
        private string SetNonceCookie()
        {
            var nonce = Guid.NewGuid().ToString("N");
            Response.Cookies.Append("nonce", nonce, new Microsoft.AspNet.Http.CookieOptions { Expires = DateTime.Now.AddMinutes(10) });

            return nonce;
        }
        private string GetStateCookie()
        {
            return Request.Cookies["state"];
        }
        private string GetNonceCookie()
        {
            return Request.Cookies["nonce"];
        }
        private string GetCodeFromQuery()
        {
            return Request.Query["code"];
        }
        private string GetStateFromQuery()
        {
            return Request.Query["state"];
        }
        private string GetNonceFromQuery()
        {
            return Request.Query["nonce"];
        }

        private string GetOAuthAuthorizationUrl(OAuthProviderSettings oauth)
        {
            var oAuth2Client = new OAuth2Client(new Uri(oauth.AuthorizationEndpoint));

            return oAuth2Client.CreateCodeFlowUrl(oauth.ClientId, oauth.Scope, oauth.RedirectUrl, SetStateCookie(), SetNonceCookie());
        }

        #endregion
        #region API
        //общаемся с информикой
        private string GetOrganizationInfo(string inn)
        {
            //TODO url move to config
            var webRequest = WebRequest.Create(@"http://www.sciencemon.ru/ext-api/v1.0/org/" + inn);
            webRequest.Method = "GET";
            webRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("dev:informika"));
            var httpWebResponse = webRequest.GetResponse() as HttpWebResponse;
            string responseString;
            using (var stream = httpWebResponse.GetResponseStream())
            {
                var streamReader = new StreamReader(stream, Encoding.UTF8);
                responseString = streamReader.ReadToEnd();
            }
            return responseString;
        }

        #endregion


        [PageTitle("Оставить отзыв")]
        //Сюда редиректит после OAuth авторизации
        public async Task<ActionResult> Callback()
        {
            _authorizeService.Initialize(Request, Response);
            Tuple<AuthorizeUserTypes, AuthorizeResourceTypes> authorizationCookies = GetAuthorizationCookies();
            ClaimsPrincipal claimsPrincipal = null;
            switch (authorizationCookies.Item1)
            {
                case AuthorizeUserTypes.Admin:

                    break;
                case AuthorizeUserTypes.Organization:
                    switch (authorizationCookies.Item2)
                    {
                        case AuthorizeResourceTypes.Sciencemon:
                            if (!string.IsNullOrEmpty(GetStateFromQuery()) && !string.IsNullOrEmpty(GetStateCookie()) && GetStateFromQuery().Equals(GetStateCookie()))
                            {
                                if (!string.IsNullOrEmpty(GetCodeFromQuery()))
                                {
                                    var tokenResponse = await _authorizeService.GetOAuthAuthorizeTokenAsync(_oauthSettings.Options.Sciencemon, GetCodeFromQuery());

                                    if (!string.IsNullOrEmpty(tokenResponse.AccessToken))
                                    {
                                        var claims = await _authorizeService.GetOAuthUserAndTokensClaimsAsync(_oauthSettings.Options.Sciencemon, tokenResponse);

                                        OAuthOrgClaim orgClaim = JsonConvert.DeserializeObject<OAuthOrgClaim>(claims.Find(f => f.Type.Equals("org")).Value);

                                        //var orgUser = _userManager.FindByName(orgClaim.Inn);
                                        var orgUser = _userManager.FindByEmail(claims.Find(f => f.Type.Equals("email")).Value);

                                        if (orgUser == null)
                                        {
                                            OAuthOrgInformation organizationInformation =
                                                JsonConvert.DeserializeObject<OAuthOrgInformation>(
                                                    GetOrganizationInfo(orgClaim.Inn));
                                            AccountOrganizationRegisterViewModel orgModel =
                                                Mapper.Map<AccountOrganizationRegisterViewModel>(organizationInformation);

                                            //TODO - сделать всё в маппинге
                                            //orgModel.UserName = claims.Find(f => f.Type.Equals("username")).Value;
                                            orgModel.UserName = orgClaim.Inn;

                                            orgModel.Claims = claims.Where(w => w.Type.Equals("lastname")
                                                                                || w.Type.Equals("firstname")
                                                                                || w.Type.Equals("access_token")
                                                                                || w.Type.Equals("expires_in")
                                                                                || w.Type.Equals("refresh_token"))
                                                .ToList();


                                            var user = _mediator.Send(new RegisterUserOrganizationCommand { Data = orgModel });

                                            claimsPrincipal = _authorizeService.LogOutAndLogInUser(Response, _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie));
                                        }
                                        else
                                        {
                                            claimsPrincipal = _authorizeService.RefreshUserClaimTokensAndReauthorize(orgUser, claims, Response);
                                        }
                                    }
                                    else throw new ArgumentNullException("Token response is null");
                                }
                                else throw new ArgumentNullException("Oauth authorization code is null or empty");
                            }
                            else throw new ArgumentException("Oauth state mismatch");
                            break;
                    }
                    break;
                case AuthorizeUserTypes.Researcher:
                    switch (authorizationCookies.Item2)
                    {
                        case AuthorizeResourceTypes.ScienceMap:
                            if (!string.IsNullOrEmpty(GetStateFromQuery()) && !string.IsNullOrEmpty(GetStateCookie()) && GetStateFromQuery().Equals(GetStateCookie()))
                            {
                                if (!string.IsNullOrEmpty(GetCodeFromQuery()))
                                {
                                    var tokenResponse = await _authorizeService.GetOAuthAuthorizeTokenAsync(_oauthSettings.Options.Mapofscience, GetCodeFromQuery());

                                    if (!string.IsNullOrEmpty(tokenResponse.AccessToken))
                                    {
                                        var claims = await _authorizeService.GetOAuthUserAndTokensClaimsAsync(_oauthSettings.Options.Mapofscience, tokenResponse);

                                        var resUser = _userManager.FindByEmail(claims.Find(f => f.Type.Equals("email")).Value);

                                        if (resUser == null)
                                        {
                                            OAuthResProfile researcherProfile = JsonConvert.DeserializeObject<OAuthResProfile>(_authorizeService.GetResearcherProfile(tokenResponse.AccessToken));

                                            var accountResearcherRegisterDataModel =
                                                Mapper.Map<ResearcherRegisterDataModel>(researcherProfile);

                                            if (!string.IsNullOrWhiteSpace(accountResearcherRegisterDataModel.SciMapNumber))
                                            {
                                                //TODO: если пользователь у нас есть пользователь с таким же идентификатором Карты Наук, то для него сделать обновление данных
                                            }

                                            accountResearcherRegisterDataModel.UserName =
                                                claims.Find(f => f.Type.Equals("login")).Value;

                                            accountResearcherRegisterDataModel.Claims =
                                                claims.Where(w => w.Type.Equals("lastName")
                                                                  || w.Type.Equals("firstName")
                                                                  || w.Type.Equals("patronymic")
                                                                  || w.Type.Equals("access_token")
                                                                  || w.Type.Equals("expires_in")
                                                                  || w.Type.Equals("refresh_token")).ToList();

                                            //TODO: сохранять ID пользователя из внешней системы, чтобы не терять его если он сменит логин и почту

                                            var user = _mediator.Send(new RegisterUserResearcherCommand { Data = accountResearcherRegisterDataModel });

                                            claimsPrincipal = _authorizeService.LogOutAndLogInUser(Response, _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie));
                                        }
                                        else
                                        {
                                            //TODO: что делать если у нас есть пользователь с таким же логином

                                            //TODO: что делать если кто-то в нашей системе прописал email (такой же как у авторизованного пользователя)
                                            if (!resUser.EmailConfirmed)
                                            {
                                                //удалить "нашего" пользователя
                                            }
                                            else
                                            {
                                                //выполнить слияние с "нашим" пользователем
                                            }


                                            claimsPrincipal = _authorizeService.RefreshUserClaimTokensAndReauthorize(resUser, claims, Response);
                                        }
                                    }
                                    else throw new ArgumentNullException("Token response is null");
                                }
                                else throw new ArgumentNullException("Oauth authorization code is null or empty");
                            }
                            else throw new ArgumentException("Oauth state mismatch");
                            break;
                    }
                    break;
            }

            if (claimsPrincipal != null)
                return RedirectToAccount(claimsPrincipal);

            return RedirectToAccount(User);
        }

        [PageTitle("Регистрация")]
        public ViewResult Register() => View(new AccountResearcherRegisterViewModel());

        [PageTitle("Регистрация")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AccountResearcherRegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            /* Validation start */
            if (!model.Agreement)
                ModelState.AddModelError("Agreement", "Нет согласия на обработку персональных данных");

            SciVacUser existingUser = null;
            existingUser = await _userManager.FindByNameAsync(model.UserName);
            if (existingUser != null)
                ModelState.AddModelError("UserName", "Пользователь с таким логином уже существует");

            //existingUser = await _userManager.FindByEmailAsync(model.Email);
            //if (existingUser != null)
            //    //TODO: что делать если пользователь еще не подтвердил email (удалить его?)
            //    ModelState.AddModelError("Email", "Пользователь с таким Email уже существует");

            if (ModelState.ErrorCount > 0)
            {
                ModelState.AddModelError("Password", "Введите еще раз пароль");
                ModelState.AddModelError("ConfirmPassword", "Повторите ввод пароля");
                model.Captcha = null;
                ModelState.AddModelError("Captcha", "Введите код");
                return View(model);
            }
            /* Validation end */

            //создаем Исследователя
            var createUserResearcherCommand1 = new RegisterUserResearcherCommand
            {
                Data = new ResearcherRegisterDataModel
                {
                    Email = model.Email,
                    Phone = model.Phone,
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    SecondName = model.SecondName,
                    Patronymic = model.Patronymic,
                    FirstNameEng = model.FirstNameEng,
                    SecondNameEng = model.SecondNameEng,
                    PatronymicEng = model.PatronymicEng,
                    BirthYear = model.BirthYear,
                    Password = model.Password,
                    ConfirmPassword = model.ConfirmPassword
                }
            };
            var user = _mediator.Send(createUserResearcherCommand1);
            var researcherGuid1 = Guid.Parse(user.Claims.Single(s => s.ClaimType.Equals(ConstTerms.ClaimTypeResearcherId)).ClaimValue);

            //выходим и входим заново
            var identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            var claimsPrincipal = _authorizeService.LogOutAndLogInUser(Response, identity);

            //запускаем процедуру активации аккаунта
            var researcher = _mediator.Send(new SingleResearcherQuery { ResearcherGuid = researcherGuid1 });
            await _authorizeService.CallUserActivationAsync(user, researcher);

            return RedirectToAccount(claimsPrincipal);
        }

        #region restore password
        /// <summary>
        /// т.к. мы проверяем уникальность Логинов, то по нему ищем почту и запускаем процесс восстановления пароля
        /// </summary>
        /// <returns></returns>
        [PageTitle("Восстановление доступа к Системе")]
        public ViewResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// т.к. мы проверяем уникальность Логинов, то по нему ищем почту и запускаем процесс восстановления пароля
        /// </summary>
        /// <returns></returns>
        [PageTitle("Восстановление доступа к Системе")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string username)
        {
            // !!! Восстанавливаем пароль даже на неактивированную учётную запись !!!

            #region validation
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return View("Error", model: "Мы не нашли пользователя");
            
            //проверяем, что пользователь ЧУЖОЙ (и нам Не нужно подтверждать его Email)
            var logins = await _userManager.GetLoginsAsync(user.Id);
            if (logins != null && logins.Any())
            {
                return View("Error", model: $"Для восстановления пароля восползуйтесь порталом (или сайтом), через который Вы выполнили авторизацию ({string.Join(", ", logins.Select(c => c.LoginProvider).ToList())}).");
            }

            //todo: проверить что пользователь уже не заблокирован

            //проверяем последние запросы на сброс пароля
            var lastRequests = user.LastRequests;

                lastRequests.Add(DateTime.Now.ToUniversalTime());
            user.LastRequests = lastRequests;

            _userManager.Update(user);
            #endregion

            //TODO: сгенерировать каптчу, если PasswordRecoveryPeriod > x
            //TODO: заблокировать учётку, если PasswordRecoveryCount > y


            //TODO: сгенерировать новый код
            //_userManager.GeneratePasswordResetTokenAsync();
            //TODO: отправить письмо с новым кодом на восстановление пароля
            //_authorizeService.....

            return View();
        }

        /// <summary>
        /// пользователь вводит новые пароли
        /// </summary>
        /// <returns></returns>
        [PageTitle("Восстановление доступа к Системе")]
        public ViewResult RestorePassword(string token)
        {
            //1 TODO: проверить, что код еще не использовался

            //2 TODO: найти пользователя по коду

            return View();
        }

        /// <summary>
        /// пользователь ввёл новые пароли
        /// </summary>
        /// <returns></returns>
        [PageTitle("Восстановление доступа к Системе")]
        public ViewResult RestorePasswordConfirm(string userName, string password, string passwordConfirm)
        {
            return View();
        }
        #endregion

        #region account activation
        /// <summary>
        /// запросить письмо для активации учётной записи
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [PageTitle("Запрос письма для активации учетной записи")]
        [BindResearcherIdFromClaims]
        public async Task<IActionResult> ActivationRequest(Guid researcherGuid)
        {
            #region validation
            //если пользователь имеет роль Исследователя, то он активирован
            if (User.IsInRole(ConstTerms.RequireRoleResearcher))
            {
                return RedirectToAction("account", "researchers");
            }

            var user = await _userManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
                return View("Error", model: "Мы не нашли пользователя");

            var logins = await _userManager.GetLoginsAsync(user.Id);
            //проверяем, что пользователь ЧУЖОЙ (и нам Не нужно подтверждать его Email)
            if (logins != null && logins.Any())
            {
                return View("Error", model: "Ваша учётная запись должна была активироваться автоматически");
            }
            #endregion

            return View();
        }

        /// <summary>
        /// отправлено письмо для активации учётной записи
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [PageTitle("Запрошено письмо для активации учетной записи")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [BindResearcherIdFromClaims]
        public async Task<IActionResult> ActivationRequestPost(Guid researcherGuid)
        {
            #region validation
            //если пользователь имеет роль Исследователя, то он активирован
            if (User.IsInRole(ConstTerms.RequireRoleResearcher))
            {
                return RedirectToAction("account", "researchers");
            }

            var user = await _userManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
                return View("Error", model: "Мы не нашли пользователя");

            var logins = await _userManager.GetLoginsAsync(user.Id);
            //проверяем, что пользователь ЧУЖОЙ (и нам Не нужно подтверждать его Email)
            if (logins != null && logins.Any())
            {
                return View("Error", model: "Ваша учётная запись должна была активироваться автоматически");
            }
            #endregion

            var researcher = _mediator.Send(new SingleResearcherQuery { ResearcherGuid = researcherGuid });
            if (researcher == null)
                return View("Error", model: "Мы не нашли исследователя");

            await _authorizeService.CallUserActivationAsync(user, researcher);

            return View(model: "На указанный в профиле email отправлено письмо, для активации учётной записи. Пожалуйста, проверьте свою почту.");
        }

        /// <summary>
        /// подтверждение email и активация учётной записи
        /// </summary>
        /// <returns></returns>
        [PageTitle("Проверка подтверждения Email")]
        public async Task<IActionResult> ActivateAccount(string userName, string token)
        {
            #region validation
            //если пользователь имеет роль Исследователя, то он активирован
            if (User.IsInRole(ConstTerms.RequireRoleResearcher))
            {
                return RedirectToAction("account", "researchers");
            }

            var user = await _userManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
                return View("Error", model: "Мы не нашли пользователя");

            var logins = await _userManager.GetLoginsAsync(user.Id);
            //проверяем, что пользователь ЧУЖОЙ (и нам Не нужно подтверждать его Email)
            if (logins != null && logins.Any())
            {
                return View("Error", model: "Ваша учётная запись должна была активироваться автоматически");
            }
            #endregion

            var identityResult = await _userManager.ConfirmEmailAsync(user.Id, token);

            if (identityResult.Succeeded)
            {
                _userManager.AddToRole(user.Id, ConstTerms.RequireRoleResearcher);
                _authorizeService.LogOutAndLogInUser(Response, _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie));
            }

            return View(identityResult);
        }
        #endregion

        [PageTitle("Выход")]
        public ActionResult Logout()
        {
            Context.Response.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToHome();
        }


        private RedirectToActionResult RedirectToAccount(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal.IsInRole(ConstTerms.RequireRoleResearcher))
                return RedirectToAction("account", "researchers");

            if (claimsPrincipal.IsInRole(ConstTerms.RequireRoleOrganizationAdmin))
                return RedirectToAction("account", "organizations");

            return RedirectToAction("index", "home");
        }

        private RedirectToActionResult RedirectToHome()
        {
            return RedirectToAction("index", "home");
        }
    }
}
