using System;
using SciVacancies.Domain.DataModels;

namespace SciVacancies.Domain.Events
{
    public class OrganizationEventBase : EventBase
    {
        public Guid OrganizationGuid { get; set; }
    }

    /// <summary>
    /// Организация создана. Данные слиты с внешнего ресурса.
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
    /// Организация удалена.
    /// </summary>
    public class OrganizationRemoved : OrganizationEventBase
    {
    }

}
