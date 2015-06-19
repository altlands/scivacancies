using SciVacancies.Domain.DataModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Events
{
    public class PositionEventBase : EventBase
    {
        public PositionEventBase() : base() { }

        public Guid PositionGuid { get; set; }
        public Guid OrganizationGuid { get; set; }
    }
    /// <summary>
    /// Позиция (шаблон вакансии) создана, никаких вакансий по данной позиции ещё нет
    /// </summary>
    public class PositionCreated : PositionEventBase
    {
        public PositionCreated() : base() { }

        public PositionDataModel Data { get; set; }
    }
    /// <summary>
    /// Информация по позиции обновлена.
    /// </summary>
    public class PositionUpdated : PositionEventBase
    {
        public PositionUpdated() : base() { }

        public PositionDataModel Data { get; set; }
    }
    /// <summary>
    /// Позиция удалена. Такое возможно, если по позиции не было создано ни одной вакансии.
    /// </summary>
    public class PositionRemoved : PositionEventBase
    {
        public PositionRemoved() : base() { }
    }
}
