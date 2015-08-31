using System;
using System.Collections.Generic;
using System.Globalization;
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
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.OptionsModel;
using Newtonsoft.Json;
using SciVacancies.Domain.DataModels;
using Thinktecture.IdentityModel.Client;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Infrastructure.Identity;
using SciVacancies.WebApp.Infrastructure.WebAuthorize;
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
        private readonly IAuthorizeService _authorizeService;

        public AccountIntegrationController(SciVacUserManager userManager, IOptions<OAuthSettings> oAuthSettings, IMediator mediator, IAuthorizeService authorizeService)
        {
            _userManager = userManager;
            _oauthSettings = oAuthSettings;
            _mediator = mediator;
            _authorizeService = authorizeService;
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
            var model = new ResearcherProfileCompareModel();
            _authorizeService.Initialize(User, Request, Response);

            if (User.Claims.SingleOrDefault(c => c.Type.Equals("access_token")) == null)
            {
                model.Error = "Отсутствует AccessToken";
                return View(model);
            }

            //TODO: как обновлять профили пользователей, изначально зарегистрировавшихся через наш портал, а не через Карту Науки.

            //1-проверка access token expires in
            if (!User.Claims.Any(c => c.Type.Equals("expires_in"))
                || DateTime.Parse(User.Claims.Single(c => c.Type.Equals("expires_in")).Value) < DateTime.Now)
            {
                //2-если надо, обновляем access token
                if (!User.Claims.Any(c => c.Type.Equals("refresh_token")))
                {
                    model.Error = "Отсутствует RefreshToken";
                    return View(model);
                }
                var refreshToken = User.Claims.Single(c => c.Type.Equals("refresh_token")).Value;
                
                //получили новые данные - и обновляем их у себя
                var tokenResponse = await _authorizeService.GetOAuthRefreshedTokenAsync(_oauthSettings.Options.Mapofscience, refreshToken);

                var claims = new List<Claim>
                {
                    new Claim("access_token", tokenResponse.AccessToken),
                    new Claim("expires_in", DateTime.Now.AddSeconds(tokenResponse.ExpiresIn).ToString(CultureInfo.InvariantCulture)),
                    new Claim("refresh_token", tokenResponse.RefreshToken)
                };
                _authorizeService.RefreshClaimsTokens(_userManager.FindByEmail(User.Identity.Name), claims);
            }

            //3-отправляем запрос к api
            var profileAsString =
                _authorizeService.GetResearcherProfile(User.Claims.Single(c => c.Type.Equals("access_token")).Value);

            OAuthResProfile oAuthResProfile = JsonConvert.DeserializeObject<OAuthResProfile>(profileAsString);
            //4 - mapping
            var accountResearcherUpdateViewModel = Mapper.Map<ProfileResearcherUpdateDataModel>(oAuthResProfile);
            var researcherDataModel = Mapper.Map<ResearcherDataModel>(accountResearcherUpdateViewModel);
            var newResearcherProfileEditModel = Mapper.Map<ResearcherProfileCompareModelItem>(researcherDataModel);

            //get current profile
            var oldResearcherReadModel = _mediator.Send(new SingleResearcherQuery { ResearcherGuid = researcherGuid });
            var oldResearcherDataModel = Mapper.Map<ResearcherDataModel>(oldResearcherReadModel);
            var researcherProfileEditModel = Mapper.Map<ResearcherProfileCompareModelItem>(oldResearcherDataModel);

            model.New = newResearcherProfileEditModel;
            model.Original = researcherProfileEditModel;
            return View(model);

            //5 - отправляем команду через медиатор
            //_mediator.Send(new UpdateResearcherCommand {ResearcherGuid = researcherGuid, Data = researcherDataModel});
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





    }
}
