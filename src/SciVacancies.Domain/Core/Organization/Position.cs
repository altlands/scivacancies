using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Core
{
    public class Position
    {
        public Guid PositionGuid { get; set; }
        public Guid OrganizationGuid { get; set; }

        public PositionDataModel Data { get; set; }

        public PositionStatus Status { get; set; }
    }
}
