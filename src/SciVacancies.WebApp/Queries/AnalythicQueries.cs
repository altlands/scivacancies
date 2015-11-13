using SciVacancies.WebApp.Models.Analythic;

using System.Collections.Generic;

using MediatR;

namespace SciVacancies.WebApp.Queries
{
    public class AverageVacancyAndPositionAnalythicQuery : SciVacancies.Services.Elastic.VacancyPositionsAnalythicQuery, IRequest<List<PositionsHistogram>>
    {

    }

    public class AveragePaymentAndVacancyCountByRegionAnalythicQuery : SciVacancies.Services.Elastic.VacancyPaymentsAnalythicQuery, IRequest<List<PaymentsHistogram>>
    {

    }
}
