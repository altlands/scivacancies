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

        /// <summary>
        /// минимальный период от Публикации Вакансии до её перевода На Рассмотрение (в минутах)
        /// </summary>
        public int DeltaFromPublishToInCommitteeMinMinutes { get; set; }
        /// <summary>
        /// Период На Рассмотрение (в минутах) от начала до конца
        /// </summary>
        public int DeltaFromInCommitteeStartToEndMinutes { get; set; }
    }
    public class CommitteeDateSettings
    {
        public int FirstNotificationMinutes { get; set; }
        public int SecondNotificationMinutes { get; set; }
        public int ProlongingMinutes { get; set; }
    }
    public class OfferResponseAwaitingDateSettings
    {
        public int WinnerNotificationMinutes { get; set; }
        public int PretenderNotificationMinutes { get; set; }
    }
}
