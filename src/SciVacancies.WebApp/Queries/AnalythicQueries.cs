using System.Collections.Generic;

using MediatR;

namespace SciVacancies.WebApp.Queries
{
    public class AverageVacancyAndPositionAnalythicQuery : SciVacancies.Services.Elastic.VacancyPositionsAnalythicQuery, IRequest<IDictionary<string, Nest.IAggregation>>
    {

    }

    public class AveragePaymentAndVacancyCountByRegionAnalythicQuery : SciVacancies.Services.Elastic.VacancyPaymentsAnalythicQuery, IRequest<IDictionary<string, Nest.IAggregation>>
    {

    }
}
