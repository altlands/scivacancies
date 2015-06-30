using System;
using MediatR;
using Microsoft.AspNet.Mvc;
using SciVacancies.WebApp.Commands;

namespace SciVacancies.WebApp.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly IMediator _mediator;
        public NotificationsController(IMediator mediator) { _mediator = mediator; }

        [BindResearcherIdFromClaims]
        public ActionResult MarkNotificationRead(Guid notificationGuid)
        {
            //TODO: Notifications -> MarkNotificationRead : реализовать
            //TODO: Notifications -> MarkNotificationRead : перевести уведомление в статус "прочитано" - при клике на конверт показывается попап с полным текстом и вызывается этот метод
            _mediator.Send(new SwitchNotificationToReadCommand { NotificationGuid = notificationGuid });

            return RedirectToAction("notifications", "researchers");
        }

        [BindResearcherIdFromClaims]
        public ActionResult DeleteNotification(Guid notificationGuid)
        {
            //TODO: Notifications -> DeleteNotification : реализовать
            _mediator.Send(new RemoveNotificationCommand { NotificationGuid = notificationGuid });
            return RedirectToAction("notifications", "researchers");
        }

    }
}
