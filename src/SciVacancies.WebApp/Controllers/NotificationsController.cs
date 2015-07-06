using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using SciVacancies.WebApp.Commands;
using SciVacancies.WebApp.Queries;

namespace SciVacancies.WebApp.Controllers
{
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
            //TODO: Notifications -> MarkNotificationRead : перевести уведомление в статус "прочитано" - при клике на конверт показывается попап с полным текстом и вызывается этот метод
            if (notificationGuids == null)
                if (string.IsNullOrWhiteSpace(Request.Headers["referer"]))
                    return Redirect(Request.Headers["referer"]);
                else
                    throw new ArgumentNullException(nameof(notificationGuids));

            if (researcherGuid == Guid.Empty && organizationGuid == Guid.Empty)
                throw new Exception("Мы не смогли получить идентификатор Заявителя или Организации");

            var isResearcher = researcherGuid != Guid.Empty;

            if (notificationGuids.Count == 0)
                return RedirectToAction("notifications", isResearcher ? "researchers" : "organizations");

            foreach (var notificationGuid in notificationGuids)
            {
                var userNotification = _mediator.Send(new SingleNotificationQuery { NotificationGuid = notificationGuid });
                if (isResearcher)
                {
                    if (userNotification.ResearcherGuid != researcherGuid)
                        throw new Exception(ExceptionTextRemoveOnlyMyNotifications);
                }
                else if (userNotification.OrganizationGuid != organizationGuid)
                    throw new Exception(ExceptionTextRemoveOnlyMyNotifications);

                _mediator.Send(new SwitchNotificationToReadCommand { NotificationGuid = notificationGuid });
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
                throw new Exception("Мы не смогли получить идентификатор Заявителя или Организации");

            var isResearcher = researcherGuid != Guid.Empty;

            if (notificationGuids.Count == 0)
                return RedirectToAction("notifications", isResearcher ? "researchers" : "organizations");

            foreach (var notificationGuid in notificationGuids)
            {
                var userNotification = _mediator.Send(new SingleNotificationQuery { NotificationGuid = notificationGuid });
                if (isResearcher)
                {
                    if (userNotification.ResearcherGuid != researcherGuid)
                        throw new Exception(ExceptionTextRemoveOnlyMyNotifications);
                }
                else if (userNotification.OrganizationGuid != organizationGuid)
                    throw new Exception(ExceptionTextRemoveOnlyMyNotifications);

                _mediator.Send(new RemoveNotificationCommand { NotificationGuid = notificationGuid });
            }

            return RedirectToAction("notifications", isResearcher ? "researchers" : "organizations");
        }

    }
}
