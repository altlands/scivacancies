using System;
using NPoco;
using SciVacancies.ReadModel.Core;

namespace SciVacancies.WebApp.ViewModels
{
    public class NotificationsInResearcherIndexViewModel
    {
        public Page<Notification> PagedItems { get; set; }

    }
}
