using SciVacancies.Domain.DataModels;

using System;

using MediatR;

namespace SciVacancies.WebApp.Commands
{

    public class CreateOrganizationCommand : CommandBase, IRequest<Guid>
    {
        public OrganizationDataModel Data { get; set; }
    }
    public class UpdateOrganizationCommand : CommandBase, IRequest
    {
        public Guid OrganizationGuid { get; set; }

        public OrganizationDataModel Data { get; set; }
    }
    public class RemoveOrganizationCommand : CommandBase, IRequest
    {
        public Guid OrganizationGuid { get; set; }
    }
}
