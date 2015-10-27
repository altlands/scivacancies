using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using SciVacancies.ReadModel.Core;
using SciVacancies.SmtpNotifications.SmtpNotificators;
using SciVacancies.WebApp.Controllers;
using SciVacancies.WebApp.Infrastructure.Identity;
using SciVacancies.WebApp.Queries;
using Thinktecture.IdentityModel.Client;

namespace SciVacancies.WebApp.Infrastructure.WebAuthorize
{
    /// <summary>
    /// Сервис-помощник для выполнения задач по авторизации
    /// </summary>
    public class AuthorizeService : IAuthorizeService
    {
        private readonly SciVacUserManager _userManager;
        private readonly ISmtpNotificatorAccountService _smtpNotificatorAccountService;

        public AuthorizeService(SciVacUserManager userManager, ISmtpNotificatorAccountService smtpNotificatorAccountService)
        {
            _userManager = userManager;
            _smtpNotificatorAccountService = smtpNotificatorAccountService;
        }


        //общаемся с картой науки
        public string GetResearcherProfile(string accessToken)
        {
            //TODO url move to config
            var webRequest = WebRequest.Create(@"https://mapofscience.ru/scimap-sso/api/v1/users/current/profile" + "?access_token=" + accessToken);
            webRequest.Method = "GET";
            var httpWebResponse = webRequest.GetResponse() as HttpWebResponse;
            string responseString = "";
            using (var stream = httpWebResponse.GetResponseStream())
            {
                var streamReader = new StreamReader(stream, Encoding.UTF8);
                responseString = streamReader.ReadToEnd();
            }
            return responseString;
        }
        //public ClaimsPrincipal LogOutAndLogInUser(ClaimsIdentity identity)
        //{
        //    if (_response == null)
        //        throw new ObjectNotFoundException($"Параметр {nameof(_response)} не инициализирован");

        //    return LogOutAndLogInUser(_response, identity);
        //}
        public ClaimsPrincipal LogOutAndLogInUser(HttpResponse response, ClaimsIdentity identity)
        {
            var cp = new ClaimsPrincipal(identity);

            response.HttpContext.Authentication.SignOutAsync(DefaultAuthenticationTypes.ApplicationCookie);
            response.HttpContext.Authentication.SignInAsync(DefaultAuthenticationTypes.ApplicationCookie, cp);

            return cp;
        }

        public async Task<TokenResponse> GetOAuthAuthorizeTokenAsync(OAuthProviderSettings oauth, string code)
        {
            var client = new OAuth2Client(new Uri(oauth.TokenEndpoint), oauth.ClientId, oauth.ClientSecret);

            return await client.RequestAuthorizationCodeAsync(code, oauth.RedirectUrl);
        }
        public async Task<TokenResponse> GetOAuthRefreshTokenAsync(OAuthProviderSettings oauth, string refreshToken)
        {
            var client = new OAuth2Client(new Uri(oauth.TokenEndpoint), oauth.ClientId, oauth.ClientSecret);
            return await client.RequestRefreshTokenAsync(refreshToken);
        }
        public async Task<List<Claim>> GetOAuthUserClaimsAsync(OAuthProviderSettings oAuthProviderSettings, string accessToken)
        {
            var userInfoClient = new UserInfoClient(new Uri(oAuthProviderSettings.UserEndpoint), accessToken);
            var userInfoResponse = await userInfoClient.GetAsync();

            var claimsList = new List<Claim>();
            userInfoResponse.Claims.ToList().ForEach(f => claimsList.Add(new Claim(f.Item1, f.Item2)));

            return claimsList;
        }
        public async Task<List<Claim>> GetOAuthUserAndTokensClaimsAsync(OAuthProviderSettings oAuthProviderSettings, TokenResponse tokenResponse)
        {
            var claims = new List<Claim>();
            claims.AddRange(
                await GetOAuthUserClaimsAsync(oAuthProviderSettings, tokenResponse.AccessToken));

            claims.Add(new Claim("access_token", tokenResponse.AccessToken));
            claims.Add(new Claim("expires_in", DateTime.Now.AddSeconds(tokenResponse.ExpiresIn).ToString()));
            if (!string.IsNullOrEmpty(tokenResponse.RefreshToken))
                claims.Add(new Claim("refresh_token", tokenResponse.RefreshToken));
            return claims;
        }

