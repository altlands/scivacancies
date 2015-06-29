using System;
using MediatR;
using Microsoft.AspNet.Mvc;
using SciVacancies.WebApp.Commands;

namespace SciVacancies.WebApp.Controllers
{
    public class SubscriptionsController: Controller
    {
        private readonly IMediator _mediator;
        public SubscriptionsController(IMediator mediator){_mediator = mediator;}

        [BindResearcherIdFromClaims]
        public ActionResult DeleteSubscription(Guid researcherGuid, Guid subscriptionGuid)
        {
            _mediator.Send(new RemoveSearchSubscriptionCommand { ResearcherGuid = researcherGuid, SearchSubscriptionGuid = subscriptionGuid });
            return RedirectToAction("subscriptions", "researchers");
        }

        [BindResearcherIdFromClaims]
        public ActionResult ActivateSubscription(Guid researcherGuid, Guid subscriptionGuid)
        {
            _mediator.Send(new ActivateSearchSubscriptionCommand { ResearcherGuid = researcherGuid, SearchSubscriptionGuid = subscriptionGuid });

            return RedirectToAction("subscriptions", "researchers");
        }

        [BindResearcherIdFromClaims]
        public ActionResult CancelSubscription(Guid researcherGuid, Guid subscriptionGuid)
        {
            _mediator.Send(new CancelSearchSubscriptionCommand { ResearcherGuid = researcherGuid, SearchSubscriptionGuid = subscriptionGuid });

            return RedirectToAction("subscriptions", "researchers");
        }
    }
}
