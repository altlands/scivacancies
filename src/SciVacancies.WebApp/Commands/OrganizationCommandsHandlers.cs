using SciVacancies.Domain.Aggregates;

using System;

using CommonDomain.Persistence;
using MediatR;

namespace SciVacancies.WebApp.Commands
{
    public class CreateOrganizationCommandHandler : IRequestHandler<CreateOrganizationCommand, Guid>
    {
        private readonly IRepository _repository;

        public CreateOrganizationCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public Guid Handle(CreateOrganizationCommand msg)
        {
            Organization organization = new Organization(Guid.NewGuid(), msg.Data);
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

        protected override void HandleCore(UpdateOrganizationCommand msg)
        {
            if (msg.OrganizationGuid == Guid.Empty) throw new ArgumentNullException($"OrganizationGuid is empty: {msg.OrganizationGuid}");

            Organization organization = _repository.GetById<Organization>(msg.OrganizationGuid);
            organization.Update(msg.Data);
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

        protected override void HandleCore(RemoveOrganizationCommand msg)
        {
            if (msg.OrganizationGuid == Guid.Empty) throw new ArgumentNullException($"OrganizationGuid is empty: {msg.OrganizationGuid}");

            Organization organization = _repository.GetById<Organization>(msg.OrganizationGuid);
            organization.Remove();
            _repository.Save(organization, Guid.NewGuid(), null);
        }
    }
}
