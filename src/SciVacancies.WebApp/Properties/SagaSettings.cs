namespace SciVacancies.WebApp
{
    public class SagaSettings
    {
        public SagaDateSettings Date { get; set; } = new SagaDateSettings();
    }
    public class SagaDateSettings
    {
        public CommitteeDateSettings Committee { get; set; } = new CommitteeDateSettings();
        public OfferResponseAwaitingDateSettings OfferResponseAwaiting { get; set; } = new OfferResponseAwaitingDateSettings();
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
