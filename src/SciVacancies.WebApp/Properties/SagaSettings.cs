namespace SciVacancies.WebApp
{
    public class SagaSettings
    {
        public SagaDateSettings Date { get; set; }
    }
    public class SagaDateSettings
    {
        public CommitteeDateSettings Committee { get; set; }
        public OfferResponseAwaitingDateSettings OfferResponseAwaiting { get; set; }
    }
    public class CommitteeDateSettings
    {
        public int FirstNotificationDays { get; set; }
        public int SecondNotificationDays { get; set; }
        public int ProlongingDays { get; set; }
    }
    public class OfferResponseAwaitingDateSettings
    {
        public int WinnerNotificationDays { get; set; }
        public int PretenderNotificationDays { get; set; }
    }
}
