namespace SciVacancies.ReadModel.ElasticSearchModel.Model
{
    public class VacancyCriteria
    {
        public int CriteriaId { get; set; }

        public int? CriteriaParentId { get; set; }

        public string CriteriaTitle { get; set; }

        public string CriteriaCode { get; set; }

        public long? From { get; set; }

        public long? To { get; set; }
    }
}
