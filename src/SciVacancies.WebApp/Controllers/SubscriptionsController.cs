using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using SciVacancies.WebApp.Commands;
using SciVacancies.WebApp.Engine;
using SciVacancies.WebApp.Queries;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Controllers
{
        [ResponseCache(NoStore = true)]
    [Authorize(Roles = ConstTerms.RequireRoleResearcher)]
    public class SubscriptionsController : Controller
    {
        private readonly IMediator _mediator;
        public SubscriptionsController(IMediator mediator) { _mediator = mediator; }

        [BindResearcherIdFromClaims]
        public ActionResult Delete(Guid researcherGuid, Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            _mediator.Send(new RemoveSearchSubscriptionCommand { ResearcherGuid = researcherGuid, SearchSubscriptionGuid = id });
            return RedirectToAction("subscriptions", "researchers");
        }

        [BindResearcherIdFromClaims]
        public ActionResult Activate(Guid researcherGuid, Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            _mediator.Send(new ActivateSearchSubscriptionCommand { ResearcherGuid = researcherGuid, SearchSubscriptionGuid = id });
            return RedirectToAction("subscriptions", "researchers");
        }

        [BindResearcherIdFromClaims]
        public ActionResult Cancel(Guid researcherGuid, Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            _mediator.Send(new CancelSearchSubscriptionCommand { ResearcherGuid = researcherGuid, SearchSubscriptionGuid = id });
            return RedirectToAction("subscriptions", "researchers");
        }

        [BindResearcherIdFromClaims]
        public ActionResult Details(Guid researcherGuid, Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var searchSubscription = _mediator.Send(new SingleSearchSubscriptionQuery { SearchSubscriptionGuid = id });

            if (searchSubscription.researcher_guid != researcherGuid)
                return View("Error", "Вы не можете просматривать чужие подписки");

            var model = new VacanciesFilterModel();
            model.VacancyStates = JsonConvert.DeserializeObject<IEnumerable<int>>(searchSubscription.vacancy_statuses);
            model.SalaryMax = searchSubscription.salary_to;
            model.SalaryMin = searchSubscription.salary_from;
            model.ResearchDirections = JsonConvert.DeserializeObject<IEnumerable<int>>(searchSubscription.researchdirection_ids);
            model.PositionTypes = JsonConvert.DeserializeObject<IEnumerable<int>>(searchSubscription.positiontype_ids);
            model.Search = searchSubscription.query;
            model.OrderBy = searchSubscription.@orderby;
            model.Regions = JsonConvert.DeserializeObject<IEnumerable<int>>(searchSubscription.region_ids);
            model.Foivs = JsonConvert.DeserializeObject<IEnumerable<int>>(searchSubscription.foiv_ids);

            TempData["VacanciesFilterModel"] = JsonConvert.SerializeObject(model);

            return RedirectToAction("index", "search");
        }

    }
}
