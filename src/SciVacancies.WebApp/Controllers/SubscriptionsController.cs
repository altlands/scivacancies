using System;
using MediatR;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using SciVacancies.WebApp.Commands;
using SciVacancies.WebApp.Engine;

namespace SciVacancies.WebApp.Controllers
{
    [Authorize(Roles = ConstTerms.RequireRoleResearcher)]
    public class SubscriptionsController: Controller
    {
        private readonly IMediator _mediator;
        public SubscriptionsController(IMediator mediator){_mediator = mediator;}

        [BindResearcherIdFromClaims]
        public ActionResult Delete(Guid researcherGuid, Guid id)
        {
            //TODO: Subscriptions -> DeleteSubscription : реализовать
            _mediator.Send(new RemoveSearchSubscriptionCommand { ResearcherGuid = researcherGuid, SearchSubscriptionGuid = id });
            return RedirectToAction("subscriptions", "researchers");
        }

        [BindResearcherIdFromClaims]
        public ActionResult Activate(Guid researcherGuid, Guid id)
        {
            //TODO: Subscriptions -> ActivateSubscription : реализовать
            _mediator.Send(new ActivateSearchSubscriptionCommand { ResearcherGuid = researcherGuid, SearchSubscriptionGuid = id });

            return RedirectToAction("subscriptions", "researchers");
        }

        [BindResearcherIdFromClaims]
        public ActionResult Cancel(Guid researcherGuid, Guid id)
        {
            //TODO: Subscriptions -> CancelSubscription : реализовать
            _mediator.Send(new CancelSearchSubscriptionCommand { ResearcherGuid = researcherGuid, SearchSubscriptionGuid = id });

            return RedirectToAction("subscriptions", "researchers");
        }
    }
}
