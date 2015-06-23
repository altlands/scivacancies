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


namespace SciVacancies.WebApp.Commands
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, SciVacUser>
    {
        private readonly IRepository _repository;
        private readonly SciVacUserManager _userManager;

        public RegisterUserCommandHandler(IRepository repository, SciVacUserManager userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public SciVacUser Handle(RegisterUserCommand message)
        {
            //TODO: validate user model

            var user = new SciVacUser()
            {
                UserName = message.Data.Email
            };


            var rdm = Mapper.Map<AccountRegisterViewModel, ResearcherDataModel>(message.Data);
            rdm.UserId = user.Id;
            var researcher = new Researcher(Guid.NewGuid(), rdm);
            user.Claims.Add(new IdentityUserClaim()
            {
                ClaimType = "researcher_id",
                ClaimValue = researcher.Id.ToString(),
                UserId = user.Id
            });

            _repository.Save(researcher, Guid.NewGuid(), null);
            _userManager.Create(user);
            _userManager.AddToRole(user.Id, "researcher");

            return user;
        }
    }
}
