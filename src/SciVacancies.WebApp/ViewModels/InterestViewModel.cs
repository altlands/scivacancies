using System.ComponentModel.DataAnnotations;

namespace SciVacancies.WebApp.ViewModels
{
    public class InterestDetailsViewModel : InterestEditViewModel
    {
    }

    public class InterestEditViewModel
    {
        [MaxLength(1500, ErrorMessage = "Длина строки не более 1500 символов")]
        public string IntName { get; set; }
        [MaxLength(1500, ErrorMessage = "Длина строки не более 1500 символов")]
        public string IntNameEn { get; set; }
    }
}
