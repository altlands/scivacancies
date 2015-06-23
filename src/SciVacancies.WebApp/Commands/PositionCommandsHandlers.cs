using SciVacancies.Domain.Aggregates;

using System;

using AutoMapper;
using CommonDomain.Persistence;
using MediatR;

namespace SciVacancies.WebApp.Commands
{
    public class CreatePositionCommandHandler : IRequestHandler<CreatePositionCommand, Guid>
    {
        private readonly IRepository _repository;

        public CreatePositionCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public Guid Handle(CreatePositionCommand message)
        {
            if (message.OrganizationGuid == Guid.Empty) throw new Exception($"OrganizationGuid is empty: {message.OrganizationGuid}");

            var rdm = message.Data;

            Organization organization = _repository.GetById<Organization>(message.OrganizationGuid);
            Guid positionGuid = organization.CreatePosition(rdm);
            _repository.Save(organization, Guid.NewGuid(), null);

            return positionGuid;
        }
    }
    public class UpdatePositionCommandHandler : RequestHandler<UpdatePositionCommand>
    {
        private readonly IRepository _repository;

        public UpdatePositionCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(UpdatePositionCommand message)
        {
            if (message.OrganizationGuid == Guid.Empty) throw new Exception($"OrganizationGuid is empty: {message.OrganizationGuid}");
            if (message.PositionGuid == Guid.Empty) throw new Exception($"PositionGuid is empty: {message.PositionGuid}");

            var rdm = message.Data;

            Organization organization = _repository.GetById<Organization>(message.OrganizationGuid);
            organization.UpdatePosition(message.PositionGuid, rdm);
            _repository.Save(organization, Guid.NewGuid(), null);
        }
    }
    public class RemovePositionCommandHandler : RequestHandler<RemovePositionCommand>
    {
        private readonly IRepository _repository;

        public RemovePositionCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(RemovePositionCommand message)
        {
            Organization organization = _repository.GetById<Organization>(message.OrganizationGuid);
            organization.RemovePosition(message.PositionGuid);
            _repository.Save(organization, Guid.NewGuid(), null);
        }
    }
}