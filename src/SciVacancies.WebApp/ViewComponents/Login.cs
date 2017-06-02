using System;
using System.ComponentModel;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNet.Mvc;
using SciVacancies.WebApp.Controllers;
using SciVacancies.WebApp.Queries;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.ViewComponents
{
    public class Login : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly IMemoryCache _cache;
        private MemoryCacheEntryOptions _cacheOptions => new MemoryCacheEntryOptions().SetAbsoluteExpiration(DateTimeOffset.Now.AddSeconds(120));

        public Login(
            IMediator mediator,
            IMemoryCache cache
            )
        {
            _mediator = mediator;
            _cache = cache;
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

                    int unreadNotificationCount;
                    if (!_cache.TryGetValue<int>("UnreadNotificationCount"+idClaim.Value, out unreadNotificationCount))
                    {
                        unreadNotificationCount = _mediator.Send(new CountResearcherNotificationsUnreadQuery { ResearcherGuid = userGuid });
                        _cache.Set<int>("UnreadNotificationCount" + idClaim.Value, unreadNotificationCount, _cacheOptions);
                    }
                    model.UnreadNotificationCount = unreadNotificationCount;
                }
            }
            else if (User.IsInRole(ConstTerms.RequireRoleOrganizationAdmin))
            {
                var idClaim = ((ClaimsPrincipal)User).Claims.FirstOrDefault(c => c.Type == ConstTerms.ClaimTypeOrganizationId);
                if (idClaim != null)
                {
                    var userGuid = (Guid)TypeDescriptor.GetConverter(typeof(Guid)).ConvertFrom(idClaim.Value);

                    int unreadNotificationCount;
                    if (!_cache.TryGetValue<int>("UnreadNotificationCount" + idClaim.Value, out unreadNotificationCount))
                    {
                        unreadNotificationCount = _mediator.Send(new CountOrganizationNotificationsUnreadQuery { OrganizationGuid = userGuid });
                        _cache.Set<int>("UnreadNotificationCount" + idClaim.Value, unreadNotificationCount, _cacheOptions);
                    }
                    model.UnreadNotificationCount = unreadNotificationCount;

                }
            }

            return View("/Views/Partials/_Login", model);
        }
    }
}
