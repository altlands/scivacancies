using SciVacancies.Domain.DataModels;
using System;

namespace SciVacancies.Domain.Events
{
    public class VacancyEventBase : EventBase
    {
        public Guid VacancyGuid { get; set; }
        public Guid PositionGuid { get; set; }
        public Guid OrganizationGuid { get; set; }
    }
    /// <summary>
    /// Вакансия опубликована, но приём заявок временно закрыт
    /// </summary>
    public class VacancyPublished : VacancyEventBase
    {
        public VacancyDataModel Data { get; set; }
    }
    /// <summary>
    /// Вакансия в статусе "приём заявок"
    /// </summary>
    public class VacancyAcceptApplications : VacancyEventBase
    {
    }
    /// <summary>
    /// Вакансия в статусе "заявки на рассмотрении комиссии"
    /// </summary>
    public class VacancyInCommittee : VacancyEventBase
    {
    }
    /// <summary>
    /// Вакансия закрыта, объявление победителей
    /// </summary>
    public class VacancyClosed : VacancyEventBase
    {
        public Guid WinnerGuid { get; set; }
        public Guid PretenderGuid { get; set; }
    }
    /// <summary>
    /// Вакансия отменена организацией
    /// </summary>
    public class VacancyCancelled : VacancyEventBase
    {
        public string Reason { get; set; }
    }
    /// <summary>
    /// Для вакансии выбран победитель (первое место)
    /// </summary>
    public class VacancyWinnerSet : VacancyEventBase
    {
        public Guid ReasearcherGuid { get; set; }
        public Guid VacancyApplicationGuid { get; set; }

        public string Reason { get; set; }
    }
    /// <summary>
    /// Для вакансии выбран претендент (второе место)
    /// </summary>
    public class VacancyPretenderSet : VacancyEventBase
    {
        public Guid ReasearcherGuid { get; set; }
        public Guid VacancyApplicationGuid { get; set; }

        public string Reason { get; set; }
    }
    /// <summary>
    /// Вакансия добавлена соискателем в избранное
    /// </summary>
    public class VacancyAddedToFavorites : VacancyEventBase
    {
        public Guid ResearcherGuid { get; set; }
    }
    /// <summary>
    /// Вакансия удалена соискателем из избранного
    /// </summary>
    public class VacancyRemovedFromFavorites : VacancyEventBase
    {
        public Guid ResearcherGuid { get; set; }
    }
}
