using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using SciVacancies.WebApp.Commands;
using SciVacancies.WebApp.Queries;

namespace SciVacancies.WebApp.Controllers
{
        [ResponseCache(NoStore = true)]
    [Authorize]
    public class NotificationsController : Controller
    {
        private const string ExceptionTextRemoveOnlyMyNotifications = "Вы можете изменять и удалять только свои уведомления";
        private readonly IMediator _mediator;
        public NotificationsController(IMediator mediator) { _mediator = mediator; }

        [BindResearcherIdFromClaims]
        [BindOrganizationIdFromClaims]
        public ActionResult MarkNotificationRead(IList<Guid> notificationGuids, Guid researcherGuid, Guid organizationGuid)
        {
            if (notificationGuids == null)
                if (string.IsNullOrWhiteSpace(Request.Headers["referer"]))
                    return Redirect(Request.Headers["referer"]);
                else
                    throw new ArgumentNullException(nameof(notificationGuids));

            if (researcherGuid == Guid.Empty && organizationGuid == Guid.Empty)
                return View("Error", "Мы не смогли получить идентификатор Заявителя или Организации");

            var isResearcher = researcherGuid != Guid.Empty;

            if (notificationGuids.Count == 0)
                return RedirectToAction("notifications", isResearcher ? "researchers" : "organizations");

            foreach (var notificationGuid in notificationGuids)
            {
                if (isResearcher)
                {
                    var userNotification = _mediator.Send(new SingleResearcherNotificationQuery { Guid = notificationGuid });
                    if (userNotification.researcher_guid != researcherGuid)
                        return View("Error", ExceptionTextRemoveOnlyMyNotifications);
                    _mediator.Send(new SwitchResearcherNotificationToReadCommand { NotificationGuid = notificationGuid });
                }
                else
                {
                    var userNotification = _mediator.Send(new SingleOrganizationNotificationQuery { Guid = notificationGuid });
                    if (userNotification.organization_guid != organizationGuid)
                        return View("Error", ExceptionTextRemoveOnlyMyNotifications);
                    _mediator.Send(new SwitchOrganizationNotificationToReadCommand { NotificationGuid = notificationGuid });
                }

            }

            return RedirectToAction("notifications", isResearcher ? "researchers" : "organizations");
        }

        [BindResearcherIdFromClaims]
        [BindOrganizationIdFromClaims]
        public ActionResult Delete(IList<Guid> notificationGuids, Guid researcherGuid, Guid organizationGuid)
        {
            if (notificationGuids == null)
                if (string.IsNullOrWhiteSpace(Request.Headers["referer"]))
                    return Redirect(Request.Headers["referer"]);
                else
                    throw new ArgumentNullException(nameof(notificationGuids));

            if (researcherGuid == Guid.Empty && organizationGuid == Guid.Empty)
                return View("Error", "Мы не смогли получить идентификатор Заявителя или Организации");

            var isResearcher = researcherGuid != Guid.Empty;

            if (notificationGuids.Count == 0)
                return RedirectToAction("notifications", isResearcher ? "researchers" : "organizations");

            foreach (var notificationGuid in notificationGuids)
            {
                if (isResearcher)
                {
                    var userNotification = _mediator.Send(new SingleResearcherNotificationQuery { Guid = notificationGuid });
                    if (userNotification.researcher_guid!= researcherGuid)
                        return View("Error", ExceptionTextRemoveOnlyMyNotifications);
                    _mediator.Send(new RemoveResearcherNotificationCommand { NotificationGuid = notificationGuid });
                }
                else
                {
                    var userNotification = _mediator.Send(new SingleOrganizationNotificationQuery { Guid = notificationGuid });
                    if (userNotification.organization_guid!= organizationGuid)
                        return View("Error", ExceptionTextRemoveOnlyMyNotifications);
                    _mediator.Send(new RemoveOrganizationNotificationCommand { NotificationGuid = notificationGuid });
                }

            }

            return RedirectToAction("notifications", isResearcher ? "researchers" : "organizations");
        }

    }
}
