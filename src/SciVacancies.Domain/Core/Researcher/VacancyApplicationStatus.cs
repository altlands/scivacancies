using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Core
{
    public enum VacancyApplicationStatus
    {
        InProcess = 0,
        Applied = 1,
        Cancelled = 2,
        Won = 3,
        Pretended = 4,
        Lost = 5
    }
}
