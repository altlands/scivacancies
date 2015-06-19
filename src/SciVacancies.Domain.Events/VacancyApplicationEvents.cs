using SciVacancies.Domain.DataModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Events
{
    public class VacancyApplicationEventBase : EventBase
    {
        public VacancyApplicationEventBase() : base() { }

        public Guid VacancyApplicationGuid { get; set; }
        public Guid VacancyGuid { get; set; }
        public Guid ResearcherGuid { get; set; }
    }
    /// <summary>
    /// Создан шаблон заявки, заявка не отправлена
    /// </summary>
    public class VacancyApplicationCreated : VacancyApplicationEventBase
    {
        public VacancyApplicationCreated() : base() { }

        public VacancyApplicationDataModel Data { get; set; }
    }
    /// <summary>
    /// Шаблон заявки обновлён, заявка не отправлена
    /// </summary>
    public class VacancyApplicationUpdated : VacancyApplicationEventBase
    {
        public VacancyApplicationUpdated() : base() { }

        public VacancyApplicationDataModel Data { get; set; }
    }
    /// <summary>
    /// Шаблон заявки удалён
    /// </summary>
    public class VacancyApplicationRemoved : VacancyApplicationEventBase
    {
        public VacancyApplicationRemoved() : base() { }
    }
    /// <summary>
    /// Работа с шаблоном завершена. Заявка отправлена
    /// </summary>
    public class VacancyApplicationApplied : VacancyApplicationEventBase
    {
        public VacancyApplicationApplied() : base() { }
    }
    /// <summary>
    /// Заявка отклонена из-за отмены вакансии(конкурса)
    /// </summary>
    public class VacancyApplicationCancelled : VacancyApplicationEventBase
    {
        public VacancyApplicationCancelled() : base() { }
    }
    /// <summary>
    /// Заявка выйграла вакансию(конкурс)
    /// </summary>
    public class VacancyApplicationWon : VacancyApplicationEventBase
    {
        public VacancyApplicationWon() : base() { }
    }
    /// <summary>
    /// Заявка заняла второе место
    /// </summary>
    public class VacancyApplicationPretended : VacancyApplicationEventBase
    {
        public VacancyApplicationPretended() : base() { }
    }
    /// <summary>
    /// Заявка проиграла вакансию (конкурс). Событие для всех заявок, не занявших 1-ое или 2-ое места.
    /// </summary>
    public class VacancyApplicationLost : VacancyApplicationEventBase
    {
        public VacancyApplicationLost() : base() { }
    }
}
