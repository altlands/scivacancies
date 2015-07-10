using System;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.ViewModels
{
    public class PublicationEditViewModel : ViewModelBase
    {
        public Guid ResearcherGuid { get; set; }

        public string Name { get; set; }
    }
}