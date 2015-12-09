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
using SciVacancies.ReadModel.Core;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Infrastructure.Identity;
using SciVacancies.WebApp.Infrastructure.WebAuthorize;
using SciVacancies.WebApp.Models.DataModels;
using SciVacancies.WebApp.Models.OAuth;
using SciVacancies.WebApp.Queries;
using SciVacancies.WebApp.ViewModels;
using SciVacancies.WebApp.Commands;

namespace SciVacancies.WebApp.Controllers
{
    public class AccountIntegrationController : Controller
    {

        private readonly IOptions<OAuthSettings> _oauthSettings;
        private readonly SciVacUserManager _userManager;
        private readonly IMediator _mediator;
        private readonly IAuthorizeService _authorizeService;
        private readonly IOptions<ApiSettings> _apiSettings;

        public AccountIntegrationController(SciVacUserManager userManager, IOptions<OAuthSettings> oAuthSettings,
            IOptions<ApiSettings> apiSettings, IMediator mediator, IAuthorizeService authorizeService)
        {
            _userManager = userManager;
            _oauthSettings = oAuthSettings;
            _mediator = mediator;
            _authorizeService = authorizeService;
            _apiSettings = apiSettings;
        }

        /// <summary>
        /// Обновить профиль ислледователя
        /// </summary>
        /// <returns></returns>
        [PageTitle("Обновление профиля данными с Карты наук")]
        [Authorize]
        [BindResearcherIdFromClaims]
        public async Task<ViewResult> UpdateResearcherAccountFromOutside(Guid researcherGuid)
        {
            var model = new ResearcherProfileCompareModel();

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
                var tokenResponse = await _authorizeService.GetOAuthRefreshTokenAsync(_oauthSettings.Value.Mapofscience, refreshToken);

                if (tokenResponse.IsError)
                {
                    model.Error = "Произошла ошибка при попытке получить данные. Попробуйте выйти и авторизоваться ещё раз. Затем повторите Загрузку данных.";
                    return View(model);
                }

                var claims = new List<Claim>
                {
                    new Claim("access_token", tokenResponse.AccessToken),
                    new Claim("expires_in", DateTime.Now.AddSeconds(tokenResponse.ExpiresIn).ToString())
                };
                if (!string.IsNullOrEmpty(tokenResponse.RefreshToken))
                    claims.Add(new Claim("refresh_token", tokenResponse.RefreshToken));

                _authorizeService.RefreshUserClaimTokensAndReauthorize(_userManager.FindByEmail(User.Identity.Name), claims, Response);

            }

            //3-отправляем запрос к api
            var profileAsString =
                _authorizeService.GetResearcherProfile(User.Claims.Single(c => c.Type.Equals("access_token")).Value, _apiSettings.Value.Mapofscience.UserProfile);

            OAuthResProfile oAuthResProfile = JsonConvert.DeserializeObject<OAuthResProfile>(profileAsString);
            //4 - mapping
            var accountResearcherUpdateViewModel = Mapper.Map<ResearcherUpdateDataModel>(oAuthResProfile);
            var researcherDataModel = Mapper.Map<ResearcherDataModel>(accountResearcherUpdateViewModel);
            var newResearcherProfileEditModel = Mapper.Map<ResearcherProfileCompareModelItem>(researcherDataModel);

            //get current profile
            var oldResearcherReadModel = _mediator.Send(new SingleResearcherQuery { ResearcherGuid = researcherGuid });

            IEnumerable<Education> educations =
                _mediator.Send(new SelectResearcherEducationsQuery { ResearcherGuid = researcherGuid });
            if (educations != null && educations.Any())
            {
                oldResearcherReadModel.educations = Mapper.Map<List<Education>>(educations);
            }
            IEnumerable<Publication> publications =
                _mediator.Send(new SelectResearcherPublicationsQuery { ResearcherGuid = researcherGuid });
            if (publications != null && publications.Any())
                oldResearcherReadModel.publications = Mapper.Map<List<Publication>>(publications);

            var oldResearcherDataModel = Mapper.Map<ResearcherDataModel>(oldResearcherReadModel);
            var researcherProfileEditModel = Mapper.Map<ResearcherProfileCompareModelItem>(oldResearcherDataModel);

            model.New = newResearcherProfileEditModel;
            model.Original = researcherProfileEditModel;
            return View(model);

            //TODO: продолжтиь обработку в отдельном Action
            //5 - отправляем команду через медиатор
            //_mediator.Send(new UpdateResearcherCommand {ResearcherGuid = researcherGuid, Data = researcherDataModel});
        }



