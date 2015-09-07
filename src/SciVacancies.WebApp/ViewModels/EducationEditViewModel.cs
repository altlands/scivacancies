using System;
using System.ComponentModel.DataAnnotations;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.ViewModels
{
    public class EducationEditViewModel: ViewModelBase
    {
        public Guid ResearcherGuid { get; set; }

        [Required(ErrorMessage = "Укажите название города")]
        public string City { get; set; }

        [Required(ErrorMessage = "Укажите название университета")]
        public string UniversityShortName { get; set; }

        [Required(ErrorMessage = "Укажите название факультета")]
        public string FacultyShortName { get; set; }

        [Required(ErrorMessage = "Укажите год выпуска")]
        public int? GraduationYear { get; set; }

        [Required(ErrorMessage = "Укажите учёную степень")]
        public string Degree { get; set; }
    }
}