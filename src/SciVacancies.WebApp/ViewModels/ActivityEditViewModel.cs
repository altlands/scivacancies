using System.ComponentModel.DataAnnotations;

namespace SciVacancies.WebApp.ViewModels
{
    /// <summary>
    /// Деятельность учёного (исследовательская, преподавательская, прочая )
    /// </summary>
    public class ActivityEditViewModel
    {
        [MaxLength(1500, ErrorMessage = "Длина строки не более 1500 символов")]
        public string organization { get; set; }
        [MaxLength(1500, ErrorMessage = "Длина строки не более 1500 символов")]
        public string position { get; set; }
        [MaxLength(1500, ErrorMessage = "Длина строки не более 1500 символов")]
        public string title { get; set; }
        public int yearFrom { get; set; }
        public int yearTo { get; set; }
        //[MaxLength(1500, ErrorMessage = "Длина строки не более 1500 символов")]
        //public string type { get; set; }
    }
}