using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.ReadModel.Handlers
{
    public class SearchSubscriptionCreatedHandler : EventBaseHandler<SearchSubscriptionCreated>
    {
        public SearchSubscriptionCreatedHandler(IDatabase db) : base(db) { }
        public override void Handle(SearchSubscriptionCreated msg)
        {
            //TODO
        }
    }
    public class SearchSubscriptionActivatedHandler : EventBaseHandler<SearchSubscriptionActivated>
    {
        public SearchSubscriptionActivatedHandler(IDatabase db) : base(db) { }
        public override void Handle(SearchSubscriptionActivated msg)
        {
            //TODO
        }
    }
    public class SearchSubscriptionCanceledHandler : EventBaseHandler<SearchSubscriptionCanceled>
    {
        public SearchSubscriptionCanceledHandler(IDatabase db) : base(db) { }
        public override void Handle(SearchSubscriptionCanceled msg)
        {
            //TODO
        }
    }
    public class SearchSubscriptionRemovedHandler : EventBaseHandler<SearchSubscriptionRemoved>
    {
        public SearchSubscriptionRemovedHandler(IDatabase db) : base(db) { }
        public override void Handle(SearchSubscriptionRemoved msg)
        {
            //TODO
        }
    }
}
