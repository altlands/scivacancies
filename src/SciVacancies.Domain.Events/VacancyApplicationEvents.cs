﻿using System;
using SciVacancies.Domain.DataModels;

namespace SciVacancies.Domain.Events
{
    public class VacancyApplicationEventBase : EventBase
    {
        public Guid VacancyApplicationGuid { get; set; }
        public Guid ResearcherGuid { get; set; }

        public Guid VacancyGuid { get; set; }
    }

    /// <summary>
    /// Создан шаблон заявки, заявка не отправлена
    /// </summary>
    public class VacancyApplicationCreated : VacancyApplicationEventBase
    {
        public VacancyApplicationDataModel Data { get; set; }
    }

    /// <summary>
    /// Шаблон заявки обновлён, заявка не отправлена
    /// </summary>
    public class VacancyApplicationUpdated : VacancyApplicationEventBase
    {
        public VacancyApplicationDataModel Data { get; set; }
    }

    /// <summary>
    /// Шаблон заявки удалён
    /// </summary>
    public class VacancyApplicationRemoved : VacancyApplicationEventBase
    {
    }

    /// <summary>
    /// Работа с шаблоном завершена. Заявка отправлена
    /// </summary>
    public class VacancyApplicationApplied : VacancyApplicationEventBase
    {
    }

    /// <summary>
    /// Заявка отклонена из-за отмены конкурса на вакансии
    /// </summary>
    public class VacancyApplicationCancelled : VacancyApplicationEventBase
    {
    }

    /// <summary>
    /// Заявка выйграла в конкурсе на вакансию
    /// </summary>
    public class VacancyApplicationWon : VacancyApplicationEventBase
    {
        public string Reason { get; set; }
    }

    /// <summary>
    /// Заявка заняла второе место
    /// </summary>
    public class VacancyApplicationPretended : VacancyApplicationEventBase
    {
        public string Reason { get; set; }
    }

    /// <summary>
    /// Заявка проиграла в конкурсе на вакансию. Событие для всех заявок, не занявших 1-ое или 2-ое места.
    /// </summary>
    public class VacancyApplicationLost : VacancyApplicationEventBase
    {
    }
}
