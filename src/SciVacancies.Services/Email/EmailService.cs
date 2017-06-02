using System;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SciVacancies.Services.Email
{
    public class EmailService : IEmailService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private SmtpClient _smtpClient;

        public EmailService(IConfiguration configuration, ILoggerFactory loggerFactory)
        {

            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<EmailService>();

        }

        private bool Initialized;
        private bool sendEmailEnabled;

        private void Initialize()
        {
            _logger.LogInformation("_smtpClient configuration starting");
            if (!string.IsNullOrWhiteSpace(_configuration["EmailSettings:Login"])
                && !string.IsNullOrWhiteSpace(_configuration["EmailSettings:Password"])
                && bool.Parse(_configuration["EmailSettings:UseDefaultCredentials"]))
            {
                _smtpClient = new SmtpClient
                {
                    Host = _configuration["EmailSettings:Host"],
                    Port = int.Parse(_configuration["EmailSettings:Port"], CultureInfo.InvariantCulture),
                    EnableSsl = bool.Parse(_configuration["EmailSettings:EnableSSL"]),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    //UseDefaultCredentials = bool.Parse(_configuration["EmailSettings:UseDefaultCredentials"]),
                    Credentials =
                        new NetworkCredential(_configuration["EmailSettings:Login"], _configuration["EmailSettings:Password"])
                };

                _logger.LogInformation("_smtpClient configured to work with credentials");
            }
            else
            {
                _smtpClient = new SmtpClient
                {
                    Host = _configuration["EmailSettings:Host"],
                    Port = int.Parse(_configuration["EmailSettings:Port"], CultureInfo.InvariantCulture),
                    EnableSsl = bool.Parse(_configuration["EmailSettings:EnableSSL"]),
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                _logger.LogInformation("_smtpClient configured to work without credentials");
            }

            //Неведомый костыль для работы с ssl сертификатами
            if (bool.Parse(_configuration["EmailSettings:EnableSSL"]))
                ServicePointManager.ServerCertificateValidationCallback = (s, certificate, chain, sslPolicyErrors) => true;

            sendEmailEnabled = bool.Parse(_configuration["EmailSettings:SendEmailEnabled"]);
            Initialized = true;
        }

        public virtual void Send(MailMessage message)
        {
            if (!Initialized)
            {
                Initialize();
            }

            try
            {
                _logger.LogInformation($"Attempt to send email to {message.To}");

                if (sendEmailEnabled)
                    _smtpClient.Send(message);
                else
                _logger.LogInformation($"SendEmailEnabled: {_configuration["EmailSettings:SendEmailEnabled"]}, so Email not sent");

                _logger.LogInformation($"Email sended to {message.To}");
            }
            catch (Exception e)
            {
                _logger.LogError($"Email failed to send: {e.Message}", e);
            }
        }

        public void Dispose()
        {
            //if (_smtpClient.IsValueCreated)
            //{
            //    _smtpClient.Value.Dispose();
            //}
        }
    }
}
