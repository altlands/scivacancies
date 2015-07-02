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
        public ActionResult Delete(Guid researcherGuid, Guid subscriptionGuid)
        {
            //TODO: Subscriptions -> DeleteSubscription : реализовать
            _mediator.Send(new RemoveSearchSubscriptionCommand { ResearcherGuid = researcherGuid, SearchSubscriptionGuid = subscriptionGuid });
            return RedirectToAction("subscriptions", "researchers");
        }

        [BindResearcherIdFromClaims]
        public ActionResult Activate(Guid researcherGuid, Guid subscriptionGuid)
        {
            //TODO: Subscriptions -> ActivateSubscription : реализовать
            _mediator.Send(new ActivateSearchSubscriptionCommand { ResearcherGuid = researcherGuid, SearchSubscriptionGuid = subscriptionGuid });

            return RedirectToAction("subscriptions", "researchers");
        }

        [BindResearcherIdFromClaims]
        public ActionResult Cancel(Guid researcherGuid, Guid subscriptionGuid)
        {
            //TODO: Subscriptions -> CancelSubscription : реализовать
            _mediator.Send(new CancelSearchSubscriptionCommand { ResearcherGuid = researcherGuid, SearchSubscriptionGuid = subscriptionGuid });

            return RedirectToAction("subscriptions", "researchers");
        }
    }
}
