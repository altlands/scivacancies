using SciVacancies.Domain.Enums;
using SciVacancies.Domain.DataModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Core
{
    public class Position
    {
        public Guid PositionGuid { get; set; }
        [Obsolete("Field will be removed")]
        public Guid OrganizationGuid { get; set; }

        public PositionDataModel Data { get; set; }
        [Obsolete("Field will be moved to DataModel")]
        public PositionStatus Status { get; set; }
    }
}
