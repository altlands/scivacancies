using SciVacancies.Domain.DataModels;

using System;

using MediatR;

namespace SciVacancies.WebApp.Commands
{
    public class CreateSearchSubscriptionCommand : CommandBase, IRequest<Guid>
    {
        public Guid ResearcherGuid { get; set; }

        public SearchSubscriptionDataModel Data { get; set; }
    }
    public class CancelSearchSubscriptionCommand : CommandBase, IRequest
    {
        public Guid ResearcherGuid { get; set; }
        public Guid SearchSubscriptionGuid { get; set; }
    }
    public class ActivateSearchSubscriptionCommand : CommandBase, IRequest
    {
        public Guid ResearcherGuid { get; set; }
        public Guid SearchSubscriptionGuid { get; set; }
    }
    public class RemoveSearchSubscriptionCommand : CommandBase, IRequest
    {
        public Guid ResearcherGuid { get; set; }
        public Guid SearchSubscriptionGuid { get; set; }
    }
}
