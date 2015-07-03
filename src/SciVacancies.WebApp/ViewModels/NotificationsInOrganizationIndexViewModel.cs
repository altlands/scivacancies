﻿using System;
using SciVacancies.ReadModel.Core;
using SciVacancies.ReadModel.Pager;

namespace SciVacancies.WebApp.ViewModels
{
    public class NotificationsInOrganizationIndexViewModel
    {
     public Guid OrganizationGuid { get; set; }
        public PagedList<Notification> PagedNotifications { get; set; }
        public string Name { get; set; }

    }
}
