using SciVacancies.Domain.DataModels;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Infrastructure.Identity;
using SciVacancies.WebApp.ViewModels;
using SciVacancies.Domain.Aggregates;

using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using AutoMapper;
using MediatR;
using CommonDomain.Persistence;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SciVacancies.WebApp.Engine.SmtpNotificators;
using System.Security.Claims;

namespace SciVacancies.WebApp.Commands
{
    public class RegisterUserResearcherCommandHandler : IRequestHandler<RegisterUserResearcherCommand, SciVacUser>
    {
        private readonly SciVacUserManager _userManager;
        private readonly IMediator _mediator;
        private readonly IRepository _repository;

        public RegisterUserResearcherCommandHandler(SciVacUserManager userManager, IMediator mediator, IRepository repository)
        {
            _userManager = userManager;
            _mediator = mediator;
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

            var researcherDataModel = Mapper.Map<AccountResearcherRegisterViewModel, ResearcherDataModel>(message.Data);
            //researcherDataModel.UserId = user.Id;

            var researcher = new Researcher(Guid.NewGuid(), researcherDataModel);
            _repository.Save(researcher, Guid.NewGuid(), null);

            var researcherGuid = researcher.Id;

            user.Claims.Add(new IdentityUserClaim
            {
                ClaimType = ConstTerms.ClaimTypeResearcherId,
                ClaimValue = researcherGuid.ToString(),
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

            var identity = _userManager.Create(user);
            if (!identity.Succeeded) throw new ArgumentException("UserManager failed to create identity");

            _userManager.AddToRole(user.Id, ConstTerms.RequireRoleResearcher);

            //TODO - вынести в хэндлер
            //send email notification
            var registerSmtpNotificator = new RegisterSmtpNotificator();
            registerSmtpNotificator.Send(researcherDataModel.FullName, message.Data.UserName, message.Data.Password, message.Data.Email);
            if (!string.IsNullOrWhiteSpace(message.Data.ExtraEmail))
                registerSmtpNotificator.Send(researcherDataModel.FullName, message.Data.UserName, message.Data.Password, message.Data.ExtraEmail);

            return user;
        }
    }
    public class RegisterUserOrganizationCommandHandler : IRequestHandler<RegisterUserOrganizationCommand, SciVacUser>
    {
        private readonly SciVacUserManager _userManager;
        private readonly IMediator _mediator;
        private readonly IRepository _repository;

        public RegisterUserOrganizationCommandHandler(SciVacUserManager userManager, IMediator mediator, IRepository repository)
        {
            _userManager = userManager;
            _mediator = mediator;
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
