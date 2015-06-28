using SciVacancies.Domain.Enums;

namespace SciVacancies.Domain.DataModels
{
    public class SearchSubscriptionDataModel
    {
        public string Title { get; set; }
        public SearchSubscriptionStatus Status { get; set; }
    }
}
