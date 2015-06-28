using System;
using CommonDomain.Persistence;
using MediatR;
using SciVacancies.Domain.Aggregates;

namespace SciVacancies.WebApp.Commands
{
    public class CreateOrganizationCommandHandler : IRequestHandler<CreateOrganizationCommand, Guid>
    {
        private readonly IRepository _repository;

        public CreateOrganizationCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public Guid Handle(CreateOrganizationCommand message)
        {
            var rdm = message.Data;

            Organization organization = new Organization(Guid.NewGuid(), rdm);
            _repository.Save(organization, Guid.NewGuid(), null);

            return organization.Id;
        }
    }
    public class UpdateOrganizationCommandHandler : RequestHandler<UpdateOrganizationCommand>
    {
        private readonly IRepository _repository;

        public UpdateOrganizationCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(UpdateOrganizationCommand message)
        {
            if (message.OrganizationGuid == Guid.Empty) throw new ArgumentNullException($"OrganizationGuid is empty: {message.OrganizationGuid}");

            var rdm = message.Data;

            Organization organization = _repository.GetById<Organization>(message.OrganizationGuid);
            organization.Update(rdm);
            _repository.Save(organization, Guid.NewGuid(), null);
        }
    }
    public class RemoveOrganizationCommandHandler : RequestHandler<RemoveOrganizationCommand>
    {
        private readonly IRepository _repository;

        public RemoveOrganizationCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(RemoveOrganizationCommand message)
        {
            if (message.OrganizationGuid == Guid.Empty) throw new ArgumentNullException($"OrganizationGuid is empty: {message.OrganizationGuid}");

            Organization organization = _repository.GetById<Organization>(message.OrganizationGuid);
            organization.Remove();
            _repository.Save(organization, Guid.NewGuid(), null);
        }
    }
}
