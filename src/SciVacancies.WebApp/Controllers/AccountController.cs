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
using Microsoft.Framework.Logging;
using SciVacancies.Domain.DataModels;
using SciVacancies.WebApp.Engine;
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
        private readonly IOptions<ApiSettings> _apiSettings;
        private readonly ILogger _logger;

        public AccountController(SciVacUserManager userManager, IMediator mediator, IOptions<OAuthSettings> oAuthSettings, IOptions<ApiSettings> apiSettings, IAuthorizeService authorizeService, ILoggerFactory loggerFactory)
        {
            _mediator = mediator;
            _userManager = userManager;
            _oauthSettings = oAuthSettings;
            _authorizeService = authorizeService;
            _apiSettings = apiSettings;
            _logger = loggerFactory.CreateLogger<AccountController>();
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

        [PageTitle("Вход")]
        [HttpGet]
        public IActionResult LoginOrganization()
        {
            SetAuthorizationCookies(AuthorizeUserTypes.Organization, AuthorizeResourceTypes.Sciencemon);
            return Redirect(GetOAuthAuthorizationUrl(_oauthSettings.Value.Sciencemon));
        }

        [PageTitle("Вход")]
        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Login(AccountLoginViewModel model)
        {
            if (model.Login == null && model.User == null && model.Password == null && model.Resource == null)
                return View(new AccountLoginViewModel());

            switch (model.User)
            {
                case AuthorizeUserTypes.Researcher:
                    switch (model.Resource)
                    {
                        case AuthorizeResourceTypes.OwnAuthorization:

                            if (!ModelState.IsValid)
                                return View(model);

                            if (string.IsNullOrWhiteSpace(model.Login) || string.IsNullOrWhiteSpace(model.Password))
                                ModelState.AddModelError("Login", "Вы не указали логин и/или пароль");

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
                                    ModelState.AddModelError("Password", "Вы ввели неверный пароль");
                                }
                                ModelState.AddModelError("Login", "Пользователь не найден");
                            }

                            if (!ModelState.IsValid)
                                return View(model);

                            var cp = _authorizeService.LogOutAndLogInUser(Response, _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie));
                            return RedirectToAccount(cp);
                        case AuthorizeResourceTypes.ScienceMap:
                            SetAuthorizationCookies(AuthorizeUserTypes.Researcher, AuthorizeResourceTypes.ScienceMap);
                            return Redirect(GetOAuthAuthorizationUrl(_oauthSettings.Value.Mapofscience));
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

        private string GetErrorFromQuery()
        {
            return Request.Query["error"];
        }

        private string GetErrorDescriptionFromQuery()
        {
            return Request.Query["error_description"];
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

        //Сюда редиректит после OAuth авторизации
        public async Task<ActionResult> Callback()
        {
            Tuple<AuthorizeUserTypes, AuthorizeResourceTypes> authorizationCookies = GetAuthorizationCookies();
            ClaimsPrincipal claimsPrincipal = null;
            switch (authorizationCookies.Item1)
            {
                case AuthorizeUserTypes.Admin:
                    throw new NotImplementedException();
                case AuthorizeUserTypes.Organization:
                    switch (authorizationCookies.Item2)
                    {
                        case AuthorizeResourceTypes.Sciencemon:
                            if (!string.IsNullOrEmpty(GetStateFromQuery()) && !string.IsNullOrEmpty(GetStateCookie()) && GetStateFromQuery().Equals(GetStateCookie()))
                            {
                                if (!string.IsNullOrEmpty(GetCodeFromQuery()))
                                {
                                    var tokenResponse =
                                        await
                                            _authorizeService.GetOAuthAuthorizeTokenAsync(
                                                _oauthSettings.Value.Sciencemon, GetCodeFromQuery());

                                    if (!string.IsNullOrEmpty(tokenResponse.AccessToken))
                                    {
                                        var claims =
                                            await
                                                _authorizeService.GetOAuthUserAndTokensClaimsAsync(
                                                    _oauthSettings.Value.Sciencemon, tokenResponse);

                                        OAuthOrgClaim orgClaim =
                                            JsonConvert.DeserializeObject<OAuthOrgClaim>(
                                                claims.Find(f => f.Type.Equals("org")).Value);

                                        var orgUser = _userManager.FindByName(orgClaim.Inn);

                                        if (orgUser == null)
                                            orgUser = _userManager.FindByEmail(claims.Find(f => f.Type.Equals("email")).Value);

                                        var serializedOrganizationRawInfo = GetOrganizationInfo(orgClaim.Inn);
                                        OAuthOrgInformation organizationInformation = JsonConvert.DeserializeObject<OAuthOrgInformation>(serializedOrganizationRawInfo);

                                        var accountOrganizationRegisterViewModel = Mapper.Map<AccountOrganizationRegisterViewModel>(organizationInformation);

                                        if (orgUser == null)
                                        {
                                            //TODO - сделать всё в маппинге
                                            //orgModel.UserName = claims.Find(f => f.Type.Equals("username")).Value;
                                            accountOrganizationRegisterViewModel.UserName = orgClaim.Inn;

                                            accountOrganizationRegisterViewModel.Claims = claims.Where(w => w.Type.Equals("lastname")
                                                                                || w.Type.Equals("firstname")
                                                                                || w.Type.Equals("access_token")
                                                                                || w.Type.Equals("expires_in")
                                                                                || w.Type.Equals("refresh_token"))
                                                .ToList();


                                            var user =
                                                _mediator.Send(new RegisterUserOrganizationCommand { Data = accountOrganizationRegisterViewModel });

                                            claimsPrincipal = _authorizeService.LogOutAndLogInUser(Response,
                                                _userManager.CreateIdentity(user,
                                                    DefaultAuthenticationTypes.ApplicationCookie));
                                        }
                                        else
                                        {
                                            claimsPrincipal =
                                                _authorizeService.RefreshUserClaimTokensAndReauthorize(orgUser, claims,
                                                    Response);

                                            try
                                            {
                                                var organizationGuid = Guid.Parse(orgUser.Claims.Single(c => c.ClaimType == ConstTerms.ClaimTypeOrganizationId).ClaimValue);
                                                var organizationReadModel = _mediator.Send(new SingleOrganizationQuery { OrganizationGuid = organizationGuid });
                                                if (organizationReadModel.address != organizationInformation.postAddress
                                                    || organizationReadModel.email != organizationInformation.email
                                                    || organizationReadModel.head_firstname != organizationInformation.headFirstName
                                                    || organizationReadModel.head_patronymic != organizationInformation.headPatronymic
                                                    || organizationReadModel.head_secondname != organizationInformation.headLastName
                                                    || organizationReadModel.inn != organizationInformation.inn
                                                    || organizationReadModel.ogrn != organizationInformation.ogrn
                                                    || organizationReadModel.name != organizationInformation.title
                                                    || organizationReadModel.shortname != organizationInformation.shortTitle
                                                    || organizationReadModel.foiv_id != int.Parse(organizationInformation.foiv.id)
                                                    || organizationReadModel.orgform_id != int.Parse(organizationInformation.opf.id)
                                                   
                                                    //|| !(
                                                    //    (organizationReadModel.researchdirections == null || organizationReadModel.researchdirections.Count == 0)
                                                    //    && (organizationInformation.researchDirections == null || organizationInformation.researchDirections.Count == 0)
                                                    //    )

                                                    //|| (
                                                    //    organizationReadModel.researchdirections != null
                                                    //    && organizationReadModel.researchdirections.Count > 0

                                                    //    && organizationInformation.researchDirections != null
                                                    //    && organizationInformation.researchDirections.Count > 0

                                                    //    && organizationReadModel.researchdirections.First().id != int.Parse(organizationInformation.researchDirections.First().id)
                                                    //    )

                                                    )
                                                {
                                                    var organizationDataModel = Mapper.Map<OrganizationDataModel>(accountOrganizationRegisterViewModel);
                                                    _mediator.Send(new UpdateOrganizationCommand
                                                    {
                                                        OrganizationGuid = organizationGuid,
                                                        Data = organizationDataModel
                                                    });
                                                }
                                            }
                                            catch (Exception exception)
                                            {
                                                _logger.LogError($"Exception happend with updating organizaiton info: {exception.Message}");
                                                throw exception;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        _logger.LogError("Token response is null");
                                        throw new ArgumentNullException("Token response is null");
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrWhiteSpace(GetErrorFromQuery()))
                                    {
                                        if (!string.IsNullOrWhiteSpace(GetErrorDescriptionFromQuery()))
                                        {
                                            _logger.LogError(GetErrorDescriptionFromQuery());
                                            return View("Error", GetErrorDescriptionFromQuery());
                                        }
                                        _logger.LogError(GetErrorFromQuery());
                                        return View("Error", GetErrorFromQuery());
                                    }
                                    _logger.LogError("Oauth authorization code is null or empty");
                                    throw new ArgumentNullException("Oauth authorization code is null or empty");
                                }
                            }
                            else
                            {
                                _logger.LogError("Oauth state mismatch");
                                throw new ArgumentException("Oauth state mismatch");
                            }
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
                                    var tokenResponse =
                                        await
                                            _authorizeService.GetOAuthAuthorizeTokenAsync(
                                                _oauthSettings.Value.Mapofscience, GetCodeFromQuery());

                                    if (!string.IsNullOrEmpty(tokenResponse.AccessToken))
                                    {
                                        var claims =
                                            await
                                                _authorizeService.GetOAuthUserAndTokensClaimsAsync(
                                                    _oauthSettings.Value.Mapofscience, tokenResponse);

                                        //ищем пользователя по Email в нашей БД


                                        var resUser = _userManager.FindByName(claims.Find(f => f.Type.Equals("login")).Value);

                                        if (resUser == null)
                                            resUser = _userManager.FindByEmail(claims.Find(f => f.Type.Equals("email")).Value);

                                        //получаем информацию о Пользователе с Карты Наук
                                        var jsonUser = _authorizeService.GetResearcherProfile(tokenResponse.AccessToken, _apiSettings.Value.Mapofscience.UserProfile);
                                        OAuthResProfile researcherProfile =
                                            JsonConvert.DeserializeObject<OAuthResProfile>(jsonUser);
                                        var accountResearcherRegisterDataModel =
                                            Mapper.Map<ResearcherRegisterDataModel>(researcherProfile);

                                        if (resUser == null)
                                        //в нашей БД пользователь по Email не найден
                                        {

                                            //TODO: что делать если у нас есть пользователь с таким же логином

                                            if (
                                                !string.IsNullOrWhiteSpace(
                                                    accountResearcherRegisterDataModel.SciMapNumber))
                                            {
                                                //TODO: если у нас есть пользователь с таким же идентификатором Карты Наук, то для него сделать обновление данных
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

                                            var user =
                                                _mediator.Send(new RegisterUserResearcherCommand
                                                {
                                                    Data = accountResearcherRegisterDataModel
                                                });

                                            claimsPrincipal = _authorizeService.LogOutAndLogInUser(Response,
                                                _userManager.CreateIdentity(user,
                                                    DefaultAuthenticationTypes.ApplicationCookie));
                                        }
                                        else
                                        {

                                            //если кто-то в нашей системе прописал email (такой же как у авторизованного с Карты Науки пользователя)
                                            if (resUser.Email == accountResearcherRegisterDataModel.Email
                                                &&
                                                !string.IsNullOrWhiteSpace(
                                                    accountResearcherRegisterDataModel.SciMapNumber)
                                                /*пользователь с Карты Науки прошел их собственную авторизацию */
                                                )
                                            {
                                                //проверить, что Пользователя Карты наук еще нет в нашей БД
                                                var logins = await _userManager.GetLoginsAsync(resUser.Id);

                                                if (logins == null
                                                    ||
                                                    logins.All(
                                                        c =>
                                                            c.LoginProvider != ConstTerms.LoginProviderScienceMap &&
                                                            c.ProviderKey !=
                                                            accountResearcherRegisterDataModel.SciMapNumber))
                                                {
                                                    //выполнить слияние с "нашим" пользователем
                                                    resUser = _mediator.Send(new MergeSciMapAndUserResearcherCommand
                                                    {
                                                        UserId = resUser.Id,
                                                        SciMapNumber = accountResearcherRegisterDataModel.SciMapNumber,
                                                        Claims = claims.Where(w => //w.Type.Equals("lastName")
                                                                                   //|| w.Type.Equals("firstName")
                                                                                   //|| w.Type.Equals("patronymic")
                                                            w.Type.Equals("access_token")
                                                            || w.Type.Equals("expires_in")
                                                            || w.Type.Equals("refresh_token")).ToList()
                                                    });
                                                }
                                            }

                                            claimsPrincipal =
                                                _authorizeService.RefreshUserClaimTokensAndReauthorize(resUser, claims,
                                                    Response);
                                        }
                                    }
                                    else throw new ArgumentNullException("Token response is null");
                                }
                                else
                                {
                                    _logger.LogError("Oauth authorization code is null or empty");
                                    return RedirectToAction("index", "home");
                                    throw new ArgumentNullException("Oauth authorization code is null or empty");
                                }
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
        public async Task<IActionResult> Register(AccountResearcherRegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            /* Validation start */
            if (!model.Agreement)
                ModelState.AddModelError("Agreement", "Нет согласия на обработку персональных данных");

            if (await _userManager.FindByNameAsync(model.UserName) != null)
                ModelState.AddModelError("UserName", "Пользователь с таким логином уже существует");

            //if (await _userManager.FindByEmailAsync(model.Email) != null)
            //    //TODO: что делать если пользователь еще не подтвердил email (удалить его?)
            //    ModelState.AddModelError("Email", "Пользователь с таким Email уже существует");

            if (ModelState.ErrorCount > 0)
            {
                ModelState.AddModelError("Password", "Введите еще раз пароль");
                ModelState.AddModelError("ConfirmPassword", "Повторите ввод пароля");
                return View(model);
            }
            /* Validation end */

            //создаем Исследователя
            RegisterUserResearcherCommand createUserResearcherCommand1;

            try
            {
                createUserResearcherCommand1 = new RegisterUserResearcherCommand
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
            }
            catch (Exception e)
            {
                Console.WriteLine("не удалось создать команду RegisterUserResearcherCommand");
                throw e;
            }

            SciVacUser user;
            try
            {
                user = _mediator.Send(createUserResearcherCommand1);
            }
            catch (Exception e)
            {
                Console.WriteLine("не удалось отправить команду");
                Console.WriteLine(e.Message);
                throw e;
            }

            var researcherGuid1 = Guid.Parse(user.Claims.Single(s => s.ClaimType.Equals(ConstTerms.ClaimTypeResearcherId)).ClaimValue);

            //выходим и входим заново
            var identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            var claimsPrincipal = _authorizeService.LogOutAndLogInUser(Response, identity);

            //запускаем процедуру активации аккаунта
            var researcher = _mediator.Send(new SingleResearcherQuery { ResearcherGuid = researcherGuid1 });
            await _authorizeService.CallUserActivationAsync(user, researcher);

            return RedirectToAction("SuccessfulRegister");
        }

        public IActionResult SuccessfulRegister() => View("SuccessfulRegister");

        #region restore password
        /// <summary>
        /// т.к. мы проверяем уникальность Логинов, то по нему ищем почту и запускаем процесс восстановления пароля
        /// </summary>
        /// <returns></returns>
        [PageTitle("Восстановление доступа к Системе")]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            var model = new ForgotPasswordViewModel();
            if (User.Identity.IsAuthenticated)
                model.UserName = User.Identity.Name;

            return View(model);
        }

        /// <summary>
        /// т.к. мы проверяем уникальность Логинов, то по нему ищем почту и запускаем процесс восстановления пароля
        /// </summary>
        /// <returns></returns>
        [PageTitle("Восстановление доступа к Системе")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            // Восстанавливаем пароль даже на неактивированную учётную запись
            #region validation
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                ModelState.AddModelError("UserName", "Мы не нашли пользователя");
                return View(model);
            }

            //проверяем, что пользователь ЧУЖОЙ (и нам Не нужно подтверждать его Email)
            var logins = await _userManager.GetLoginsAsync(user.Id);
            if (logins != null && logins.Any())
                ModelState.AddModelError("UserName", $"Для восстановления пароля воспользуйтесь порталом (или сайтом), через который Вы выполнили авторизацию ({string.Join(", ", logins.Select(c => c.LoginProvider).ToList())}).");

            if (ModelState.ErrorCount > 0)
                return View(model);
            #endregion

            var lastRequests = user.LastRequests;
            lastRequests.Sort((x, y) => y.CompareTo(x));

            //если пользователь заблокирован
            var lockoutEndDate = _userManager.GetLockoutEndDate(user.Id);
            var lockoutEnabled = _userManager.GetLockoutEnabled(user.Id);
            if (lockoutEnabled
                && lockoutEndDate > DateTime.UtcNow)
            {
                return View("Error", $"Ваш пользователь временно заблокирован. Повторите попытку через {Math.Round((lockoutEndDate - DateTime.UtcNow).TotalMinutes, 0)} минут");
            }

            //сбросить блокировки
            await _userManager.SetLockoutEnabledAsync(user.Id, false);
            await _userManager.SetLockoutEndDateAsync(user.Id, DateTimeOffset.Now.AddSeconds(-1));

            if (lastRequests.Any())
            {
                //todo: вынести все параметры в конфиг

                var afterLastRequestPeriod = new TimeSpan(0, 10, 0); //10 min
                //если последний запрос на сброс пароля был менее, чем afterLastRequestPeriod (минут/секунд) назад, то показывать капчу.
                if ((DateTime.UtcNow - lastRequests.Max()) < afterLastRequestPeriod)
                {
                    //todo: показывать капчу
                }

                var nLastRequestsCount = 3; //последние n-запросов
                var nLastRequestsPeriod = new TimeSpan(0, 60, 0);
                //период, в котором не должно быть запросов пароля, чтобы не заблокировать пользователя
                var tBlockPeriod = new TimeSpan(0, 90, 0); //90 min
                //если последние n-запрос на сброс пароля были менее, в течение nLastRequestsCountPeriod (минут/секунд), то временно заблокировать пользователя на t-период.
                if (lastRequests.Count >= nLastRequestsCount
                    && (DateTime.UtcNow - lastRequests.Take(nLastRequestsCount).Min()) < nLastRequestsPeriod)
                {
                    //блокируем пользователя на t-период
                    await _userManager.SetLockoutEnabledAsync(user.Id, true);
                    await
                        _userManager.SetLockoutEndDateAsync(user.Id,
                            new DateTimeOffset(DateTime.UtcNow.Add(tBlockPeriod)));

                    return View("Error", "Ваш пользователь временно заблокирован.");
                }

            }

            //фиксируем текущий запрос на сброс пароля
            lastRequests.Add(DateTime.UtcNow);
            user.LastRequests = lastRequests;
            _userManager.Update(user);

            var userClaims = _userManager.GetClaimsAsync(user.Id);
            if (userClaims != null)
            {
                var researcherClaim = user.Claims.SingleOrDefault(c => c.ClaimType == ConstTerms.ClaimTypeResearcherId);
                if (researcherClaim != null)
                {
                    var researcher =
                        _mediator.Send(new SingleResearcherQuery
                        {
                            ResearcherGuid = Guid.Parse(researcherClaim.ClaimValue)
                        });

                    //отправить письмо с новым кодом на восстановление пароля
                    await _authorizeService.CallPasswordResetAsync(user, researcher);

                    //return View("Success", "Для восстановления доступа к Порталу мы отправили вам письмо на электронную почту, указанную при регистрации.");
                    return RedirectToAction("ForgotPasswordEmailSent");
                }
            }

            return View("Error", "Мы не нашли ваш профиль, чтобы отправить вам письмо.");
        }

        public ViewResult ForgotPasswordEmailSent()
        {
            return View("Success", "Для восстановления доступа к Порталу мы отправили вам письмо на электронную почту, указанную при регистрации.");
        }

        /// <summary>
        /// пользователь вводит новые пароли
        /// </summary>
        /// <returns></returns>
        [PageTitle("Восстановление доступа к Системе")]
        public async Task<IActionResult> RestorePassword(string userName, string token)
        {
            #region validation
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                return View("Error", "Мы не нашли пользователя");

            //проверяем, что пользователь ЧУЖОЙ (и нам Не нужно подтверждать его Email)
            var logins = await _userManager.GetLoginsAsync(user.Id);
            if (logins != null && logins.Any())
                return View("Error", $"Для восстановления пароля воспользуйтесь порталом (или сайтом), через который Вы выполнили авторизацию ({string.Join(", ", logins.Select(c => c.LoginProvider).ToList())}).");
            #endregion

            if (await _userManager.VerifyUserTokenAsync(user.Id, "ChangePa$$bord", token))
            {
                var model = new RestorePasswordViewModel
                {
                    ResetToken = await _userManager.GeneratePasswordResetTokenAsync(user.Id),
                    UserName = user.UserName
                };

                //удалить данные о времени запроса сброса пароля
                user.LastRequests = null;
                _userManager.Update(user);

                return View(model);
            }

            return View("Error", "Неверные данные для сброса пароля");
        }

        /// <summary>
        /// пользователь ввёл новые пароли
        /// </summary>
        /// <returns></returns>
        [PageTitle("Восстановление доступа к Системе")]
        [HttpPost]
        public async Task<IActionResult> RestorePasswordConfirm(RestorePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View("RestorePassword", model);

            #region validation
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
                return View("Error", "Мы не нашли пользователя");

            //проверяем, что пользователь ЧУЖОЙ (и нам Не нужно подтверждать его Email)
            var logins = await _userManager.GetLoginsAsync(user.Id);
            if (logins != null && logins.Any())
                return View("Error", $"Для восстановления пароля воспользуйтесь порталом (или сайтом), через который Вы выполнили авторизацию ({string.Join(", ", logins.Select(c => c.LoginProvider).ToList())}).");
            #endregion

            //обновить ключ перед сменой пароля, чтобы куки авторизации в браузерах перестали быть валидными
            var identityResult = await _userManager.ResetPasswordAsync(user.Id, model.ResetToken, model.Password);
            await _userManager.UpdateSecurityStampAsync(user.Id);

            if (identityResult.Succeeded)
                _authorizeService.LogOutAndLogInUser(Response, _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie));

            TempData["modelErrors"] = identityResult.Errors;
            return RedirectToAction("RestorePasswordConfirmed");
        }

        /// <summary>
        /// вспомогательный класс для отчета об успешной авторизации
        /// </summary>
        internal class SuccessfulIdentityResultViewModel : IdentityResult
        {
            public SuccessfulIdentityResultViewModel() : base(true) { }
        }

        [PageTitle("Восстановление доступа к Системе")]
        public IActionResult RestorePasswordConfirmed()
        {
            var errorsList = new List<string>();
            var errors = TempData["modelErrors"] as string[];
            if (errors != null && errors.Any())
            {
                errorsList.AddRange(errors);
            }
            return View(errorsList.Count == 0 ? new SuccessfulIdentityResultViewModel() : new IdentityResult(errorsList));
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
                return View("Error", "Мы не нашли пользователя");

            var logins = await _userManager.GetLoginsAsync(user.Id);
            //проверяем, что пользователь ЧУЖОЙ (и нам Не нужно подтверждать его Email)
            if (logins != null && logins.Any())
            {
                return View("Error", "Ваша учётная запись должна была активироваться автоматически");
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
                return View("Error", "Мы не нашли пользователя");

            var logins = await _userManager.GetLoginsAsync(user.Id);
            //проверяем, что пользователь ЧУЖОЙ (и нам Не нужно подтверждать его Email)
            if (logins != null && logins.Any())
            {
                return View("Error", "Ваша учётная запись должна была активироваться автоматически");
            }
            #endregion

            var researcher = _mediator.Send(new SingleResearcherQuery { ResearcherGuid = researcherGuid });
            if (researcher == null)
                return View("Error", "Мы не нашли исследователя");

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

            if (string.IsNullOrWhiteSpace(userName))
                return View("Error", "Не указано имя пользователя");

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                return View("Error", "Мы не нашли пользователя");

            var logins = await _userManager.GetLoginsAsync(user.Id);
            //проверяем, что пользователь ЧУЖОЙ (и нам Не нужно подтверждать его Email)
            if (logins != null && logins.Any())
            {
                return View("Error", "Ваша учётная запись должна была активироваться автоматически");
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
            HttpContext.Authentication.SignOutAsync(DefaultAuthenticationTypes.ApplicationCookie);
            //Context.Response.Cookies.Delete(DefaultAuthenticationTypes.ApplicationCookie);
            //Context.Response.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
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
