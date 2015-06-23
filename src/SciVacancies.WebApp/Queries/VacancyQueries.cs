using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;

using MediatR;

namespace SciVacancies.WebApp.Queries
{
    public class VacancyDetailsQuery : IRequest<Vacancy>
    {
        public Guid VacancyGuid { get; set; }
    }
}