        /// <summary>
        /// Обновить профиль ислледователя
        /// </summary>
        /// <returns></returns>
        [PageTitle("Обновление профиля данными с Карты наук")]
        [Authorize]
        [BindResearcherIdFromClaims]
        [HttpPost]
        public IActionResult UpdateResearcherAccountFromOutside(ResearcherProfileCompareModel model, Guid researcherGuid)
        {
            if (model == null || model.New == null)
                return RedirectToAction("account", "researchers");

            if (!model.New.SelectCommon
                /*
                && !(model.New.Conferences != null && model.New.Conferences.Any() && model.New.ConferencesChecked)
                && !(model.New.Rewards != null && model.New.Rewards.Any() && model.New.RewardsChecked)
                && !(model.New.Memberships != null && model.New.Memberships.Any() && model.New.MembershipsChecked)
                && !(model.New.Educations != null && model.New.Educations.Any() && model.New.EducationsChecked)
                && !(model.New.Publications != null && model.New.Publications.Any() && model.New.PublicationsChecked)
                && !(model.New.Interests != null && model.New.Interests.Any() && model.New.InterestsChecked)
                && !(model.New.ResearchActivity != null && model.New.ResearchActivity.Any() && model.New.ResearchActivityChecked)
                && !(model.New.TeachingActivity != null && model.New.TeachingActivity.Any() && model.New.TeachingActivityChecked)
                && !(model.New.OtherActivity != null && model.New.OtherActivity.Any() && model.New.OtherActivityChecked)                
                */
                && !(model.New.ConferencesChecked)
                && !(model.New.RewardsChecked)
                && !(model.New.MembershipsChecked)
                && !(model.New.EducationsChecked)
                && !(model.New.PublicationsChecked)
                && !(model.New.InterestsChecked)
                && !(model.New.ResearchActivityChecked)
                && !(model.New.TeachingActivityChecked)
                && !(model.New.OtherActivityChecked)
                )
            {
                return RedirectToAction("account", "researchers");
            }

            //get current profile
            var oldResearcherReadModel = _mediator.Send(new SingleResearcherQuery { ResearcherGuid = researcherGuid });

            IEnumerable<Education> educations =
                _mediator.Send(new SelectResearcherEducationsQuery { ResearcherGuid = researcherGuid });
            if (educations != null && educations.Any())
            {
                oldResearcherReadModel.educations = Mapper.Map<List<Education>>(educations);
            }
            IEnumerable<Publication> publications =
                _mediator.Send(new SelectResearcherPublicationsQuery { ResearcherGuid = researcherGuid });
            if (publications != null && publications.Any())
                oldResearcherReadModel.publications = Mapper.Map<List<Publication>>(publications);
            var oldResearcherDataModel = Mapper.Map<ResearcherDataModel>(oldResearcherReadModel);

            if (model.New.SelectCommon)
            {
                oldResearcherDataModel.FirstName = model.New.FirstName;
                oldResearcherDataModel.Patronymic = model.New.MiddleName;
                oldResearcherDataModel.SecondName = model.New.LastName;
                oldResearcherDataModel.PreviousSecondName = model.New.PreviousLastName;

                oldResearcherDataModel.FirstNameEng = model.New.FirstNameEng;
                oldResearcherDataModel.PatronymicEng = model.New.MiddleNameEng;
                oldResearcherDataModel.SecondNameEng = model.New.LastNameEng;
                oldResearcherDataModel.PreviousSecondNameEng = model.New.PreviousLastNameEng;


                oldResearcherDataModel.ExtNumber = model.New.ExtNumber;
                oldResearcherDataModel.BirthDate = new DateTime(model.New.BirthYear == 0 ? 1 : model.New.BirthYear, 1,1);
                oldResearcherDataModel.Email = model.New.Email;
                oldResearcherDataModel.Phone = model.New.Phone;
                oldResearcherDataModel.ScienceDegree= model.New.ScienceDegree;
                oldResearcherDataModel.ScienceRank= model.New.ScienceRank;
            }

            if (model.New.ResearchActivityChecked)
                if (model.New.ResearchActivity != null && model.New.ResearchActivity.Any())
                    oldResearcherDataModel.ResearchActivity = JsonConvert.SerializeObject(model.New.ResearchActivity);
                else
                    oldResearcherDataModel.ResearchActivity = string.Empty;

            if (model.New.TeachingActivityChecked)
                if (model.New.TeachingActivity != null && model.New.TeachingActivity.Any())
                    oldResearcherDataModel.TeachingActivity = JsonConvert.SerializeObject(model.New.TeachingActivity);
                else
                    oldResearcherDataModel.TeachingActivity = string.Empty;

            if (model.New.OtherActivityChecked)
                if (model.New.OtherActivity != null && model.New.OtherActivity.Any())
                    oldResearcherDataModel.OtherActivity = JsonConvert.SerializeObject(model.New.OtherActivity);
                else
                    oldResearcherDataModel.OtherActivity = string.Empty;

            if (model.New.RewardsChecked)
                if (model.New.Rewards != null && model.New.Rewards.Any())
                    oldResearcherDataModel.Rewards = JsonConvert.SerializeObject(model.New.Rewards);
                else
                    oldResearcherDataModel.Rewards = string.Empty;

            if (model.New.MembershipsChecked)
                if (model.New.Memberships != null && model.New.Memberships.Any())
                    oldResearcherDataModel.Memberships = JsonConvert.SerializeObject(model.New.Memberships);
                else
                    oldResearcherDataModel.Memberships = string.Empty;

            if (model.New.EducationsChecked)
                if (model.New.Educations != null && model.New.Educations.Any())
                    oldResearcherDataModel.Educations = Mapper.Map<List<SciVacancies.Domain.Core.Education>>(model.New.Educations);
                else
                    oldResearcherDataModel.Educations = null;

            if (model.New.ConferencesChecked)
                if (model.New.Conferences != null && model.New.Conferences.Any())
                    oldResearcherDataModel.Conferences = JsonConvert.SerializeObject(model.New.Conferences);
                else
                    oldResearcherDataModel.Conferences = string.Empty;

            if (model.New.PublicationsChecked)
                if (model.New.Publications != null && model.New.Publications.Any())
                    oldResearcherDataModel.Publications = Mapper.Map<List<SciVacancies.Domain.Core.Publication>>(model.New.Publications);
                else
                    oldResearcherDataModel.Publications = null;

            if (model.New.InterestsChecked)
                if (model.New.Interests != null && model.New.Interests.Any())
                    oldResearcherDataModel.Interests = JsonConvert.SerializeObject(model.New.Interests);
                else
                    oldResearcherDataModel.Interests = string.Empty;


            //5 - отправляем команду через медиатор
            _mediator.Send(new UpdateResearcherCommand { ResearcherGuid = researcherGuid, Data = oldResearcherDataModel });

            return RedirectToAction("account", "researchers");
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
