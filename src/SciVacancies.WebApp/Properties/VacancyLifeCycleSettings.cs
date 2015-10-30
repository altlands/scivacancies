namespace SciVacancies.WebApp
{
    /// <summary>
    /// настройки жизненного цикла Вакансии
    /// </summary>
    public  class VacancyLifeCycleSettings
    {
        /// <summary>
        /// минимальный период от Публикации Вакансии до её перевода На Рассмотрение (в минутах)
        /// </summary>
        public int DeltaFromPublishToInCommitteeMinMinutes { get; set; }
    }
}