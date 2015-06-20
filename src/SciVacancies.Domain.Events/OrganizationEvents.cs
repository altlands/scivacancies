using SciVacancies.Domain.DataModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Events
{
    public class OrganizationEventBase : EventBase
    {
        public OrganizationEventBase() : base() { }

        public Guid OrganizationGuid { get; set; }
    }
    /// <summary>
    /// Организация создана. Данные слиты с внешнего ресурса.
    /// </summary>
    public class OrganizationCreated : OrganizationEventBase
    {
        public OrganizationCreated() : base() { }

        public OrganizationDataModel Data { get; set; }
    }
    /// <summary>
    /// Профиль организации обновлён
    /// </summary>
    public class OrganizationUpdated : OrganizationEventBase
    {
        public OrganizationUpdated() : base() { }

        public OrganizationDataModel Data { get; set; }
    }
    /// <summary>
    /// Организация удалена.
    /// </summary>
    public class OrganizationRemoved : OrganizationEventBase
    {
        public OrganizationRemoved() : base() { }
    }

}
