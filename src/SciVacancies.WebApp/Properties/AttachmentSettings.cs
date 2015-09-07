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
        /// ������������ ������ ������ �������
        /// </summary>
        public int MaxItemSize { get; set; }
        /// <summary>
        /// ������������ ��������� ������
        /// </summary>
        public int MaxTotalSize { get; set; }
        /// <summary>
        /// ����� ���� � ���������� ����� �� �����
        /// </summary>
        public string PhisicalPathPart { get; set; }
        /// <summary>
        /// ����� URL ��� ����� � ������������ �������
        /// </summary>
        public string UrlPathPart { get; set; }
        /// <summary>
        /// ���������� ���������� ������
        /// </summary>
        public string AllowExtensions { get; set; }
    }
}