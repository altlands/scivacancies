﻿using SciVacancies.ReadModel.Core;
using SciVacancies.ReadModel.Pager;

namespace SciVacancies.WebApp.ViewModels
{
    public class NotificationsInResearcherIndexViewModel
    {
        public PagedList<ResearcherNotification> PagedItems { get; set; }

    }
}
