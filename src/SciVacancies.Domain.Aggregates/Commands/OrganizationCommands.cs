using SciVacancies.Domain.DataModels;

using System;

using MediatR;

namespace SciVacancies.Domain.Aggregates.Commands
{

    public class CreateOrganization : CommandBase, IRequest<Guid>
    {
        public CreateOrganization() : base() { }

        OrganizationDataModel Data { get; set; }
    }
    public class UpdateOrganization : IRequest
    {
        public UpdateOrganization() : base() { }

        public Guid OrganizationGuid { get; set; }

        public OrganizationDataModel Data { get; set; }
    }
    public class RemoveOrganization : IRequest
    {
        public Guid OrganizationGuid { get; set; }
    }
}
