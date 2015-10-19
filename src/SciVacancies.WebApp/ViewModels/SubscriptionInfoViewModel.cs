using System;

namespace SciVacancies.WebApp.ViewModels
{

    public class SubscriptionInfoViewModel
    {
        /// <summary>
        /// Только что создана новая подписка
        /// </summary>
        public bool NewJustAdded { get; set; }

        /// <summary>
        /// Guid новой добавленной подписки
        /// </summary>
        public Guid NewGuid { get; set; }

        public string Title { get; set; }
    }
}