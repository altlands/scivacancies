using System;
using System.ComponentModel;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNet.Mvc;
using SciVacancies.WebApp.Controllers;
using SciVacancies.WebApp.Queries;
using System.Linq;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.ViewComponents
{
    public class Login : ViewComponent
    {
        private readonly IMediator _mediator;

        public Login(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindResearcherIdFromClaims]
        public IViewComponentResult Invoke()
        {
            var model = new AccountLoginViewModel();
            if (User.IsInRole(ConstTerms.RequireRoleResearcher))
            {
                var idClaim = ((ClaimsPrincipal)User).Claims.FirstOrDefault(c => c.Type == ConstTerms.ClaimTypeResearcherId);
                if (idClaim != null)
                {
                    var userGuid = (Guid)TypeDescriptor.GetConverter(typeof(Guid)).ConvertFrom(idClaim.Value);
                    model.UnreadNotificationCount = _mediator.Send(new CountResearcherNotificationsUnreadQuery { ResearcherGuid = userGuid });
                }
            }
            else if (User.IsInRole(ConstTerms.RequireRoleOrganizationAdmin))
            {
                var idClaim = ((ClaimsPrincipal)User).Claims.FirstOrDefault(c => c.Type == ConstTerms.ClaimTypeOrganizationId);
                if (idClaim != null)
                {
                    var userGuid = (Guid)TypeDescriptor.GetConverter(typeof(Guid)).ConvertFrom(idClaim.Value);
                    model.UnreadNotificationCount = _mediator.Send(new CountOrganizationNotificationsUnreadQuery { OrganizationGuid = userGuid });
                }
            }

            return View("/Views/Partials/_Login", model);
        }
    }
}
