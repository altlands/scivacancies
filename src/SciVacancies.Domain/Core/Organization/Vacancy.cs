using SciVacancies.Domain.Enums;
using SciVacancies.Domain.DataModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Core
{
    public class Vacancy
    {
        public Guid VacancyGuid { get; set; }
        public Guid PositionGuid { get; set; }
        [Obsolete("Field will be removed")]
        public Guid OrganizationGuid { get; set; }

        public VacancyDataModel Data { get; set; }
        [Obsolete("Field will be moved to DataModel")]
        public VacancyStatus Status { get; set; }
    }
}
