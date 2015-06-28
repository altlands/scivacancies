using SciVacancies.ReadModel.Core;

namespace SciVacancies.WebApp.ViewModels
{
    public class WinnersViewModel
    {
        public Vacancy Vacancy { get; set; }
        public Researcher Winner { get; set; }
        public Researcher Pretender { get; set; }
    }
}