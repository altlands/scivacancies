using SciVacancies.Domain.DataModels;

using System;

using MediatR;
using SciVacancies.ReadModel.Core;

namespace SciVacancies.WebApp.Commands
{
    public class CreatePositionCommand : CommandBase, IRequest<Guid>
    {
        public CreatePositionCommand() : base() { }

        public Guid OrganizationGuid { get; set; }

        public PositionDataModel Data { get; set; }
    }
    public class UpdatePositionCommand : CommandBase, IRequest
    {
        public UpdatePositionCommand() : base() { }

        public Guid OrganizationGuid { get; set; }
        public Guid PositionGuid { get; set; }

        public PositionDataModel Data { get; set; }
    }
    public class RemovePositionCommand : CommandBase, IRequest
    {
        public RemovePositionCommand() : base() { }

        public Guid OrganizationGuid { get; set; }
        public Guid PositionGuid { get; set; }
    }
}
