namespace SciVacancies.WebApp.ViewModels
{
    /// <summary>
    /// Деятельность учёного (исследовательская, преподавательская, прочая )
    /// </summary>
    public class ActivityEditViewModel
    {
        public string organization { get; set; }
        public string position { get; set; }
        public string title { get; set; }
        public int yearFrom { get; set; }
        public int yearTo { get; set; }
        public string type { get; set; }
    }
}