namespace SciVacancies.SearchSubscriptionsService
{
    /// <summary>
    /// ищет подписки и вызывает обработчик для них в параллельных потоках
    /// </summary>
    public interface ISearchSubscriptionManager
    {
        void Combine();
    }
}
