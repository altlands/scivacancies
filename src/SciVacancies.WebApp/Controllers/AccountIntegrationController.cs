using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using SciVacancies.WebApp.Engine;

namespace SciVacancies.WebApp.Controllers
{
    [Authorize]
    public class AccountIntegrationController: Controller
    {
        private readonly IMediator _mediator;

        public AccountIntegrationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = ConstTerms.RequireRoleResearcher)]
        [BindResearcherIdFromClaims]
        public IActionResult UpdateResearcherAccountFromOutside(Guid researcherGuid, AuthorizeResourceTypes dataSource)
        {

            return null;
        }
    }
}
