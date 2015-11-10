namespace SciVacancies.WebApp
{
    public class ApiSettings
    {
        public ApiProviderSettings Sciencemon { get; set; } = new ApiProviderSettings();
        public ApiProviderSettings Mapofscience { get; set; } = new ApiProviderSettings();
    }
    public class ApiProviderSettings
    {
        public string UserProfile { get; set; }
    }
}
