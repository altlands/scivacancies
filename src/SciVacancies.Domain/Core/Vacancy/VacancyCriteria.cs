namespace SciVacancies.Domain.Core
{
    public class VacancyCriteria
    {
        public int CriteriaId { get; set; }

        public int? CriteriaParentId { get; set; }

        public string CriteriaTitle { get; set; }

        public string CriteriaCode { get; set; }

        public long? from { get; set; }

        public long? to { get; set; }
    }
}
