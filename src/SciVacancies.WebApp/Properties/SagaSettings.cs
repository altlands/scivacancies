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
        public int FirstNotificationMinutes { get; set; }
        public int SecondNotificationMinutes { get; set; }
        public int ProlongingDays { get; set; }
    }
    public class OfferResponseAwaitingDateSettings
    {
        public int WinnerNotificationMinutes { get; set; }
        public int PretenderNotificationMinutes { get; set; }
    }
}
