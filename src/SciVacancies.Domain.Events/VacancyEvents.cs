using System;

namespace SciVacancies.Domain.Events
{
    public class VacancyEventBase : EventBase
    {
        public VacancyEventBase() : base() { }

        public Guid VacancyGuid { get; set; }
        public Guid PositionGuid { get; set; }
        public Guid OrganizationGuid { get; set; }
    }
    /// <summary>
    /// Вакансия опубликована, но приём заявок временно закрыт
    /// </summary>
    public class VacancyPublished : VacancyEventBase
    {
        public VacancyPublished() : base() { }

        public VacancyDataModel Data { get; set; }
    }
    /// <summary>
    /// Вакансия в статусе "приём заявок"
    /// </summary>
    public class VacancyAcceptApplications : VacancyEventBase
    {
        public VacancyAcceptApplications() : base() { }
    }
    /// <summary>
    /// Вакансия в статусе "заявки на рассмотрении комиссии"
    /// </summary>
    public class VacancyInCommittee : VacancyEventBase
    {
        public VacancyInCommittee() : base() { }
    }
    /// <summary>
    /// Вакансия закрыта, объявление победителей
    /// </summary>
    public class VacancyClosed : VacancyEventBase
    {
        public VacancyClosed() : base() { }

        public Guid WinnerGuid { get; set; }
        public Guid PretenderGuid { get; set; }
    }
    /// <summary>
    /// Вакансия отменена организацией
    /// </summary>
    public class VacancyCancelled : VacancyEventBase
    {
        public VacancyCancelled() : base() { }

        public string Reason { get; set; }
    }
    /// <summary>
    /// Вакансия добавлена соискателем в избранное
    /// </summary>
    public class VacancyAddedToFavorites : VacancyEventBase
    {
        public VacancyAddedToFavorites() : base() { }

        public Guid ResearcherGuid { get; set; }
    }
    /// <summary>
    /// Вакансия удалена соискателем из избранного
    /// </summary>
    public class VacancyRemovedFromFavorites : VacancyEventBase
    {
        public VacancyRemovedFromFavorites() : base() { }

        public Guid ResearcherGuid { get; set; }
    }
}
