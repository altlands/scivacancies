using Nest;

namespace SciVacancies.Services.Elastic
{
    public class VacancyPaymentsAnalythicQuery
    {
        public int? RegionId { get; set; }
        public DateInterval Interval { get; set; }
    }
}
