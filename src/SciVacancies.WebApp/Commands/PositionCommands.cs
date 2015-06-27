using SciVacancies.Domain.DataModels;

using System;

using MediatR;
using SciVacancies.ReadModel.Core;

namespace SciVacancies.WebApp.Commands
{
    public class CreatePositionCommand : CommandBase, IRequest<Guid>
    {
        public Guid OrganizationGuid { get; set; }

        public PositionDataModel Data { get; set; }
    }
    public class UpdatePositionCommand : CommandBase, IRequest
    {
        public Guid OrganizationGuid { get; set; }
        public Guid PositionGuid { get; set; }

        public PositionDataModel Data { get; set; }
    }
    public class RemovePositionCommand : CommandBase, IRequest
    {
        public Guid OrganizationGuid { get; set; }
        public Guid PositionGuid { get; set; }
    }
}
