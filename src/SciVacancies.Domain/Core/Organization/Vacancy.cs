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

        public Guid OrganizationGuid { get; set; }

        public VacancyDataModel Data { get; set; }

        public VacancyStatus Status { get; set; }
    }
}
