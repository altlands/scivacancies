using System.ComponentModel.DataAnnotations;

namespace SciVacancies.WebApp.ViewModels
{
    public class ConferenceEditViewModel
    {
        [MaxLength(1500, ErrorMessage = "Длина строки не более 1500 символов")]
        public string conference { get; set; }
        [MaxLength(1500, ErrorMessage = "Длина строки не более 1500 символов")]
        public string title { get; set; }
        [MaxLength(1500, ErrorMessage = "Длина строки не более 1500 символов")]
        public string categoryType { get; set; }
        public int year { get; set; }
    }
}