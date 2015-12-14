using System.Collections.Generic;

using Nest;

namespace SciVacancies.Services.Elastic
{
    public interface IAnalythicService
    {
        IDictionary<string, IAggregation> VacancyPayments(VacancyPaymentsAnalythicQuery query);
        IDictionary<string, IAggregation> VacancyPayments(VacancyPaymentsByResearchDirectionAnalythicQuery query);
        IDictionary<string, IAggregation> VacancyPositions(VacancyPositionsAnalythicQuery query);
    }
}
