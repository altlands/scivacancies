using SciVacancies.Domain.DataModels;

using System;

using MediatR;

namespace SciVacancies.Domain.Commands
{
    public class CreateSearchSubscription : CommandBase, IRequest<Guid>
    {
        public CreateSearchSubscription() : base() { }

        public Guid ResearcherGuid { get; set; }

        public SearchSubscriptionDataModel Data { get; set; }
    }
    public class CancelSearchSubscription : CommandBase, IRequest
    {
        public CancelSearchSubscription() : base() { }

        public Guid ResearcherGuid { get; set; }
        public Guid SearchSubscriptionGuid { get; set; }
    }
    public class ActivateSearchSubscription : CommandBase, IRequest
    {
        public ActivateSearchSubscription() : base() { }

        public Guid ResearcherGuid { get; set; }
        public Guid SearchSubscriptionGuid { get; set; }
    }
    public class RemoveSearchSubscription : CommandBase, IRequest
    {
        public RemoveSearchSubscription() : base() { }

        public Guid ResearcherGuid { get; set; }
        public Guid SearchSubscriptionGuid { get; set; }
    }
}
