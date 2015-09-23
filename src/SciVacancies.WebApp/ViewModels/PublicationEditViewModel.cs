using System;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.ViewModels
{
    public class PublicationEditViewModel : ViewModelBase
    {
        public Guid ResearcherGuid { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string ExtId { get; set; }

        public string Authors { get; set; }

        public DateTime? Updated { get; set; }

    }
}