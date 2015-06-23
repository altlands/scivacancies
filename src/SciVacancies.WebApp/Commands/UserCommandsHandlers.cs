using SciVacancies.Domain.Aggregates;
using SciVacancies.Domain.DataModels;
using SciVacancies.WebApp.Infrastructure.Identity;
using SciVacancies.WebApp.ViewModels;

using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using AutoMapper;
using CommonDomain.Persistence;
using MediatR;
using SciVacancies.WebApp.Engine;

namespace SciVacancies.WebApp.Commands
{
    public class RegisterUserResearcherCommandHandler : IRequestHandler<RegisterUserResearcherCommand, SciVacUser>
    {
        private readonly SciVacUserManager _userManager;
        private readonly IMediator _mediator;

        public RegisterUserResearcherCommandHandler(SciVacUserManager userManager, IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        public SciVacUser Handle(RegisterUserResearcherCommand message)
        {
            //TODO: validate user model

            var user = new SciVacUser()
            {
                UserName = message.Data.Email,
            };

            var researcherDataModel = Mapper.Map<AccountRegisterViewModel, ResearcherDataModel>(message.Data);
            researcherDataModel.UserId = user.Id;

            Guid researcherGuid = _mediator.Send(new CreateResearcherCommand()
            {
                Data = researcherDataModel
            });

            user.Claims.Add(new IdentityUserClaim()
            {
                ClaimType = "researcher_id",
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

        public RegisterUserOrganizationCommandHandler(SciVacUserManager userManager, IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        public SciVacUser Handle(RegisterUserOrganizationCommand message)
        {
            //TODO: validate user model

            var user = new SciVacUser()
            {
                UserName = message.Data.Email,
            };

            var organizationDataModel = Mapper.Map<AccountRegisterViewModel, OrganizationDataModel>(message.Data);
            organizationDataModel.UserId = user.Id;

            Guid organizationGuid = _mediator.Send(new CreateOrganizationCommand()
            {
                Data = organizationDataModel
            });

            user.Claims.Add(new IdentityUserClaim()
            {
                ClaimType = "organization_id",
                ClaimValue = organizationGuid.ToString(),
                UserId = user.Id
            });

            _userManager.Create(user);
            _userManager.AddToRole(user.Id, ConstTerms.RequireRoleOrganizationAdmin);

            return user;
        }
    }
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, SciVacUser>
    {
        private readonly SciVacUserManager _userManager;
        private readonly IMediator _mediator;

        public RegisterUserCommandHandler(SciVacUserManager userManager, IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        public SciVacUser Handle(RegisterUserCommand message)
        {
            //TODO: validate user model

            var user = new SciVacUser()
            {
                UserName = message.Data.Email
            };


            var researcherDataModel = Mapper.Map<AccountRegisterViewModel, ResearcherDataModel>(message.Data);
            researcherDataModel.UserId = user.Id;

            Guid researcherGuid = _mediator.Send(new CreateResearcherCommand()
            {
                Data = researcherDataModel
            });

            //var researcher = new Researcher(Guid.NewGuid(), researcherDataModel);
            user.Claims.Add(new IdentityUserClaim()
            {
                ClaimType = "researcher_id",
                ClaimValue = researcherGuid.ToString(),
                //ClaimValue = researcher.Id.ToString(),
                UserId = user.Id
            });

            //_repository.Save(researcher, Guid.NewGuid(), null);
            _userManager.Create(user);
            _userManager.AddToRole(user.Id, "researcher");

            return user;
        }
    }
}
