using System.ComponentModel.DataAnnotations;

namespace SciVacancies.WebApp.ViewModels
{
    public class CustomCriteriaViewModel
    {
        /// <summary>
        /// значение критерия
        /// </summary>
        [Required(ErrorMessage = "Не указано количественное значение квалифицированного требования")]
        [Range(1, int.MaxValue, ErrorMessage = "Количественное значение квалифицированного требования должно быть больше 0")]
        public int Count { get; set; }
        /// <summary>
        /// Название криетерия
        /// </summary>
        [Required(ErrorMessage = "Не указано название квалифицированного требования")]
        public string Title { get; set; }
    }
}
