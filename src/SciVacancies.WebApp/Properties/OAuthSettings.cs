namespace SciVacancies.WebApp
{
    public class OAuthSettings
    {
        public OAuthProviderSettings Sciencemon { get; set; } = new OAuthProviderSettings();
        public OAuthProviderSettings Mapofscience { get; set; } = new OAuthProviderSettings();
    }
    public class OAuthProviderSettings
    {
        public string AuthorizationEndpoint { get; set; }
        public string TokenEndpoint { get; set; }
        public string UserEndpoint { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
        public string RedirectUrl { get; set; }
    }
}
