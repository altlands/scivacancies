namespace SciVacancies.WebApp
{
    public class AttachmentSettings
    {
        public AttachmentsParameters VacancyApplication { get; set; } = new AttachmentsParameters();
        public AttachmentsParameters Vacancy { get; set; }  = new AttachmentsParameters();
        public AttachmentsParameters Researcher { get; set; } = new AttachmentsParameters();
    }

    public class AttachmentsParameters
    {
        /// <summary>
        /// максимальный размер одного объекта
        /// </summary>
        public int MaxItemSize { get; set; }
        /// <summary>
        /// максимальный суммарный размер
        /// </summary>
        public int MaxTotalSize { get; set; }
        /// <summary>
        /// часть путь к физической папке на диске
        /// </summary>
        public string PhisicalPathPart { get; set; }
        /// <summary>
        /// часть URL как папке с приложенными файлами
        /// </summary>
        public string UrlPathPart { get; set; }
        /// <summary>
        /// допустимые расширения файлов
        /// </summary>
        public string AllowExtensions { get; set; }
    }
}