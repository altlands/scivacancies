using System.ComponentModel.DataAnnotations;

namespace SciVacancies.WebApp.ViewModels.Base
{
    /// <summary>
    /// Модель для отбражения сведений классификатора
    /// </summary>
    public class ClassisierItemViewModelBase : ViewModelBase
    {
        [MaxLength(1500, ErrorMessage = "Длина строки не более 1500 символов")]
        public string Name { get; set; }
    }
}
