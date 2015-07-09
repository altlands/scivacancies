using SciVacancies.Domain.Enums;
using SciVacancies.Domain.DataModels;

using System;

namespace SciVacancies.Domain.Core
{
    [Obsolete("Positions will be removed from workflow")]
    public class Position
    {
        public Guid PositionGuid { get; set; }
        [Obsolete("Field will be removed")]
        public Guid OrganizationGuid { get; set; }

        public PositionDataModel Data { get; set; }

        public PositionStatus Status { get; set; }
    }
}
