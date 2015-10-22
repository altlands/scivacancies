using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace SciVacancies.Services.Email
{
    public class SmtpEmailService : IEmailService, IDisposable
    {
        private readonly string _login;
        private readonly string _password;
        private readonly Lazy<SmtpClient> _smtpClient;

        public SmtpEmailService(string login, string password, string host, int port, bool enableSsl, bool useDefaultCredentials)
        {
            _login = login;
            _password = password;

            _smtpClient = new Lazy<SmtpClient>(() => new SmtpClient
            {
                Host = host,
                Port = port,
                EnableSsl = enableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = useDefaultCredentials,
                Credentials = new NetworkCredential(_login, _password)
            });
            //Неведомый костыль для работы с ssl сертификатами
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };
        }

        public virtual void SendEmail(MailMessage message)
        {
            try
            {
                _smtpClient.Value.Send(message);
            }
            catch (Exception exception)
            {
                //todo: логировать ошибки при отправке почты
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
