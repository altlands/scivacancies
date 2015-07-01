using System;
using System.Collections.Generic;
using NPoco;
using SciVacancies.ReadModel.Core;

namespace SciVacancies.WebApp.ViewModels
{
    public class NotificationsInOrganizationIndexViewModel
    {
        public Guid OrganizationGuid { get; set; }
        public Page<Notification> PagedNotifications { get; set; }
        public string Name { get; set; }

    }
}
