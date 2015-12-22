using System;
using System.ComponentModel.DataAnnotations;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.ViewModels
{
    public class EducationEditViewModel: ViewModelBase
    {
        public Guid ResearcherGuid { get; set; }

        [Required(ErrorMessage = "Укажите название города")]
        [MaxLength(1500, ErrorMessage = "Длина строки не более 1500 символов")]
        public string City { get; set; }

        [Required(ErrorMessage = "Укажите название университета")]
        [MaxLength(1500, ErrorMessage = "Длина строки не более 1500 символов")]
        public string UniversityShortName { get; set; }

        [Required(ErrorMessage = "Укажите название факультета")]
        [MaxLength(1500, ErrorMessage = "Длина строки не более 1500 символов")]
        public string FacultyShortName { get; set; }

        [Required(ErrorMessage = "Укажите год выпуска")]
        public int? GraduationYear { get; set; }

        [Required(ErrorMessage = "Укажите учёную степень")]
        [MaxLength(1500, ErrorMessage = "Длина строки не более 1500 символов")]
        public string Degree { get; set; }
    }
}