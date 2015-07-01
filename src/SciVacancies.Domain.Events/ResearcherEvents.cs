using System;
using SciVacancies.Domain.DataModels;

namespace SciVacancies.Domain.Events
{
    public class ResearcherEventBase : EventBase
    {
        public Guid ResearcherGuid { get; set; }
    }

    /// <summary>
    /// Профиль исследователя создан
    /// </summary>
    public class ResearcherCreated : ResearcherEventBase
    {
        public ResearcherDataModel Data { get; set; }
    }

    /// <summary>
    /// Профиль исследователя обновлён
    /// </summary>
    public class ResearcherUpdated : ResearcherEventBase
    {
        public ResearcherDataModel Data { get; set; }
    }

    /// <summary>
    /// Профиль исследователя удалён (удаление доступно только администратору)
    /// //TODO - нужно ли это?
    /// </summary>
    public class ResearcherRemoved : ResearcherEventBase
    {
    }
}