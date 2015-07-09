using System;
using System.ComponentModel.DataAnnotations;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.ViewModels
{
    public class EducationEditViewModel: ViewModelBase
    {
        public Guid ResearcherGuid { get; set; }

        [Required(ErrorMessage = "")]
        public string City { get; set; }
        [Required(ErrorMessage = "")]
        public string UniversityShortName { get; set; }
        public string FacultyShortName { get; set; }
        [Required(ErrorMessage = "")]
        public DateTime? GraduationYear { get; set; }

        public string Degree { get; set; }
    }
}