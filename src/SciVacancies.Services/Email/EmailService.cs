using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Framework.Logging;
using Microsoft.Framework.Configuration;


namespace SciVacancies.Services.Email
{
    public class EmailService : IEmailService, IDisposable
    {
        public ILogger Logger { get; set; }

        private readonly IConfiguration _configuration;
        private readonly Lazy<SmtpClient> _smtpClient;

        public EmailService(IConfiguration configuration, ILoggerFactory loggerFactory)
        {

            _configuration = configuration;
            Logger = loggerFactory.CreateLogger<EmailService>();

            Logger.LogInformation("_smtpClient configuration starting");
            if (!string.IsNullOrWhiteSpace(_configuration["EmailSettings:Login"])
                && !string.IsNullOrWhiteSpace(_configuration["EmailSettings:Password"])
                && bool.Parse(_configuration["EmailSettings:UseDefaultCredentials"]))
            {
                _smtpClient = new Lazy<SmtpClient>(() =>
                new SmtpClient
                {
                    Host = _configuration["EmailSettings:Host"],
                    Port = int.Parse(_configuration["EmailSettings:Port"], CultureInfo.InvariantCulture),
                    EnableSsl = bool.Parse(_configuration["EmailSettings:EnableSSL"]),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    //UseDefaultCredentials = bool.Parse(_configuration["EmailSettings:UseDefaultCredentials"]),
                    Credentials = new NetworkCredential(_configuration["EmailSettings:Login"], _configuration["EmailSettings:Password"])
                });

                Logger.LogInformation("_smtpClient configured to work with credentials");
            }
            else
            {
                _smtpClient = new Lazy<SmtpClient>(() =>
                new SmtpClient
                {
                    Host = _configuration["EmailSettings:Host"],
                    Port = int.Parse(_configuration["EmailSettings:Port"], CultureInfo.InvariantCulture),
                    EnableSsl = bool.Parse(_configuration["EmailSettings:EnableSSL"]),
                    DeliveryMethod = SmtpDeliveryMethod.Network
                });

                Logger.LogInformation("_smtpClient configured to work without credentials");
            }

            //Неведомый костыль для работы с ssl сертификатами
            if (bool.Parse(_configuration["EmailSettings:EnableSSL"]))
                ServicePointManager.ServerCertificateValidationCallback = (s, certificate, chain, sslPolicyErrors) => true;
        }

        public virtual void Send(MailMessage message)
        {
            try
            {
                Logger.LogInformation($"Attempt to send email to {message.To}");
                _smtpClient.Value.Send(message);
                Logger.LogInformation($"Email sended to {message.To}");
            }
            catch (Exception e)
            {
                Logger.LogError($"Email failed to send: {e.Message}", e);
            }
        }

        public void Dispose()
        {
            if (_smtpClient.IsValueCreated)
            {
                _smtpClient.Value.Dispose();
            }
        }
    }
}
