using SciVacancies.Domain.DataModels;

using System;

using MediatR;

namespace SciVacancies.Domain.Commands
{
    public class CreatePosition : CommandBase, IRequest<Guid>
    {
        public CreatePosition() : base() { }

        public Guid OrganizationGuid { get; set; }

        public PositionDataModel Data { get; set; }
    }
    public class UpdatePosition : CommandBase, IRequest
    {
        public UpdatePosition() : base() { }

        public Guid OrganizationGuid { get; set; }
        public Guid PositionGuid { get; set; }

        public PositionDataModel Data { get; set; }
    }
    public class RemovePosition : CommandBase, IRequest
    {
        public RemovePosition() : base() { }

        public Guid OrganizationGuid { get; set; }
        public Guid PositionGuid { get; set; }
    }
}
