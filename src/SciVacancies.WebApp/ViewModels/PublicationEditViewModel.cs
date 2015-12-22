using System;
using System.ComponentModel.DataAnnotations;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.ViewModels
{
    public class PublicationEditViewModel : ViewModelBase
    {
        public Guid ResearcherGuid { get; set; }
        [MaxLength(1500, ErrorMessage = "Длина строки не более 1500 символов")]
        public string Name { get; set; }
        [MaxLength(1500, ErrorMessage = "Длина строки не более 1500 символов")]
        public string Type { get; set; }
        public string ExtId { get; set; }
        [MaxLength(1500, ErrorMessage = "Длина строки не более 1500 символов")]
        public string Authors { get; set; }
        public DateTime? Updated { get; set; }

    }
}