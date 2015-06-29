using System;
using AutoMapper;
using MediatR;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SciVacancies.Domain.DataModels;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Infrastructure.Identity;
using SciVacancies.WebApp.ViewModels;
using SciVacancies.Domain.Aggregates;
using CommonDomain.Persistence;

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
                UserName = message.Data.UserName
            };

            var researcherDataModel = Mapper.Map<AccountResearcherRegisterViewModel, ResearcherDataModel>(message.Data);
            researcherDataModel.UserId = user.Id;

            Researcher researcher = new Researcher(Guid.NewGuid(), researcherDataModel);
            _repository.Save(researcher, Guid.NewGuid(), null);

            var researcherGuid = researcher.Id;

            user.Claims.Add(new IdentityUserClaim
            {
                ClaimType = ConstTerms.ClaimTypeResearcherId,
                ClaimValue = researcherGuid.ToString(),
                UserId = user.Id
            });

            _userManager.Create(user);
            _userManager.AddToRole(user.Id, ConstTerms.RequireRoleResearcher);

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
                UserName = message.Data.UserName
            };

            var organizationDataModel = Mapper.Map<AccountOrganizationRegisterViewModel, OrganizationDataModel>(message.Data);
            organizationDataModel.UserId = user.Id;

            Organization organization = new Organization(Guid.NewGuid(), organizationDataModel);
            _repository.Save(organization, Guid.NewGuid(), null);

            var organizationGuid = organization.Id;

            _userManager.Create(user);
            _userManager.AddClaim(user.Id, new System.Security.Claims.Claim(ConstTerms.ClaimTypeOrganizationId, organizationGuid.ToString()));
            //user.Claims.Add(new IdentityUserClaim
            //{
            //    ClaimType = ConstTerms.ClaimTypeOrganizationId,
            //    ClaimValue = organizationGuid.ToString(),
            //    UserId = user.Id
            //});


            _userManager.AddToRole(user.Id, ConstTerms.RequireRoleOrganizationAdmin);

            return user;
        }
    }
}
