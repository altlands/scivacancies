namespace SciVacancies.WebApp.ViewModels
{
    public class ConferenceDetailsViewModel: ConferenceEditViewModel
    {
    }

    public class ConferenceEditViewModel
    {
        public string conference { get; set; }
        public string title { get; set; }
        public string categoryType { get; set; }
        public int year { get; set; }
    }
}