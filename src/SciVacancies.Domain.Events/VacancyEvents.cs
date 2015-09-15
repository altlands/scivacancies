using SciVacancies.Domain.DataModels;
using System;
using System.Collections.Generic;
using SciVacancies.Domain.Core;

namespace SciVacancies.Domain.Events
{
    public class VacancyEventBase : EventBase
    {
        public Guid VacancyGuid { get; set; }
        public Guid OrganizationGuid { get; set; }
    }

    /// <summary>
    /// Черновик вакансии создан. Возможно редактирование. Позиция (шаблон вакансии) создана, никаких вакансий по данной позиции ещё нет
    /// </summary>
    public class VacancyCreated : VacancyEventBase
    {
        public VacancyDataModel Data { get; set; }
    }

    /// <summary>
    /// Информация в черновике вакансии обновлена
    /// </summary>
    public class VacancyUpdated : VacancyEventBase
    {
        public VacancyDataModel Data { get; set; }
    }

    /// <summary>
    /// Черновик вакансии удалён
    /// </summary>
    public class VacancyRemoved : VacancyEventBase
    {
    }

    /// <summary>
    /// Вакансия опубликована, но приём заявок временно закрыт
    /// </summary>
    public class VacancyPublished : VacancyEventBase
    {
        public DateTime InCommitteeStartDate { get; set; }
        public DateTime InCommitteeEndDate { get; set; }
    }

    /// <summary>
    /// Вакансия в статусе "заявки на рассмотрении комиссии"
    /// </summary>
    public class VacancyInCommittee : VacancyEventBase
    {
    }

    /// <summary>
    /// Статус вакансии "на комиссии" продлён
    /// </summary>
    public class VacancyProlongedInCommittee : VacancyEventBase
    {
        public string Reason { get; set; }
        public DateTime InCommitteeEndDate { get; set; }
    }

    /// <summary>
    /// К вакансии прикреплено решение комиссии с нужными файлами
    /// </summary>
    public class VacancyCommitteeResolutionSet : VacancyEventBase
    {
        public string Resolution { get; set; }
        public List<VacancyAttachment> Attachments { get; set; } = new List<VacancyAttachment>();
    }

    /// <summary>
    /// Для вакансии выбран победитель (первое место)
    /// </summary>
    public class VacancyWinnerSet : VacancyEventBase
    {
        public Guid WinnerReasearcherGuid { get; set; }
        public Guid WinnerVacancyApplicationGuid { get; set; }
    }

    /// <summary>
    /// Для вакансии выбран претендент (второе место), после этого начинается работа с победителем и претендетом (отправка поочерёдно им предложения о работе)
    /// </summary>
    public class VacancyPretenderSet : VacancyEventBase
    {
        public Guid PretenderReasearcherGuid { get; set; }
        public Guid PretenderVacancyApplicationGuid { get; set; }
    }

    /// <summary>
    /// Предложение подписать контракт отправлено победителю
    /// </summary>
    public class VacancyInOfferResponseAwaitingFromWinner : VacancyEventBase
    {
    }

    /// <summary>
    /// Предложение принято победителем
    /// </summary>
    public class VacancyOfferAcceptedByWinner : VacancyEventBase
    {
    }

    /// <summary>
    /// Предложение отклонено победителем
    /// </summary>
    public class VacancyOfferRejectedByWinner : VacancyEventBase
    {
    }

    /// <summary>
    /// Предложение подписать контракт отправлено претенденту
    /// </summary>
    public class VacancyInOfferResponseAwaitingFromPretender : VacancyEventBase
    {
    }

    /// <summary>
    /// Предложение принято претендентом
    /// </summary>
    public class VacancyOfferAcceptedByPretender : VacancyEventBase
    {
    }

    /// <summary>
    /// Предложение отклонено претендентом
    /// </summary>
    public class VacancyOfferRejectedByPretender : VacancyEventBase
    {
    }

    /// <summary>
    /// Вакансия закрыта (только в случае, если победитель или претендент приняли условия контракта)
    /// </summary>
    public class VacancyClosed : VacancyEventBase
    {
        public Guid WinnerResearcherGuid { get; set; }
        public Guid WinnerVacancyApplicationGuid { get; set; }
        public bool IsWinnerAccept { get; set; }

        public Guid PretenderResearcherGuid { get; set; }
        public Guid PretenderVacancyApplicationGuid { get; set; }
        public bool? IsPretenderAccept { get; set; }
    }

    /// <summary>
    /// Вакансия отменена организацией, все заявки к этой вакансии отменяются
    /// </summary>
    public class VacancyCancelled : VacancyEventBase
    {
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