        public ClaimsPrincipal RefreshUserClaimTokensAndReauthorize(SciVacUser sciVacUser, List<Claim> claims, HttpResponse response)
        {
            var identity = RefreshClaimsTokens(sciVacUser, claims);
            return LogOutAndLogInUser(response, identity);
        }
        public ClaimsIdentity RefreshClaimsTokens(SciVacUser sciVacUser, List<Claim> claims)
        {
            var identity = _userManager.CreateIdentity(sciVacUser, DefaultAuthenticationTypes.ApplicationCookie);
            var userId = identity.GetUserId();

            if (claims == null || !claims.Any())
                return identity;

            var tokenTypes = new List<string> { "access_token", "expires_in", "refresh_token" };
            tokenTypes.ForEach(tokenType =>
            {
                if (claims.Any(f => f.Type.Equals(tokenType)))
                {
                    if (identity.Claims.Any(f => f.Type.Equals(tokenType)))
                        _userManager.RemoveClaim(userId, identity.Claims.FirstOrDefault(f => f.Type.Equals(tokenType)));
                    _userManager.AddClaim(userId, claims.First(f => f.Type.Equals(tokenType)));
                }
            });

            identity = _userManager.CreateIdentity(sciVacUser, DefaultAuthenticationTypes.ApplicationCookie);

            return identity;
        }

        public async Task CallUserActivationAsync(SciVacUser sciVacUser, Researcher researcher)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(sciVacUser.Id);
            _smtpNotificatorAccountService.SendUserActivation(researcher, sciVacUser.UserName, sciVacUser.Email, token);
        }

        public async Task CallPasswordResetAsync(SciVacUser sciVacUser, Researcher researcher)
        {
            var token = await _userManager.GenerateUserTokenAsync("ChangePa$$bord", sciVacUser.Id);
            _smtpNotificatorAccountService.SendPasswordRestore(researcher, sciVacUser.UserName, sciVacUser.Email, token);
        }

    }

    /// <summary>
    /// Сервис-помощник для выполнения задач по авторизации
    /// </summary>
    public interface IAuthorizeService
    {
        Task<TokenResponse> GetOAuthAuthorizeTokenAsync(OAuthProviderSettings oauth, string code);
        Task<TokenResponse> GetOAuthRefreshTokenAsync(OAuthProviderSettings oauth, string refreshToken);
        /// <summary>
        /// общаемся с картой науки
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        string GetResearcherProfile(string accessToken);

        /// <summary>
        /// выполнить переавторизацию пользователя (выйти и войти снова)
        /// </summary>
        ClaimsPrincipal LogOutAndLogInUser(HttpResponse response, ClaimsIdentity identity);

        Task<List<Claim>> GetOAuthUserClaimsAsync(OAuthProviderSettings oAuthProviderSettings, string accessToken);

        Task<List<Claim>> GetOAuthUserAndTokensClaimsAsync(OAuthProviderSettings oAuthProviderSettings, TokenResponse tokenResponse);

        /// <summary>
        /// refresh token values in user claims. NEW TOKENS will apply after LogOut and LogIn
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="user"></param>
        ClaimsIdentity RefreshClaimsTokens(SciVacUser user, List<Claim> claims);


        /// <summary>
        /// refresh token values in user claims4 then LogOut and LogIn user
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="user"></param>
        ClaimsPrincipal RefreshUserClaimTokensAndReauthorize(SciVacUser user, List<Claim> claims, HttpResponse response);

        /// <summary>
        /// отправить письмо для активации пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <param name="researcher"></param>
        /// <returns></returns>
        Task CallUserActivationAsync(SciVacUser user, Researcher researcher);

        /// <summary>
        /// отправить письмо для сброса пароля пользователя
        /// </summary>
        Task CallPasswordResetAsync(SciVacUser user, Researcher researcher);
    }
}
