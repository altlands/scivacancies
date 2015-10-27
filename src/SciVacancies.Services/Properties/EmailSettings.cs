namespace SciVacancies.Services
{
    public class EmailSettings
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public bool UseDefaultCredentials { get; set; }

        public string Domain { get; set; }
        public string PortalLink { get; set; }
    }
}
