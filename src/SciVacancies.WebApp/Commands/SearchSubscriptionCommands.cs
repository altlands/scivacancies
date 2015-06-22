using SciVacancies.Domain.DataModels;

using System;

using MediatR;

namespace SciVacancies.WebApp.Commands
{
    public class CreateSearchSubscriptionCommand : CommandBase, IRequest<Guid>
    {
        public CreateSearchSubscriptionCommand() : base() { }

        public Guid ResearcherGuid { get; set; }

        public SearchSubscriptionDataModel Data { get; set; }
    }
    public class CancelSearchSubscriptionCommand : CommandBase, IRequest
    {
        public CancelSearchSubscriptionCommand() : base() { }

        public Guid ResearcherGuid { get; set; }
        public Guid SearchSubscriptionGuid { get; set; }
    }
    public class ActivateSearchSubscriptionCommand : CommandBase, IRequest
    {
        public ActivateSearchSubscriptionCommand() : base() { }

        public Guid ResearcherGuid { get; set; }
        public Guid SearchSubscriptionGuid { get; set; }
    }
    public class RemoveSearchSubscriptionCommand : CommandBase, IRequest
    {
        public RemoveSearchSubscriptionCommand() : base() { }

        public Guid ResearcherGuid { get; set; }
        public Guid SearchSubscriptionGuid { get; set; }
    }
}
