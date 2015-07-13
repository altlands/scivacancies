using SciVacancies.Domain.DataModels;

using System;

namespace SciVacancies.Domain.Events
{
    public class OrganizationEventBase : EventBase
    {
        public Guid OrganizationGuid { get; set; }
    }

    /// <summary>
    /// Организация создана (данные слиты с внешнего ресурса)
    /// </summary>
    public class OrganizationCreated : OrganizationEventBase
    {
        public OrganizationDataModel Data { get; set; }
    }

    /// <summary>
    /// Профиль организации обновлён
    /// </summary>
    public class OrganizationUpdated : OrganizationEventBase
    {
        public OrganizationDataModel Data { get; set; }
    }

    /// <summary>
    /// Организация удалена (удаление доступно только администратору)
    /// //TODO - нужно ли это?
    /// </summary>
    public class OrganizationRemoved : OrganizationEventBase
    {
    }

}
