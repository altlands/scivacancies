using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using SciVacancies.WebApp.Infrastructure.Identity;
using Thinktecture.IdentityModel.Client;

namespace SciVacancies.WebApp.Infrastructure.WebAuthorize
{
    /// <summary>
    /// Сервис-помощник для выполнения задач по авторизации
    /// </summary>
    public class AuthorizeService : IAuthorizeService
    {
        private readonly IMediator _mediator;
        private readonly SciVacUserManager _userManager;
        private ClaimsPrincipal _user;
        private HttpRequest _request;
        private HttpResponse _response;

        public AuthorizeService(SciVacUserManager userManager, IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        public void Initialize(ClaimsPrincipal user, HttpRequest request, HttpResponse response)
        {
            _user = user;
            _request = request;
            _response = response;
        }

        //общаемся с картой науки
        public string GetResearcherProfile(string accessToken)
        {
            //TODO url move to config
            var webRequest =
                WebRequest.Create(@"http://scimap-sso.alt-lan.com/scimap-sso/user/profile" + "?access_token=" +
                                  accessToken);
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
        public void LogOutAndLogInUser(ClaimsIdentity identity)
        {
            var cp = new ClaimsPrincipal(identity);
            _response.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            _response.SignIn(DefaultAuthenticationTypes.ApplicationCookie, cp);
        }
        public void LogInUser(ClaimsIdentity identity)
        {
            var cp = new ClaimsPrincipal(identity);
            _response.SignIn(DefaultAuthenticationTypes.ApplicationCookie, cp);
        }
        public async Task<TokenResponse> GetOAuthTokensAsync(OAuthProviderSettings oauth, string code)
        {
            var client = new OAuth2Client(new Uri(oauth.TokenEndpoint), oauth.ClientId, oauth.ClientSecret);

            return await client.RequestAuthorizationCodeAsync(code, oauth.RedirectUrl);
        }
        public async Task<TokenResponse> GetOAuthRefreshedTokenAsync(OAuthProviderSettings oauth, string refreshToken)
        {
            var client = new OAuth2Client(new Uri(oauth.TokenEndpoint), oauth.ClientId, oauth.ClientSecret);
            return await client.RequestRefreshTokenAsync(refreshToken);
        }
        public async Task<IEnumerable<Claim>> GetOAuthUserClaimsAsync(OAuthProviderSettings oauth, string accessToken)
        {
            var userInfoClient = new UserInfoClient(new Uri(oauth.UserEndpoint), accessToken);
            var userInfoResponse = await userInfoClient.GetAsync();

            var claimsList = new List<Claim>();
            userInfoResponse.Claims.ToList().ForEach(f => claimsList.Add(new Claim(f.Item1, f.Item2)));

            return claimsList;
        }
        public async Task<List<Claim>> GetTokensClaimsList(TokenResponse tokenResponse, OAuthProviderSettings oAuthProviderSettings)
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

        public void RefreshClaimsTokens(SciVacUser sciVacUser, List<Claim> claims)
        {
            if (claims == null || !claims.Any())
                return;

            var identity = _userManager.CreateIdentity(sciVacUser, DefaultAuthenticationTypes.ApplicationCookie);
            var userId = identity.GetUserId();

            var tokenType = "access_token";
            if (claims.Any(f => f.Type.Equals(tokenType)))
            {
                if (identity.Claims.Any(f => f.Type.Equals(tokenType)))
                    _userManager.RemoveClaim(userId, identity.Claims.FirstOrDefault(f => f.Type.Equals(tokenType)));
                _userManager.AddClaim(userId, claims.First(f => f.Type.Equals(tokenType)));
            }

            tokenType = "expires_in";
            if (claims.Any(f => f.Type.Equals(tokenType)))
            {
                if (identity.Claims.Any(f => f.Type.Equals(tokenType)))
                {
                    if (identity.Claims.Any(f => f.Type.Equals(tokenType)))
                        _userManager.RemoveClaim(userId, identity.Claims.FirstOrDefault(f => f.Type.Equals(tokenType)));
                    _userManager.AddClaim(userId, claims.First(f => f.Type.Equals(tokenType)));
                }
            }

            tokenType = "refresh_token";
            if (claims.Any(f => f.Type.Equals(tokenType)))
            {
                if (identity.Claims.Any(f => f.Type.Equals(tokenType)))
                    _userManager.RemoveClaim(userId, identity.Claims.FirstOrDefault(f => f.Type.Equals(tokenType)));
                _userManager.AddClaim(userId, claims.First(f => f.Type.Equals(tokenType)));
            }
        }

    }

    /// <summary>
    /// Сервис-помощник для выполнения задач по авторизации
    /// </summary>
    public interface IAuthorizeService
    {
        void Initialize(ClaimsPrincipal user, HttpRequest request, HttpResponse response);

        Task<TokenResponse> GetOAuthTokensAsync(OAuthProviderSettings oauth, string code);
        Task<TokenResponse> GetOAuthRefreshedTokenAsync(OAuthProviderSettings oauth, string refreshToken);
        /// <summary>
        /// общаемся с картой науки
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        string GetResearcherProfile(string accessToken);

        /// <summary>
        /// выполнить переавторизацию пользователя (выйти и войти снова)
        /// </summary>
        void LogOutAndLogInUser(ClaimsIdentity identity);

        /// <summary>
        /// выполнить авторизацию пользователя
        /// </summary>
        void LogInUser(ClaimsIdentity identity);


        Task<IEnumerable<Claim>> GetOAuthUserClaimsAsync(OAuthProviderSettings oauth, string accessToken);

        Task<List<Claim>> GetTokensClaimsList(TokenResponse tokenResponse, OAuthProviderSettings oAuthProviderSettings);

        /// <summary>
        /// refresh token values in user claims. НОВЫЕ ТОКЕНЫ вступят в силу ПОСЛЕ переавторизации
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="user"></param>
        void RefreshClaimsTokens(SciVacUser user, List<Claim> claims);
    }
}
