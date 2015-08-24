using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.OptionsModel;
using Newtonsoft.Json;
using SciVacancies.WebApp.Commands;
using Thinktecture.IdentityModel.Client;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Infrastructure.Identity;
using SciVacancies.WebApp.Models.OAuth;
using SciVacancies.WebApp.Queries;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Controllers
{
    public class AccountIntegrationController : Controller
    {

        private readonly IOptions<OAuthSettings> _oauthSettings;
        private readonly SciVacUserManager _userManager;
        private readonly IMediator _mediator;

        public AccountIntegrationController(SciVacUserManager userManager, IOptions<OAuthSettings> oAuthSettings, IMediator mediator)
        {
            _userManager = userManager;
            _oauthSettings = oAuthSettings;
            _mediator = mediator;
        }

        /// <summary>
        /// Обновить профиль ислледователя
        /// </summary>
        /// <returns></returns>
        [PageTitle("Обновление данных профиля")]
        [Authorize]
        [BindResearcherIdFromClaims]
        public async Task<ViewResult> UpdateResearcherAccountFromOutside(Guid researcherGuid)
        {
            var model = "Подтвердите обновление данных профиля с внешнего ресурса";
            //TODO
            var accessTokenTemp = User.Claims.SingleOrDefault(c => c.Type.Equals("access_token"));
            if (accessTokenTemp == null)
                return View(model: "Отсутствует AccessToken");

            var accessToken = accessTokenTemp.Value;
            //1-проверка access token expires in
            if (!User.Claims.Any(c => c.Type.Equals("expires_in"))
                || DateTime.Parse(User.Claims.Single(c => c.Type.Equals("expires_in")).Value) < DateTime.Now)
            {
                //2-если надо, обновляем access token
                if (!User.Claims.Any(c => c.Type.Equals("refresh_token")))
                    return View(model: "Отсутствует RefreshToken");

                var refreshToken = User.Claims.Single(c => c.Type.Equals("refresh_token")).Value;
                //получили новые данные - и обновляем их у себя
                var tokenResponse = await GetOAuthRefreshedToken(_oauthSettings.Options.Mapofscience, refreshToken);

                var tUser = _userManager.FindByEmail(User.Identity.Name); //email
                var identity = _userManager.CreateIdentity(tUser, DefaultAuthenticationTypes.ApplicationCookie);

                if (tokenResponse.AccessToken != accessToken)
                {
                    _userManager.RemoveClaim(tUser.Id, identity.Claims.FirstOrDefault(f => f.Type.Equals("access_token")));
                    _userManager.AddClaim(tUser.Id, new Claim("access_token", tokenResponse.AccessToken));
                    accessToken = tokenResponse.AccessToken;
                }

                if (DateTime.Now.AddSeconds(tokenResponse.ExpiresIn) != DateTime.Parse(identity.Claims.Single(c => c.Type.Equals("expires_in")).Value))
                {
                    _userManager.RemoveClaim(tUser.Id, identity.Claims.FirstOrDefault(f => f.Type.Equals("expires_in")));
                    _userManager.AddClaim(tUser.Id, new Claim("expires_in", DateTime.Now.AddSeconds(tokenResponse.ExpiresIn).ToString()));
                }
                if (!string.IsNullOrEmpty(tokenResponse.RefreshToken))
                {
                    if (tokenResponse.RefreshToken != refreshToken)
                    {
                        _userManager.RemoveClaim(tUser.Id, identity.Claims.FirstOrDefault(f => f.Type.Equals("refresh_token")));
                        _userManager.AddClaim(tUser.Id, new Claim("refresh_token", tokenResponse.RefreshToken));
                        refreshToken = tokenResponse.RefreshToken;
                    }
                }
            }

            //3-отправляем запрос к api
            //TODO url move to config
            OAuthResProfile researcherProfile = JsonConvert.DeserializeObject<OAuthResProfile>(GetResearcherProfile(accessToken));
            var tests = researcherProfile.birthday;
            //4 - mapping
            var accountResearcherUpdateViewModel = Mapper.Map<AccountResearcherUpdateViewModel>(researcherProfile);
            //5 - отправляем команду через медиатор
            //_mediator.Send(new UpdateResearcherCommand {ResearcherGuid = researcherGuid, accountResearcherUpdateViewModel });

            return View(model: model);
        }


        /// <summary>
        /// Обновить профиль организации
        /// </summary>
        /// <param name="organizationGuid"></param>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        [Authorize(Roles = ConstTerms.RequireRoleOrganizationAdmin)]
        [BindResearcherIdFromClaims]
        [PageTitle("Обновление данных профиля")]
        [BindOrganizationIdFromClaims]
        [Authorize]
        public ViewResult UpdateOrganizationAccountFromOutside(Guid organizationGuid, AuthorizeResourceTypes dataSource)
        {
            var model = "Подтвердите обновление данных профиля с внешнего ресурса";
            return View(model: model);
        }



        //общаемся с картой науки
        protected string GetResearcherProfile(string accessToken)
        {
            //TODO url move to config
            var webRequest = WebRequest.Create(@"http://scimap-sso.alt-lan.com/scimap-sso/user/profile" + "?access_token=" + accessToken);
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


        private async Task<TokenResponse> GetOAuthRefreshedToken(OAuthProviderSettings oauth, string refreshToken)
        {
            var client = new OAuth2Client(new Uri(oauth.TokenEndpoint), oauth.ClientId, oauth.ClientSecret);

            return await client.RequestRefreshTokenAsync(refreshToken);
        }

    }
}
