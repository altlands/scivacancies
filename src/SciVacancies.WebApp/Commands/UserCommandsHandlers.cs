using AutoMapper;
using CommonDomain.Persistence;
using MediatR;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SciVacancies.Domain.Aggregates;
using SciVacancies.Domain.DataModels;
using SciVacancies.WebApp.Infrastructure.Identity;
using SciVacancies.WebApp.Models.DataModels;
using SciVacancies.WebApp.ViewModels;
using System;
using System.Security.Claims;

namespace SciVacancies.WebApp.Commands
{
    public class RegisterUserResearcherCommandHandler : IRequestHandler<RegisterUserResearcherCommand, SciVacUser>
    {
        private readonly SciVacUserManager _userManager;
        private readonly IRepository _repository;

        public RegisterUserResearcherCommandHandler(SciVacUserManager userManager, IRepository repository)
        {
            _userManager = userManager;
            _repository = repository;
        }

        public SciVacUser Handle(RegisterUserResearcherCommand message)
        {
            //TODO: validate user model

            var user = new SciVacUser
            {
                UserName = message.Data.UserName,
                Email = message.Data.Email,
                PhoneNumber = message.Data.Phone
            };

            var researcherDataModel = Mapper.Map<ResearcherRegisterDataModel, ResearcherDataModel>(message.Data);
            //researcherDataModel.UserId = user.Id;

            var researcher = new Researcher(Guid.NewGuid(), researcherDataModel);
            _repository.Save(researcher, Guid.NewGuid(), null);

            user.Claims.Add(new IdentityUserClaim
            {
                ClaimType = ConstTerms.ClaimTypeResearcherId,
                ClaimValue = researcher.Id.ToString(),
                UserId = user.Id
            });

            if (message.Data.Claims != null)
            {
                foreach (Claim claim in message.Data.Claims)
                {
                    user.Claims.Add(new IdentityUserClaim
                    {
                        ClaimType = claim.Type,
                        ClaimValue = claim.Value,
                        UserId = user.Id
                    });
                }
            }

            IdentityResult identity =
                string.IsNullOrWhiteSpace(message.Data.Password)
                ? _userManager.Create(user)
                : _userManager.Create(user, message.Data.Password)
                ;

            if (!identity.Succeeded) throw new ArgumentException("UserManager failed to create identity");

            _userManager.AddToRole(user.Id, ConstTerms.RequireRoleResearcher);

            if (!string.IsNullOrWhiteSpace(message.Data.SciMapNumber))
            {
                _userManager.AddLogin(user.Id, new UserLoginInfo("ScienceMap", message.Data.SciMapNumber));
            }

            return user;
        }
    }
    public class RegisterUserOrganizationCommandHandler : IRequestHandler<RegisterUserOrganizationCommand, SciVacUser>
    {
        private readonly SciVacUserManager _userManager;
        private readonly IRepository _repository;

        public RegisterUserOrganizationCommandHandler(SciVacUserManager userManager, IRepository repository)
        {
            _userManager = userManager;
            _repository = repository;
        }

        public SciVacUser Handle(RegisterUserOrganizationCommand message)
        {
            //TODO: validate user model

            var user = new SciVacUser
            {
                UserName = message.Data.UserName,
                Email = message.Data.Email
            };

            var organizationDataModel = Mapper.Map<AccountOrganizationRegisterViewModel, OrganizationDataModel>(message.Data);
            //organizationDataModel.UserId = user.Id;

            var organization = new Organization(Guid.NewGuid(), organizationDataModel);
            _repository.Save(organization, Guid.NewGuid(), null);

            var organizationGuid = organization.Id;

            user.Claims.Add(new IdentityUserClaim
            {
                ClaimType = ConstTerms.ClaimTypeOrganizationId,
                ClaimValue = organizationGuid.ToString(),
                UserId = user.Id
            });

            if (message.Data.Claims != null)
            {
                foreach (Claim claim in message.Data.Claims)
                {
                    user.Claims.Add(new IdentityUserClaim
                    {
                        ClaimType = claim.Type,
                        ClaimValue = claim.Value,
                        UserId = user.Id
                    });
                }
            }

            //_userManager.CreateAsync(user).Wait();
            var identity = _userManager.Create(user);
            if (!identity.Succeeded) throw new ArgumentException("UserManager failed to create identity");

            //_userManager.AddClaim(user.Id, new System.Security.Claims.Claim(ConstTerms.ClaimTypeOrganizationId, organizationGuid.ToString()));
            //_userManager.AddClaim(user.Id, new System.Security.Claims.Claim("Email", organizationDataModel.Email));
            _userManager.AddToRole(user.Id, ConstTerms.RequireRoleOrganizationAdmin);

            return user;
        }
    }
}
