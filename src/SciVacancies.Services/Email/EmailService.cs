using System;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Framework.Configuration;

namespace SciVacancies.Services.Email
{
    public class EmailService : IEmailService, IDisposable
    {
        readonly IConfiguration Configuration;

        readonly Lazy<SmtpClient> SmtpClient;

        public EmailService(IConfiguration configuration)
        {
            this.Configuration = configuration;

            if (string.IsNullOrWhiteSpace(Configuration["EmailSettings:Login"]))
                throw new ArgumentNullException("Не указан логин для подключения к серверу рассылку email-уведомлений");
            //if (string.IsNullOrWhiteSpace(Configuration["EmailSettings:Password"]))
            //    throw new ArgumentNullException("Не указан пароль для подключения к серверу рассылку email-уведомлений");

            this.SmtpClient = new Lazy<SmtpClient>(() => new SmtpClient
            {
                Host = Configuration["EmailSettings:Host"],
                Port = int.Parse(Configuration["EmailSettings:Port"], CultureInfo.InvariantCulture),
                EnableSsl = bool.Parse(Configuration["EmailSettings:EnableSSL"]),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = bool.Parse(Configuration["EmailSettings:UseDefaultCredentials"]),
                Credentials = new NetworkCredential(Configuration["EmailSettings:Login"], Configuration["EmailSettings:Password"])
            });
            //Неведомый костыль для работы с ssl сертификатами
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };
        }

        public virtual void Send(MailMessage message)
        {
            try
            {
                SmtpClient.Value.Send(message);
            }
            catch (Exception e)
            {
                throw e;
                //todo: логировать ошибки при отправке почты
            }
        }

        public void Dispose()
        {
            if (SmtpClient.IsValueCreated)
            {
                SmtpClient.Value.Dispose();
            }
        }
    }
}
